using AutoFixture;
using Endeavor.Crm.DeltabatchService;
using Endeavor.Crm.DeltabatchService.CancellationCodes.CancellationCodeLogic;
using Moq;
using NUnit.Framework;
using Skanetrafiken.Crm.Schema.Generated;
using System;

namespace Endeavor.Crm.UnitTest.CancellationCodes
{
    [TestFixture]
    public class ProtectedStatusCodeLogicTests
    {
        private Fixture _fixture;
        private Mock<IDateTimeProvider> _dateTimeProvider;

        [SetUp]
        public void InitTests()
        {
            _fixture = AutoFixtureFactory.CreateFixture();
            _dateTimeProvider = new Mock<IDateTimeProvider>();
            _dateTimeProvider.Setup(dtp => dtp.Now()).Returns(DateTime.Now);
        }

        #region AddressList
        // Excludes ed_Address1_CommunityNumber
        private readonly string[] _addressFieldsForClearing = new string[]
        {
            "address1_addresstypecode",
            "address1_city",
            "address1_composite",
            "address1_country",
            "address1_county",
            "address1_fax",
            "address1_freighttermscode",
            "address1_latitude",
            "address1_line1",
            "address1_line2",
            "address1_line3",
            "address1_longitude",
            "address1_name",
            "address1_postalcode",
            "address1_postofficebox",
            "address1_primarycontactname",
            "address1_shippingmethodcode",
            "address1_stateorprovince",
            "address1_telephone1",
            "address1_telephone2",
            "address1_telephone3",
            "address1_upszone",
            "address1_utcoffset",
            "address2_addresstypecode",
            "address2_city",
            "address2_composite",
            "address2_country",
            "address2_county",
            "address2_fax",
            "address2_freighttermscode",
            "address2_latitude",
            "address2_line1",
            "address2_line2",
            "address2_line3",
            "address2_longitude",
            "address2_name",
            "address2_postalcode",
            "address2_postofficebox",
            "address2_primarycontactname",
            "address2_shippingmethodcode",
            "address2_stateorprovince",
            "address2_telephone1",
            "address2_telephone2",
            "address2_telephone3",
            "address2_upszone",
            "address2_utcoffset",
            "address3_addresstypecode",
            "address3_city",
            "address3_composite",
            "address3_country",
            "address3_county",
            "address3_fax",
            "address3_freighttermscode",
            "address3_latitude",
            "address3_line1",
            "address3_line2",
            "address3_line3",
            "address3_longitude",
            "address3_name",
            "address3_postalcode",
            "address3_postofficebox",
            "address3_primarycontactname",
            "address3_shippingmethodcode",
            "address3_stateorprovince",
            "address3_telephone1",
            "address3_telephone2",
            "address3_telephone3",
            "address3_upszone",
            "address3_utcoffset",

            "ed_address1_community",
            "ed_address1_communityopt",
            "ed_address1_country",
            "ed_address1_countynumber"
        };
        #endregion

        [Test]
        public void HandleStatusCode_StatusCodeProtected_ShouldClearAddressFields()
        {
            // Arrange
            Contact contact = _fixture.Build<Contact>().With(c => c.ed_Serviceresor, true).Create();
            DeltaBatchUpdateRow row = new DeltaBatchUpdateRow();

            ICancellationCodeLogic handler = new ProtectedStatusCodeLogic(_dateTimeProvider.Object);
            string address1_line2 = contact.Address1_Line2;

            // Act
            handler.HandleStatusCode(contact, row);

            // Assert
            Assert.AreNotEqual(address1_line2, contact.Address1_Line2);
            foreach (var field in _addressFieldsForClearing)
            {
                if (field == Contact.Fields.Address1_Line2)
                {
                    continue;
                }

                Assert.IsNull(contact[field]);
            }
        }

        /// <summary>
        /// Test a few non-address related fields.
        /// </summary>
        [Test]
        public void HandleStatusCode_StatusCodeProtected_ShouldNotClearNonAddressFields()
        {
            // Arrange
            Contact contact = _fixture.Build<Contact>().With(c => c.ed_Serviceresor, true).Create();
            DeltaBatchUpdateRow row = new DeltaBatchUpdateRow();

            ICancellationCodeLogic handler = new ProtectedStatusCodeLogic(_dateTimeProvider.Object);

            // Act
            handler.HandleStatusCode(contact, row);

            // Assert
            Assert.IsNotNull(contact.ed_Address1_CommunityNumber);
            Assert.IsNotNull(contact.EMailAddress1);
            Assert.IsNotNull(contact.ContactId);
            Assert.IsNotNull(contact.FirstName);
        }

        [Test]
        public void HandleStatusCode_StatusCodeProtected_ShouldSetSpecialValues()
        {
            // Arrange
            Contact contact = _fixture.Build<Contact>()
                                      .With(c => c.ed_CreditsafeRejectionCode, () => null)
                                      .With(c => c.ed_CreditsafeRejectionComment, () => null)
                                      .With(c => c.ed_CreditsafeRejectionText, () => null)
                                      .With(c => c.Address1_Line2, () => null)
                                      .With(c => c.ed_Serviceresor, true)
                                      .Create();
            DeltaBatchUpdateRow row = new DeltaBatchUpdateRow();

            DateTime now = DateTime.Now;
            _dateTimeProvider.Setup(dtp => dtp.Now()).Returns(now);
            ICancellationCodeLogic handler = new ProtectedStatusCodeLogic(_dateTimeProvider.Object);

            // Act
            handler.HandleStatusCode(contact, row);

            // Assert
            Assert.AreEqual("x", contact.FirstName);
            Assert.AreEqual("y", contact.LastName);
            Assert.IsTrue(string.IsNullOrEmpty(contact.Address1_Line2));
            Assert.AreEqual(ed_creditsaferejectcodes.Protected, contact.ed_CreditsafeRejectionCode);
            Assert.AreEqual("Skyddad", contact.ed_CreditsafeRejectionText);
            Assert.AreEqual(now.ToString("yyyy-MM-dd"), contact.ed_CreditsafeRejectionComment);
        }

        [Test]
        public void HandleStatusCode_StatusCodeProtected_ShouldNotClearCommunityNumber()
        {
            // Arrange
            int testCommunityNumber = 80;
            Contact contact = _fixture.Build<Contact>()
                                      .With(c => c.ed_Address1_CommunityNumber, testCommunityNumber)
                                      .With(c => c.ed_Serviceresor, true)
                                      .Create();

            DeltaBatchUpdateRow row = new DeltaBatchUpdateRow();
            ICancellationCodeLogic handler = new ProtectedStatusCodeLogic(_dateTimeProvider.Object);

            // Act
            handler.HandleStatusCode(contact, row);

            // Assert
            Assert.AreEqual(testCommunityNumber, contact.ed_Address1_CommunityNumber);
        }
    }
}
