using System.Runtime.Serialization;

namespace CGIXrmGetOrders.Classes
{
    /*
      <Address>Gata 1B</Address>
        <City>Stad</City>
        <Co>C/O</Co>
        <CompanyName>Företagsnamn (om beställaren är en företagskund)</CompanyName>
        <Country>Land</Country>
        <CellPhoneNumber>0701234567</CellPhoneNumber>
        <Email>E-post</Email>
        <FirstName>Förnamn</FirstName>
        <LastName>Efternamn</LastName>
        <PostalCode>43125</PostalCode>
        <ExtraInfo>Här kan extra information läggas framöver</ExtraInfo>
     */

    [DataContract]
    public class ShippingAddress
    {
        #region Public Properties

        [DataMember]
        public string Address { get; set; }

        [DataMember]
        public string City { get; set; }

        [DataMember]
        public string Co { get; set; }

        [DataMember]
        public string CompanyName { get; set; }

        [DataMember]
        public string Country { get; set; }

        [DataMember]
        public string CellPhoneNumber { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public string FirstName { get; set; }

        [DataMember]
        public string LastName { get; set; }

        [DataMember]
        public string PostalCode { get; set; }

        [DataMember]
        public string ExtraInfo { get; set; }

        #endregion
    }
}