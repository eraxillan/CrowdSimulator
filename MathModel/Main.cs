using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using SigmaDC.Interfaces;
using SigmaDC.Common.MathEx;

namespace SigmaDC.MathModel
{
    public class SigmaDCModel : IEvacuationModel
    {
        public class Parameters
        {
            /// <summary>
            /// Visual range
            /// </summary>
            public float r;
            public float w = 0;
            public float deltaD = 0.01f;
            
            /// <summary>
            /// Grid step size, 10 cm by default
            /// </summary>
            public float a = 0.1f;

            /// <summary>
            /// The number of directions where human can move
            /// </summary>
            public int q = 8;

            public float kw;
            public float kp;
            public float ks;
        }

        enum AdultClothes { LightClothing, InBetweenSeason, Winter };
        enum ChildClothes { LeisureWear, LeisureWearPlusBag, OutdoorClothes };
        enum ChildrenAgeGroups { Junior, Middle, Senior };
        enum PathType { Horizontal, Aperture, StairwayDown, StairwayUp, HorizontalOutsideBuilding };

        Parameters m_modelParams = new Parameters();
        IEnumerable<IHuman> m_people = new List<IHuman>();
        List<SdcRectangle> m_obstacleExtents = new List<SdcRectangle>();

        public void SetupParameters( Dictionary<string, object> modelParams )
        {
            m_modelParams.r = ( float )modelParams[ "r" ];
            m_modelParams.w = ( float )modelParams[ "w" ];
            m_modelParams.deltaD = ( float )modelParams[ "deltaD" ];

            m_modelParams.kw = ( float )modelParams[ "kw" ];
            m_modelParams.kp = ( float )modelParams[ "kp" ];
            m_modelParams.ks = ( float )modelParams[ "ks" ];
        }

        public void SetupObstacles( List<RectangleF> obstacleExtents )
        {
            foreach ( var extent in obstacleExtents )
            {
                m_obstacleExtents.Add( new SdcRectangle( extent ) );
            }
        }

        public void SetupPeople( IEnumerable<IHuman> people )
        {
            m_people = people;
        }

        public void ValidateParameters()
        {
            // FIXME: implement
            throw new NotImplementedException();

            // r >= min( d[i]/2 ), i = 1, N, where N - people count
            // a <= min( [i] )
            // kW > 0, kP > 0, kS > 0
        }

        void FillProjectionDiametersTables()
        {
            // weather -> circular projection diameter (meters, m)
            Dictionary<AdultClothes, double> adultDiameters = new Dictionary<AdultClothes, double>();
            adultDiameters.Add( AdultClothes.LightClothing, 0.35 );
            adultDiameters.Add( AdultClothes.InBetweenSeason, 0.36 );
            adultDiameters.Add( AdultClothes.Winter, 0.40 );

            Dictionary<ChildClothes, Dictionary<ChildrenAgeGroups, double>> childDiameters = new Dictionary<ChildClothes, Dictionary<ChildrenAgeGroups, double>>();
            childDiameters.Add( ChildClothes.LeisureWear, new Dictionary<ChildrenAgeGroups, double>() { { ChildrenAgeGroups.Junior, 0.22 }, { ChildrenAgeGroups.Middle, 0.27 }, { ChildrenAgeGroups.Senior, 0.32 } } );
            childDiameters.Add( ChildClothes.LeisureWearPlusBag, new Dictionary<ChildrenAgeGroups, double>() { { ChildrenAgeGroups.Junior, 0.28 }, { ChildrenAgeGroups.Middle, 0.356 }, { ChildrenAgeGroups.Senior, 0.42 } } );
            childDiameters.Add( ChildClothes.OutdoorClothes, new Dictionary<ChildrenAgeGroups, double>() { { ChildrenAgeGroups.Junior, 0.34 }, { ChildrenAgeGroups.Middle, 0.4 }, { ChildrenAgeGroups.Senior, 0.45 } } );
        }

        public struct SpeedConst
        {
            public struct SpeedInitialValues
            {
                public double v0;
                public double sigma_v0;

                public SpeedInitialValues( double v0, double sigma_v0 )
                {
                    this.v0 = v0;
                    this.sigma_v0 = sigma_v0;
                }
            }

            public double F0;
            public double a_l;
            public Dictionary<HumanEmotionState, SpeedInitialValues> initValues;

            public SpeedConst( double F0, double a_l, Dictionary<HumanEmotionState, SpeedInitialValues> initValues )
            {
                this.F0 = F0;
                this.a_l = a_l;
                this.initValues = initValues;
            }
        }

        void FillSpeedConstants()
        {
            var speedConst = new Dictionary<PathType, SpeedConst>();

            var horizontalConst = new Dictionary<HumanEmotionState, SpeedConst.SpeedInitialValues>();
            horizontalConst.Add( HumanEmotionState.Comfort, new SpeedConst.SpeedInitialValues( 0.57, 0.08 ) );
            horizontalConst.Add( HumanEmotionState.Calm, new SpeedConst.SpeedInitialValues( 0.96, 0.047 ) );
            horizontalConst.Add( HumanEmotionState.Active, new SpeedConst.SpeedInitialValues( 1.3, 0.66 ) );
            horizontalConst.Add( HumanEmotionState.VeryActive, new SpeedConst.SpeedInitialValues( 1.75, 0.083 ) );
            speedConst.Add( PathType.Horizontal, new SpeedConst( 0.06, 0.295, horizontalConst ) );

            var apertureConst = new Dictionary<HumanEmotionState, SpeedConst.SpeedInitialValues>();
            apertureConst.Add( HumanEmotionState.Comfort, new SpeedConst.SpeedInitialValues( 0.57, 0.08 ) );
            apertureConst.Add( HumanEmotionState.Calm, new SpeedConst.SpeedInitialValues( 0.96, 0.047 ) );
            apertureConst.Add( HumanEmotionState.Active, new SpeedConst.SpeedInitialValues( 1.3, 0.66 ) );
            apertureConst.Add( HumanEmotionState.VeryActive, new SpeedConst.SpeedInitialValues( 1.75, 0.083 ) );
            speedConst.Add( PathType.Horizontal, new SpeedConst( 0.08, 0.295, apertureConst ) );

            var stairwayDownConst = new Dictionary<HumanEmotionState, SpeedConst.SpeedInitialValues>();
            stairwayDownConst.Add( HumanEmotionState.Comfort, new SpeedConst.SpeedInitialValues( 0.57, 0.08 ) );
            stairwayDownConst.Add( HumanEmotionState.Calm, new SpeedConst.SpeedInitialValues( 0.96, 0.047 ) );
            stairwayDownConst.Add( HumanEmotionState.Active, new SpeedConst.SpeedInitialValues( 1.3, 0.66 ) );
            stairwayDownConst.Add( HumanEmotionState.VeryActive, new SpeedConst.SpeedInitialValues( 1.75, 0.083 ) );
            speedConst.Add( PathType.Horizontal, new SpeedConst( 0.10, 0.400, stairwayDownConst ) );

            var stairwayUpConst = new Dictionary<HumanEmotionState, SpeedConst.SpeedInitialValues>();
            stairwayUpConst.Add( HumanEmotionState.Comfort, new SpeedConst.SpeedInitialValues( 0.31, 0.47 ) );
            stairwayUpConst.Add( HumanEmotionState.Calm, new SpeedConst.SpeedInitialValues( 0.54, 0.03 ) );
            stairwayUpConst.Add( HumanEmotionState.Active, new SpeedConst.SpeedInitialValues( 0.775, 0.47 ) );
            stairwayUpConst.Add( HumanEmotionState.VeryActive, new SpeedConst.SpeedInitialValues( 1.08, 0.05 ) );
            speedConst.Add( PathType.Horizontal, new SpeedConst( 0.08, 0.305, stairwayUpConst ) );

            var horizontalOutsideBuildingConst = new Dictionary<HumanEmotionState, SpeedConst.SpeedInitialValues>();
            speedConst.Add( PathType.HorizontalOutsideBuilding, new SpeedConst( 0.08, 0.407, horizontalOutsideBuildingConst ) );
        }

        public void InitShortestPathField()
        {
            throw new NotImplementedException();
        }

        float HeavisideFunction( float x )
        {
            if ( x < 0 ) return 0;
            if ( MathUtils.NearlyZero( x ) ) return 0.5f;
            return 1;
        }

        float Density( float r_j, IHuman currHuman )
        {
            // FIXME: precision loss during double ---> float conversion
            /*RectangleF V = new RectangleF( (float)currHuman.projectionCenter.X, (float)currHuman.projectionCenter.Y, 
                (float)currHuman.projectionDiameter, (float)r_j );
            
            List<Human> m_people = new List<Human>();
            foreach(var human in m_people)
            {
                if(human.Ex)
            }*/
            throw new NotImplementedException();
        }

        float WallApproching( float x )
        {
            return ( x > m_modelParams.w ) ? 1 : 0;
        }

        public void NextStepAll( IDistanceField S, ref List<HumanRuntimeInfo> hi )
        {
            foreach ( var h in m_people )
            {
                var hiTemp = new HumanRuntimeInfo(h);
                NextStep( h, S, ref hiTemp );
                hi.Add( hiTemp );
            }
        }

        public Vector2 NextStep( IHuman human, IDistanceField S, ref HumanRuntimeInfo humanInfo )
        {
            Vector2 xPrev = human.ProjectionCenter;

            // TODO: j == q case is reduntant
            for ( int j = 1; j <= m_modelParams.q; ++j )
            {
                float phi = MathUtils.TwoPi * j / m_modelParams.q;

                var e_j = new Vector2( ( float )Math.Cos( phi ), ( float )Math.Sin( phi ) );

                SdcRectangle rect = new SdcRectangle( xPrev, human.ProjectionDiameter, m_modelParams.r, phi );
  
                humanInfo.RotateAngles.Add( phi );
                humanInfo.MoveDirections.Add( e_j );
                humanInfo.VisibilityAreas.Add( rect );
            }

            //
            // x, xPrev - 2D-coordinates (current and previous), non-dimensional quantity
            // v - speed, meters/second
            // e - direction, non-dimensional quantity
            // dt - time step, seconds
            // x = xPrev + v*e*dt
            //
            // 1.1) Calculate move probabilities to e[j] direction:
            //    e[j] = ( cos( 2*Pi*j/q ), sin( 2*Pi*j/q ) ), j=[1,q]
            //    p[j] = p^[j] / norm,
            //    p^[j] = exp( -kW * ( 1 - r*[j]/r ) * l( dS[j] ) ) 
            //          * exp( -kP * F( r*[j] ) ) * exp( kS * dS[j] )               (1)
            //          * W( r*[j] - d/2 ),
            //    where:
            //    norm = sum( 1, q, p^[j] ),
            //    F( r*[j] ) - people density,
            //    F( r*[j] ) = S2 / (d * r*[j]), where S2 - people occupied area in V[j] = d x r*[j] rectangle
            //    dS[j] = S( xPrev ) - S( x_r ), where x_r = xPrev + 0.1*e[j],
            //    W( r*[j] - d/2 ) = ( r*[j] - d/2 ) > w ? 1 : 0 - regulate human approaching to walls,
            //    where w (model parameter) regulates wall "sticking" degree
            //
            // Limitations:
            //    r >= min( d[i]/2 ), i = 1,N, where N - people count
            //    min( d[i]/2 ) <= r*[j] <= r
            //
            List<KeyValuePair<float, Vector2>> directions = new List<KeyValuePair<float, Vector2>>();
            List<float> probabilities = new List<float>( m_modelParams.q );
            float norm = 0;
            for ( int j = 1; j <= m_modelParams.q; ++j )
            {
                //float angle = 2 * (float)Math.PI * j / m_modelParams.q;
                //var e_j = new Vector2( (float)Math.Cos( angle ), (float)Math.Sin( angle ) );
                //directions.Add( new KeyValuePair<float,Vector2>( angle, e_j ) );

                // 0.1) Calculate r*[j] - the distance from human projection centre to first obstacle on his way
                float r_j;
                if ( !FindNearestObstacle( j, m_modelParams.r, humanInfo, out r_j ) )
                {
                    r_j = m_modelParams.r;
                }
                humanInfo.MinDistToObstacle.Add( r_j );

//                Debug.Assert( r_j <= m_modelParams.r );

                // 0.2) dS[j] - the difference between old and new shortest path field values
 /*               var x_r = new Vector2( xPrev.X + 0.1f * humanInfo.MoveDirections[ j ].X, xPrev.Y + 0.1f * humanInfo.MoveDirections[ j ].Y );
                float d_S = S.Get( xPrev.X, xPrev.Y ) - S.Get( x_r.X, x_r.Y );

                float p_j = (float)Math.Exp( -m_modelParams.kw * ( 1 - r_j / m_modelParams.r ) * HeavisideFunction( d_S ) )
                    * ( float )Math.Exp( -m_modelParams.kp * Density( r_j, human ) )
                    * ( float )Math.Exp( m_modelParams.ks * d_S )
                    * WallApproching( r_j - human.projectionDiameter / 2.0f );
                probabilities.Add( (float)p_j );

                norm += p_j;*/
            }

            // FIXME: remove
            return new Vector2();
            
            //
            // p[j] = p^[j] / norm,
            // FIXME: check this is working code!
            //
            probabilities.ForEach( x => x /= norm );

            //
            // 1.2) norm == 0 ==> man will not move
            //
            if ( MathUtils.NearlyZero( norm ) ) return new Vector2();

            //
            // 1.3) norm != 0 ==> select ("raffle") the move direction e^[j] = ( cos( 2*Pi*j^/q ), sin( 2*Pi*j^/q ) )
            //      using previosly calculated probability distribution
            //
            int j_selected = RaffleProbability( probabilities );

            //
            // 1.4) Calculate new human coordinates
            //
            // 1.4.1) Calculate possible coordinates:
            //        x = xPrev + e[j^]*v*dt, (2)
            //        where speed v depend from density F(r*[j^]), a kind of path,
            //        human age group (TODO: currently only EmSt variable used),
            //        and determine using corresponding table or calculated using formula below
            //
            var selectedDir = directions[ j_selected ];
            
            //
            // 1.4.2) Select the e^[j] direction if human[i] track will not be crossed
            //        by other humans or will be, but inside of compression coefficient range:
            //        Trace( P[i] )*I( U( l!=i, P_prev[l] belong_to V[j] )*P_prev[l] ) == empty_set
            //     || Trace( P[i] )*I( U( l!=i, P_prev[l] belong_to V[j] )*P_prev[l] ) != empty_set,
            //        l: | x[i] - xPrev[l] | <= d[i]/2 + d[l]/2 - deltaD,
            //        where Trace( P[i] ) - the track of i-th human producing by move from xPrev to x,
            //        deltaD - people compression coefficient (model parameter), can be a constant
            //                 or density-dependant function
            //
            // TODO:

            //
            // 1.4.3) If conditions from 1.4.2 are ruled out, then supposed new human coordinates x[i]
            //        must be corrected to be met:
            //        x[i] = xPrev[i] + ( v[i] - xi )*e[j^]*dt,
            //        xi: Trace( P[i] )*I( U( l!=i, P_prev[l] belong_to V[j] )*P_prev[l] ) != empty_set,
            //        l: | x[i] - xPrev[l] | <= d[i]/2 + d[l]/2 - deltaD
            //        If such condition can not be met, then the human will stay on his previous position
            //

            //
            // 1.4.4) After than all people supposed coordinates was calculated,
            //        the collision resolution procedure must be applied in case
            //        of multiple people pretend to the same place:
            //        exist l,m: | x[l] - x[m] | <= d[l]/2 + d[m]/2 - deltaD
            //        Then the all of people movement will be denied with probability tau belong_to [0;1].
            //        With a probability ( 1 - tau ) one randomly selected human will be moved to the
            //        controversial coordinates, the others will stay of their previous positions
            //

            //
            // 1.4.5) People that are able to move will be moved to new coodinates in the same time;
            //        if the human arrived to the exit from building, it will not be taken into account
            //        in calculations above
            //

            // FIXME: remove
            return new Vector2();
        }

        int RaffleProbability( List<float> probabilities )
        {
            Trace.Assert( probabilities.Count == m_modelParams.q );

            System.Random rndGen = new Random();
            double randomNum = rndGen.NextDouble();
            for ( int i = 0; i < probabilities.Count;++i )
            {
                if ( randomNum >= probabilities[ i ] ) return i;
            }

            Trace.Assert( false, "Invalid probability value was generated || empty probability list was specified" );
            return 0;
        }

        bool FindNearestObstacle( int j, float r_max, HumanRuntimeInfo hi, out float dist )
        {
            // NOTE: Test case human IDs': 412, 531

            var distances = new List<float>();
            foreach ( var rect in m_obstacleExtents )
            {
                if ( rect.Intersects( hi.VisibilityAreas[ j - 1 ] ) && !rect.Contains( hi.VisibilityAreas[ j - 1 ] ) )
                {
                    // Obtain actual visibility area rectangle (rotated one)
                    var rotatedVa = hi.VisibilityAreas[ j - 1 ].Rotate( hi.VisibilityAreas[ j - 1 ].RotationCenter, hi.VisibilityAreas[ j - 1 ].RotationAngle );

                    float distTemp = rect.DistanceTo( rotatedVa );

                    // Check whether the obstacle just "touches" the visibility area rect:
                    // the minimum distance can't be smaller than hi.ProjectionDiameter/2,
                    // i.e. human can't "grow into" the wall
                    if ( MathUtils.NearlyZero( distTemp ) ) continue;
                    
                    distances.Add( distTemp );
                }
            }

            if ( distances.Count == 0 )
            {
                dist = float.NaN;
                return false;
            }

            dist = distances.Min();
            if ( dist > r_max ) dist = r_max;
            return true;
        }
    }
}
