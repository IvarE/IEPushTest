
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
        private Guid ContactGuid;
        //AccountNumber
        private string portalId;

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
        public Guid ContactId
        {
            get
            {
                return this.ContactGuid;
            }
            set
            {
                this.ContactGuid = value;
            }
        }

        /// <summary>
        /// TODO
        /// </summary>
        public string PortalId
        {
            get
            {
                return this.portalId;
            }
            set
            {
                this.portalId = value;
            }
        }
    }
}
