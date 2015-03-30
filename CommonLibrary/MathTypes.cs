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
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace SigmaDC.Common.MathEx
{
    public static class MathUtils
    {
        /// <summary>
        /// Represents the mathematical constant e(2.71828175).
        /// </summary>
        public const float E = ( float )Math.E;

        /// <summary>
        /// Represents the log base ten of e(0.4342945).
        /// </summary>
        public const float Log10E = 0.4342945f;

        /// <summary>
        /// Represents the log base two of e(1.442695).
        /// </summary>
        public const float Log2E = 1.442695f;

        /// <summary>
        /// Represents the value of pi(3.14159274).
        /// </summary>
        public const float Pi = ( float )Math.PI;

        /// <summary>
        /// Represents the value of pi divided by two(1.57079637).
        /// </summary>
        public const float PiOver2 = ( float )( Math.PI / 2.0 );

        /// <summary>
        /// Represents the value of pi divided by four(0.7853982).
        /// </summary>
        public const float PiOver4 = ( float )( Math.PI / 4.0 );

        /// <summary>
        /// Represents the value of pi times two(6.28318548).
        /// </summary>
        public const float TwoPi = ( float )( Math.PI * 2.0 );

        /// <summary>
        /// Converts radians to degrees.
        /// </summary>
        /// <param name="radians">The angle in radians.</param>
        /// <returns>The angle in degrees.</returns>
        /// <remarks>
        /// This method uses double precission internally,
        /// though it returns single float
        /// Factor = 180 / pi
        /// </remarks>
        public static float ToDegrees( float radians )
        {
            return ( float )( radians * 57.295779513082320876798154814105 );
        }

        /// <summary>
        /// Converts degrees to radians.
        /// </summary>
        /// <param name="degrees">The angle in degrees.</param>
        /// <returns>The angle in radians.</returns>
        /// <remarks>
        /// This method uses double precission internally,
        /// though it returns single float
        /// Factor = pi / 180
        /// </remarks>
        public static float ToRadians( float degrees )
        {
            return ( float )( degrees * 0.017453292519943295769236907684886 );
        }

        /// <summary>
        /// Reduces a given angle to a value between π and -π.
        /// </summary>
        /// <param name="angle">The angle to reduce, in radians.</param>
        /// <returns>The new angle, in radians.</returns>
        public static float WrapAngle( float angle )
        {
            angle = ( float )Math.IEEERemainder( ( double )angle, 6.2831854820251465 );
            if ( angle <= -3.14159274f )
            {
                angle += 6.28318548f;
            }
            else
            {
                if ( angle > 3.14159274f )
                {
                    angle -= 6.28318548f;
                }
            }
            return angle;
        }

        public static float Sqr( float x )
        {
            return x * x;
        }

        public static float MinVec( params float[] vec )
        {
            if ( vec.Length == 0 ) return float.NaN;

            float minValue = vec[ 0 ];
            for ( int i = 1; i < vec.Length; ++i )
            {
                if ( vec[ i ] < minValue ) minValue = vec[ i ];
            }
            return minValue;
        }

        /// <summary>
        /// Determines if value is powered by two.
        /// </summary>
        /// <param name="value">A value.</param>
        /// <returns><c>true</c> if <c>value</c> is powered by two; otherwise <c>false</c>.</returns>
        public static bool IsPowerOfTwo( int value )
        {
            return ( value > 0 ) && ( ( value & ( value - 1 ) ) == 0 );
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static bool NearlyEqual( float a, float b, float epsilon = 0.001f )
        {
            if ( epsilon <= 0.0f ) throw new ArgumentException();

            float diff = ( a - b >= 0 ) ? a - b : b - a;
            return ( diff < epsilon );
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static bool NearlyEqual( double a, double b, double epsilon = 0.001 )
        {
            if ( epsilon <= 0.0f ) throw new ArgumentException();

            double diff = ( a - b >= 0 ) ? a - b : b - a;
            return ( diff < epsilon );
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static bool NearlyZero( float x, float epsilon = 0.001f )
        {
            if ( epsilon <= 0.0f ) throw new ArgumentException();

            float diff = ( x >= 0 ) ? x : -x;
            return ( diff < epsilon );
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static bool NearlyZero( double x, double epsilon = 0.001 )
        {
            if ( epsilon <= 0.0f ) throw new ArgumentException();

            double diff = ( x >= 0 ) ? x : -x;
            return ( diff < epsilon );
        }
    }

    /*public static class DotNetExtensions
    {
        public static double Min( this List<double> collection  )
        {
            double minValue = double.PositiveInfinity;
            foreach( var aValue in collection)
            {
                if ( aValue < minValue ) minValue = aValue;
            }
            return minValue;
        }
    }*/

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

        public double X
        {
            get { return this.x; }
        }

        public double Y
        {
            get { return this.y; }
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

    /// <summary>
    /// Describes a 2D-vector.
    /// </summary>
    [System.Diagnostics.DebuggerDisplay( "{DebugDisplayString,nq}" )]
    public struct Vector2 : IEquatable<Vector2>
    {
        #region Private Fields

        private static Vector2 zeroVector = new Vector2( 0f, 0f );
        private static Vector2 unitVector = new Vector2( 1f, 1f );
        private static Vector2 unitXVector = new Vector2( 1f, 0f );
        private static Vector2 unitYVector = new Vector2( 0f, 1f );

        #endregion

        #region Public Fields

        /// <summary>
        /// The x coordinate of this <see cref="Vector2"/>.
        /// </summary>
        public float X;

        /// <summary>
        /// The y coordinate of this <see cref="Vector2"/>.
        /// </summary>
        public float Y;

        #endregion

        #region Properties

        /// <summary>
        /// Returns a <see cref="Vector2"/> with components 0, 0.
        /// </summary>
        public static Vector2 Zero
        {
            get { return zeroVector; }
        }

        /// <summary>
        /// Returns a <see cref="Vector2"/> with components 1, 1.
        /// </summary>
        public static Vector2 One
        {
            get { return unitVector; }
        }

        /// <summary>
        /// Returns a <see cref="Vector2"/> with components 1, 0.
        /// </summary>
        public static Vector2 UnitX
        {
            get { return unitXVector; }
        }

        /// <summary>
        /// Returns a <see cref="Vector2"/> with components 0, 1.
        /// </summary>
        public static Vector2 UnitY
        {
            get { return unitYVector; }
        }

        #endregion

        #region Internal Properties

        internal string DebugDisplayString
        {
            get
            {
                return string.Concat(
                    this.X.ToString(), "  ",
                    this.Y.ToString()
                );
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of <see cref="Vector2"/> struct, with the specified position.
        /// </summary>
        /// <param name="x">The x coordinate in 2d-space.</param>
        /// <param name="y">The y coordinate in 2d-space.</param>
        public Vector2( float x, float y )
        {
            this.X = x;
            this.Y = y;
        }

        /// <summary>
        /// Creates a new instance of <see cref="Vector2"/> struct, with the specified position.
        /// </summary>
        /// <param name="value">The x and y coordinates in 2d-space.</param>
        public Vector2( float value )
        {
            this.X = value;
            this.Y = value;
        }

        #endregion

        #region Operators

        /// <summary>
        /// Inverts values in the specified <see cref="Vector2"/>.
        /// </summary>
        /// <param name="value">Source <see cref="Vector2"/> on the right of the sub sign.</param>
        /// <returns>Result of the inversion.</returns>
        public static Vector2 operator -( Vector2 value )
        {
            value.X = -value.X;
            value.Y = -value.Y;
            return value;
        }

        /// <summary>
        /// Adds two vectors.
        /// </summary>
        /// <param name="value1">Source <see cref="Vector2"/> on the left of the add sign.</param>
        /// <param name="value2">Source <see cref="Vector2"/> on the right of the add sign.</param>
        /// <returns>Sum of the vectors.</returns>
        public static Vector2 operator +( Vector2 value1, Vector2 value2 )
        {
            value1.X += value2.X;
            value1.Y += value2.Y;
            return value1;
        }

        /// <summary>
        /// Subtracts a <see cref="Vector2"/> from a <see cref="Vector2"/>.
        /// </summary>
        /// <param name="value1">Source <see cref="Vector2"/> on the left of the sub sign.</param>
        /// <param name="value2">Source <see cref="Vector2"/> on the right of the sub sign.</param>
        /// <returns>Result of the vector subtraction.</returns>
        public static Vector2 operator -( Vector2 value1, Vector2 value2 )
        {
            value1.X -= value2.X;
            value1.Y -= value2.Y;
            return value1;
        }

        /// <summary>
        /// Multiplies the components of two vectors by each other.
        /// </summary>
        /// <param name="value1">Source <see cref="Vector2"/> on the left of the mul sign.</param>
        /// <param name="value2">Source <see cref="Vector2"/> on the right of the mul sign.</param>
        /// <returns>Result of the vector multiplication.</returns>
        public static Vector2 operator *( Vector2 value1, Vector2 value2 )
        {
            value1.X *= value2.X;
            value1.Y *= value2.Y;
            return value1;
        }

        /// <summary>
        /// Multiplies the components of vector by a scalar.
        /// </summary>
        /// <param name="value">Source <see cref="Vector2"/> on the left of the mul sign.</param>
        /// <param name="scaleFactor">Scalar value on the right of the mul sign.</param>
        /// <returns>Result of the vector multiplication with a scalar.</returns>
        public static Vector2 operator *( Vector2 value, float scaleFactor )
        {
            value.X *= scaleFactor;
            value.Y *= scaleFactor;
            return value;
        }

        /// <summary>
        /// Multiplies the components of vector by a scalar.
        /// </summary>
        /// <param name="scaleFactor">Scalar value on the left of the mul sign.</param>
        /// <param name="value">Source <see cref="Vector2"/> on the right of the mul sign.</param>
        /// <returns>Result of the vector multiplication with a scalar.</returns>
        public static Vector2 operator *( float scaleFactor, Vector2 value )
        {
            value.X *= scaleFactor;
            value.Y *= scaleFactor;
            return value;
        }

        /// <summary>
        /// Divides the components of a <see cref="Vector2"/> by the components of another <see cref="Vector2"/>.
        /// </summary>
        /// <param name="value1">Source <see cref="Vector2"/> on the left of the div sign.</param>
        /// <param name="value2">Divisor <see cref="Vector2"/> on the right of the div sign.</param>
        /// <returns>The result of dividing the vectors.</returns>
        public static Vector2 operator /( Vector2 value1, Vector2 value2 )
        {
            value1.X /= value2.X;
            value1.Y /= value2.Y;
            return value1;
        }

        /// <summary>
        /// Divides the components of a <see cref="Vector2"/> by a scalar.
        /// </summary>
        /// <param name="value1">Source <see cref="Vector2"/> on the left of the div sign.</param>
        /// <param name="divider">Divisor scalar on the right of the div sign.</param>
        /// <returns>The result of dividing a vector by a scalar.</returns>
        public static Vector2 operator /( Vector2 value1, float divider )
        {
            float factor = 1 / divider;
            value1.X *= factor;
            value1.Y *= factor;
            return value1;
        }

        /// <summary>
        /// Compares whether two <see cref="Vector2"/> instances are equal.
        /// </summary>
        /// <param name="value1"><see cref="Vector2"/> instance on the left of the equal sign.</param>
        /// <param name="value2"><see cref="Vector2"/> instance on the right of the equal sign.</param>
        /// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
        public static bool operator ==( Vector2 value1, Vector2 value2 )
        {
            return value1.X == value2.X && value1.Y == value2.Y;
        }

        /// <summary>
        /// Compares whether two <see cref="Vector2"/> instances are not equal.
        /// </summary>
        /// <param name="value1"><see cref="Vector2"/> instance on the left of the not equal sign.</param>
        /// <param name="value2"><see cref="Vector2"/> instance on the right of the not equal sign.</param>
        /// <returns><c>true</c> if the instances are not equal; <c>false</c> otherwise.</returns>	
        public static bool operator !=( Vector2 value1, Vector2 value2 )
        {
            return value1.X != value2.X || value1.Y != value2.Y;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Performs vector addition on <paramref name="value1"/> and <paramref name="value2"/>.
        /// </summary>
        /// <param name="value1">The first vector to add.</param>
        /// <param name="value2">The second vector to add.</param>
        /// <returns>The result of the vector addition.</returns>
        public static Vector2 Add( Vector2 value1, Vector2 value2 )
        {
            value1.X += value2.X;
            value1.Y += value2.Y;
            return value1;
        }

        /// <summary>
        /// Performs vector addition on <paramref name="value1"/> and
        /// <paramref name="value2"/>, storing the result of the
        /// addition in <paramref name="result"/>.
        /// </summary>
        /// <param name="value1">The first vector to add.</param>
        /// <param name="value2">The second vector to add.</param>
        /// <param name="result">The result of the vector addition.</param>
        public static void Add( ref Vector2 value1, ref Vector2 value2, out Vector2 result )
        {
            result.X = value1.X + value2.X;
            result.Y = value1.Y + value2.Y;
        }

        /// <summary>
        /// Returns the distance between two vectors.
        /// </summary>
        /// <param name="value1">The first <see cref="Vector2"/>.</param>
        /// <param name="value2">The second <see cref="Vector2"/>.</param>
        /// <returns>The distance between two vectors.</returns>
        public static float Distance( Vector2 value1, Vector2 value2 )
        {
            float v1 = value1.X - value2.X, v2 = value1.Y - value2.Y;
            return ( float )Math.Sqrt( ( v1 * v1 ) + ( v2 * v2 ) );
        }

        /// <summary>
        /// Returns the distance between two vectors.
        /// </summary>
        /// <param name="value1">The first <see cref="Vector2"/>.</param>
        /// <param name="value2">The second <see cref="Vector2"/>.</param>
        /// <param name="result">The distance between two vectors as an output parameter.</param>
        public static void Distance( ref Vector2 value1, ref Vector2 value2, out float result )
        {
            float v1 = value1.X - value2.X, v2 = value1.Y - value2.Y;
            result = ( float )Math.Sqrt( ( v1 * v1 ) + ( v2 * v2 ) );
        }

        /// <summary>
        /// Returns the squared distance between two vectors.
        /// </summary>
        /// <param name="value1">The first <see cref="Vector2"/>.</param>
        /// <param name="value2">The second <see cref="Vector2"/>.</param>
        /// <returns>The squared distance between two vectors.</returns>
        public static float DistanceSquared( Vector2 value1, Vector2 value2 )
        {
            float v1 = value1.X - value2.X, v2 = value1.Y - value2.Y;
            return ( v1 * v1 ) + ( v2 * v2 );
        }

        /// <summary>
        /// Returns the squared distance between two vectors.
        /// </summary>
        /// <param name="value1">The first <see cref="Vector2"/>.</param>
        /// <param name="value2">The second <see cref="Vector2"/>.</param>
        /// <param name="result">The squared distance between two vectors as an output parameter.</param>
        public static void DistanceSquared( ref Vector2 value1, ref Vector2 value2, out float result )
        {
            float v1 = value1.X - value2.X, v2 = value1.Y - value2.Y;
            result = ( v1 * v1 ) + ( v2 * v2 );
        }

        /// <summary>
        /// Divides the components of a <see cref="Vector2"/> by the components of another <see cref="Vector2"/>.
        /// </summary>
        /// <param name="value1">Source <see cref="Vector2"/>.</param>
        /// <param name="value2">Divisor <see cref="Vector2"/>.</param>
        /// <returns>The result of dividing the vectors.</returns>
        public static Vector2 Divide( Vector2 value1, Vector2 value2 )
        {
            value1.X /= value2.X;
            value1.Y /= value2.Y;
            return value1;
        }

        /// <summary>
        /// Divides the components of a <see cref="Vector2"/> by the components of another <see cref="Vector2"/>.
        /// </summary>
        /// <param name="value1">Source <see cref="Vector2"/>.</param>
        /// <param name="value2">Divisor <see cref="Vector2"/>.</param>
        /// <param name="result">The result of dividing the vectors as an output parameter.</param>
        public static void Divide( ref Vector2 value1, ref Vector2 value2, out Vector2 result )
        {
            result.X = value1.X / value2.X;
            result.Y = value1.Y / value2.Y;
        }

        /// <summary>
        /// Divides the components of a <see cref="Vector2"/> by a scalar.
        /// </summary>
        /// <param name="value1">Source <see cref="Vector2"/>.</param>
        /// <param name="divider">Divisor scalar.</param>
        /// <returns>The result of dividing a vector by a scalar.</returns>
        public static Vector2 Divide( Vector2 value1, float divider )
        {
            float factor = 1 / divider;
            value1.X *= factor;
            value1.Y *= factor;
            return value1;
        }

        /// <summary>
        /// Divides the components of a <see cref="Vector2"/> by a scalar.
        /// </summary>
        /// <param name="value1">Source <see cref="Vector2"/>.</param>
        /// <param name="divider">Divisor scalar.</param>
        /// <param name="result">The result of dividing a vector by a scalar as an output parameter.</param>
        public static void Divide( ref Vector2 value1, float divider, out Vector2 result )
        {
            float factor = 1 / divider;
            result.X = value1.X * factor;
            result.Y = value1.Y * factor;
        }

        /// <summary>
        /// Returns a dot product of two vectors.
        /// </summary>
        /// <param name="value1">The first <see cref="Vector2"/>.</param>
        /// <param name="value2">The second <see cref="Vector2"/>.</param>
        /// <returns>The dot product of two vectors.</returns>
        public static float Dot( Vector2 value1, Vector2 value2 )
        {
            return ( value1.X * value2.X ) + ( value1.Y * value2.Y );
        }

        /// <summary>
        /// Returns a dot product of two vectors.
        /// </summary>
        /// <param name="value1">The first <see cref="Vector2"/>.</param>
        /// <param name="value2">The second <see cref="Vector2"/>.</param>
        /// <param name="result">The dot product of two vectors as an output parameter.</param>
        public static void Dot( ref Vector2 value1, ref Vector2 value2, out float result )
        {
            result = ( value1.X * value2.X ) + ( value1.Y * value2.Y );
        }

        /// <summary>
        /// Compares whether current instance is equal to specified <see cref="Object"/>.
        /// </summary>
        /// <param name="obj">The <see cref="Object"/> to compare.</param>
        /// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
        public override bool Equals( object obj )
        {
            if ( obj is Vector2 )
            {
                return Equals( ( Vector2 )obj );
            }

            return false;
        }

        /// <summary>
        /// Compares whether current instance is equal to specified <see cref="Vector2"/>.
        /// </summary>
        /// <param name="other">The <see cref="Vector2"/> to compare.</param>
        /// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
        public bool Equals( Vector2 other )
        {
            return ( X == other.X ) && ( Y == other.Y );
        }

        /// <summary>
        /// Gets the hash code of this <see cref="Vector2"/>.
        /// </summary>
        /// <returns>Hash code of this <see cref="Vector2"/>.</returns>
        public override int GetHashCode()
        {
            return X.GetHashCode() + Y.GetHashCode();
        }

        /// <summary>
        /// Returns the length of this <see cref="Vector2"/>.
        /// </summary>
        /// <returns>The length of this <see cref="Vector2"/>.</returns>
        public float Length()
        {
            return ( float )Math.Sqrt( ( X * X ) + ( Y * Y ) );
        }

        /// <summary>
        /// Returns the squared length of this <see cref="Vector2"/>.
        /// </summary>
        /// <returns>The squared length of this <see cref="Vector2"/>.</returns>
        public float LengthSquared()
        {
            return ( X * X ) + ( Y * Y );
        }

        /// <summary>
        /// Creates a new <see cref="Vector2"/> that contains a maximal values from the two vectors.
        /// </summary>
        /// <param name="value1">The first <see cref="Vector2"/>.</param>
        /// <param name="value2">The second <see cref="Vector2"/>.</param>
        /// <returns>The <see cref="Vector2"/> with maximal values from the two vectors.</returns>
        public static Vector2 Max( Vector2 value1, Vector2 value2 )
        {
            return new Vector2( value1.X > value2.X ? value1.X : value2.X,
                               value1.Y > value2.Y ? value1.Y : value2.Y );
        }

        /// <summary>
        /// Creates a new <see cref="Vector2"/> that contains a maximal values from the two vectors.
        /// </summary>
        /// <param name="value1">The first <see cref="Vector2"/>.</param>
        /// <param name="value2">The second <see cref="Vector2"/>.</param>
        /// <param name="result">The <see cref="Vector2"/> with maximal values from the two vectors as an output parameter.</param>
        public static void Max( ref Vector2 value1, ref Vector2 value2, out Vector2 result )
        {
            result.X = value1.X > value2.X ? value1.X : value2.X;
            result.Y = value1.Y > value2.Y ? value1.Y : value2.Y;
        }

        /// <summary>
        /// Creates a new <see cref="Vector2"/> that contains a minimal values from the two vectors.
        /// </summary>
        /// <param name="value1">The first <see cref="Vector2"/>.</param>
        /// <param name="value2">The second <see cref="Vector2"/>.</param>
        /// <returns>The <see cref="Vector2"/> with minimal values from the two vectors.</returns>
        public static Vector2 Min( Vector2 value1, Vector2 value2 )
        {
            return new Vector2( value1.X < value2.X ? value1.X : value2.X,
                               value1.Y < value2.Y ? value1.Y : value2.Y );
        }

        /// <summary>
        /// Creates a new <see cref="Vector2"/> that contains a minimal values from the two vectors.
        /// </summary>
        /// <param name="value1">The first <see cref="Vector2"/>.</param>
        /// <param name="value2">The second <see cref="Vector2"/>.</param>
        /// <param name="result">The <see cref="Vector2"/> with minimal values from the two vectors as an output parameter.</param>
        public static void Min( ref Vector2 value1, ref Vector2 value2, out Vector2 result )
        {
            result.X = value1.X < value2.X ? value1.X : value2.X;
            result.Y = value1.Y < value2.Y ? value1.Y : value2.Y;
        }

        /// <summary>
        /// Creates a new <see cref="Vector2"/> that contains a multiplication of two vectors.
        /// </summary>
        /// <param name="value1">Source <see cref="Vector2"/>.</param>
        /// <param name="value2">Source <see cref="Vector2"/>.</param>
        /// <returns>Result of the vector multiplication.</returns>
        public static Vector2 Multiply( Vector2 value1, Vector2 value2 )
        {
            value1.X *= value2.X;
            value1.Y *= value2.Y;
            return value1;
        }

        /// <summary>
        /// Creates a new <see cref="Vector2"/> that contains a multiplication of two vectors.
        /// </summary>
        /// <param name="value1">Source <see cref="Vector2"/>.</param>
        /// <param name="value2">Source <see cref="Vector2"/>.</param>
        /// <param name="result">Result of the vector multiplication as an output parameter.</param>
        public static void Multiply( ref Vector2 value1, ref Vector2 value2, out Vector2 result )
        {
            result.X = value1.X * value2.X;
            result.Y = value1.Y * value2.Y;
        }

        /// <summary>
        /// Creates a new <see cref="Vector2"/> that contains a multiplication of <see cref="Vector2"/> and a scalar.
        /// </summary>
        /// <param name="value1">Source <see cref="Vector2"/>.</param>
        /// <param name="scaleFactor">Scalar value.</param>
        /// <returns>Result of the vector multiplication with a scalar.</returns>
        public static Vector2 Multiply( Vector2 value1, float scaleFactor )
        {
            value1.X *= scaleFactor;
            value1.Y *= scaleFactor;
            return value1;
        }

        /// <summary>
        /// Creates a new <see cref="Vector2"/> that contains a multiplication of <see cref="Vector2"/> and a scalar.
        /// </summary>
        /// <param name="value1">Source <see cref="Vector2"/>.</param>
        /// <param name="scaleFactor">Scalar value.</param>
        /// <param name="result">Result of the multiplication with a scalar as an output parameter.</param>
        public static void Multiply( ref Vector2 value1, float scaleFactor, out Vector2 result )
        {
            result.X = value1.X * scaleFactor;
            result.Y = value1.Y * scaleFactor;
        }

        /// <summary>
        /// Creates a new <see cref="Vector2"/> that contains the specified vector inversion.
        /// </summary>
        /// <param name="value">Source <see cref="Vector2"/>.</param>
        /// <returns>Result of the vector inversion.</returns>
        public static Vector2 Negate( Vector2 value )
        {
            value.X = -value.X;
            value.Y = -value.Y;
            return value;
        }

        /// <summary>
        /// Creates a new <see cref="Vector2"/> that contains the specified vector inversion.
        /// </summary>
        /// <param name="value">Source <see cref="Vector2"/>.</param>
        /// <param name="result">Result of the vector inversion as an output parameter.</param>
        public static void Negate( ref Vector2 value, out Vector2 result )
        {
            result.X = -value.X;
            result.Y = -value.Y;
        }

        /// <summary>
        /// Turns this <see cref="Vector2"/> to a unit vector with the same direction.
        /// </summary>
        public void Normalize()
        {
            float val = 1.0f / ( float )Math.Sqrt( ( X * X ) + ( Y * Y ) );
            X *= val;
            Y *= val;
        }

        /// <summary>
        /// Creates a new <see cref="Vector2"/> that contains a normalized values from another vector.
        /// </summary>
        /// <param name="value">Source <see cref="Vector2"/>.</param>
        /// <returns>Unit vector.</returns>
        public static Vector2 Normalize( Vector2 value )
        {
            float val = 1.0f / ( float )Math.Sqrt( ( value.X * value.X ) + ( value.Y * value.Y ) );
            value.X *= val;
            value.Y *= val;
            return value;
        }

        /// <summary>
        /// Creates a new <see cref="Vector2"/> that contains a normalized values from another vector.
        /// </summary>
        /// <param name="value">Source <see cref="Vector2"/>.</param>
        /// <param name="result">Unit vector as an output parameter.</param>
        public static void Normalize( ref Vector2 value, out Vector2 result )
        {
            float val = 1.0f / ( float )Math.Sqrt( ( value.X * value.X ) + ( value.Y * value.Y ) );
            result.X = value.X * val;
            result.Y = value.Y * val;
        }

        /// <summary>
        /// Creates a new <see cref="Vector2"/> that contains reflect vector of the given vector and normal.
        /// </summary>
        /// <param name="vector">Source <see cref="Vector2"/>.</param>
        /// <param name="normal">Reflection normal.</param>
        /// <returns>Reflected vector.</returns>
        public static Vector2 Reflect( Vector2 vector, Vector2 normal )
        {
            Vector2 result;
            float val = 2.0f * ( ( vector.X * normal.X ) + ( vector.Y * normal.Y ) );
            result.X = vector.X - ( normal.X * val );
            result.Y = vector.Y - ( normal.Y * val );
            return result;
        }

        /// <summary>
        /// Creates a new <see cref="Vector2"/> that contains reflect vector of the given vector and normal.
        /// </summary>
        /// <param name="vector">Source <see cref="Vector2"/>.</param>
        /// <param name="normal">Reflection normal.</param>
        /// <param name="result">Reflected vector as an output parameter.</param>
        public static void Reflect( ref Vector2 vector, ref Vector2 normal, out Vector2 result )
        {
            float val = 2.0f * ( ( vector.X * normal.X ) + ( vector.Y * normal.Y ) );
            result.X = vector.X - ( normal.X * val );
            result.Y = vector.Y - ( normal.Y * val );
        }

        /// <summary>
        /// Creates a new <see cref="Vector2"/> that contains subtraction of on <see cref="Vector2"/> from a another.
        /// </summary>
        /// <param name="value1">The first <see cref="Vector2"/>.</param>
        /// <param name="value2">The second <see cref="Vector2"/>.</param>
        /// <returns>The result of the vector subtraction.</returns>
        public static Vector2 Subtract( Vector2 value1, Vector2 value2 )
        {
            value1.X -= value2.X;
            value1.Y -= value2.Y;
            return value1;
        }

        /// <summary>
        /// Creates a new <see cref="Vector2"/> that contains subtraction of on <see cref="Vector2"/> from a another.
        /// </summary>
        /// <param name="value1">The first <see cref="Vector2"/>.</param>
        /// <param name="value2">The second <see cref="Vector2"/>.</param>
        /// <param name="result">The result of the vector subtraction as an output parameter.</param>
        public static void Subtract( ref Vector2 value1, ref Vector2 value2, out Vector2 result )
        {
            result.X = value1.X - value2.X;
            result.Y = value1.Y - value2.Y;
        }

        /// <summary>
        /// Returns a <see cref="String"/> representation of this <see cref="Vector2"/> in the format:
        /// {X:[<see cref="X"/>] Y:[<see cref="Y"/>]}
        /// </summary>
        /// <returns>A <see cref="String"/> representation of this <see cref="Vector2"/>.</returns>
        public override string ToString()
        {
            return "{X:" + X + " Y:" + Y + "}";
        }

        /// <summary>
        /// Gets a <see cref="Point"/> representation for this object.
        /// </summary>
        /// <returns>A <see cref="Point"/> representation for this object.</returns>
        public Point ToPoint()
        {
            return new Point( ( int )X, ( int )Y );
        }

        /// <summary>
        /// Creates a new <see cref="Vector2"/> that contains a transformation of vector(position.X,position.Y) by the specified <see cref="Matrix"/>.
        /// </summary>
        /// <param name="position">Source <see cref="Vector2"/>.</param>
        /// <param name="matrix">The transformation <see cref="Matrix"/>.</param>
        /// <returns>Transformed <see cref="Vector2"/>.</returns>
        public static Vector2 Transform( Vector2 position, Matrix2 matrix )
        {
            return new Vector2( ( position.X * matrix.M11 ) + ( position.Y * matrix.M12 ), ( position.X * matrix.M21 ) + ( position.Y * matrix.M22 ) );
        }

        /// <summary>
        /// Creates a new <see cref="Vector2"/> that contains a transformation of vector(position.X,position.Y,0,1) by the specified <see cref="Matrix"/>.
        /// </summary>
        /// <param name="position">Source <see cref="Vector2"/>.</param>
        /// <param name="matrix">The transformation <see cref="Matrix"/>.</param>
        /// <param name="result">Transformed <see cref="Vector2"/> as an output parameter.</param>
        public static void Transform( ref Vector2 position, ref Matrix2 matrix, out Vector2 result )
        {
            var x = ( position.X * matrix.M11 ) + ( position.Y * matrix.M21 );
            var y = ( position.X * matrix.M12 ) + ( position.Y * matrix.M22 );
            result.X = x;
            result.Y = y;
        }

        public static Vector2 RotateAroundPoint( Vector2 point, Vector2 origin, float angle )
        {
            return new Vector2(
                ( float )( Math.Cos( angle ) * ( point.X - origin.X ) - Math.Sin( angle ) * ( point.Y - origin.Y ) + origin.X ),
                ( float )( Math.Sin( angle ) * ( point.X - origin.X ) + Math.Cos( angle ) * ( point.Y - origin.Y ) + origin.Y ) );
        }
        #endregion
    }

    [System.Diagnostics.DebuggerDisplay( "{DebugDisplayString,nq}" )]
    public struct Matrix2 : IEquatable<Matrix2>
    {
        #region Public Constructors

        public Matrix2( float m11, float m12, float m21, float m22 )
        {
            this.M11 = m11;
            this.M12 = m12;
            this.M21 = m21;
            this.M22 = m22;
        }

        #endregion Public Constructors

        #region Public Fields

        public float M11;
        public float M12;
        public float M21;
        public float M22;

        #endregion Public Fields

        #region Indexers

        public float this[ int index ]
        {
            get
            {
                switch ( index )
                {
                    case 0: return M11;
                    case 1: return M12;
                    case 2: return M21;
                    case 3: return M22;
                }
                throw new ArgumentOutOfRangeException();
            }

            set
            {
                switch ( index )
                {
                    case 0: M11 = value; break;
                    case 1: M12 = value; break;
                    case 2: M21 = value; break;
                    case 3: M22 = value; break;
                    default: throw new ArgumentOutOfRangeException();
                }
            }
        }

        public float this[ int row, int column ]
        {
            get
            {
                return this[ ( row * 4 ) + column ];
            }

            set
            {
                this[ ( row * 4 ) + column ] = value;
            }
        }

        #endregion

        #region Private Members
        private static Matrix2 identity = new Matrix2( 1f, 0f, 0f, 1f );
        #endregion Private Members

        #region Public Properties

        public static Matrix2 Identity
        {
            get { return identity; }
        }


        // required for OpenGL 2.0 projection matrix stuff
        // TODO: have this work correctly for 3x3 Matrices. Needs to return
        // a float[9] for a 3x3, and a float[16] for a 4x4
        public static float[] ToFloatArray( Matrix2 mat )
        {
            float[] matarray = {
									mat.M11, mat.M12,
									mat.M21, mat.M22
								};
            return matarray;
        }

        /* public Vector2 Translation
         {
             get
             {
                 return new Vector2( this.M41, this.M42, this.M43 );
             }
             set
             {
                 this.M41 = value.X;
                 this.M42 = value.Y;
                 this.M43 = value.Z;
             }
         }*/

        #endregion Public Properties

        #region Public Methods

        public static Matrix2 Add( Matrix2 matrix1, Matrix2 matrix2 )
        {
            matrix1.M11 += matrix2.M11;
            matrix1.M12 += matrix2.M12;
            matrix1.M21 += matrix2.M21;
            matrix1.M22 += matrix2.M22;
            return matrix1;
        }


        public static void Add( ref Matrix2 matrix1, ref Matrix2 matrix2, out Matrix2 result )
        {
            result.M11 = matrix1.M11 + matrix2.M11;
            result.M12 = matrix1.M12 + matrix2.M12;
            result.M21 = matrix1.M21 + matrix2.M21;
            result.M22 = matrix1.M22 + matrix2.M22;
        }

        public static Matrix2 CreateRotation( float radians )
        {
            Matrix2 result;
            CreateRotation( radians, out result );
            return result;
        }


        public static void CreateRotation( float radians, out Matrix2 result )
        {
            result = Matrix2.Identity;

            var val1 = ( float )Math.Cos( radians );
            var val2 = ( float )Math.Sin( radians );

            result.M11 = val1;      // cos
            result.M12 = -val2;     // -sin
            result.M21 = val2;      // sin
            result.M22 = val1;      // cos
        }

        public static Matrix2 CreateRotationZ( float radians )
        {
            Matrix2 result;
            CreateRotationZ( radians, out result );
            return result;
        }


        public static void CreateRotationZ( float radians, out Matrix2 result )
        {
            result = Matrix2.Identity;

            var val1 = ( float )Math.Cos( radians );
            var val2 = ( float )Math.Sin( radians );

            result.M11 = val1;
            result.M12 = val2;
            result.M21 = -val2;
            result.M22 = val1;
        }

        public static Matrix2 CreateScale( float scale )
        {
            Matrix2 result;
            CreateScale( scale, scale, out result );
            return result;
        }


        public static void CreateScale( float scale, out Matrix2 result )
        {
            CreateScale( scale, scale, out result );
        }


        public static Matrix2 CreateScale( float xScale, float yScale )
        {
            Matrix2 result;
            CreateScale( xScale, yScale, out result );
            return result;
        }


        public static void CreateScale( float xScale, float yScale, out Matrix2 result )
        {
            result.M11 = xScale;
            result.M12 = 0;
            result.M21 = 0;
            result.M22 = yScale;
        }

        public static Matrix2 CreateTranslation( float xPosition, float yPosition )
        {
            Matrix2 result;
            CreateTranslation( xPosition, yPosition, out result );
            return result;
        }


        public static void CreateTranslation( ref Vector2 position, out Matrix2 result )
        {
            result.M11 = 1;
            result.M12 = 0;
            result.M21 = 0;
            result.M22 = 1;
        }


        public static Matrix2 CreateTranslation( Vector2 position )
        {
            Matrix2 result;
            CreateTranslation( ref position, out result );
            return result;
        }


        public static void CreateTranslation( float xPosition, float yPosition, out Matrix2 result )
        {
            result.M11 = 1;
            result.M12 = 0;
            result.M21 = 0;
            result.M22 = 1;
        }
        /*
                public static Matrix2 CreateReflection( Plane value )
                {
                    Matrix2 result;
                    CreateReflection( ref value, out result );
                    return result;
                }

                public static void CreateReflection( ref Plane value, out Matrix result )
                {
                    Plane plane;
                    Plane.Normalize( ref value, out plane );
                    value.Normalize();
                    float x = plane.Normal.X;
                    float y = plane.Normal.Y;
                    float z = plane.Normal.Z;
                    float num3 = -2f * x;
                    float num2 = -2f * y;
                    float num = -2f * z;
                    result.M11 = ( num3 * x ) + 1f;
                    result.M12 = num2 * x;
                    result.M13 = num * x;
                    result.M14 = 0;
                    result.M21 = num3 * y;
                    result.M22 = ( num2 * y ) + 1;
                    result.M23 = num * y;
                    result.M24 = 0;
                    result.M31 = num3 * z;
                    result.M32 = num2 * z;
                    result.M33 = ( num * z ) + 1;
                    result.M34 = 0;
                    result.M41 = num3 * plane.D;
                    result.M42 = num2 * plane.D;
                    result.M43 = num * plane.D;
                    result.M44 = 1;
                }
                */

        public float Determinant()
        {
            return M11 * M22 - M12 * M21;
        }


        public static Matrix2 Divide( Matrix2 matrix1, Matrix2 matrix2 )
        {
            matrix1.M11 = matrix1.M11 / matrix2.M11;
            matrix1.M12 = matrix1.M12 / matrix2.M12;
            matrix1.M21 = matrix1.M21 / matrix2.M21;
            matrix1.M22 = matrix1.M22 / matrix2.M22;
            return matrix1;
        }


        public static void Divide( ref Matrix2 matrix1, ref Matrix2 matrix2, out Matrix2 result )
        {
            result.M11 = matrix1.M11 / matrix2.M11;
            result.M12 = matrix1.M12 / matrix2.M12;
            result.M21 = matrix1.M21 / matrix2.M21;
            result.M22 = matrix1.M22 / matrix2.M22;
        }


        public static Matrix2 Divide( Matrix2 matrix1, float divider )
        {
            float num = 1f / divider;
            matrix1.M11 = matrix1.M11 * num;
            matrix1.M12 = matrix1.M12 * num;
            matrix1.M21 = matrix1.M21 * num;
            matrix1.M22 = matrix1.M22 * num;
            return matrix1;
        }


        public static void Divide( ref Matrix2 matrix1, float divider, out Matrix2 result )
        {
            float num = 1f / divider;
            result.M11 = matrix1.M11 * num;
            result.M12 = matrix1.M12 * num;
            result.M21 = matrix1.M21 * num;
            result.M22 = matrix1.M22 * num;
        }


        public bool Equals( Matrix2 other )
        {
            return ( ( this.M11 == other.M11 ) && ( this.M22 == other.M22 ) && ( this.M12 == other.M12 ) && ( this.M21 == other.M21 ) );
        }


        public override bool Equals( object obj )
        {
            bool flag = false;
            if ( obj is Matrix2 )
            {
                flag = this.Equals( ( Matrix2 )obj );
            }
            return flag;
        }


        public override int GetHashCode()
        {
            return ( this.M11.GetHashCode() + this.M12.GetHashCode() + this.M21.GetHashCode() + this.M22.GetHashCode() );
        }


        public static Matrix2 Invert( Matrix2 matrix )
        {
            Invert( ref matrix, out matrix );
            return matrix;
        }


        public static void Invert( ref Matrix2 matrix, out Matrix2 result )
        {
            float det = matrix.Determinant();
            if ( det == 0.0f ) throw new InvalidOperationException( "Matrix determinant is null" );

            result.M11 = matrix.M22 / det;
            result.M12 = -matrix.M12 / det;
            result.M21 = -matrix.M21 / det;
            result.M22 = matrix.M11 / det;
        }

        public static void Multiply( ref Matrix2 matrix1, ref Matrix2 matrix2, out Matrix2 result )
        {
            result.M11 = ( matrix1.M11 * matrix2.M11 ) + ( matrix1.M12 * matrix2.M21 );
            result.M12 = ( matrix1.M11 * matrix2.M12 ) + ( matrix1.M12 * matrix2.M22 );
            result.M21 = ( matrix1.M21 * matrix2.M11 ) + ( matrix1.M22 * matrix2.M21 );
            result.M22 = ( matrix1.M21 * matrix2.M12 ) + ( matrix1.M22 * matrix2.M22 );
        }

        public static void Multiply( ref Matrix2 matrix1, float factor, out Matrix2 result )
        {
            result.M11 = matrix1.M11 * factor;
            result.M12 = matrix1.M12 * factor;
            result.M21 = matrix1.M21 * factor;
            result.M22 = matrix1.M22 * factor;
        }

        public static void Negate( ref Matrix2 matrix, out Matrix2 result )
        {
            result.M11 = -matrix.M11;
            result.M12 = -matrix.M12;
            result.M21 = -matrix.M21;
            result.M22 = -matrix.M22;
        }


        public static Matrix2 operator +( Matrix2 matrix1, Matrix2 matrix2 )
        {
            Matrix2.Add( ref matrix1, ref matrix2, out matrix1 );
            return matrix1;
        }


        public static Matrix2 operator /( Matrix2 matrix1, Matrix2 matrix2 )
        {
            matrix1.M11 = matrix1.M11 / matrix2.M11;
            matrix1.M12 = matrix1.M12 / matrix2.M12;
            matrix1.M21 = matrix1.M21 / matrix2.M21;
            matrix1.M22 = matrix1.M22 / matrix2.M22;
            return matrix1;
        }


        public static Matrix2 operator /( Matrix2 matrix, float divider )
        {
            float num = 1f / divider;
            matrix.M11 = matrix.M11 * num;
            matrix.M12 = matrix.M12 * num;
            matrix.M21 = matrix.M21 * num;
            matrix.M22 = matrix.M22 * num;
            return matrix;
        }


        public static bool operator ==( Matrix2 matrix1, Matrix2 matrix2 )
        {
            return (
                matrix1.M11 == matrix2.M11 &&
                matrix1.M12 == matrix2.M12 &&
                matrix1.M21 == matrix2.M21 &&
                matrix1.M22 == matrix2.M22
                );
        }


        public static bool operator !=( Matrix2 matrix1, Matrix2 matrix2 )
        {
            return (
                matrix1.M11 != matrix2.M11 ||
                matrix1.M12 != matrix2.M12 ||
                matrix1.M21 != matrix2.M21 ||
                matrix1.M22 != matrix2.M22
                );
        }


        public static Matrix2 operator *( Matrix2 matrix1, Matrix2 matrix2 )
        {
            matrix1.M11 = ( matrix1.M11 * matrix2.M11 ) + ( matrix1.M12 * matrix2.M21 );
            matrix1.M12 = ( matrix1.M11 * matrix2.M12 ) + ( matrix1.M12 * matrix2.M22 );
            matrix1.M21 = ( matrix1.M21 * matrix2.M11 ) + ( matrix1.M22 * matrix2.M21 );
            matrix1.M22 = ( matrix1.M21 * matrix2.M12 ) + ( matrix1.M22 * matrix2.M22 );
            return matrix1;
        }


        public static Matrix2 operator *( Matrix2 matrix, float scaleFactor )
        {
            matrix.M11 = matrix.M11 * scaleFactor;
            matrix.M12 = matrix.M12 * scaleFactor;
            matrix.M21 = matrix.M21 * scaleFactor;
            matrix.M22 = matrix.M22 * scaleFactor;
            return matrix;
        }


        public static Matrix2 operator -( Matrix2 matrix1, Matrix2 matrix2 )
        {
            matrix1.M11 = matrix1.M11 - matrix2.M11;
            matrix1.M12 = matrix1.M12 - matrix2.M12;
            matrix1.M21 = matrix1.M21 - matrix2.M21;
            matrix1.M22 = matrix1.M22 - matrix2.M22;
            return matrix1;
        }


        public static Matrix2 operator -( Matrix2 matrix )
        {
            matrix.M11 = -matrix.M11;
            matrix.M12 = -matrix.M12;
            matrix.M21 = -matrix.M21;
            matrix.M22 = -matrix.M22;
            return matrix;
        }


        public static Matrix2 Subtract( Matrix2 matrix1, Matrix2 matrix2 )
        {
            matrix1.M11 = matrix1.M11 - matrix2.M11;
            matrix1.M12 = matrix1.M12 - matrix2.M12;
            matrix1.M21 = matrix1.M21 - matrix2.M21;
            matrix1.M22 = matrix1.M22 - matrix2.M22;
            return matrix1;
        }


        public static void Subtract( ref Matrix2 matrix1, ref Matrix2 matrix2, out Matrix2 result )
        {
            result.M11 = matrix1.M11 - matrix2.M11;
            result.M12 = matrix1.M12 - matrix2.M12;
            result.M21 = matrix1.M21 - matrix2.M21;
            result.M22 = matrix1.M22 - matrix2.M22;
        }

        internal string DebugDisplayString
        {
            get
            {
                if ( this == Identity )
                {
                    return "Identity";
                }

                return string.Concat(
                     "( ", this.M11.ToString(), "  ", this.M12.ToString(), "  ", " )  \r\n",
                     "( ", this.M21.ToString(), "  ", this.M22.ToString(), "  ", " )" );
            }
        }

        public override string ToString()
        {
            return "{M11:" + M11 + " M12:" + M12 + "}"
                + " {M21:" + M21 + " M22:" + M22 + "}";
        }


        public static Matrix2 Transpose( Matrix2 matrix )
        {
            Matrix2 ret;
            Transpose( ref matrix, out ret );
            return ret;
        }


        public static void Transpose( ref Matrix2 matrix, out Matrix2 result )
        {
            Matrix2 ret;

            ret.M11 = matrix.M11;
            ret.M12 = matrix.M21;
            ret.M21 = matrix.M12;
            ret.M22 = matrix.M22;

            result = ret;
        }
        #endregion Public Methods
    }

    public struct SdcLineSegment
    {
        public Vector2 P1 { get; set; }
        public Vector2 P2 { get; set; }

/*        public SdcLineSegment()
        {
            P1 = new Vector2();
            P2 = new Vector2();
        }
        */

        // Inheritance is required, see http://stackoverflow.com/a/7670854/1794089
        public SdcLineSegment( Vector2 first, Vector2 second ) : this()
        {
            Trace.Assert( second.X >= first.X );
            Trace.Assert( second.Y >= first.Y );

            P1 = first;
            P2 = second;
        }

         // TODO: move to other common functions
        const float SMALL_NUM = 0.00000001f; // anything that avoids division overflow

        /// <summary>
        /// dot product (3D) which allows vector operations in arguments
        /// </summary>
        /// <param name="u"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        public static float dot( Vector2 u, Vector2 v )
        {
            return ( u.X * v.X + u.Y * v.Y /*+ u.Z * v.Z*/ );
        }

        /// <summary>
        /// norm = length of  vector
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static float norm( Vector2 v )
        {
            return ( float )Math.Sqrt( dot( v, v ) );
        }
    
        /// <summary>
        /// distance = norm of difference
        /// </summary>
        /// <param name="u"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        public static float d( Vector2 u, Vector2 v )
        {
            return norm( u - v );
        }

        /// <summary>
        /// Get the 2D minimum distance between 2 segments
        /// </summary>
        /// <param name="S1">2D line segment</param>
        /// <param name="S2">2D line segment</param>
        /// <returns>The shortest distance between S1 and S2</returns>
        public static float Distance2D( SdcLineSegment S1, SdcLineSegment S2 )
        {
            Vector2 u = S1.P2 - S1.P1;
            Vector2 v = S2.P2 - S2.P1;
            Vector2 w = S1.P1 - S2.P1;
            float a = dot( u, u );         // always >= 0
            float b = dot( u, v );
            float c = dot( v, v );         // always >= 0
            float d = dot( u, w );
            float e = dot( v, w );
            float D = a * c - b * b;        // always >= 0
            float sc, sN, sD = D;       // sc = sN / sD, default sD = D >= 0
            float tc, tN, tD = D;       // tc = tN / tD, default tD = D >= 0

            // compute the line parameters of the two closest points
            if ( D < SMALL_NUM )
            {
                // the lines are almost parallel
                sN = 0.0f;         // force using point P0 on segment S1
                sD = 1.0f;         // to prevent possible division by 0.0 later
                tN = e;
                tD = c;
            }
            else
            {
                // get the closest points on the infinite lines
                sN = ( b * e - c * d );
                tN = ( a * e - b * d );
                if ( sN < 0.0f )
                {        // sc < 0 => the s=0 edge is visible
                    sN = 0.0f;
                    tN = e;
                    tD = c;
                }
                else if ( sN > sD )
                {
                    // sc > 1  => the s=1 edge is visible
                    sN = sD;
                    tN = e + b;
                    tD = c;
                }
            }

            if ( tN < 0.0f )
            {
                // tc < 0 => the t=0 edge is visible
                tN = 0.0f;
                // recompute sc for this edge
                if ( -d < 0.0f )
                    sN = 0.0f;
                else if ( -d > a )
                    sN = sD;
                else
                {
                    sN = -d;
                    sD = a;
                }
            }
            else if ( tN > tD )
            {
                // tc > 1  => the t=1 edge is visible
                tN = tD;
                // recompute sc for this edge
                if ( ( -d + b ) < 0.0f )
                    sN = 0;
                else if ( ( -d + b ) > a )
                    sN = sD;
                else
                {
                    sN = ( -d + b );
                    sD = a;
                }
            }
            // finally do the division to get sc and tc
            sc = ( Math.Abs( sN ) < SMALL_NUM ? 0.0f : sN / sD );
            tc = ( Math.Abs( tN ) < SMALL_NUM ? 0.0f : tN / tD );

            // get the difference of the two closest points
            Vector2 dP = w + ( sc * u ) - ( tc * v );  // =  S1(sc) - S2(tc)

            return norm( dP );   // return the closest distance
        }
    }

    /// <summary>
    /// General rectangle, including non axis aligned ones
    /// <remarks>http://programyourfaceoff.blogspot.ru/2011/12/intersection-testing.html</remarks>
    /// </summary>
    public class SdcCircle
    {
        public SdcCircle( Vector2 aCenter, float aRadius )
        {
            Center = aCenter;
            Radius = aRadius;
        }

        public Vector2 Center { get; set; }
        public float Radius { get; set; }

        /// <summary>
        /// Returns true if this circle intersects another or they are 'just touching'.
        /// </summary>
        /// <param name="circle"></param>
        /// <returns></returns>
        public bool Intersects( SdcCircle circle )
        {
            Vector2 center1 = Center;
            Vector2 center2 = circle.Center;
            float radius1 = Radius;
            float radius2 = circle.Radius;

            float distanceSquared =
                ( float )System.Math.Pow( ( center1.X - center2.X ), 2 ) +
                ( float )System.Math.Pow( ( center1.Y - center2.Y ), 2 );
            float sumOfRadiiSquared = ( float )System.Math.Pow( radius1 + radius2, 2 );

            return ( distanceSquared <= sumOfRadiiSquared );
        }
    }

    /// <summary>
    /// <remarks>http://programyourfaceoff.blogspot.ru/2011/12/intersection-testing.html</remarks>
    /// </summary>
    public class SdcRectangle
    {
        public Vector2 Center { get; set; }

        // We store these as floats for now, we only need the vector versions during intersection testing
        #region For intersection testing only
        public float HalfWidth { get; set; }
        public float HalfHeight { get; set; }
        public float Angle { get; set; }
        #endregion

        #region Common rectangle properties
        public Vector2 LeftBottom { get; set; }
        public Vector2 LeftTop { get; set; }
        public Vector2 RightBottom { get; set; }
        public Vector2 RightTop { get; set; }
        #endregion

        #region For line segment intersection testing only
        public SdcLineSegment Bottom { get; set; }
        public SdcLineSegment Top { get; set; }
        public SdcLineSegment Left { get; set; }
        public SdcLineSegment Right { get; set; }
        #endregion

        public float Width { get { return RightTop.X - LeftBottom.X; } }
        public float Height { get { return RightTop.Y - LeftBottom.Y; } }

        public SdcRectangle()
        {
            Center = new Vector2();

            LeftBottom = new Vector2();
            LeftTop = new Vector2();
            RightBottom = new Vector2();
            RightTop = new Vector2();

            Bottom = new SdcLineSegment();
            Top = new SdcLineSegment();
            Left = new SdcLineSegment();
            Right = new SdcLineSegment();
        }

        public SdcRectangle( SdcRectangle other )
        {
            Center = other.Center;

            LeftBottom = other.LeftBottom;
            LeftTop = other.LeftTop;
            RightBottom = other.RightBottom;
            RightTop = other.RightTop;

            Bottom = other.Bottom;
            Top = other.Top;
            Left = other.Left;
            Right = other.Right;
        }

        public SdcRectangle( RectangleF otherRect )
        {
            Center = new Vector2( ( otherRect.Left + otherRect.Right ) / 2, ( otherRect.Bottom + otherRect.Top ) / 2 );
            HalfWidth = otherRect.Width / 2;
            HalfHeight = otherRect.Height / 2;
            Angle = 0.0f;

            float top = otherRect.Top, bottom = otherRect.Bottom;
            if ( otherRect.Top < otherRect.Bottom ) { top = otherRect.Bottom; bottom = otherRect.Top; }

            LeftBottom = new Vector2( otherRect.Left, bottom );
            LeftTop = new Vector2( otherRect.Left, top );
            RightBottom = new Vector2( otherRect.Right, bottom );
            RightTop = new Vector2( otherRect.Right, top );

            Bottom = new SdcLineSegment( LeftBottom, RightBottom );
            Top = new SdcLineSegment( LeftTop, RightTop );
            Left = new SdcLineSegment( LeftBottom, LeftTop );
            Right = new SdcLineSegment( RightBottom, RightTop );

            Debug.Assert( Width >= 0 && Height >= 0 );
            // TODO: other validation, like Right >= Left
        }

        /// <summary>
        /// Create visibility range rectangle from specified human and model parameters
        /// </summary>
        /// <param name="manProjCenter"></param>
        /// <param name="manDiameter"></param>
        /// <param name="visibilityRadius"></param>
        public SdcRectangle( Vector2 manProjCenter, float manDiameter, float visibilityRadius, float angle )
        {
            var manRadius = manDiameter / 2.0f;

            // 1) Find X-axis aligned rectangle
            LeftBottom = new Vector2( manProjCenter.X, manProjCenter.Y - manRadius );
            LeftTop = new Vector2( manProjCenter.X, manProjCenter.Y + manRadius );
            RightBottom = new Vector2( manProjCenter.X + visibilityRadius, manProjCenter.Y - manRadius );
            RightTop = new Vector2( manProjCenter.X + visibilityRadius, manProjCenter.Y + manRadius );

            Bottom = new SdcLineSegment( LeftBottom, RightBottom );
            Top = new SdcLineSegment( LeftTop, RightTop );
            Left = new SdcLineSegment( LeftBottom, LeftTop );
            Right = new SdcLineSegment( RightBottom, RightTop );

            // 2) Fill the intersection-specific field values
            Center = new Vector2( ( LeftBottom.X + RightTop.X ) / 2, ( LeftBottom.Y + RightTop.Y ) / 2 );
            HalfWidth = Width / 2;
            HalfHeight = Height / 2;

            // FIXME: should we rotate rect right here and do not store the angle?
//            Angle = 0.0f;
            Angle = angle;
        }

        public SdcRectangle Rotate( Vector2 origin, float angle )
        {
            var result = new SdcRectangle();

            result.LeftBottom = Vector2.RotateAroundPoint( LeftBottom, origin, angle );
            result.RightBottom = Vector2.RotateAroundPoint( RightBottom, origin, angle );
            result.RightTop = Vector2.RotateAroundPoint( RightTop, origin, angle);
            result.LeftTop = Vector2.RotateAroundPoint( LeftTop, origin, angle );

            return result;

            /*rect.LeftBottom -= pt;
            rect.RightBottom -= pt;
            rect.RightTop -= pt;
            rect.LeftTop -= pt;

            rect.LeftBottom = Vector2.Transform( rect.LeftBottom, rotMatrix );
            rect.RightBottom = Vector2.Transform( rect.RightBottom, rotMatrix );
            rect.RightTop = Vector2.Transform( rect.RightTop, rotMatrix );
            rect.LeftTop = Vector2.Transform( rect.LeftTop, rotMatrix );

            rect.LeftBottom += pt;
            rect.RightBottom += pt;
            rect.RightTop += pt;
            rect.LeftTop += pt;*/
        }

        public float DistanceTo( SdcLineSegment segm )
        {
            float a = SdcLineSegment.Distance2D( Left, segm );
            float b = SdcLineSegment.Distance2D( Right, segm );
            float c = SdcLineSegment.Distance2D( Top, segm );
            float d = SdcLineSegment.Distance2D( Bottom, segm );
            float min = MathUtils.MinVec( a, b, c, d );
            return min;
        }

        public bool Intersects( SdcRectangle box )
        {
            // Transform the half measures
            Vector2 halfWidthVectOne = Vector2.Transform( this.HalfWidth * Vector2.UnitX, Matrix2.CreateRotation/*Z*/( Angle ) );
            Vector2 halfHeightVectOne = Vector2.Transform( this.HalfHeight * Vector2.UnitY, Matrix2.CreateRotation/*Z*/( Angle ) );
            Vector2 halfWidthVectTwo = Vector2.Transform( box.HalfWidth * Vector2.UnitX, Matrix2.CreateRotation/*Z*/( box.Angle ) );
            Vector2 halfHeightVectTwo = Vector2.Transform( box.HalfHeight * Vector2.UnitY, Matrix2.CreateRotation/*Z*/( box.Angle ) );

            // They'll work as normals too
            Vector2[] normals = new Vector2[ 4 ];
            normals[ 0 ] = halfWidthVectOne;
            normals[ 1 ] = halfWidthVectTwo;
            normals[ 2 ] = halfHeightVectOne;
            normals[ 3 ] = halfHeightVectTwo;

            for ( int i = 0; i < 4; i++ )
            {
                normals[ i ].Normalize();

                //Project the half measures onto the normal...
                Vector2 projectedHWOne = Vector2.Dot( halfWidthVectOne, normals[ i ] ) * normals[ i ];
                Vector2 projectedHHOne = Vector2.Dot( halfHeightVectOne, normals[ i ] ) * normals[ i ];
                Vector2 projectedHWTwo = Vector2.Dot( halfWidthVectTwo, normals[ i ] ) * normals[ i ];
                Vector2 projectedHHTwo = Vector2.Dot( halfHeightVectTwo, normals[ i ] ) * normals[ i ];

                //Calculate the half lengths along the separation axis.
                float halfLengthOne = projectedHWOne.Length() + projectedHHOne.Length();
                float halfLengthTwo = projectedHWTwo.Length() + projectedHHTwo.Length();

                //Find the distance between object centers along the separation axis.
                Vector2 difference = ( this.Center - box.Center );
                Vector2 projectedDiff = Vector2.Dot( difference, normals[ i ] ) * normals[ i ];
                float projectedDistance = projectedDiff.Length();

                //Test for early out.
                if ( projectedDistance > halfLengthOne + halfLengthTwo )
                {
                    return false;
                }
            }

            //We tested every normal axis,
            //we must be in intersection!
            return true;
        }

        public bool Intersects( SdcCircle circle )
        {
            //Transform the half measures
            Vector2 halfWidthVect = Vector2.Transform(
                HalfWidth * Vector2.UnitX, Matrix2.CreateRotationZ( Angle ) );
            Vector2 halfHeightVect = Vector2.Transform(
                HalfHeight * Vector2.UnitY, Matrix2.CreateRotationZ( Angle ) );

            Vector2[] normals = new Vector2[ 6 ];
            normals[ 0 ] = halfHeightVect;
            normals[ 1 ] = halfWidthVect;
            normals[ 2 ] = circle.Center - ( Center + halfHeightVect );
            normals[ 3 ] = circle.Center - ( Center - halfHeightVect );
            normals[ 4 ] = circle.Center - ( Center + halfWidthVect );
            normals[ 5 ] = circle.Center - ( Center - halfWidthVect );

            for ( int i = 0; i < 6; i++ )
            {
                normals[ i ].Normalize();
                Vector2 projectedHalfWidth = Vector2.Dot( halfWidthVect, normals[ i ] ) * normals[ i ];
                Vector2 projectedHalfHeight = Vector2.Dot( halfHeightVect, normals[ i ] ) * normals[ i ];

                float halfLength = projectedHalfHeight.Length() + projectedHalfWidth.Length();

                Vector2 difference = Center - circle.Center;
                Vector2 projectedDifference = Vector2.Dot( difference, normals[ i ] ) * normals[ i ];
                float projectedDistance = projectedDifference.Length();

                if ( projectedDistance > halfLength + circle.Radius )
                {
                    return false;
                }
            }
            return true;
        }
    }
}
