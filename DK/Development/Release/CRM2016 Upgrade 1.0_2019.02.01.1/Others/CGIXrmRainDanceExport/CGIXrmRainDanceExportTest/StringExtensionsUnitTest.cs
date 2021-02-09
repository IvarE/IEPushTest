using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CGIXrmRainDanceExport.Classes;

namespace CGIXrmRainDanceExportTest
{
    [TestClass]
    public class StringExtensionsUnitTest
    {
        [TestMethod]
        public void SetToFixedLengthTest_NullString()
        {
            //arrange
            string actual = null;
            var expected = new String(' ', 30);

            //act
            var result = actual.SetToFixedLengthPadRight(30);

            //assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void SetToFixedLengthTest_EmptyString()
        {
            //arrange
            var actual = "";
            var expected = new String(' ', 30);

            //act
            var result = actual.SetToFixedLengthPadRight(30);

            //assert
            Assert.AreEqual(expected, result);
        }
        
        [TestMethod]
        public void SetToFixedLengthTest_StringShorterThanMaxLength()
        {
            //arrange
            string actual = "Regeringsgatan 67";
            var expected = "Regeringsgatan 67             ";

            //act
            var result = actual.SetToFixedLengthPadRight(30);

            //assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void SetToFixedLengthTest_StringEqualToMaxLength()
        {
            //arrange
            string actual = "Regeringsg";
            var expected = "Regeringsg";

            //act
            var result = actual.SetToFixedLengthPadRight(10);

            //assert
            Assert.AreEqual(expected, result);
        }
        
        [TestMethod]
        public void SetToFixedLengthTest_StringEqualToMaxLengthTwo()
        {
            //arrange
            string actual = "This is a really long address exactly 60 characters asdfghjj";
            var expected = "This is a really long address exactly 60 characters asdfghjj";

            //act
            var result = actual.SetToFixedLengthPadRight(60);

            //assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void SetToFixedLengthTest_StringLongerThanMaxLength()
        {
            //arrange
            string actual = "This is a really long address exactly 60 characters asdfghjj";
            var expected = "This is a really long address ";

            //act
            var result = actual.SetToFixedLengthPadRight(30);

            //assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void SetToFixedLengthTest_StringLongerThanMaxLengthTwo()
        {
            //arrange
            string actual = "This is a really long address exactly 60 characters asdfghjj";
            var expected = "This ";

            //act
            var result = actual.SetToFixedLengthPadRight(5);

            //assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void SetToFixedLengthTest_ZeroMaxLength()
        {
            //arrange
            string actual = "Regeringsg";
            var expected = "";

            //act
            var result = actual.SetToFixedLengthPadRight(0);

            //assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentOutOfRangeException))]
        public void SetToFixedLengthTest_NegativeMaxLength()
        {
            //arrange
            string actual = "Just a random address";

            //act
            var result = actual.SetToFixedLengthPadRight(-5);

            //No assert - exception expected           
        }

    }
 }
