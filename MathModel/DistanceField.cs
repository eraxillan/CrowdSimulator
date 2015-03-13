using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using SigmaDC.Common.Math;
using SigmaDC.Types;

namespace MathModel
{
    public class DistanceField
    {
        BuildingWrapper m_building = null;
        float m_a = 0.1f;
        int m_M = -1;
        int m_N = -1;
        float m_x0 = float.NaN;
        float m_y0 = float.NaN;
        DrawMode m_drawMode = DrawMode.None;
        int[ , ] m_G = null;
        double[ , ] m_S = null;
        double m_obstacleConst = double.NaN;

        public enum DrawMode { None = 0, Raw, G, S, Count };

        public DistanceField( BuildingWrapper building, float a, int M, int N, float x0, float y0, DrawMode dm = DrawMode.None )
        {
            m_building = building;
            m_a = a;
            m_M = M;
            m_N = N;
            m_x0 = x0;
            m_y0 = y0;
            m_drawMode = dm;
            m_obstacleConst = M * N;
        }

        static RectangleF NormalizeRect( RectangleF src, float a )
        {
            var rect = src;
            // TODO: what is the correct inflate constant?
            if ( rect.Width < a ) rect.Inflate( a / 8, 0 );
            if ( rect.Height < a ) rect.Inflate( 0, a / 8 );
            return rect;
        }

        bool IntersectBox( RectangleF cellRect )
        {
            foreach ( var box in m_building.CurrentFloor.Geometry )
            {
                RectangleF boxExtents = box.Extents;

                bool b = ( !boxExtents.IsEmpty && ( boxExtents.Top < boxExtents.Bottom ) );
                if ( !b ) System.Diagnostics.Debugger.Break();

                // We are interesting in those cases to check intersection presents:
                // TODO: check whether we need to detect tangent to obstacles cells
                // 1) Cell rectangle is tangent to the box one and entirely contained in it
                // 2) Cell rectange is tangent to the box one, but from the outer face of it
                /*const float EPS = 0.000001f;
                if ( MathUtils.NearlyEqual( box.Extents.Left, cellRect.Left, EPS )
                        || MathUtils.NearlyEqual( box.Extents.Right, cellRect.Right, EPS )
                        || MathUtils.NearlyEqual( box.Extents.Top, cellRect.Top, EPS )
                        || MathUtils.NearlyEqual( box.Extents.Bottom, cellRect.Bottom, EPS ) )
                    return true;*/

                // 3) Cell rectangle intersect one of the rectangle sides
                var rect = RectangleF.Intersect( boxExtents, cellRect );
                if ( ( rect.Width > 0 || rect.Height > 0 ) && !boxExtents.Contains( cellRect ) )
                {
                    return true;
                }
            }

            return false;
        }

        bool IntersectFurniture( RectangleF cellRect )
        {
            foreach ( var furn in m_building.CurrentFloor.Furniture )
            {
                RectangleF furnExtents = furn.Extents;

                bool b = ( !furnExtents.IsEmpty && ( furnExtents.Top < furnExtents.Bottom ) );
                if ( !b ) System.Diagnostics.Debugger.Break();

                var rect = RectangleF.Intersect( furnExtents, cellRect );
                if ( ( rect.Width > 0 || rect.Height > 0 ) )
                {
                    return true;
                }
            }

            return false;
        }

        bool IntersectStairway( RectangleF cellRect )
        {
            foreach ( var st in m_building.CurrentFloor.Stairways )
            {
                RectangleF stExtents = st.Extents;

                bool b = ( !stExtents.IsEmpty && ( stExtents.Top < stExtents.Bottom ) );
                if ( !b ) System.Diagnostics.Debugger.Break();

                var rect = RectangleF.Intersect( stExtents, cellRect );
                if ( ( rect.Width > 0 || rect.Height > 0 ) && !stExtents.Contains( cellRect ) )
                {
                    return true;
                }
            }

            return false;
        }

        bool IntersectFakeAperture( RectangleF cellRect )
        {
            foreach ( var aper in m_building.CurrentFloor.FakeApertures )
            {
                RectangleF aperExtents = aper.Extents;

                bool b = ( ( aperExtents.Width > 0 || aperExtents.Height > 0 ) && ( aperExtents.Top <= aperExtents.Bottom ) );
                if ( !b ) System.Diagnostics.Debugger.Break();

                var rect = RectangleF.Intersect( aperExtents, cellRect );
                if ( ( rect.Width > 0 || rect.Height > 0 ) && !aperExtents.Contains( cellRect ) )
                {
                    if ( MathUtils.NearlyZero( rect.Height ) )
                    {
                        if ( rect.Left == aperExtents.Left ) continue;
                        if ( rect.Right == aperExtents.Right ) continue;
                    }

                    if ( MathUtils.NearlyZero( rect.Width ) )
                    {
                        if ( rect.Top == aperExtents.Top ) continue;
                        if ( rect.Bottom == aperExtents.Bottom ) continue;
                    }

                    return true;
                }
            }

            return false;
        }

        bool IntersectExitDoor( RectangleF cellRect, out int exitId )
        {
            exitId = -2;

            // NOTE: this call is expensive
            int i = 1;
            foreach ( var exit in m_building.CurrentFloor.Exits )
            {
                var rect = NormalizeRect( exit.Extents, m_a );
                var intersectRect = RectangleF.Intersect( rect, cellRect );
                if ( !intersectRect.IsEmpty && !rect.Contains( cellRect ) )
                {
                    exitId = i;
                    return true;
                }

                ++i;
            }

            return false;

            /*int exitCount = m_building.CurrentFloor.Exits.Count();
            for ( int k = 1; k <= exitCount; ++k )
            {
                var ext = NormalizeRect( m_building.CurrentFloor.Exits[ k - 1 ].Extents, m_a );
                var rect = RectangleF.Intersect( ext, cellRect );
                if ( !rect.IsEmpty && !ext.Contains( cellRect ) )
                {
                    exitId = k;
                    return true;
                }
            }

            return false;*/
        }

        bool IntersectInnerDoor( RectangleF cellRect )
        {
            foreach ( var aper in m_building.CurrentFloor.Doors )
            {
                RectangleF aperExtents = aper.Extents;

                bool b = ( ( aperExtents.Width > 0 || aperExtents.Height > 0 ) && ( aperExtents.Top <= aperExtents.Bottom ) );
                if ( !b ) System.Diagnostics.Debugger.Break();

                var rect = RectangleF.Intersect( aperExtents, cellRect );
                if ( ( rect.Width > 0 || rect.Height > 0 ) && !aperExtents.Contains( cellRect ) )
                {
                    if ( MathUtils.NearlyZero( rect.Height ) )
                    {
                        if ( rect.Left == aperExtents.Left ) continue;
                        if ( rect.Right == aperExtents.Right ) continue;
                    }

                    if ( MathUtils.NearlyZero( rect.Width ) )
                    {
                        if ( rect.Top == aperExtents.Top ) continue;
                        if ( rect.Bottom == aperExtents.Bottom ) continue;
                    }

                    return true;
                }
            }

            return false;
        }

        bool IntersectWindow( RectangleF cellRect )
        {
            foreach ( var aper in m_building.CurrentFloor.Windows )
            {
                RectangleF aperExtents = aper.Extents;

                bool b = ( ( aperExtents.Width > 0 || aperExtents.Height > 0 ) && ( aperExtents.Top <= aperExtents.Bottom ) );
                if ( !b ) System.Diagnostics.Debugger.Break();

                var rect = RectangleF.Intersect( aperExtents, cellRect );
                if ( ( rect.Width > 0 || rect.Height > 0 ) && !aperExtents.Contains( cellRect ) )
                {
                    return true;
                }
            }

            return false;
        }

        public int[ , ] CalcGField( int selectedExitId )
        {
            if ( selectedExitId < 0 || selectedExitId > m_building.CurrentFloor.Exits.Count() )
                System.Diagnostics.Debugger.Break();

            var G = new int[ m_M, m_N ];

            for ( int i = 0; i < m_M; ++i )
            {
                for ( int j = 0; j < m_N; ++j )
                {
                    G[ i, j ] = -1; // "empty" cell

                    // Calculate cell rectangle
                    float cellX = m_x0 + m_a * i;
                    float cellY = m_y0 + m_a * j;
                    RectangleF cellRect = new RectangleF( cellX, cellY, m_a, m_a );

                    // Check whether G[i,j] intersects with an building exit aperture/exit aperture part
                    int exitId;
                    if ( IntersectExitDoor( cellRect, out exitId ) )
                    {
                        if ( exitId == selectedExitId )
                            G[ i, j ] = exitId;
                        else
                            G[ i, j ] = 0;

                        continue;
                    }

                    // Check whether G[i,j] intersects with an door aperture/exit aperture part
                    if ( IntersectInnerDoor( cellRect ) )
                    {
                        continue;
                    }

                    // Check whether G[i,j] intersects with a wall/wall part
                    if ( IntersectBox( cellRect ) )
                    {
                        if ( !IntersectFakeAperture( cellRect ) )
                        {
                            G[ i, j ] = 0;
                            continue;
                        }
                    }

                    // Check whether G[i,j] intersects with a stairway part
                    if ( IntersectStairway( cellRect ) )
                    {
                        if ( !IntersectFakeAperture( cellRect ) )
                        {
                            G[ i, j ] = 0;
                            continue;
                        }
                    }

                    // Check whether G[i,j] intersects with furniture/furniture part
                    if ( IntersectFurniture( cellRect ) )
                    {
                        G[ i, j ] = 0;
                        continue;
                    }

                    // Check whether G[i,j] intersects with a window/window part
                    if ( IntersectWindow( cellRect ) )
                    {
                        G[ i, j ] = 0;
                        continue;
                    }
                }
            }

            return G;
        }

        public double[ , ] InitSField( out int emptyCellCount )
        {
            emptyCellCount = 0;

            var S = new double[ m_M, m_N ];
            // Init distance field
            for ( int i = 0; i < m_M; ++i )
            {
                for ( int j = 0; j < m_N; ++j )
                {
                    float cellX = m_x0 + m_a * i;
                    float cellY = m_y0 + m_a * j;

                    if ( m_G[ i, j ] == 0 )   // obstacle
                    {
                        S[ i, j ] = m_obstacleConst;
                    }
                    else if ( m_G[ i, j ] > 0 ) // exit
                    {
                        S[ i, j ] = 1;
                    }
                    else
                    {
                        S[ i, j ] = 0;
                        emptyCellCount++;
                    }
                }
            }

            return S;
        }

        public void CalcSField( double[ , ] S, ref int emptyCellCount )
        {
            double sqrt_2 = Math.Sqrt( 2 );
            double sqrt_5 = Math.Sqrt( 5 );

            // Field traversal
            for ( int i = 0; i < m_M; ++i )
            {
                for ( int j = 0; j < m_N; ++j )
                {
                    if ( !MathUtils.NearlyZero( S[ i, j ] ) ) continue;

                    var stepValues = new List<double>();

                    //                  ( i-2, j-1 )
                    //                       ||
                    // ( i-1, j-2 ) <== ( i-1, j-1 )
                    if ( ( i >= 1 && j >= 1 ) && ( !MathUtils.NearlyEqual( S[ i - 1, j - 1 ], m_obstacleConst ) ) )
                    {
                        if ( ( i >= 1 && j >= 1 ) && ( !MathUtils.NearlyZero( S[ i - 1, j - 1 ] ) ) )
                        {
                            stepValues.Add( sqrt_2 + S[ i - 1, j - 1 ] );
                        }

                        if ( ( i >= 1 && j >= 2 ) && !MathUtils.NearlyZero( S[ i - 1, j - 2 ] ) && !MathUtils.NearlyEqual( S[ i - 1, j - 2 ], m_obstacleConst ) )
                        {
                            stepValues.Add( sqrt_5 + S[ i - 1, j - 2 ] );
                        }

                        if ( ( i >= 2 && j >= 1 ) && !MathUtils.NearlyZero( S[ i - 2, j - 1 ] ) && !MathUtils.NearlyEqual( S[ i - 2, j - 1 ], m_obstacleConst ) )
                        {
                            stepValues.Add( sqrt_5 + S[ i - 2, j - 1 ] );
                        }
                    }

                    // ( i-2, j+1 )
                    //      ||
                    // ( i-1, j+1 ) ==> ( i-1, j+2 )
                    if ( ( i >= 1 && j <= m_N - 2 ) && !MathUtils.NearlyEqual( S[ i - 1, j + 1 ], m_obstacleConst ) )
                    {
                        if ( ( i >= 1 && j <= m_N - 2 ) && ( !MathUtils.NearlyZero( S[ i - 1, j + 1 ] ) ) )
                        {
                            stepValues.Add( sqrt_2 + S[ i - 1, j + 1 ] );
                        }

                        if ( ( i >= 1 && j <= m_N - 3 ) && !MathUtils.NearlyZero( S[ i - 1, j + 2 ] ) && !MathUtils.NearlyEqual( S[ i - 1, j + 2 ], m_obstacleConst ) )
                        {
                            stepValues.Add( sqrt_5 + S[ i - 1, j + 2 ] );
                        }

                        if ( ( i >= 2 && j <= m_N - 2 ) && !MathUtils.NearlyZero( S[ i - 2, j + 1 ] ) && !MathUtils.NearlyEqual( S[ i - 2, j + 1 ], m_obstacleConst ) )
                        {
                            stepValues.Add( sqrt_5 + S[ i - 2, j + 1 ] );
                        }
                    }

                    // ( i+1, j+1 ) ==> ( i+1, j+2 )
                    //      ||
                    // ( i+2, j+1 )
                    if ( ( i <= m_M - 2 && j <= m_N - 2 ) && !MathUtils.NearlyEqual( S[ i + 1, j + 1 ], m_obstacleConst ) )
                    {
                        if ( ( i <= m_M - 2 && j <= m_N - 2 ) && ( !MathUtils.NearlyZero( S[ i + 1, j + 1 ] ) ) )
                        {
                            stepValues.Add( sqrt_2 + S[ i + 1, j + 1 ] );
                        }

                        if ( ( i <= m_M - 2 && j <= m_N - 3 ) && !MathUtils.NearlyZero( S[ i + 1, j + 2 ] ) && !MathUtils.NearlyEqual( S[ i + 1, j + 2 ], m_obstacleConst ) )
                        {
                            stepValues.Add( sqrt_5 + S[ i + 1, j + 2 ] );
                        }

                        if ( ( i <= m_M - 3 && j <= m_N - 2 ) && !MathUtils.NearlyZero( S[ i + 2, j + 1 ] ) && !MathUtils.NearlyEqual( S[ i + 2, j + 1 ], m_obstacleConst ) )
                        {
                            stepValues.Add( sqrt_5 + S[ i + 2, j + 1 ] );
                        }
                    }

                    // ( i+1, j-2 ) ==> ( i+1, j-1 )
                    //                       ||
                    //                  ( i+2, j-1 )
                    if ( ( i <= m_M - 2 && j >= 1 ) && !MathUtils.NearlyEqual( S[ i + 1, j - 1 ], m_obstacleConst ) )
                    {
                        if ( ( i <= m_M - 2 && j >= 1 ) && ( !MathUtils.NearlyZero( S[ i + 1, j - 1 ] ) ) )
                        {
                            stepValues.Add( sqrt_2 + S[ i + 1, j - 1 ] );
                        }

                        if ( ( i <= m_M - 2 && j >= 2 ) && !MathUtils.NearlyZero( S[ i + 1, j - 2 ] ) && !MathUtils.NearlyEqual( S[ i + 1, j - 2 ], m_obstacleConst ) )
                        {
                            stepValues.Add( sqrt_5 + S[ i + 1, j - 2 ] );
                        }

                        if ( ( i <= m_M - 3 && j >= 1 ) && !MathUtils.NearlyZero( S[ i + 2, j - 1 ] ) && !MathUtils.NearlyEqual( S[ i + 2, j - 1 ], m_obstacleConst ) )
                        {
                            stepValues.Add( sqrt_5 + S[ i + 2, j - 1 ] );
                        }
                    }

                    // ( i-1, j )
                    if ( ( i >= 1 ) && !MathUtils.NearlyZero( S[ i - 1, j ] ) && !MathUtils.NearlyEqual( S[ i - 1, j ], m_obstacleConst ) )
                    {
                        stepValues.Add( 1 + S[ i - 1, j ] );
                    }

                    // ( i, j+1 )
                    if ( ( j <= m_N - 2 ) && !MathUtils.NearlyZero( S[ i, j + 1 ] ) && !MathUtils.NearlyEqual( S[ i, j + 1 ], m_obstacleConst ) )
                    {
                        stepValues.Add( 1 + S[ i, j + 1 ] );
                    }

                    // ( i+1, j )
                    if ( ( i <= m_M - 2 ) && !MathUtils.NearlyZero( S[ i + 1, j ] ) && !MathUtils.NearlyEqual( S[ i + 1, j ], m_obstacleConst ) )
                    {
                        stepValues.Add( 1 + S[ i + 1, j ] );
                    }

                    // ( i, j-1 )
                    if ( ( j >= 1 ) && !MathUtils.NearlyZero( S[ i, j - 1 ] ) && !MathUtils.NearlyEqual( S[ i, j - 1 ], m_obstacleConst ) )
                    {
                        stepValues.Add( 1 + S[ i, j - 1 ] );
                    }

                    // Assign to (i,j) cell the minimum distance value
                    if ( stepValues.Count >= 1 )
                    {
                        double minStep = stepValues.Min<double>();
                        System.Diagnostics.Debug.Assert( minStep > 0 );
                        System.Diagnostics.Debug.Assert( stepValues.Count <= 16 );

                        S[ i, j ] = minStep;
                        emptyCellCount--;
                    }
                }
            }
        }

        public double[ , ] CalcDistanceField()
        {
            List<int[ , ]> G_exits = new List<int[ , ]>();
            List<double[ , ]> S_exits = new List<double[ , ]>();

            // Eval distance field for every exit (others are considered as closed)
            for ( int k = 1; k <= m_building.CurrentFloor.Exits.Count(); ++k )
            {
                m_G = CalcGField( k );

                int emptyCellCount;
                m_S = InitSField( out emptyCellCount );

                // FIXME: in case of no exit this loop will hang!
                //        We should handle this situation properly
                while ( emptyCellCount >= 1 )
                {
                    CalcSField( m_S, ref emptyCellCount );
                }

                G_exits.Add( ( int[ , ] )m_G.Clone() );
                S_exits.Add( ( double[ , ] )( m_S ) );
            }

            // Eval final distance field as minimum of previously evaluated ones
            for ( int i = 0; i < m_M; ++i )
            {
                for ( int j = 0; j < m_N; ++j )
                {
                    double minValue = double.PositiveInfinity;
                    for ( int k = 0; k < m_building.CurrentFloor.Exits.Count(); ++k )
                    {
                        if ( S_exits[ k ][ i, j ] < minValue ) minValue = S_exits[ k ][ i, j ];
                    }

                    m_S[ i, j ] = minValue;
                }
            }

#if DEBUG
            for ( int i = 0; i < m_M; ++i )
            {
                for ( int j = 0; j < m_N; ++j )
                {
                    if ( MathUtils.NearlyZero( m_S[ i, j ] ) )
                    {
                        System.Diagnostics.Debugger.Break();
                    }
                }
            }
#endif

            return m_S;
        }

        public double Get( float x, float y )
        {
            int i = ( int )( ( x - m_x0 ) / m_a );
            int j = ( int )( ( y - m_y0 ) / m_a );
            return m_S[ i, j ];
        }

        public double Get( double x, double y )
        {
            int i = ( int )( ( x - m_x0 ) / m_a );
            int j = ( int )( ( y - m_y0 ) / m_a );
            return m_S[ i, j ];
        }

        public void SetDrawMode( DrawMode dm )
        {
            m_drawMode = dm;
        }

        public void Visualize( Graphics g )
        {
            // Field visualization
            double maxDistValue = double.NegativeInfinity;
            double minDistValue = double.PositiveInfinity;
            for ( int i = 0; i < m_M; ++i )
            {
                for ( int j = 0; j < m_N; ++j )
                {
                    if ( m_S[ i, j ] >= m_M * m_N ) continue;

                    if ( maxDistValue < m_S[ i, j ] ) maxDistValue = m_S[ i, j ];
                    if ( minDistValue > m_S[ i, j ] ) minDistValue = m_S[ i, j ];
                }
            }

            for ( int i = 0; i < m_M; ++i )
            {
                for ( int j = 0; j < m_N; ++j )
                {
                    float cellX = m_x0 + m_a * i;
                    float cellY = m_y0 + m_a * j;

                    switch ( m_drawMode )
                    {
                        case DrawMode.Raw:
                        {
                            using ( var p = new Pen( Color.LightSteelBlue, 0.5f / g.DpiX ) )
                            {
                                g.DrawRectangle( p, cellX, cellY, m_a, m_a );
                            }
                            break;
                        }
                        case DrawMode.G:
                        {
                            if ( m_G[ i, j ] == 0 )
                            {
                                using ( var p = new Pen( Color.Red, 1.0f / g.DpiX ) )
                                {
                                    g.DrawRectangle( p, cellX, cellY, m_a, m_a );
                                }
                            }
                            else if ( m_G[ i, j ] > 0 ) // exit
                            {
                                using ( var p = new Pen( Color.Blue, 1.0f / g.DpiX ) )
                                {
                                    g.DrawRectangle( p, cellX, cellY, m_a, m_a );
                                }
                            }
                            break;
                        }
                        case DrawMode.S:
                        {
                            if ( m_S[ i, j ] >= m_M * m_N ) continue;

                            float coeff = 1 - ( float )( m_S[ i, j ] / maxDistValue );
                            Color c1 = Color.Lime;
                            Color t = Color.FromArgb( c1.A, ( int )( c1.R * coeff ), ( int )( c1.G * coeff ), ( int )( c1.B * coeff ) );

                            using ( var b = new SolidBrush( t ) )
                            {
                                g.FillRectangle( b, cellX, cellY, m_a, m_a );
                            }
                            break;
                        }
                    }
                }
            }
        }
    }
}