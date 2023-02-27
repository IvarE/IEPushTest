using Moq;
using NUnit.Framework;
using System;
using Skanetrafiken.Crm.Schema.Generated;
using Endeavor.Crm.DeltabatchService.CancellationCodes.CancellationCodeLogic;

namespace Endeavor.Crm.UnitTest.CancellationCodes
{
    [TestFixture]
    public class CancellationCodeLogicFactoryTests
    {
        private CancellationCodeLogicFactory _factoryInstance;
        private Mock<IDateTimeProvider> _dateTimeProvider;

        [SetUp]
        public void SetUp()
        {
            _dateTimeProvider = new Mock<IDateTimeProvider>();
            _factoryInstance = new CancellationCodeLogicFactory(_dateTimeProvider.Object);
        }

        [Test]
        [TestCase(ed_creditsaferejectcodes.Protected, typeof(ProtectedStatusCodeLogic))]
        [TestCase(ed_creditsaferejectcodes.Deceased, typeof(DeceasedStatusCodeLogic))]
        [TestCase(ed_creditsaferejectcodes.Emigrated, typeof(EmigratedStatusCodeLogic))]
        public void GetCancellationCodeHandler_ValidStatusCode_ReturnsValidLogicInstance(ed_creditsaferejectcodes code, Type logicType)
        {
            // Arrange

            // Act
            ICancellationCodeLogic logic = _factoryInstance.GetCancellationCodeHandler(code);

            // Assert
            Assert.IsNotNull(logic);
            Assert.IsInstanceOf(logicType, logic);
        }

        [Test]
        public void GetCancellationCodeHandler_InvalidStatusCode_ReturnsNull()
        {
            // Arrange
            int invalidCode = -1;

            // Act
            var logic = _factoryInstance.GetCancellationCodeHandler((ed_creditsaferejectcodes)invalidCode);

            // Assert
            Assert.IsNull(logic);
        }
    }
}
