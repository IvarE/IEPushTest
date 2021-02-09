using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xrm.Sdk;
using CRM2013.SkanetrafikenPlugins;



namespace SkanetrafikenPluginsUnitTests
{
    [TestClass]
    public class Refund_Update_Test
    {
        [TestMethod]
        public void GiftCodeHasExpiredTest_NoExpiryDateOnRefund()
        {
            //arrange
            Entity refund = new Entity();
            var expected = false;

            //act
            var actual = refund.GiftCodeHasExpired();

            //assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GiftCodeHasExpiredTest_ExpiryDateIsNull()
        {
            //arrange
            Entity refund = new Entity();
            refund["cgi_last_valid"] = null;
            var expected = false;

            //act
            var actual = refund.GiftCodeHasExpired();

            //assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GiftCodeHasExpiredTest_ExpiryDateHasPassed()
        {
            //arrange
            Entity refund = new Entity();
            refund["cgi_last_valid"] = new DateTime(2000, 1, 1);

            var expected = true;

            //act
            var actual = refund.GiftCodeHasExpired();

            //assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GiftCodeHasExpiredTest_ExpiryDateHasPassedRecently()
        {
            //arrange
            Entity refund = new Entity();
            refund["cgi_last_valid"] = new DateTime(2015, 9, 18,9,20,0);

            var expected = true;

            //act
            var actual = refund.GiftCodeHasExpired();

            //assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GiftCodeHasExpiredTest_ExpiryDateInTheDistantFuture()
        {
            //arrange
            Entity refund = new Entity();
            refund["cgi_last_valid"] = new DateTime(2100, 1, 1);

            var expected = false;

            //act
            var actual = refund.GiftCodeHasExpired();

            //assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GiftCodeHasBeenUsedTest_NoUsedDateOnRefund()
        {
            //arrange
            Entity refund = new Entity();

            var expected = false;

            //act
            var actual = refund.GiftCodeHasBeenUsed();

            //assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GiftCodeHasBeenUsedTest_UsedDateIsNull()
        {
            //arrange
            Entity refund = new Entity();
            refund["cgi_value_code_used"] = null;

            var expected = false;

            //act
            var actual = refund.GiftCodeHasBeenUsed();

            //assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GiftCodeHasBeenUsedTest_UsedDateHasPassed()
        {
            //arrange
            Entity refund = new Entity();
            refund["cgi_value_code_used"] = new DateTime(2000, 1, 1);

            var expected = true;

            //act
            var actual = refund.GiftCodeHasBeenUsed();

            //assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GiftCodeHasBeenUsedTest_UsedDateHasPassedRecently()
        {
            //arrange
            Entity refund = new Entity();
            refund["cgi_value_code_used"] = new DateTime(2015, 9, 18, 9, 56, 0);

            var expected = true;

            //act
            var actual = refund.GiftCodeHasBeenUsed();

            //assert
            Assert.AreEqual(expected, actual);
        }

    }
}
