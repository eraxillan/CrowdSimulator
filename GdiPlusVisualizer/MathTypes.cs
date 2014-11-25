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

public static class MathUtils
{
    public static bool NearlyEqual( float a, float b, float epsilon = 0.001f )
    {
        float diff = Math.Abs( a - b );
        return( diff < epsilon );
    }

    public static bool NearlyZero(float x, float epsilon=0.001f)
    {
        return NearlyEqual( x, 0.0f, epsilon );
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