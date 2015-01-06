/* MathTypes.cs - useful mathematical objects
 * 
 * Copyright (C) 2014 Alexander Kamyshnikov
 *
 * This file is part of CrowdSimulator.
 *
 * CrowdSimulator is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as
 * published by the Free Software Foundation; either version 2.1 of
 * the License, or (at your option) any later version.
 *
 * CrowdSimulator is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public
 * License along with this program; if not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Generic;
using System.Drawing;

namespace SigmaDC.Common.Math
{

    public static class MathUtils
    {
        public static bool NearlyEqual( float a, float b, float epsilon = 0.001f )
        {
            float diff = System.Math.Abs( a - b );
            return ( diff < epsilon );
        }

        public static bool NearlyEqual( double a, double b, double epsilon = 0.001 )
        {
            double diff = System.Math.Abs( a - b );
            return ( diff < epsilon );
        }

        public static bool NearlyZero( float x, float epsilon = 0.001f )
        {
            return NearlyEqual( x, 0.0f, epsilon );
        }

        public static bool NearlyZero( double x, double epsilon = 0.001 )
        {
            return NearlyEqual( x, 0.0f, epsilon );
        }
    }

    public static class DotNetExtensions
    {
        /*public static double Min( this List<double> collection  )
        {
            double minValue = double.PositiveInfinity;
            foreach( var aValue in collection)
            {
                if ( aValue < minValue ) minValue = aValue;
            }
            return minValue;
        }*/
    }

    public class PointD
    {
        double x = double.NaN;
        double y = double.NaN;

        public PointD()
        {
        }

        public PointD( double x, double y )
        {
            this.x = x;
            this.y = y;
        }

        public override string ToString()
        {
            return "( X=" + this.x + ", Y=" + this.y + " )";
        }

        public override int GetHashCode()
        {
            return ( this.x.GetHashCode() + 2 ) ^ ( this.y.GetHashCode() + 2 );
        }

        public override bool Equals( object obj )
        {
            if ( !( obj is PointD ) ) return false;
            PointD other = obj as PointD;
            return ( this == other );
        }

        public static bool operator ==( PointD p1, PointD p2 )
        {
            return ( p1.x == p2.x ) && ( p1.y == p2.y );
        }

        public static bool operator !=( PointD p1, PointD p2 )
        {
            return !( p1 == p2 );
        }
    }

    public class Point3F
    {
        float m_X = float.NaN;
        float m_Y = float.NaN;
        float m_Z = float.NaN;

        public Point3F( float X, float Y, float Z = 0 )
        {
            m_X = X;
            m_Y = Y;
            m_Z = Z;
        }

        public bool IsNull
        {
            get { return ( float.IsNaN( m_X ) || float.IsNaN( m_Y ) || float.IsNaN( m_Z ) ); }
        }

        public float X
        {
            get { return m_X; }
        }

        public float Y
        {
            get { return m_Y; }
        }

        public float Z
        {
            get { return m_Z; }
        }

        public override string ToString()
        {
            if ( IsNull )
                return "<Invalid>";

            string pntString = "{ ";
            pntString += X.ToString( "F3" );
            pntString += "; ";
            pntString += Y.ToString( "F3" );
            pntString += "; ";
            pntString += Z.ToString( "F3" );
            pntString += " }";
            return pntString;
        }
    }
}