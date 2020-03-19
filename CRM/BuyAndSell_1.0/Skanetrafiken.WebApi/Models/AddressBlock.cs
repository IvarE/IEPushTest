using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Skanetrafiken.Crm.Models
{
    public class Person
    {
        public string Email { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }

    }
    public class AddressBlock
    {
        public string City { get; set; }
        public string Co { get; set; }
        public string CountryIso { get; set; }
        public string Line1 { get; set; }
        public string PostalCode { get; set; }
        public Person Person { get; set; }

    }
}