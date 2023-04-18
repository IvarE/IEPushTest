using AutoFixture;
using Endeavor.Crm.DeltabatchService.CancellationCodes.CancellationCodeLogic;
using Endeavor.Crm.DeltabatchService;
using NUnit.Framework;
using Skanetrafiken.Crm.Schema.Generated;

namespace Endeavor.Crm.UnitTest.CancellationCodes
{
    [TestFixture]
    public class EmigratedStatusCodeLogicTests
    {
        private Fixture _fixture;

        [SetUp]
        public void SetUp()
        {
            _fixture = AutoFixtureFactory.CreateFixture();
        }

        [Test]
        public void HandleStatusCode_ShouldSetAddress2Fields()
        {
            // Arrange
            var contactData = _fixture.Build<DeltaBatchUpdateRow>().Create();
            Contact contact = new Contact();

            var handler = new EmigratedStatusCodeLogic();

            // Act
            handler.HandleStatusCode(contact, contactData);

            // Assert
            Assert.AreEqual(contactData.SpecialCo, contact.Address2_Line1);
            Assert.AreEqual(contactData.SpecialPostalCode, contact.Address2_PostalCode);
            Assert.AreEqual(contactData.SpecialCountry, contact.Address2_Country);
            Assert.AreEqual(contactData.SpecialCity, contact.Address2_City);
            Assert.AreEqual(contactData.SpecialRegAddr, contact.Address2_Line2);
        }
    }
}
