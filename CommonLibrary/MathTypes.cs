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
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace SigmaDC.Common.MathEx
{
    public static class MathUtils
    {
        public const float Epsilon = 0.001f;

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

        public static float MaxVec( params float[] vec )
        {
            if ( vec.Length == 0 ) return float.NaN;

            float maxValue = vec[ 0 ];
            for ( int i = 1; i < vec.Length; ++i )
            {
                if ( vec[ i ] > maxValue ) maxValue = vec[ i ];
            }
            return maxValue;
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
            if ( IsNull ) return "<Invalid>";

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
                return string.Concat( "{ " + this.X.ToString(), "; ", this.Y.ToString() + " }" );
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
            // NOTE: fuzzy compare used here
            return ( MathUtils.NearlyEqual( value1.X, value2.X ) && MathUtils.NearlyEqual( value1.Y, value2.Y ) );
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
        /// Returns a pseudo-cross product of two vectors.
        /// </summary>
        /// <remarks>"True" cross product is defined only for 3D vectors. This formula is just mathematical hack.</remarks>
        /// <param name="value1">The first <see cref="Vector2"/>.</param>
        /// <param name="value2">The second <see cref="Vector2"/>.</param>
        /// <returns>The pseudo-cross product of two vectors.</returns>
        public static float Cross( Vector2 value1, Vector2 value2 )
        {
            return ( value1.X * value2.Y - value2.X - value1.Y );
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
            return "{ X:" + X + "; Y:" + Y + " }";
        }

        /// <summary>
        /// Gets a <see cref="Point"/> representation for this object.
        /// </summary>
        /// <returns>A <see cref="Point"/> representation for this object.</returns>
        public Point ToPoint()
        {
            return new Point( ( int )X, ( int )Y );
        }

        public PointF ToPointF()
        {
            return new PointF( X, Y );
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

    public struct SdcLine
    {
        public Vector2 P1 { get; set; }
        public Vector2 P2 { get; set; }

        private struct GeneralLineEquation
        {
            // A*x + B*y = C
            public float A;
            public float B;
            public float C;

            public GeneralLineEquation( Vector2 P, Vector2 Q )
            {
                // A = y2-y1
                // B = x1-x2
                // C = A*x1 + B*y1
                A = Q.Y - P.Y;
                B = P.X - Q.X;
                C = A * P.X + B * P.Y;
            }
        }

        // Inheritance is required, see http://stackoverflow.com/a/7670854/1794089
        public SdcLine( Vector2 first, Vector2 second ) : this()
        {
            Trace.Assert( second.X >= first.X );
            Trace.Assert( second.Y >= first.Y );

            P1 = first;
            P2 = second;
        }

        public bool IntersectsWith( SdcLineSegment other, out Vector2 ptIntersection )
        {
            ptIntersection = new Vector2();

            GeneralLineEquation t_le = new GeneralLineEquation( this.P1, this.P2 ); // A1, B1, C1
            GeneralLineEquation o_le = new GeneralLineEquation( other.P1, other.P2 );  // A2, B2, C2

            float det = t_le.A * o_le.B - o_le.A * t_le.B;
            if ( MathUtils.NearlyZero( det ) )
            {
                //Lines are parallel
                return false;
            }

            float x = ( o_le.B * t_le.C - t_le.B * o_le.C ) / det;
            float y = ( t_le.A * o_le.C - o_le.A * t_le.C ) / det;
            ptIntersection = new Vector2( x, y );
            return true;
        }
    }

    public struct SdcLineSegment
    {
        public Vector2 P1 { get; set; }
        public Vector2 P2 { get; set; }

        // Inheritance is required, see http://stackoverflow.com/a/7670854/1794089
        public SdcLineSegment( Vector2 first, Vector2 second )
            : this()
        {
            //Trace.Assert( second.X >= first.X );
            //Trace.Assert( second.Y >= first.Y );
            if ( !( ( second.X >= first.X ) || ( second.Y >= first.Y ) ) ) Debugger.Break();
            Trace.Assert( second.X >= first.X || second.Y >= first.Y );

            if ( MathUtils.NearlyEqual( first.X, second.X ) ) Trace.Assert( !MathUtils.NearlyEqual( first.Y, second.Y ) );
            if ( MathUtils.NearlyEqual( first.Y, second.Y ) ) Trace.Assert( !MathUtils.NearlyEqual( first.X, second.X ) );

            P1 = first;
            P2 = second;
        }

        /*public SdcLineSegment Rotate( Vector2 origin, float angle )
        {
            var result = new SdcLineSegment();
            result.P1 = Vector2.RotateAroundPoint( this.P1, origin, angle );
            result.P2 = Vector2.RotateAroundPoint( this.P2, origin, angle );
            return result;
        }*/

        public float Length()
        {
            return ( P1 - P2 ).Length();
        }

        #region Check whether the line segment contains the specified point

        public static bool Contains( SdcLineSegment S, Vector2 C )
        {
            var crossProduct = ( C.Y - S.P1.Y ) * ( S.P2.X - S.P1.X ) - ( C.X - S.P1.X ) * ( S.P2.Y - S.P1.Y );
            if ( !MathUtils.NearlyZero( crossProduct ) ) return false;

            var dotProduct = ( C.X - S.P1.X ) * ( S.P2.X - S.P1.X ) + ( C.Y - S.P1.Y ) * ( S.P2.Y - S.P1.Y );
            if ( dotProduct < 0.0f ) return false;

            var segmSquaredLength = ( S.P2.X - S.P1.X ) * ( S.P2.X - S.P1.X ) + ( S.P2.Y - S.P1.Y ) * ( S.P2.Y - S.P1.Y );
            if ( dotProduct > segmSquaredLength ) return false;

            return true;
        }

        public bool Contains( Vector2 P )
        {
            return SdcLineSegment.Contains( this, P );
        }
        #endregion

        #region Private stuff for intersection testing
        // Code sources: http://martin-thoma.com/how-to-check-if-two-line-segments-intersect/, https://stackoverflow.com/a/8524921/1794089
        // NOTE: some modifications were made!
        private Vector2[] GetBoundingBox()
        {
            var points = new Vector2[ 2 ];

            if ( P1.X <= P2.X )
            {
                points[ 0 ].X = P1.X;
                points[ 1 ].X = P2.X;
            }
            else
            {
                points[ 1 ].X = P1.X;
                points[ 0 ].X = P2.X;
            }

            if ( P1.Y <= P2.Y )
            {
                points[ 0 ].Y = P1.Y;
                points[ 1 ].Y = P2.Y;
            }
            else
            {
                points[ 1 ].Y = P1.Y;
                points[ 0 ].Y = P2.Y;
            }

            return points;
        }

        private static bool BoundingBoxesIntersects( Vector2[] a, Vector2[] b )
        {
            return ( a[ 0 ].X <= b[ 1 ].X ) && ( a[ 1 ].X >= b[ 0 ].X ) && ( a[ 0 ].Y <= b[ 1 ].Y ) && ( a[ 1 ].Y >= b[ 0 ].Y );
        }

        /// <summary>
        /// Check on the which side of the segment S the point C is on
        /// </summary>
        /// <see cref="https://stackoverflow.com/a/8524921/1794089"/>
        /// <param name="S"></param>
        /// <param name="C"></param>
        /// <returns></returns>
        private static int FindPointSide( SdcLineSegment S, Vector2 C )
        {
            // First check degraded cases: vertical and horizontal line segments,
            // because they require special processing (slope will be undefined for them)
            if ( MathUtils.NearlyZero( S.P2.X - S.P1.X ) )
            {
                // Vertical line
                if ( C.X < S.P2.X )
                {
                    return S.P2.Y > S.P1.Y ? 1 : -1;
                }
                if ( C.X > S.P2.X )
                {
                    return S.P2.Y > S.P1.Y ? -1 : 1;
                }
                return 0;
            }
            if ( MathUtils.NearlyZero( S.P2.Y - S.P1.Y ) )
            {
                // Horizontal line
                if ( C.Y < S.P2.Y )
                {
                    return S.P2.X > S.P1.X ? -1 : 1;
                }
                if ( C.Y > S.P2.Y )
                {
                    return S.P2.X > S.P1.X ? 1 : -1;
                }
                return 0;
            }

            // Check common case using the slope
            double slope = ( S.P2.Y - S.P1.Y ) / ( S.P2.X - S.P1.X );
            double yIntercept = S.P1.Y - S.P1.X * slope;
            double cSolution = ( slope * C.X ) + yIntercept;
            if ( slope != 0 )
            {
                if ( C.Y > cSolution )
                {
                    return S.P2.X > S.P1.X ? 1 : -1;
                }
                if ( C.Y < cSolution )
                {
                    return S.P2.X > S.P1.X ? -1 : 1;
                }
                return 0;
            }
            return 0;
        }

        private static bool TouchesOrCrossesLine( SdcLineSegment a, SdcLineSegment b )
        {
            bool b1 = Contains( a, b.P1 );
            bool b2 = Contains( a, b.P2 );

            int sideFirst = FindPointSide( a, b.P1 );
            int sideSecond = FindPointSide( a, b.P2 );
            bool b3 = sideFirst != sideSecond;

            return b1 || b2 || b3;
        }
        #endregion

        #region Line segment to line segment intersection test
        public static bool Intersects( SdcLineSegment a, SdcLineSegment b )
        {
            var box1 = a.GetBoundingBox();
            var box2 = b.GetBoundingBox();

            bool b1 = BoundingBoxesIntersects( box1, box2 );
            bool b2 = TouchesOrCrossesLine( a, b );
            bool b3 = TouchesOrCrossesLine( b, a );
            return b1 && b2 && b3;
        }

        public bool Intersects( SdcLineSegment S )
        {
            return SdcLineSegment.Intersects( this, S );
        }

        public bool Touches( SdcLineSegment S )
        {
            return ( S.Contains( this.P1 ) || S.Contains( this.P2 ) );
        }

        public static Vector2 GetIntersectionPoint( SdcLineSegment A, SdcLineSegment B )
        {
            Vector2 result = default( Vector2 );

            float dy1 = A.P2.Y - A.P1.Y;
            float dx1 = A.P2.X - A.P1.X;
            float dy2 = B.P2.Y - B.P1.Y;
            float dx2 = B.P2.X - B.P1.X;

            if ( MathUtils.NearlyZero( dx1 ) )
            {
                Trace.Assert( MathUtils.NearlyZero( dy2 ) );
                return new Vector2( A.P1.X, B.P1.Y );
            }

            if ( MathUtils.NearlyEqual( dy1 * dx2, dy2 * dx1 ) )
            {
                Trace.Assert( false, "dy1*dx2 == dy2*dx1" );
                return result;
            }
            
                float x = ( ( B.P1.Y - A.P1.Y ) * dx1 * dx2 + dy1 * dx2 * A.P1.X - dy2 * dx1 * B.P1.X ) / ( dy1 * dx2 - dy2 * dx1 );
                float y = A.P1.Y + ( dy1 / dx1 ) * ( x - A.P1.X );
                result = new Vector2( x, y );
                return result;
            
        }

        public Vector2 GetIntersectionPoint(SdcLineSegment other)
        {
            return SdcLineSegment.GetIntersectionPoint( this, other );
        }

        #endregion

        #region Line segment to line segment minimal distance calculation

        // NOTE: stolen from http://www.geometrictools.com/Source/Distance2D.html#LinearLinear
        class DistResult
        {
            public float distance, sqrDistance;
            public float[] parameter;
            public Vector2[] closest;

            public DistResult()
            {
                distance = 0;
                sqrDistance = 0;
                parameter = new float[ 2 ];
                closest = new Vector2[ 2 ];
            }
        }

        // FIXME: UGLY! static is dangerous in multithreaded environment!

        // The coefficients of R(s,t), not including the constant term.
        static float mA, mB, mC, mD, mE;

        // dR/ds(i,j) at the four corners of the domain
        static float mF00, mF10, mF01, mF11;

        // dR/dt(i,j) at the four corners of the domain
        static float mG00, mG10, mG01, mG11;

        static float GetClampedRoot( float slope, float h0, float h1 )
        {
            // Theoretically, r is in (0,1).  However, when the slope is nearly zero,
            // then so are h0 and h1.  Significant numerical rounding problems can
            // occur when using floating-point arithmetic.  If the rounding causes r
            // to be outside the interval, clamp it.  It is possible that r is in
            // (0,1) and has rounding errors, but because h0 and h1 are both nearly
            // zero, the quadratic is nearly constant on (0,1).  Any choice of p
            // should not cause undesirable accuracy problems for the final distance
            // computation.
            //
            // NOTE:  You can use bisection to recompute the root or even use
            // bisection to compute the root and skip the division.  This is generally
            // slower, which might be a problem for high-performance applications.

            float r = 0.0f;
            if ( h0 < 0.0f )
            {
                if ( h1 > 0.0f )
                {
                    r = -h0 / slope;
                    if ( r > 1.0f )
                    {
                        r = 0.5f;
                    }
                    // The slope is positive and -h0 is positive, so there is no
                    // need to test for a negative value and clamp it.
                }
                else
                {
                    r = 1.0f;
                }
            }
            else
            {
                r = 0.0f;
            }
            return r;
        }

        static void ComputeIntersection( float[] sValue, int[] classify, ref int[] edge, ref float[ , ] end )
        {
            if ( sValue.Length != 2 ) throw new ArgumentException( "sValue parameter must contains exactly 2 items" );
            if ( classify.Length != 2 ) throw new ArgumentException( "classify parameter must contains exactly 2 items" );
            if ( edge.Length != 2 ) throw new ArgumentException( "edge parameter must contains exactly 2 items" );
            if ( end.Length != 4 ) throw new ArgumentException( "end parameter must contains exactly 2 items in the first and second dimension" );

            // The divisions are theoretically numbers in [0,1].  Numerical rounding
            // errors might cause the result to be outside the interval.  When this
            // happens, it must be that both numerator and denominator are nearly
            // zero.  The denominator is nearly zero when the segments are nearly
            // perpendicular.  The numerator is nearly zero when the P-segment is
            // nearly degenerate (mF00 = a is small).  The choice of 0.5 should not
            // cause significant accuracy problems.
            //
            // NOTE:  You can use bisection to recompute the root or even use
            // bisection to compute the root and skip the division.  This is generally
            // slower, which might be a problem for high-performance applications.

            if ( classify[ 0 ] < 0 )
            {
                edge[ 0 ] = 0;
                end[ 0, 0 ] = 0.0f;
                end[ 0, 1 ] = mF00 / mB;
                if ( end[ 0, 1 ] < 0.0f || end[ 0, 1 ] > 1.0f )
                {
                    end[ 0, 1 ] = 0.5f;
                }

                if ( classify[ 1 ] == 0 )
                {
                    edge[ 1 ] = 3;
                    end[ 1, 0 ] = sValue[ 1 ];
                    end[ 1, 1 ] = 1.0f;
                }
                else  // classify[1] > 0
                {
                    edge[ 1 ] = 1;
                    end[ 1, 0 ] = 1.0f;
                    end[ 1, 1 ] = mF10 / mB;
                    if ( end[ 1, 1 ] < 0.0f || end[ 1, 1 ] > 1.0f )
                    {
                        end[ 1, 1 ] = 0.5f;
                    }
                }
            }
            else if ( classify[ 0 ] == 0 )
            {
                edge[ 0 ] = 2;
                end[ 0, 0 ] = sValue[ 0 ];
                end[ 0, 1 ] = 0.0f;

                if ( classify[ 1 ] < 0 )
                {
                    edge[ 1 ] = 0;
                    end[ 1, 0 ] = 0.0f;
                    end[ 1, 1 ] = mF00 / mB;
                    if ( end[ 1, 1 ] < 0.0f || end[ 1, 1 ] > 1.0f )
                    {
                        end[ 1, 1 ] = 0.5f;
                    }
                }
                else if ( classify[ 1 ] == 0 )
                {
                    edge[ 1 ] = 3;
                    end[ 1, 0 ] = sValue[ 1 ];
                    end[ 1, 1 ] = 1.0f;
                }
                else
                {
                    edge[ 1 ] = 1;
                    end[ 1, 0 ] = 1.0f;
                    end[ 1, 1 ] = mF10 / mB;
                    if ( end[ 1, 1 ] < 0.0f || end[ 1, 1 ] > 1.0f )
                    {
                        end[ 1, 1 ] = 0.5f;
                    }
                }
            }
            else  // classify[0] > 0
            {
                edge[ 0 ] = 1;
                end[ 0, 0 ] = 1.0f;
                end[ 0, 1 ] = mF10 / mB;
                if ( end[ 0, 1 ] < 0.0f || end[ 0, 1 ] > 1 )
                {
                    end[ 0, 1 ] = 0.5f;
                }

                if ( classify[ 1 ] == 0 )
                {
                    edge[ 1 ] = 3;
                    end[ 1, 0 ] = sValue[ 1 ];
                    end[ 1, 1 ] = 1.0f;
                }
                else
                {
                    edge[ 1 ] = 0;
                    end[ 1, 0 ] = 0.0f;
                    end[ 1, 1 ] = mF00 / mB;
                    if ( end[ 1, 1 ] < 0.0f || end[ 1, 1 ] > 1.0f )
                    {
                        end[ 1, 1 ] = 0.5f;
                    }
                }
            }
        }

        static void ComputeMinimumParameters( int[/*2*/] edge, float[/*2*/,/*2*/] end, ref float[/*2*/] parameter )
        {
            if ( edge.Length != 2 ) throw new ArgumentException( "edge parameter must contains exactly 2 items" );
            if ( end.Length != 4 ) throw new ArgumentException( "end parameter must contains exactly 2 items in the first and second dimension" );
            if ( parameter.Length != 2 ) throw new ArgumentException( "parameter parameter must contains exactly 2 items" );

            float delta = end[ 1, 1 ] - end[ 0, 1 ];
            float h0 = delta * ( -mB * end[ 0, 0 ] + mC * end[ 0, 1 ] - mE );
            if ( h0 >= 0.0f )
            {
                if ( edge[ 0 ] == 0 )
                {
                    parameter[ 0 ] = 0.0f;
                    parameter[ 1 ] = GetClampedRoot( mC, mG00, mG01 );
                }
                else if ( edge[ 0 ] == 1 )
                {
                    parameter[ 0 ] = 1.0f;
                    parameter[ 1 ] = GetClampedRoot( mC, mG10, mG11 );
                }
                else
                {
                    parameter[ 0 ] = end[ 0, 0 ];
                    parameter[ 1 ] = end[ 0, 1 ];
                }
            }
            else
            {
                float h1 = delta * ( -mB * end[ 1, 0 ] + mC * end[ 1, 1 ] - mE );
                if ( h1 <= 0.0f )
                {
                    if ( edge[ 1 ] == 0 )
                    {
                        parameter[ 0 ] = 0.0f;
                        parameter[ 1 ] = GetClampedRoot( mC, mG00, mG01 );
                    }
                    else if ( edge[ 1 ] == 1 )
                    {
                        parameter[ 0 ] = 1.0f;
                        parameter[ 1 ] = GetClampedRoot( mC, mG10, mG11 );
                    }
                    else
                    {
                        parameter[ 0 ] = end[ 1, 0 ];
                        parameter[ 1 ] = end[ 1, 1 ];
                    }
                }
                else  // h0 < 0 and h1 > 0
                {
                    float z = Math.Min( Math.Max( h0 / ( h0 - h1 ), 0.0f ), 1.0f );
                    float omz = 1.0f - z;
                    parameter[ 0 ] = omz * end[ 0, 0 ] + z * end[ 1, 0 ];
                    parameter[ 1 ] = omz * end[ 0, 1 ] + z * end[ 1, 1 ];
                }
            }
        }

        public static float Distance2D( SdcLineSegment line1, SdcLineSegment line2 )
        {
            DistResult result = new DistResult();

            // The code allows degenerate line segments; that is, P0 and P1 can be
            // the same point or Q0 and Q1 can be the same point.  The quadratic
            // function for squared distance between the segment is
            //   R(s,t) = a*s^2 - 2*b*s*t + c*t^2 + 2*d*s - 2*e*t + f
            // for (s,t) in [0,1]^2 where
            //   a = Dot(P1-P0,P1-P0), b = Dot(P1-P0,Q1-Q0), c = Dot(Q1-Q0,Q1-Q0),
            //   d = Dot(P1-P0,P0-Q0), e = Dot(Q1-Q0,P0-Q0), f = Dot(P0-Q0,P0-Q0)
            Vector2 P1mP0 = line1.P2 - line1.P1;
            Vector2 Q1mQ0 = line2.P2 - line2.P1;
            Vector2 P0mQ0 = line1.P1 - line2.P1;
            mA = Vector2.Dot( P1mP0, P1mP0 );
            mB = Vector2.Dot( P1mP0, Q1mQ0 );
            mC = Vector2.Dot( Q1mQ0, Q1mQ0 );
            mD = Vector2.Dot( P1mP0, P0mQ0 );
            mE = Vector2.Dot( Q1mQ0, P0mQ0 );

            mF00 = mD;
            mF10 = mF00 + mA;
            mF01 = mF00 - mB;
            mF11 = mF10 - mB;

            mG00 = -mE;
            mG10 = mG00 - mB;
            mG01 = mG00 + mC;
            mG11 = mG10 + mC;

            if ( mA > 0.0f && mC > 0.0f )
            {
                // Compute the solutions to dR/ds(s0,0) = 0 and dR/ds(s1,1) = 0.  The
                // location of sI on the s-axis is stored in classifyI (I = 0 or 1).  If
                // sI <= 0, classifyI is -1.  If sI >= 1, classifyI is 1.  If 0 < sI < 1,
                // classifyI is 0.  This information helps determine where to search for
                // the minimum point (s,t).  The fij values are dR/ds(i,j) for i and j in
                // {0,1}.

                float[] sValue = new float[ 2 ];
                sValue[ 0 ] = GetClampedRoot( mA, mF00, mF10 );
                sValue[ 1 ] = GetClampedRoot( mA, mF01, mF11 );

                int[] classify = new int[ 2 ];
                for ( int i = 0; i < 2; ++i )
                {
                    if ( sValue[ i ] <= 0.0f )
                    {
                        classify[ i ] = -1;
                    }
                    else if ( sValue[ i ] >= 1.0f )
                    {
                        classify[ i ] = +1;
                    }
                    else
                    {
                        classify[ i ] = 0;
                    }
                }

                if ( classify[ 0 ] == -1 && classify[ 1 ] == -1 )
                {
                    // The minimum must occur on s = 0 for 0 <= t <= 1.
                    result.parameter[ 0 ] = 0.0f;
                    result.parameter[ 1 ] = GetClampedRoot( mC, mG00, mG01 );
                }
                else if ( classify[ 0 ] == +1 && classify[ 1 ] == +1 )
                {
                    // The minimum must occur on s = 1 for 0 <= t <= 1.
                    result.parameter[ 0 ] = 1.0f;
                    result.parameter[ 1 ] = GetClampedRoot( mC, mG10, mG11 );
                }
                else
                {
                    // The line dR/ds = 0 intersects the domain [0,1]^2 in a
                    // nondegenerate segment.  Compute the endpoints of that segment,
                    // end[0] and end[1].  The edge[i] flag tells you on which domain
                    // edge end[i] lives: 0 (s=0), 1 (s=1), 2 (t=0), 3 (t=1).
                    int[] edge = new int[ 2 ];
                    float[ , ] end = new float[ 2, 2 ];
                    ComputeIntersection( sValue, classify, ref edge, ref end );

                    // The directional derivative of R along the segment of
                    // intersection is
                    //   H(z) = (end[1][1]-end[1][0])*dR/dt((1-z)*end[0] + z*end[1])
                    // for z in [0,1].  The formula uses the fact that dR/ds = 0 on
                    // the segment.  Compute the minimum of H on [0,1].
                    ComputeMinimumParameters( edge, end, ref result.parameter );
                }
            }
            else
            {
                if ( mA > 0.0f )
                {
                    // The Q-segment is degenerate (Q0 and Q1 are the same point) and
                    // the quadratic is R(s,0) = a*s^2 + 2*d*s + f and has (half)
                    // first derivative F(t) = a*s + d.  The closest P-point is
                    // interior to the P-segment when F(0) < 0 and F(1) > 0.
                    result.parameter[ 0 ] = GetClampedRoot( mA, mF00, mF10 );
                    result.parameter[ 1 ] = 0.0f;
                }
                else if ( mC > 0.0f )
                {
                    // The P-segment is degenerate (P0 and P1 are the same point) and
                    // the quadratic is R(0,t) = c*t^2 - 2*e*t + f and has (half)
                    // first derivative G(t) = c*t - e.  The closest Q-point is
                    // interior to the Q-segment when G(0) < 0 and G(1) > 0.
                    result.parameter[ 0 ] = 0.0f;
                    result.parameter[ 1 ] = GetClampedRoot( mC, mG00, mG01 );
                }
                else
                {
                    // P-segment and Q-segment are degenerate.
                    result.parameter[ 0 ] = 0.0f;
                    result.parameter[ 1 ] = 0.0f;
                }
            }

            result.closest[ 0 ] = ( 1.0f - result.parameter[ 0 ] ) * line1.P1 + result.parameter[ 0 ] * line1.P2;
            result.closest[ 1 ] = ( 1.0f - result.parameter[ 1 ] ) * line2.P1 + result.parameter[ 1 ] * line2.P2;
            Vector2 diff = result.closest[ 0 ] - result.closest[ 1 ];
            result.sqrDistance = Vector2.Dot( diff, diff );
            result.distance = ( float )Math.Sqrt( result.sqrDistance );
            return result.distance;
        }

        public float Distance2D( SdcLineSegment S )
        {
            return SdcLineSegment.Distance2D( this, S );
        }
        #endregion

        #region Standart .Net stuff
        public override string ToString()
        {
            return string.Format( "A: ({0}; {1}), B: ({2}; {3})", this.P1.X, this.P1.Y, this.P2.X, this.P2.Y );
        }
        #endregion
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

    public class SdcTriangle
    {
        public Vector2 A { get; set; }
        public Vector2 B { get; set; }
        public Vector2 C { get; set; }

        public SdcTriangle( Vector2 A, Vector2 B, Vector2 C )
        {
            this.A = A;
            this.B = B;
            this.C = C;
        }

        public bool Contains( Vector2 P )
        {
            // Use Barycentric Technique: see http://www.blackpawn.com/texts/pointinpoly/default.html

            // Compute vectors        
            var v0 = C - A;
            var v1 = B - A;
            var v2 = P - A;

            // Compute dot products
            var dot00 = Vector2.Dot( v0, v0 );
            var dot01 = Vector2.Dot( v0, v1 );
            var dot02 = Vector2.Dot( v0, v2 );
            var dot11 = Vector2.Dot( v1, v1 );
            var dot12 = Vector2.Dot( v1, v2 );

            // Compute barycentric coordinates
            var invDenom = 1.0f / ( dot00 * dot11 - dot01 * dot01 );
            var u = ( dot11 * dot02 - dot01 * dot12 ) * invDenom;
            var v = ( dot00 * dot12 - dot01 * dot02 ) * invDenom;

            // Check if point is in triangle
            return ( u >= 0 ) && ( v >= 0 ) && ( u + v < 1 );
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
        public float RotationAngle { get; set; }
        public Vector2 RotationCenter { get; set; }
        #endregion

        #region Common rectangle properties
        
        private bool PointLess( Vector2 P, Vector2 Q )
        {
            return ( P.X <= Q.X ) && ( P.Y <= Q.Y );
        }

        // Counterclockwise direction: A, B, C, D
        public SdcLineSegment AB { get { return ( PointLess( A, B ) ? new SdcLineSegment( A, B ) : new SdcLineSegment( B, A ) ); } }
        public SdcLineSegment BC { get { return ( PointLess( B, C ) ? new SdcLineSegment( B, C ) : new SdcLineSegment( C, B ) ); } }
        public SdcLineSegment CD { get { return ( PointLess( C, D ) ? new SdcLineSegment( C, D ) : new SdcLineSegment( D, C ) ); } }
        public SdcLineSegment DA { get { return ( PointLess( D, A ) ? new SdcLineSegment( D, A ) : new SdcLineSegment( A, D ) ); } }

        /// <summary>
        /// The base edge contains human projection center point
        /// </summary>
        public SdcLineSegment BaseEdge
        {
            get
            {
                bool b1 = AB.Contains( RotationCenter );
                bool b2 = BC.Contains( RotationCenter );
                bool b3 = CD.Contains( RotationCenter );
                bool b4 = DA.Contains( RotationCenter );
                
                // One and only one edge must containt the human projection center
                Trace.Assert( b1 || b2 || b3 || b4 );
                Trace.Assert( b1 ? (b1 && !b2 && !b3 && !b4 ) : true);
                Trace.Assert( b2 ? (b2 && !b1 && !b3 && !b4 ) : true);
                Trace.Assert( b3 ? (b3 && !b1 && !b2 && !b4 ) : true);
                Trace.Assert( b4 ? (b4 && !b1 && !b2 && !b3 ) : true);

                if ( b1 ) return AB;
                if ( b2 ) return BC;
                if ( b3 ) return CD;
                if ( b4 ) return DA;

                return new SdcLineSegment();
            }
        }
        
        public Vector2 A { get; set; }
        public Vector2 B { get; set; }
        public Vector2 C { get; set; }
        public Vector2 D { get; set; }

        #endregion

        public float Width { get; set; }

        public float Height { get; set; }

        public SdcRectangle()
        {
            // FIXME:
           /* Center = new Vector2();

            HalfWidth = 0.0f;
            HalfHeight = 0.0f;
            RotationAngle = 0.0f;
            RotationCenter = new Vector2();

            Width = 0.0f;
            Height = 0.0f;

            LeftBottom = new Vector2();
            LeftTop = new Vector2();
            RightBottom = new Vector2();
            RightTop = new Vector2();

            Bottom = new SdcLineSegment();
            Top = new SdcLineSegment();
            Left = new SdcLineSegment();
            Right = new SdcLineSegment();*/
        }

        public SdcRectangle( SdcRectangle other )
        {
            Center = other.Center;

            HalfWidth = other.HalfWidth;
            HalfHeight = other.HalfHeight;
            RotationAngle = other.RotationAngle;
            RotationCenter = other.RotationCenter;

            Width = other.Width;
            Height = other.Height;

            this.A = other.A;
            this.B = other.B;
            this.C = other.C;
            this.D = other.D;
        }

        public SdcRectangle( RectangleF otherRect )
        {
            Trace.Assert( otherRect.Right >= otherRect.Left );
//            Trace.Assert( otherRect.Top >= otherRect.Bottom );

            Center = new Vector2( ( otherRect.Left + otherRect.Right ) / 2.0f, ( otherRect.Bottom + otherRect.Top ) / 2.0f );

            HalfWidth = otherRect.Width / 2.0f;
            HalfHeight = otherRect.Height / 2.0f;
            RotationAngle = 0.0f;
            RotationCenter = new Vector2();

            float top = otherRect.Top;
            float bottom = otherRect.Bottom;
            if ( otherRect.Top < otherRect.Bottom )
            {
                top = otherRect.Bottom;
                bottom = otherRect.Top;
            }

            Width = otherRect.Width;
            Height = otherRect.Height;

            var leftBottom = new Vector2( otherRect.Left, bottom );
            var leftTop = new Vector2( otherRect.Left, top );
            var rightBottom = new Vector2( otherRect.Right, bottom );
            var rightTop = new Vector2( otherRect.Right, top );

            A = rightBottom;
            B = rightTop;
            C = leftTop;
            D = leftBottom;

            
            Trace.Assert( Width >= 0 );
            Trace.Assert( Height >= 0 );

            Trace.Assert( AB.Length() > 0 );
            Trace.Assert( BC.Length() > 0 );
            Trace.Assert( CD.Length() > 0 );
            Trace.Assert( DA.Length() > 0 );
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
            Width = visibilityRadius;
            Height = manDiameter;

            var leftBottom = new Vector2( manProjCenter.X, manProjCenter.Y - manRadius );
            var leftTop = new Vector2( manProjCenter.X, manProjCenter.Y + manRadius );
            var rightBottom = new Vector2( manProjCenter.X + visibilityRadius, manProjCenter.Y - manRadius );
            var rightTop = new Vector2( manProjCenter.X + visibilityRadius, manProjCenter.Y + manRadius );

            A = rightBottom;
            B = rightTop;
            C = leftTop;
            D = leftBottom;

            // 2) Fill the intersection-specific field values
            Center = new Vector2( ( leftBottom.X + rightTop.X ) / 2.0f, ( leftBottom.Y + rightTop.Y ) / 2.0f );

            HalfWidth = Width / 2.0f;
            HalfHeight = Height / 2.0f;
            RotationAngle = angle;
            RotationCenter = manProjCenter;

            Debug.Assert( Width >= 0 && Height >= 0 );
        }

        public SdcRectangle Rotate( Vector2 origin, float angle )
        {
            if ( angle == 0.0f ) return this;

            var result = new SdcRectangle();

            result.Center = Vector2.RotateAroundPoint( this.Center, origin, angle );
            result.HalfWidth = this.HalfWidth;
            result.HalfHeight = this.HalfHeight;
            result.RotationAngle = 0.0f;
            result.RotationCenter = this.RotationCenter;

            result.Width = this.Width;
            result.Height = this.Height;

            result.A = Vector2.RotateAroundPoint( this.A, origin, angle );
            result.B = Vector2.RotateAroundPoint( this.B, origin, angle );
            result.C = Vector2.RotateAroundPoint( this.C, origin, angle );
            result.D = Vector2.RotateAroundPoint( this.D, origin, angle );
            return result;
        }

        public static float SMALL_NUM = 0.001f;

        public float DistanceTo( SdcRectangle other )
        {
            // Determine visibility area side edges (adjacent with base one) 
            Trace.Assert( other.BaseEdge.P1 == other.CD.P1 && other.BaseEdge.P2 == other.CD.P2 );
            Trace.Assert( !other.AB.Touches( other.CD ) );
            Trace.Assert( other.BC.Touches( other.CD ) );
            Trace.Assert( other.DA.Touches( other.CD ) );
            Trace.Assert( other.CD.Contains( other.BC.P1 ) || other.CD.Contains( other.BC.P2 ) );
            Trace.Assert( other.CD.Contains( other.DA.P1 ) || other.CD.Contains( other.DA.P2 ) );
            
            // Determine base edge points order
            Vector2 C = other.CD.Contains( other.BC.P1 ) ? other.BC.P1 : other.BC.P2;
            Vector2 D = other.CD.Contains( other.DA.P1 ) ? other.DA.P1 : other.DA.P2;
            // Three cases are available: rectangles can have no intersection, can touch each other
            // and have two or more intersection points
            if ( this.Touches( other ) )
            {
                return 0;
            }
            else if ( !this.Intersects( other ) )
            {
                return MathUtils.MinVec( other.CD.Distance2D( this.AB ), other.CD.Distance2D( this.BC ), other.CD.Distance2D( this.CD ), other.CD.Distance2D( this.DA ) );
            }
            else
            {
                // Determine intersection of BC and this rectangle (obstacle extent)
                List<Vector2> bcPts = new List<Vector2>();
                if ( other.BC.Intersects( this.AB ) ) bcPts.Add( other.BC.GetIntersectionPoint( this.AB ) );
                if ( other.BC.Intersects( this.BC ) ) bcPts.Add( other.BC.GetIntersectionPoint( this.BC ) );
                if ( other.BC.Intersects( this.CD ) ) bcPts.Add( other.BC.GetIntersectionPoint( this.CD ) );
                if ( other.BC.Intersects( this.DA ) ) bcPts.Add( other.BC.GetIntersectionPoint( this.DA ) );
                Trace.Assert( bcPts.Count <= 2 );

                // Determine intersection of DA and this rectangle (obstacle extent)
                List<Vector2> daPts = new List<Vector2>();
                if ( other.DA.Intersects( this.AB ) ) daPts.Add( other.DA.GetIntersectionPoint( this.AB ) );
                if ( other.DA.Intersects( this.BC ) ) daPts.Add( other.DA.GetIntersectionPoint( this.BC ) );
                if ( other.DA.Intersects( this.CD ) ) daPts.Add( other.DA.GetIntersectionPoint( this.CD ) );
                if ( other.DA.Intersects( this.DA ) ) daPts.Add( other.DA.GetIntersectionPoint( this.DA ) );
                Trace.Assert( daPts.Count <= 2 );

                // Determine distance from C to intersection points
                List<float> bcDist = new List<float>();
                for ( int i = 0; i < bcPts.Count; ++i )
                {
                    bcDist.Add( Vector2.Distance( C, bcPts[ i ] ) );
                }

                // Determine distance from D to intersection points
                List<float> daDist = new List<float>();
                for ( int i = 0; i < daPts.Count; ++i )
                {
                    daDist.Add( Vector2.Distance( D, daPts[ i ] ) );
                }

                // Return the minimum distance found
                return MathUtils.MinVec( bcDist.Count >= 1 ? bcDist.Min() : float.MaxValue, daDist.Count >= 1 ? daDist.Min() : float.MaxValue );
            }

            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return string.Format( "A: ({0}; {1}), B: ({2}; {3}), C: ({4}; {5}), D: ({6}; {7})",
                A.X, A.Y, B.X, B.Y, C.X, C.Y, D.X, D.Y );
        }

        public bool Contains( Vector2 pt )
        {
            // Let the current rectangle is described by four vertices: A, B, C, D

            // 1) Rotate rectangle
            var rotatedThis = Rotate( this.RotationCenter, this.RotationAngle );

            // 2) Split it to the two triangles: ABD, BCD
            var trABD = new SdcTriangle( rotatedThis.A, rotatedThis.B, rotatedThis.D );
            var trBCD = new SdcTriangle( rotatedThis.B, rotatedThis.C, rotatedThis.D );

            // 3) Check if one of the triangles contains the specified point
            return ( trABD.Contains( pt ) || trBCD.Contains( pt ) );
        }

        public bool Contains( SdcRectangle rect )
        {
            // One rectangle completely contains another <==> it contains all four it's points
            return ( Contains( rect.A ) && Contains( rect.B ) && Contains( rect.C ) && Contains( rect.D ) );
        }

        public bool Touches( SdcRectangle other )
        {
            bool b1 = this.AB.Touches( other.AB ) || this.AB.Touches( other.BC ) || this.AB.Touches( other.CD ) || this.AB.Touches( other.DA );
            bool b2 = this.BC.Touches( other.AB ) || this.BC.Touches( other.BC ) || this.BC.Touches( other.CD ) || this.BC.Touches( other.DA );
            bool b3 = this.CD.Touches( other.AB ) || this.CD.Touches( other.BC ) || this.CD.Touches( other.CD ) || this.CD.Touches( other.DA );
            bool b4 = this.DA.Touches( other.AB ) || this.DA.Touches( other.BC ) || this.DA.Touches( other.CD ) || this.DA.Touches( other.DA );
            return b1 || b2 || b3 || b4;
        }

        public bool Intersects( SdcRectangle other )
        {
            var rotatedThis = this.Rotate( this.RotationCenter, this.RotationAngle );
            var rotatedOther = other.Rotate( other.RotationCenter, other.RotationAngle );

            if ( rotatedThis.AB.Intersects( rotatedOther.AB ) ) return true;
            if ( rotatedThis.AB.Intersects( rotatedOther.BC ) ) return true;
            if ( rotatedThis.AB.Intersects( rotatedOther.CD ) ) return true;
            if ( rotatedThis.AB.Intersects( rotatedOther.DA ) ) return true;

            if ( rotatedThis.BC.Intersects( rotatedOther.AB ) ) return true;
            if ( rotatedThis.BC.Intersects( rotatedOther.BC ) ) return true;
            if ( rotatedThis.BC.Intersects( rotatedOther.CD ) ) return true;
            if ( rotatedThis.BC.Intersects( rotatedOther.DA ) ) return true;

            if ( rotatedThis.CD.Intersects( rotatedOther.AB ) ) return true;
            if ( rotatedThis.CD.Intersects( rotatedOther.BC ) ) return true;
            if ( rotatedThis.CD.Intersects( rotatedOther.CD ) ) return true;
            if ( rotatedThis.CD.Intersects( rotatedOther.DA ) ) return true;

            if ( rotatedThis.DA.Intersects( rotatedOther.AB ) ) return true;
            if ( rotatedThis.DA.Intersects( rotatedOther.BC ) ) return true;
            if ( rotatedThis.DA.Intersects( rotatedOther.CD ) ) return true;
            if ( rotatedThis.DA.Intersects( rotatedOther.DA ) ) return true;

            return false;

            // Transform the half measures
            /*Vector2 halfWidthVectOne = Vector2.Transform( this.HalfWidth * Vector2.UnitX, Matrix2.CreateRotation( this.RotationAngle ) );
            Vector2 halfHeightVectOne = Vector2.Transform( this.HalfHeight * Vector2.UnitY, Matrix2.CreateRotation( this.RotationAngle ) );
            Vector2 halfWidthVectTwo = Vector2.Transform( other.HalfWidth * Vector2.UnitX, Matrix2.CreateRotation( other.RotationAngle ) );
            Vector2 halfHeightVectTwo = Vector2.Transform( other.HalfHeight * Vector2.UnitY, Matrix2.CreateRotation( other.RotationAngle ) );

            // They'll work as normals too
            Vector2[] normals = new Vector2[ 4 ];
            normals[ 0 ] = halfWidthVectOne;
            normals[ 1 ] = halfWidthVectTwo;
            normals[ 2 ] = halfHeightVectOne;
            normals[ 3 ] = halfHeightVectTwo;

            for ( int i = 0; i < 4; i++ )
            {
                normals[ i ].Normalize();

                // Project the half measures onto the normal...
                Vector2 projectedHWOne = Vector2.Dot( halfWidthVectOne, normals[ i ] ) * normals[ i ];
                Vector2 projectedHHOne = Vector2.Dot( halfHeightVectOne, normals[ i ] ) * normals[ i ];
                Vector2 projectedHWTwo = Vector2.Dot( halfWidthVectTwo, normals[ i ] ) * normals[ i ];
                Vector2 projectedHHTwo = Vector2.Dot( halfHeightVectTwo, normals[ i ] ) * normals[ i ];

                // Calculate the half lengths along the separation axis.
                float halfLengthOne = projectedHWOne.Length() + projectedHHOne.Length();
                float halfLengthTwo = projectedHWTwo.Length() + projectedHHTwo.Length();

                // Find the distance between object centers along the separation axis.
                Vector2 difference = ( this.Center - other.Center );
                Vector2 projectedDiff = Vector2.Dot( difference, normals[ i ] ) * normals[ i ];
                float projectedDistance = projectedDiff.Length();

                // Test for early out.
                if ( projectedDistance > halfLengthOne + halfLengthTwo )
                {
                    return false;
                }
            }

            // We tested every normal axis,
            // we must be in intersection!
            return true;*/
        }

        public bool Intersects( SdcCircle circle )
        {
            //Transform the half measures
            Vector2 halfWidthVect = Vector2.Transform( HalfWidth * Vector2.UnitX, Matrix2.CreateRotationZ( RotationAngle ) );
            Vector2 halfHeightVect = Vector2.Transform( HalfHeight * Vector2.UnitY, Matrix2.CreateRotationZ( RotationAngle ) );

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
