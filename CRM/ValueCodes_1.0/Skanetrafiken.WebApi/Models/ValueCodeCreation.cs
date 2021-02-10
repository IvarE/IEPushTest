using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Skanetrafiken.Crm.Models
{
    /// <summary>
    /// Model for creating value code
    /// </summary>
    public class ValueCodeCreationGiftCard
    {
        /// <summary>
        /// The amount of which the value code is worth.
        /// </summary>
        public int Amount { get; set; }

        /// <summary>
        /// User's email address
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// User's mobile number
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// Decides which method the value code shall be delivered.
        /// 1 = Email, 2 = SMS
        /// </summary>
        public int deliveryMethod { get; set; }

        /// <summary>
        /// User's account id from Skånetrafiken. This indicates whether the user is logged in or not.
        /// </summary>
        public Guid? ContactId { get; set; }

        /// <summary>
        /// User's travel card
        /// </summary>
        public TravelCard TravelCard { get; set; }

        /// <summary>
        /// First name of the customer
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Last name of the customer
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// 1=Förseningsersättning (borde inte komma hit) 2=Presentkort (med saldo/blockera kort) 3=Förlustgaranti
        /// </summary>
        public int TypeOfValueCode { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class TravelCard
    {
        /// <summary>
        /// Unique travel card number
        /// </summary>
        public string TravelCardNumber { get; set; }

        /// <summary>
        /// Last three numbers on the card
        /// </summary>
        public string CVC { get; set; }
    }

    /// <summary>
    /// Model for creating value code for Loss Compensation (förlustersättning)
    /// </summary>
    public class ValueCodeLossCreation
    {
        /// <summary>
        /// Travel Card
        /// </summary>
        public TravelCard TravelCard { get; set; }

        /// <summary>
        /// User's email address
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// User's mobile number
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// Decides which method the value code shall be delivered.
        /// 1 = Email, 2 = SMS
        /// </summary>
        public int deliveryMethod { get; set; }

        /// <summary>
        /// User's account id from Skånetrafiken. This indicates whether the user is logged in or not.
        /// </summary>
        public Guid? ContactId { get; set; }
        
        /// <summary>
        /// First name of the customer
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Last name of the customer
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// 1=Förseningsersättning (borde inte komma hit) 2=Presentkort (med saldo/blockera kort) 3=Förlustgaranti
        /// </summary>
        public int TypeOfValueCode { get; set; }
    }

}