using AutoFixture;
using Endeavor.Crm.DeltabatchService;
using Endeavor.Crm.DeltabatchService.CancellationCodes.CancellationCodeLogic;
using Moq;
using NUnit.Framework;
using Skanetrafiken.Crm.Schema.Generated;
using System;
using System.Globalization;

namespace Endeavor.Crm.UnitTest.CancellationCodes
{
    [TestFixture]
    public class DeceasedStatusCodeLogicTests
    {
        private Fixture _fixture;

        [SetUp]
        public void SetUp()
        {
            _fixture = AutoFixtureFactory.CreateFixture();
        }

        [Test]
        public void HandleStatusCode_ShouldParseDeceasedDate_IfDateIsProvided()
        {
            // Arrange
            string dateString = "2022-01-01";
            var contactData = _fixture.Build<DeltaBatchUpdateRow>().With(row => row.RejectComment, dateString).Create();
            Contact contact = new Contact();

            var handler = new DeceasedStatusCodeLogic();

            // Act
            handler.HandleStatusCode(contact, contactData);

            // Assert
            var deceasedDate = DateTime.Parse(dateString);
            Assert.AreEqual(contact.ed_DeceasedDate?.Year, deceasedDate.Year);
            Assert.AreEqual(contact.ed_DeceasedDate?.Month, deceasedDate.Month);
            Assert.AreEqual(contact.ed_DeceasedDate?.Day, deceasedDate.Day);
            Assert.True(contact.ed_Deceased);
        }

        [Test]
        public void HandleStatusCode_ShouldHaveValidDeceasedDate_IfDateIsNotProvided()
        {
            // Arrange
            var contactData = _fixture.Build<DeltaBatchUpdateRow>().With(row => row.RejectComment, () => null).Create();
            Contact contact = new Contact();

            var handler = new DeceasedStatusCodeLogic();

            // Act
            handler.HandleStatusCode(contact, contactData);

            // Assert
            Assert.IsNotNull(contact.ed_DeceasedDate);
        }
    }
}
