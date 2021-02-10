
using Microsoft.Xrm.Sdk.Query;
using Skanetrafiken.Crm.Entities;
using System;
using System.Collections.Generic;

namespace Skanetrafiken.Crm
{
    public class SkaKortInfo
    {
        /// <summary>
        /// Specifies what the call is related to. 1 = KopOchSkicka | 2 = ForetagsPortal.
        /// </summary>
        private int informationSourceField;
        
        /// <summary>
        /// Number of Reskort
        /// </summary>
        private string cardNumberField;
        
        /// <summary>
        /// Name of Reskort
        /// </summary>
        private string cardNameField;
        
        /// <summary>
        /// Record Id of Contact
        /// </summary>
        private Guid ContactGuidField;
        
        /// <summary>
        /// Account Number of Account
        /// </summary>
        private string portalIdField;

        /// <summary>
        /// Type of operation
        /// </summary>
        /// <example>0 = Register | 1 = Delete | 2 = Revoke</example>
        private Operation operationField;

        /// <summary>
        /// Specifies what the call is related to. 1 = KopOchSkicka | 2 = ForetagsPortal.
        /// </summary>
        /// <example>1 = KopOchSkicka | 2 = ForetagsPortal</example>
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
        /// Number of Reskort
        /// </summary>
        /// <example>5927183216</example>
        public string CardNumber
        {
            get
            {
                return this.cardNumberField;
            }
            set
            {
                this.cardNumberField = value;
            }
        }

        /// <summary>
        /// Name of Reskort
        /// </summary>
        /// <example>Marks Reskort</example>
        public string CardName
        {
            get
            {
                return this.cardNameField;
            }
            set
            {
                this.cardNameField = value;
            }
        }

        /// <summary>
        /// Record Id of Contact
        /// </summary>
        /// <example>63dd0fac-35eb-4c66-b39f-42ff05ff3723</example>
        public Guid ContactId
        {
            get
            {
                return this.ContactGuidField;
            }
            set
            {
                this.ContactGuidField = value;
            }
        }

        /// <summary>
        /// Account Number of Account
        /// </summary>
        /// <example>12345</example>
        public string PortalId
        {
            get
            {
                return this.portalIdField;
            }
            set
            {
                this.portalIdField = value;
            }
        }

        /// <summary>
        /// Type of operation. 0 = Register | 1 = Delete | 2 = Revoke.
        /// </summary>
        /// <example>0 = Register | 1 = Delete | 2 = Revoke</example>
        public Operation Operation
        {
            get
            {
                return this.operationField;
            }
            set
            {
                this.operationField = value;
            }
        }

    }
    
    public enum Operation : ushort
    {
        Register = 0,
        Delete = 1,
        Revoke = 2
    }
}
