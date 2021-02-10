using System.Runtime.Serialization;

namespace CGIXrmGetOrders.Classes
{
    /*
         <Customer>
        <Email>user@user.se</Email>
        <AccountNumber>A273A13D-746B-454E-BC66-143A012EF5E9 (Id i SeKund om kunden är finns eller är inloggad)</AccountNumber>
        <AccountNumber2>ST123456 (Kundnummer i SeKund)</AccountNumber2>
        <IsCompany>false</IsCompany>
        <ExtraInfo>Här kan extra information läggas framöver</ExtraInfo>
      </Customer>
     */

    [DataContract]
    public class Customer
    {
        #region Public Properties

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public string AccountNumber { get; set; }

        [DataMember]
        public string AccountNumber2 { get; set; }

        [DataMember]
        public bool? IsCompany { get; set; }

        #region BizTalk
        // Queried from CRM so far, not from biztalk.
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string ExtraInfo { get; set; }
        #endregion

        #endregion
    }
}