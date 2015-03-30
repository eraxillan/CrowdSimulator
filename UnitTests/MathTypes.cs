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
}
