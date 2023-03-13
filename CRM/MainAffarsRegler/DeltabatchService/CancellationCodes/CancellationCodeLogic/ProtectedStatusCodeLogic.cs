

using Skanetrafiken.Crm.Schema.Generated;

namespace Endeavor.Crm.DeltabatchService.CancellationCodes.CancellationCodeLogic
{
    /// <summary>
    /// Handles protected status code by clearing all address related fields. The handle method:
    /// 1. Clears all address 1, 2 and 3 related fields - if contact is ed_serviceresor == true
    /// 2. Sets adress 1 line 2 to "SEKRETESSKYDD", and
    /// 3. Sets credit safe fields.
    /// </summary>
    public class ProtectedStatusCodeLogic : ICancellationCodeLogic
    {
        private readonly IDateTimeProvider _dateTimeProvider;

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

        public ProtectedStatusCodeLogic(IDateTimeProvider dateTimeProvider)
        {
            _dateTimeProvider = dateTimeProvider;
        }

        public void HandleStatusCode(Contact contact, DeltaBatchUpdateRow updatedContactData)
        {
            if(contact.ed_Serviceresor != true)
            {
                return;
            }

            foreach (var field in _addressFieldsForClearing)
            {
                contact[field] = null;
            }

            contact.FirstName = "x";
            contact.LastName = "y";
            contact.ed_CreditsafeRejectionCode = ed_creditsaferejectcodes.Protected;
            contact.ed_CreditsafeRejectionText = "Skyddad";
            contact.ed_CreditsafeRejectionComment = _dateTimeProvider.Now().ToString("yyyy-MM-dd");
        }
    }
}
