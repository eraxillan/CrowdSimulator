using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using SigmaDC.Common.MathEx;

namespace UnitTests
{
    [TestClass]
    public class MathUtilsTests
    {
        [TestMethod]
        public void Test_ToDegrees()
        {
            // TODO:
        }

        [TestMethod]
        public void Test_ToRadians()
        {
            // TODO:
        }

        [TestMethod]
        public void Test_WrapAngle()
        {
            // TODO:
        }

        [TestMethod]
        public void Test_SqrMethod()
        {
            // Arrange

            // Act
            var resultPositive = MathUtils.Sqr( 2.5f );
            var resultNegative = MathUtils.Sqr( -1.5f );
            var resultNull = MathUtils.Sqr( 0.0f );

            // Assert
            Assert.AreEqual( 6.25f, resultPositive );
            Assert.AreEqual( 2.25f, resultNegative );
            Assert.AreEqual( 0.0f, resultNull );
        }

        [TestMethod]
        public void Test_MinVec()
        {
            // Arrange

            // Act
            var resultTwoZeros = MathUtils.MinVec(0.0f, 0.0f);
            var resultTwoPositives = MathUtils.MinVec( 1.5f, 2.5f );
            var resultTwoNegatives = MathUtils.MinVec( -1.5f, -2.5f );
            var resultNegativeAndPositive = MathUtils.MinVec( 1.5f, -2.5f );
            var resultSeveralZeros = MathUtils.MinVec( 0.0f, 0.0f, 0.0f );
            var resultSeveralPositives = MathUtils.MinVec( 5.5f, 1.5f, 2.5f );
            var resultSeveralNegatives = MathUtils.MinVec( -1.5f, -5.5f, -2.5f );
            var resultSeveralNegativeAndPositives = MathUtils.MinVec( 1.5f, -2.5f, 5.5f );

            // Assert
            Assert.AreEqual( 0.0f, resultTwoZeros );
            Assert.AreEqual( 1.5f, resultTwoPositives );
            Assert.AreEqual( -2.5f, resultTwoNegatives );
            Assert.AreEqual( -2.5f, resultNegativeAndPositive );
            Assert.AreEqual( 0.0f, resultSeveralZeros );
            Assert.AreEqual( 1.5f, resultSeveralPositives );
            Assert.AreEqual( -5.5f, resultSeveralNegatives );
            Assert.AreEqual( -2.5f, resultSeveralNegativeAndPositives );
        }

        [TestMethod]
        public void Test_IsPowerOfTwo()
        {
            // Arrange

            // Act
            var resultNegative = MathUtils.IsPowerOfTwo( -5 );
            var resultPositivePowerOfTwo = MathUtils.IsPowerOfTwo( 32 );
            var resultPositiveNonPowerOfTwo = MathUtils.IsPowerOfTwo( 29 );

            // Assert
            Assert.AreEqual( false, resultNegative );
            Assert.AreEqual( true, resultPositivePowerOfTwo );
            Assert.AreEqual( false, resultPositiveNonPowerOfTwo );
        }

        [TestMethod]
        public void Test_NearlyEqualFloat()
        {
            // Arrange

            // Act
            var resultZeros = MathUtils.NearlyEqual( 0.0f, 0.0001f );
            var resultNegativeEqual = MathUtils.NearlyEqual( -3.14159f, -3.14158f );
            var resultPositiveEqual = MathUtils.NearlyEqual( 3.14159f, 3.14158f );
            var resultPosiviteAndNegativeNonEqual = MathUtils.NearlyEqual( 3.14159f, -3.14158f );
            var resultNegativeNonEqual = MathUtils.NearlyEqual( -3.14159f, -1.57f );
            var resultPositiveNonEqual = MathUtils.NearlyEqual( 3.14159f, 1.57f );

            // Assert
            Assert.AreEqual( true, resultZeros );
            Assert.AreEqual( true, resultNegativeEqual );
            Assert.AreEqual( true, resultPositiveEqual );
            Assert.AreEqual( false, resultPosiviteAndNegativeNonEqual );
            Assert.AreEqual( false, resultNegativeNonEqual );
            Assert.AreEqual( false, resultNegativeNonEqual );
        }

        [TestMethod]
        public void Test_NearlyEqualDouble()
        {
            // Arrange

            // Act
            var resultZeros = MathUtils.NearlyEqual( 0.0, 0.0001 );
            var resultNegativeEqual = MathUtils.NearlyEqual( -3.14159, -3.14158 );
            var resultPositiveEqual = MathUtils.NearlyEqual( 3.14159, 3.14158 );
            var resultPosiviteAndNegativeNonEqual = MathUtils.NearlyEqual( 3.14159, -3.14158 );
            var resultNegativeNonEqual = MathUtils.NearlyEqual( -3.14159, -1.57 );
            var resultPositiveNonEqual = MathUtils.NearlyEqual( 3.14159, 1.57 );

            // Assert
            Assert.AreEqual( true, resultZeros );
            Assert.AreEqual( true, resultNegativeEqual );
            Assert.AreEqual( true, resultPositiveEqual );
            Assert.AreEqual( false, resultPosiviteAndNegativeNonEqual );
            Assert.AreEqual( false, resultNegativeNonEqual );
            Assert.AreEqual( false, resultNegativeNonEqual );
        }

        [TestMethod]
        public void Test_NearlyZeroFloat()
        {
            // Arrange

            // Act
            var resultZeros = MathUtils.NearlyZero( 0.0f );
            var resultNegativeEqual = MathUtils.NearlyZero( -0.0005f );
            var resultPositiveEqual = MathUtils.NearlyZero( 0.0005f );
            var resultNegativeNonEqual = MathUtils.NearlyZero( -3.14159f );
            var resultPositiveNonEqual = MathUtils.NearlyZero( 3.14159f );

            // Assert
            Assert.AreEqual( true, resultZeros );
            Assert.AreEqual( true, resultNegativeEqual );
            Assert.AreEqual( true, resultPositiveEqual );
            Assert.AreEqual( false, resultNegativeNonEqual );
            Assert.AreEqual( false, resultPositiveNonEqual );
        }

        [TestMethod]
        public void Test_NearlyZeroDouble()
        {
            // Arrange

            // Act
            var resultZeros = MathUtils.NearlyZero( 0.0 );
            var resultNegativeEqual = MathUtils.NearlyZero( -0.0005 );
            var resultPositiveEqual = MathUtils.NearlyZero( 0.0005 );
            var resultNegativeNonEqual = MathUtils.NearlyZero( -3.14159 );
            var resultPositiveNonEqual = MathUtils.NearlyZero( 3.14159 );

            // Assert
            Assert.AreEqual( true, resultZeros );
            Assert.AreEqual( true, resultNegativeEqual );
            Assert.AreEqual( true, resultPositiveEqual );
            Assert.AreEqual( false, resultNegativeNonEqual );
            Assert.AreEqual( false, resultPositiveNonEqual );
        }
    }

    [TestClass]
    public class Vector2_Tests
    {
        [TestMethod]
        public void Vector2_Props()
        {
            // Arrange

            // Act
            var z = Vector2.Zero;
            var o = Vector2.One;
            var ux = Vector2.UnitX;
            var uy = Vector2.UnitY;

            // Assert
            Assert.AreEqual( true, z.X == 0.0f && z.Y == 0.0f );
            Assert.AreEqual( true, o.X == 1.0f && o.Y == 1.0f );
            Assert.AreEqual( true, ux.X == 1.0f && ux.Y == 0.0f );
            Assert.AreEqual( true, uy.X == 0.0f && uy.Y == 1.0f );
        }

        [TestMethod]
        public void Vector2_Minus_Operator()
        {
            //
        }

        [TestMethod]
        public void Vector2_Plus_Operator()
        {
            //
        }

        [TestMethod]
        public void Vector2_Mul_Operator()
        {
            //
        }

        [TestMethod]
        public void Vector2_Div_Operator()
        {
            //
        }

        [TestMethod]
        public void Vector2_Comparison_Operators()
        {
            //
        }

        [TestMethod]
        public void Vector2_Equals()
        {
            //
        }

        [TestMethod]
        public void Vector2_GetHashCode()
        {
            //
        }

        [TestMethod]
        public void Vector2_Negate_Test()
        {
            //
        }

        [TestMethod]
        public void Vector2_Add_Test()
        {
            //
        }

        [TestMethod]
        public void Vector2_Subtract_Test()
        {
            //
        }

        [TestMethod]
        public void Vector2_Multiply_Test()
        {
            //
        }

        [TestMethod]
        public void Vector2_Divide_Test()
        {
            //
        }

        [TestMethod]
        public void Vector2_Dot_Test()
        {
            //
        }

        [TestMethod]
        public void Vector2_Distance_Test()
        {
            // Arrange
            SdcLineSegment pq = new SdcLineSegment( new Vector2( 8.9f, 9.8f ), new Vector2( 9.6f, 10.5f ) );
            SdcLineSegment uv = new SdcLineSegment( new Vector2( 8.9f, 10.3f ), new Vector2( 11.4f, 10.3f ) );

            // Act
            float result = SdcLineSegment.Distance2D( pq, uv );
            Vector2 R = SdcLineSegment.GetIntersectionPoint( pq, uv );
            float t = Vector2.Distance( R, pq.P2 );
            result = t;

            // Assert
            Assert.IsTrue( MathUtils.NearlyEqual( result, 0.283f ) );
        }

        [TestMethod]
        public void Vector2_DistanceSquared_Test()
        {
            //
        }

        [TestMethod]
        public void Vector2_Length_Test()
        {
            //
        }

        [TestMethod]
        public void Vector2_LengthSquared_Test()
        {
            //
        }

        [TestMethod]
        public void Vector2_Min_Test()
        {
            //
        }

        [TestMethod]
        public void Vector2_Max_Test()
        {
            //
        }

        [TestMethod]
        public void Vector2_Normalize_Test()
        {
            //
        }

        [TestMethod]
        public void Vector2_Reflect_Test()
        {
            //
        }

        [TestMethod]
        public void Vector2_Transform_Test()
        {
            //
        }

        [TestMethod]
        public void Vector2_RotateAroundPoint_Test()
        {
            //
        }

        [TestMethod]
        public void Vector2_ToPoint_Test()
        {
            //
        }

        [TestMethod]
        public void Vector2_ToString_Test()
        {
            //
        }
    }

    [TestClass]
    public class Matrix2Tests
    {
        // TODO:
    }

   /* [TestClass]
    public class SdcLineSegmentTests
    {
        //
    }*/

    [TestClass]
    public class SdcCircleTests
    {
        //
    }

    [TestClass]
    public class SdcRectangleTests
    {
        //
    }

    // TODO: others, like SdcLine

    [TestClass]
    public class SdcLineSegmentTests
    {
        [TestMethod]
        public void Length_Test()
        {
            // Arrange
            var horSegm = new SdcLineSegment( new Vector2( 9.36f, 10.46f ), new Vector2( 9.64f, 10.46f ) );         // AD
            var vertSegm = new SdcLineSegment( new Vector2( 9.64f, 10.60f ), new Vector2( 9.64f, 11.60f ) );        // DC
            var commonSegmUp = new SdcLineSegment( new Vector2( 9.50f, 10.60f ), new Vector2( 9.64f, 11.60f ) );    // MC
            var commonSegmDown = new SdcLineSegment( new Vector2( 9.36f, 10.46f ), new Vector2( 9.50f, 10.60f ) );  // MA

            // Act
            float horSegmLength = horSegm.Length();
            float vertSegmLength = vertSegm.Length();
            float commonSegmUpLength = commonSegmUp.Length();
            float commonSegmDownLength = commonSegmDown.Length();

            // Assert
            Assert.IsTrue( MathUtils.NearlyEqual( horSegmLength, 0.28f ) );
            Assert.IsTrue( MathUtils.NearlyEqual( vertSegmLength, 1.0f ) );
            Assert.IsTrue( MathUtils.NearlyEqual( commonSegmUpLength, 1.01f ) );
            Assert.IsTrue( MathUtils.NearlyEqual( commonSegmDownLength, 0.198f ) );
        }

        [TestMethod]
        public void GetBoundingBox_Test()
        {
            //
        }

        [TestMethod]
        public void Contains_Test()
        {
            // Arrange
            var pt1 = new Vector2( 9.5f, 10.6f );
            var horLine = new SdcLineSegment( new Vector2( 9.36f, 10.6f ), new Vector2( 9.36f, 10.6f ) );

            var pt2 = new Vector2( 9.36f, 11.1f );
            var vertLine = new SdcLineSegment( new Vector2( 9.36f, 10.6f ), new Vector2( 9.36f, 11.6f ) );

            // Act
            bool bHor = horLine.Contains( pt1 );
            bool bVert = vertLine.Contains( pt2 );

            // Assert
            Assert.IsTrue( bHor );
            Assert.IsTrue( bVert );
        }

        [TestMethod]
        public void Intersects_Test()
        {
            // Arrange
            // Visibility area rectangle for human with Id = 412, rotated by Pi/2
            var rectVisLeftSegm = new SdcLineSegment( new Vector2( 9.36f, 10.46f ), new Vector2( 9.36f, 11.6f ) );
            var rectVisTopSegm = new SdcLineSegment( new Vector2( 9.36f, 11.6f ), new Vector2( 9.64f, 11.6f ) );
            var rectVisRightSegm = new SdcLineSegment( new Vector2( 9.64f, 10.46f ), new Vector2( 9.64f, 11.6f ) );
            var rectVisBottomSegm = new SdcLineSegment( new Vector2( 9.36f, 10.46f ), new Vector2( 9.64f, 10.46f ) );

            // Test obstacle one (intersects visibility area)
            var rectObstOneLeftSegm = new SdcLineSegment( new Vector2( 5.4f, 11.5f ), new Vector2( 5.4f, 11.9f ) );
            var rectObstOneTopSegm = new SdcLineSegment( new Vector2( 5.4f, 11.9f ), new Vector2( 11.4f, 11.9f ) );
            var rectObstOneRightSegm = new SdcLineSegment( new Vector2( 11.4f, 11.5f ), new Vector2( 11.4f, 11.9f ) );
            var rectObstOneBottomSegm = new SdcLineSegment( new Vector2( 5.4f, 11.5f ), new Vector2( 11.4f, 11.5f ) );

            // Test obstacle two (does not intersect visibility area)
            var rectObstTwoLeftSegm = new SdcLineSegment( new Vector2( 8.9f, 9.7f ), new Vector2( 8.9f, 10.3f ) );
            var rectObstTwoTopSegm = new SdcLineSegment( new Vector2( 8.9f, 10.3f ), new Vector2( 11.4f, 10.3f ) );
            var rectObstTwoRightSegm = new SdcLineSegment( new Vector2( 11.4f, 9.7f ), new Vector2( 11.4f, 10.3f ) );
            var rectObstTwoBottomSegm = new SdcLineSegment( new Vector2( 8.9f, 9.7f ), new Vector2( 11.4f, 9.7f ) );

            // Act
            // Check intersection with first obstacle (two cases)               | Expected result
            var b1_1 = rectVisLeftSegm.Intersects( rectObstOneLeftSegm );       // false
            var b1_2 = rectVisLeftSegm.Intersects( rectObstOneTopSegm );        // false
            var b1_3 = rectVisLeftSegm.Intersects( rectObstOneRightSegm );      // false
            var b1_4 = rectVisLeftSegm.Intersects( rectObstOneBottomSegm );     // true

            var b2_1 = rectVisTopSegm.Intersects( rectObstOneLeftSegm );        // false
            var b2_2 = rectVisTopSegm.Intersects( rectObstOneTopSegm );         // false
            var b2_3 = rectVisTopSegm.Intersects( rectObstOneRightSegm );       // false
            var b2_4 = rectVisTopSegm.Intersects( rectObstOneBottomSegm );      // false

            var b3_1 = rectVisRightSegm.Intersects( rectObstOneLeftSegm );      // false
            var b3_2 = rectVisRightSegm.Intersects( rectObstOneTopSegm );       // false
            var b3_3 = rectVisRightSegm.Intersects( rectObstOneRightSegm );     // false
            var b3_4 = rectVisRightSegm.Intersects( rectObstOneBottomSegm );    // true

            var b4_1 = rectVisBottomSegm.Intersects( rectObstOneLeftSegm );     // false
            var b4_2 = rectVisBottomSegm.Intersects( rectObstOneTopSegm );      // false
            var b4_3 = rectVisBottomSegm.Intersects( rectObstOneRightSegm );    // false
            var b4_4 = rectVisBottomSegm.Intersects( rectObstOneBottomSegm );   // false

            // Check intersection with second obstacle (no intersection at all)
            var b5_1 = rectVisLeftSegm.Intersects( rectObstTwoLeftSegm );       // false
            var b5_2 = rectVisLeftSegm.Intersects( rectObstTwoTopSegm );        // false
            var b5_3 = rectVisLeftSegm.Intersects( rectObstTwoRightSegm );      // false
            var b5_4 = rectVisLeftSegm.Intersects( rectObstTwoBottomSegm );     // false

            var b6_1 = rectVisTopSegm.Intersects( rectObstTwoLeftSegm );        // false
            var b6_2 = rectVisTopSegm.Intersects( rectObstTwoTopSegm );         // false
            var b6_3 = rectVisTopSegm.Intersects( rectObstTwoRightSegm );       // false
            var b6_4 = rectVisTopSegm.Intersects( rectObstTwoBottomSegm );      // false

            var b7_1 = rectVisRightSegm.Intersects( rectObstTwoLeftSegm );      // false
            var b7_2 = rectVisRightSegm.Intersects( rectObstTwoTopSegm );       // false
            var b7_3 = rectVisRightSegm.Intersects( rectObstTwoRightSegm );     // false
            var b7_4 = rectVisRightSegm.Intersects( rectObstTwoBottomSegm );    // false

            var b8_1 = rectVisBottomSegm.Intersects( rectObstTwoLeftSegm );     // false
            var b8_2 = rectVisBottomSegm.Intersects( rectObstTwoTopSegm );      // false
            var b8_3 = rectVisBottomSegm.Intersects( rectObstTwoRightSegm );    // false
            var b8_4 = rectVisBottomSegm.Intersects( rectObstTwoBottomSegm );   // false

            // Assert
            // First obstacle (with two intersections)
            Assert.AreEqual( false, b1_1 );
            Assert.AreEqual( false, b1_2 );
            Assert.AreEqual( false, b1_3 );
            Assert.AreEqual( true, b1_4 );

            Assert.AreEqual( false, b2_1 );
            Assert.AreEqual( false, b2_2 );
            Assert.AreEqual( false, b2_3 );
            Assert.AreEqual( false, b2_4 );

            Assert.AreEqual( false, b3_1 );
            Assert.AreEqual( false, b3_2 );
            Assert.AreEqual( false, b3_3 );
            Assert.AreEqual( true, b3_4 );

            Assert.AreEqual( false, b4_1 );
            Assert.AreEqual( false, b4_2 );
            Assert.AreEqual( false, b4_3 );
            Assert.AreEqual( false, b4_4 );

            // Second obstacle (without intersections at all)
            Assert.AreEqual( false, b5_1 );
            Assert.AreEqual( false, b5_2 );
            Assert.AreEqual( false, b5_3 );
            Assert.AreEqual( false, b5_4 );

            Assert.AreEqual( false, b6_1 );
            Assert.AreEqual( false, b6_2 );
            Assert.AreEqual( false, b6_3 );
            Assert.AreEqual( false, b6_4 );

            Assert.AreEqual( false, b7_1 );
            Assert.AreEqual( false, b7_2 );
            Assert.AreEqual( false, b7_3 );
            Assert.AreEqual( false, b7_4 );

            Assert.AreEqual( false, b8_1 );
            Assert.AreEqual( false, b8_2 );
            Assert.AreEqual( false, b8_3 );
            Assert.AreEqual( false, b8_4 );
        }

        [TestMethod]
        public void GetIntersectionPoint_Test()
        {
            //
        }

        [TestMethod]
        public void Distance2D_Test()
        {
            // Arrange
            var horLineSegm = new SdcLineSegment( new Vector2( 9.36f, 10.6f ), new Vector2( 9.64f, 10.6f ) );
            var vertLineSegm = new SdcLineSegment( new Vector2( 5.4f, 11.5f ), new Vector2( 11.4f, 11.5f ) );

            // Act
            float dist1 = horLineSegm.Distance2D( vertLineSegm );
            float dist2 = vertLineSegm.Distance2D( horLineSegm );

            // Assert
            Assert.IsTrue( MathUtils.NearlyEqual( dist1, dist2 ) );
            Assert.IsTrue( MathUtils.NearlyEqual( dist1, 0.9f ) );
        }
    }
}
