using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using SigmaDC.Interfaces;

namespace MathModel
{
    public class SigmaDCModel : IEvacuationModel
    {
        public class Parameters
        {
            /// <summary>
            /// Visual range
            /// </summary>
            public double r;
            public double w = 0;
            public double deltaD = 0.01;
            public double a = 0.1;  // Grid step size, 10 cm by default
            public int q = 4;   // The number of directions where human can move

            public double kw;
            public double kp;
            public double ks;
        }

        public enum MobilityGroup { First = 1, Second = 2, Third = 3, Fourth = 4 };
        public enum AgeGroup { First = 1, Second = 2, Third = 3, Fourth = 4, Fifth = 5 };
        public enum EmotionState { Custom = 0, Comfort = 1, Calm = 2, Active = 3, VeryActive = 4 };

        enum AdultClothes { LightClothing, InBetweenSeason, Winter };
        enum ChildClothes { LeisureWear, LeisureWearPlusBag, OutdoorClothes };
        enum ChildrenAgeGroups { Junior, Middle, Senior };

        enum PathType { Horizontal, Aperture, StairwayDown, StairwayUp, HorizontalOutsideBuilding };

        Parameters m_modelParams = new Parameters();

        public void SetupParameters( Dictionary<string, object> modelParams )
        {
            m_modelParams.r = ( double )modelParams[ "r" ];
            m_modelParams.w = ( double )modelParams[ "w" ];
            m_modelParams.deltaD = ( double )modelParams[ "deltaD" ];

            m_modelParams.kw = ( double )modelParams[ "kw" ];
            m_modelParams.kp = ( double )modelParams[ "kp" ];
            m_modelParams.ks = ( double )modelParams[ "ks" ];
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
            public Dictionary<EmotionState, SpeedInitialValues> initValues;

            public SpeedConst( double F0, double a_l, Dictionary<EmotionState, SpeedInitialValues> initValues )
            {
                this.F0 = F0;
                this.a_l = a_l;
                this.initValues = initValues;
            }
        }

        void FillSpeedConstants()
        {
            var speedConst = new Dictionary<PathType, SpeedConst>();

            var horizontalConst = new Dictionary<EmotionState, SpeedConst.SpeedInitialValues>();
            horizontalConst.Add( EmotionState.Comfort, new SpeedConst.SpeedInitialValues( 0.57, 0.08 ) );
            horizontalConst.Add( EmotionState.Calm, new SpeedConst.SpeedInitialValues( 0.96, 0.047 ) );
            horizontalConst.Add( EmotionState.Active, new SpeedConst.SpeedInitialValues( 1.3, 0.66 ) );
            horizontalConst.Add( EmotionState.VeryActive, new SpeedConst.SpeedInitialValues( 1.75, 0.083 ) );
            speedConst.Add( PathType.Horizontal, new SpeedConst( 0.06, 0.295, horizontalConst ) );

            var apertureConst = new Dictionary<EmotionState, SpeedConst.SpeedInitialValues>();
            apertureConst.Add( EmotionState.Comfort, new SpeedConst.SpeedInitialValues( 0.57, 0.08 ) );
            apertureConst.Add( EmotionState.Calm, new SpeedConst.SpeedInitialValues( 0.96, 0.047 ) );
            apertureConst.Add( EmotionState.Active, new SpeedConst.SpeedInitialValues( 1.3, 0.66 ) );
            apertureConst.Add( EmotionState.VeryActive, new SpeedConst.SpeedInitialValues( 1.75, 0.083 ) );
            speedConst.Add( PathType.Horizontal, new SpeedConst( 0.08, 0.295, apertureConst ) );

            var stairwayDownConst = new Dictionary<EmotionState, SpeedConst.SpeedInitialValues>();
            stairwayDownConst.Add( EmotionState.Comfort, new SpeedConst.SpeedInitialValues( 0.57, 0.08 ) );
            stairwayDownConst.Add( EmotionState.Calm, new SpeedConst.SpeedInitialValues( 0.96, 0.047 ) );
            stairwayDownConst.Add( EmotionState.Active, new SpeedConst.SpeedInitialValues( 1.3, 0.66 ) );
            stairwayDownConst.Add( EmotionState.VeryActive, new SpeedConst.SpeedInitialValues( 1.75, 0.083 ) );
            speedConst.Add( PathType.Horizontal, new SpeedConst( 0.10, 0.400, stairwayDownConst ) );

            var stairwayUpConst = new Dictionary<EmotionState, SpeedConst.SpeedInitialValues>();
            stairwayUpConst.Add( EmotionState.Comfort, new SpeedConst.SpeedInitialValues( 0.31, 0.47 ) );
            stairwayUpConst.Add( EmotionState.Calm, new SpeedConst.SpeedInitialValues( 0.54, 0.03 ) );
            stairwayUpConst.Add( EmotionState.Active, new SpeedConst.SpeedInitialValues( 0.775, 0.47 ) );
            stairwayUpConst.Add( EmotionState.VeryActive, new SpeedConst.SpeedInitialValues( 1.08, 0.05 ) );
            speedConst.Add( PathType.Horizontal, new SpeedConst( 0.08, 0.305, stairwayUpConst ) );

            var horizontalOutsideBuildingConst = new Dictionary<EmotionState, SpeedConst.SpeedInitialValues>();
            speedConst.Add( PathType.HorizontalOutsideBuilding, new SpeedConst( 0.08, 0.407, horizontalOutsideBuildingConst ) );
        }

        public class Human
        {
            public float projectionSize;
            public int exitId;
            public MobilityGroup mobilityGroup;
            public AgeGroup ageGroup;
            public EmotionState emotionState;
            public double diameter;
        }

        public void InitShortestPathField()
        {
            throw new NotImplementedException();
        }

        double HeavisideFunction( double x )
        {
            if ( x < 0 ) return 0;
            if ( x == 0 ) return 0.5;   // FIXME: incorrect compare without eps
            return 1;
        }

        double Density( double x )
        {
            throw new NotImplementedException();
        }

        double WallApproching( double x )
        {
            throw new NotImplementedException();
        }

        public PointD HumanNextStep( Human human, PointD xPrev )
        {
            PointD result = new PointD();
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
            double norm = 0;
            for ( int j = 0; j < m_modelParams.q; ++j )
            {
                // 0.1) Calculate r*[j] - the distance from human projection centre to first obstacle on his way
                double r_j = 0;
                // 0.2) dS[j] - the difference between old and new shortest path field values
                double d_S = 0;
                //
                double p_j = Math.Exp( -m_modelParams.kw * ( 1 - r_j / m_modelParams.r ) * HeavisideFunction( d_S ) )
                    * Math.Exp( -m_modelParams.kp * Density( r_j ) )
                    * Math.Exp( m_modelParams.ks / d_S )
                    * WallApproching( r_j - human.diameter / 2 );

                norm += p_j;
            }
            //
            // 1.2) norm == 0 ==> man will not move
            //
            // 1.3) norm != 0 ==> select ("raffle") the move direction e^[j] = ( cos( 2*Pi*j^/q ), sin( 2*Pi*j^/q ) )
            //      using previosly calculated probability distribution
            //
            // 1.4) Calculate new human coordinates
            //
            // 1.4.1) Calculate possible coordinates:
            //        x = xPrev + e[j^]*v*dt, (2)
            //        where speed v depend from density F(r*[j^]), a kind of path,
            //        human age group (TODO: currently only EmSt variable used),
            //        and determine using corresponding table or calculated using formula below
            //
            // 1.4.2) Select the e^[j] direction if human[i] track will not be crossed
            //        by other humans or will be, but inside of compression coefficient range:
            //        Trace( P[i] )*I( U( l!=i, P_prev[l] belong_to V[j] )*P_prev[l] ) = empty_set
            //     || Trace( P[i] )*I( U( l!=i, P_prev[l] belong_to V[j] )*P_prev[l] ) != empty_set,
            //        l: | x[i] - xPrev[l] | <= d[i]/2 + d[l]/2 - deltaD,
            //        where Trace( P[i] ) - the track of i-th human producing by move from xPrev to x,
            //        deltaD - people compression coefficient (model parameter), can be a constant
            //                 or density-dependant function
            //
            // 1.4.3) If conditions from 1.4.2 are ruled out, then supposed new human coordinates x[i]
            //        must be corrected to be met:
            //        x[i] = xPrev[i] + ( v[i] - xi )*e[j^]*dt,
            //        xi: Trace( P[i] )*I( U( l!=i, P_prev[l] belong_to V[j] )*P_prev[l] ) != empty_set,
            //        l: | x[i] - xPrev[l] | <= d[i]/2 + d[l]/2 - deltaD
            //        If such condition can not be met, then the human will stay on his previous position
            //
            // 1.4.4) After than all people supposed coordinates was calculated,
            //        the collision resolution procedure must be applied in case
            //        of multiple people pretend to the same place:
            //        exist l,m: | x[l] - x[m] | <= d[l]/2 + d[m]/2 - deltaD
            //        Then the all of people movement will be denied with probability tau belong_to [0;1].
            //        With a probability ( 1 - tau ) one randomly selected human will be moved to the
            //        controversial coordinates, the others will stay of their previous positions
            //
            // 1.4.5) People that are able to move will be moved to new coodinates in the same time;
            //        if the human arrived to the exit from building, it will not be taken into account
            //        in calculations above
            //
            return result;
        }
    }
}
