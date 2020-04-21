
using Microsoft.Xrm.Sdk.Query;
using Skanetrafiken.Crm.Entities;
using System;
using System.Collections.Generic;

namespace Skanetrafiken.Crm
{
    public class SkaKortInfo
    {

        //Kortnummer
        private int informationSourceField;
        //Kortnummer
        private string cardNumber;
        //Kortnamn
        private string cardName;
        //Kontakt Id
        private string contactId;
        //AccountNumber
        private string accountnumber;

        /// <summary>
        /// TODO
        /// </summary>
        public int InformationSource
        {
            get
            {
                return this.informationSourceField;
            }
            set
            {
                this.informationSourceField = value;
            }
        }

        /// <summary>
        /// TODO
        /// </summary>
        public string CardNumber
        {
            get
            {
                return this.cardNumber;
            }
            set
            {
                this.cardNumber = value;
            }
        }

        /// <summary>
        /// TODO
        /// </summary>
        public string CardName
        {
            get
            {
                return this.cardName;
            }
            set
            {
                this.cardName = value;
            }
        }

        /// <summary>
        /// TODO
        /// </summary>
        public string ContactId
        {
            get
            {
                return this.contactId;
            }
            set
            {
                this.contactId = value;
            }
        }

        /// <summary>
        /// TODO
        /// </summary>
        public string PortalId
        {
            get
            {
                return this.accountnumber;
            }
            set
            {
                this.accountnumber = value;
            }
        }
    }
}
