
using Microsoft.Xrm.Sdk.Query;
using Skanetrafiken.Crm.Entities;
using System;
using System.Collections.Generic;

namespace Skanetrafiken.Crm
{
    public class AccountInfo
    {

        private int informationSourceField;
        private string guidField;
        private string accountnumber;
        private string organizationName;
        private string cgi_organizational_number;
        private int? ed_PaymentMethod;
        private bool? ed_IsLockedPortal;
        private string EMailAddress1;
        private string ed_CostSite;
        private string ed_BillingEmailAddress;
        private int? ed_BillingMethod;
        private bool? ed_AllowCreate;
        private string ed_ReferencePortal;
        private string ed_Suborgname;
        private string ed_AccountDescription;

        private List<AddressInfo> addresses;

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
        public string Guid
        {
            get
            {
                return this.guidField;
            }
            set
            {
                this.guidField = value;
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

        /// <summary>
        /// TODO
        /// </summary>
        public string ReferencePortal
        {
            get
            {
                return this.ed_ReferencePortal;
            }
            set
            {
                this.ed_ReferencePortal = value;
            }
        }

        /// <summary>
        /// TODO
        /// </summary>
        public string OrganizationName
        {
            get
            {
                return this.organizationName;
            }
            set
            {
                this.organizationName = value;
            }
        }

        /// <summary>
        /// TODO
        /// </summary>
        public string OrganizationNumber
        {
            get
            {
                return this.cgi_organizational_number;
            }
            set
            {
                this.cgi_organizational_number = value;
            }
        }

        /// <summary>
        /// TODO
        /// </summary>
        public int? PaymentMethod
        {
            get
            {
                return this.ed_PaymentMethod;
            }
            set
            {
                this.ed_PaymentMethod = value;
            }
        }

        /// <summary>
        /// TODO
        /// </summary>
        public string PaymentMethodString
        {
            get
            {
                if (!this.PaymentMethod.HasValue)
                    return String.Empty;
                if (Enum.IsDefined(typeof(Crm.Schema.Generated.ed_account_ed_paymentmethod), this.PaymentMethod))
                    return ((Crm.Schema.Generated.ed_account_ed_paymentmethod)this.PaymentMethod).ToString();
                return String.Empty;
            }

        }

        /// <summary>
        /// TODO
        /// </summary>
        public bool? IsLockedPortal
        {
            get
            {
                return this.ed_IsLockedPortal;
            }
            set
            {
                this.ed_IsLockedPortal = value;
            }
        }

        /// <summary>
        /// TODO
        /// </summary>
        public bool? AllowCreate
        {
            get
            {
                return this.ed_AllowCreate;
            }
            set
            {
                this.ed_AllowCreate = value;
            }
        }

        /// <summary>
        /// TODO
        /// </summary>
        public string EMail
        {
            get
            {
                return this.EMailAddress1;
            }
            set
            {
                this.EMailAddress1 = value;
            }
        }

        /// <summary>
        /// TODO
        /// </summary>
        public string CostSite
        {
            get
            {
                return this.ed_CostSite;
            }
            set
            {
                this.ed_CostSite = value;
            }
        }

        /// <summary>
        /// TODO
        /// </summary>
        public string BillingEmailAddress
        {
            get
            {
                return this.ed_BillingEmailAddress;
            }
            set
            {
                this.ed_BillingEmailAddress = value;
            }
        }

        /// <summary>
        /// TODO
        /// </summary>
        public int? BillingMethod
        {
            get
            {
                return this.ed_BillingMethod;
            }
            set
            {
                this.ed_BillingMethod = value;
            }
        }

        /// <summary>
        /// TODO
        /// </summary>
        public string BillingMethodString
        {
            get
            {
                if (!this.BillingMethod.HasValue)
                    return String.Empty;
                if (Enum.IsDefined(typeof(Crm.Schema.Generated.ed_account_ed_billingmethod), this.BillingMethod))
                    return ((Crm.Schema.Generated.ed_account_ed_billingmethod)this.BillingMethod).ToString();
                return String.Empty;
            }

        }

        public string DisplayName
        {
            get
            {
                return this.ed_Suborgname;
            }
            set
            {
                this.ed_Suborgname = value;
            }
        }

        /// <summary>
        /// TODO
        /// </summary>
        public List<AddressInfo> Addresses
        {
            get
            {
                return this.addresses;
            }
            set
            {
                this.addresses = value;
            }
        }

        public AccountInfo()
        {

        }

        internal static AccountEntity GetAccountEntityFromAccountInfo(AccountInfo ai)
        {
            AccountEntity ae = new AccountEntity();

            ae.AccountNumber = ai.PortalId;

            //if (String.IsNullOrEmpty(ai.ed_CostSite))
            //    ae.Name = ai.ed_CostSite;
            //else
            //    ae.Name = ai.cgi_organizational_number + " - Kostnadsställe";

            //ae.Name = $"Kostnadsställe - {ai.OrganizationName}"; //Gamla
            //ae.Name = $"{ai.OrganizationName} - KST"; //ändra namn

            if (ai.DisplayName != null && ai.DisplayName != string.Empty)
            {
                ae.Name = ai.DisplayName;
                //ed_SubOrgNamn ska ej sättas längre, används av användare i systemet
                //ae.ed_SubOrgNamn = ai.DisplayName; //Inget skickas från fasaden ännu
            }
            else
            {
                ae.Name = $"{ai.OrganizationName} - KST"; //ändra namn
            }

            if (ai.InformationSource == (int)Schema.Generated.ed_informationsource.ForetagsPortal)
            {
                ae.ed_PortalCustomer = true;
                ae.cgi_DebtCollection = true;
                ae.AccountCategoryCode = Schema.Generated.account_accountcategorycode.Business;//optionset
            }
            else if (ai.InformationSource == (int)Schema.Generated.ed_informationsource.SeniorPortal)
            {
                ae.ed_SeniorCustomer = true;
                ae.cgi_DebtCollection = true;
                ae.AccountCategoryCode = Schema.Generated.account_accountcategorycode.Senior;
            }
            else if (ai.InformationSource == (int)Schema.Generated.ed_informationsource.SkolPortal)
            {
                ae.ed_SchoolCustomer = true;
                ae.cgi_DebtCollection = true;
                ae.AccountCategoryCode = Schema.Generated.account_accountcategorycode.School;
            }

            //ae.cgi_organizational_number = ai.OrganizationNumber; //verifiera skapandet via portal
            ae.ed_PaymentMethod = (Crm.Schema.Generated.ed_account_ed_paymentmethod?)ai.PaymentMethod;
            ae.ed_IsLockedPortal = ai.IsLockedPortal;
            ae.EMailAddress1 = ai.EMail;
            ae.ed_CostSite = ai.CostSite;
            ae.ed_BillingEmailAddress = ai.BillingEmailAddress;
            ae.ed_BillingMethod = (Crm.Schema.Generated.ed_account_ed_billingmethod?)ai.BillingMethod;
            ae.ed_ReferencePortal = ai.ReferencePortal;
            //ae.cgi_organizational_number = ai.OrganizationNumber;
            ae.ed_AllowCreate = ai.AllowCreate;

            //Account Description
            ae.ed_AccountDescription = ai.ed_AccountDescription;

            return ae;
        }

        internal static bool GetChangedAccountEntity(AccountEntity oldAccount, AccountInfo accountInfo, ref AccountEntity newAccount)
        {
            bool isChanged = false;
            newAccount.Id = oldAccount.Id;

            if (!string.IsNullOrEmpty(accountInfo.PortalId) && oldAccount.AccountNumber != accountInfo.PortalId)
            {
                newAccount.AccountNumber = accountInfo.PortalId;
                isChanged = true;
            }

            if (!string.IsNullOrEmpty(accountInfo.DisplayName) && oldAccount.Name != accountInfo.DisplayName)
            {
                newAccount.Name = accountInfo.DisplayName;
                isChanged = true;
            }

            if (!string.IsNullOrEmpty(accountInfo.OrganizationNumber) && oldAccount.cgi_organizational_number != accountInfo.OrganizationNumber)
            {
                newAccount.cgi_organizational_number = accountInfo.OrganizationNumber;
                isChanged = true;
            }

            if (accountInfo.PaymentMethod.HasValue && (int?)oldAccount.ed_PaymentMethod != accountInfo.PaymentMethod)
            {
                newAccount.ed_PaymentMethod = (Crm.Schema.Generated.ed_account_ed_paymentmethod?)accountInfo.PaymentMethod;
                isChanged = true;
            }

            if (accountInfo.IsLockedPortal.HasValue && oldAccount.ed_IsLockedPortal != accountInfo.IsLockedPortal)
            {
                newAccount.ed_IsLockedPortal = accountInfo.IsLockedPortal;
                isChanged = true;
            }

            if (accountInfo.AllowCreate.HasValue && oldAccount.ed_AllowCreate != accountInfo.AllowCreate)
            {
                newAccount.ed_AllowCreate = accountInfo.AllowCreate;
                isChanged = true;
            }

            if (!string.IsNullOrEmpty(accountInfo.EMail) && oldAccount.EMailAddress1 != accountInfo.EMail)
            {
                newAccount.EMailAddress1 = accountInfo.EMail;
                isChanged = true;
            }

            if (!string.IsNullOrEmpty(accountInfo.CostSite) && oldAccount.ed_CostSite != accountInfo.CostSite)
            {
                newAccount.ed_CostSite = accountInfo.CostSite;
                isChanged = true;
            }

            if (!string.IsNullOrEmpty(accountInfo.BillingEmailAddress) && oldAccount.ed_BillingEmailAddress != accountInfo.BillingEmailAddress)
            {
                newAccount.ed_BillingEmailAddress = accountInfo.BillingEmailAddress;
                isChanged = true;
            }

            if (accountInfo.BillingMethod.HasValue && (int?)oldAccount.ed_BillingMethod != accountInfo.BillingMethod)
            {
                newAccount.ed_BillingMethod = (Crm.Schema.Generated.ed_account_ed_billingmethod?)accountInfo.BillingMethod;
                isChanged = true;
            }

            if (!string.IsNullOrEmpty(accountInfo.ReferencePortal) && oldAccount.ed_ReferencePortal != accountInfo.ReferencePortal)
            {
                newAccount.ed_ReferencePortal = accountInfo.ReferencePortal;
                isChanged = true;
            }
            /* //ed_SubOrgNamn ska ej sättas längre, används av användare i systemet
            if (accountInfo.DisplayName == null && !string.IsNullOrEmpty(oldAccount.ed_SubOrgNamn))
            {
                newAccount.ed_SubOrgNamn = string.Empty;
                isChanged = true;
            }
            else if (!string.IsNullOrEmpty(accountInfo.DisplayName) && oldAccount.ed_SubOrgNamn != accountInfo.DisplayName)
            {
                newAccount.ed_SubOrgNamn = accountInfo.DisplayName;
                isChanged = true;
            }
            */
            if (!string.IsNullOrEmpty(accountInfo.ed_AccountDescription) && oldAccount.ed_AccountDescription != accountInfo.ed_AccountDescription)
            {
                newAccount.ed_AccountDescription = accountInfo.ed_AccountDescription;
                isChanged = true;
            }

            return isChanged;
        }

        internal static AccountInfo GetAccountInfoFromAccount(AccountEntity ae)
        {
            AccountInfo accountInfo = new AccountInfo();

            if (ae.Id != null)
            {
                accountInfo.guidField = ae.Id.ToString();
            }

            accountInfo.PortalId = ae.AccountNumber;
            accountInfo.cgi_organizational_number = ae.cgi_organizational_number;
            accountInfo.ed_PaymentMethod = (int?)ae.ed_PaymentMethod;
            accountInfo.ed_IsLockedPortal = ae.ed_IsLockedPortal;
            accountInfo.EMailAddress1 = ae.EMailAddress1;
            accountInfo.ed_CostSite = ae.ed_CostSite;
            accountInfo.ed_BillingEmailAddress = ae.ed_BillingEmailAddress;
            accountInfo.ed_BillingMethod = (int?)ae.ed_BillingMethod;
            accountInfo.ed_AllowCreate = ae.ed_AllowCreate;
            accountInfo.DisplayName = ae.ed_SubOrgNamn;
            accountInfo.ed_AccountDescription = ae.ed_AccountDescription;

            return accountInfo;
        }
    }

    public class AddressInfo
    {

        private int? AddressTypeCodeField;
        private string NameField;
        private string StreetField;
        private string PostalCodeField;
        private string CityField;
        private string CountryISOField;
        private string COField;

        /// <summary>
        /// TODO
        /// </summary>
        public int? TypeCode
        {
            get
            {
                return this.AddressTypeCodeField;
            }
            set
            {
                this.AddressTypeCodeField = value;
            }
        }

        /// <summary>
        /// TODO
        /// </summary>
        public string CO
        {
            get
            {
                return this.COField;
            }
            set
            {
                this.COField = value;
            }
        }

        /// <summary>
        /// TODO
        /// </summary>
        public string Name
        {
            get
            {
                return this.NameField;
            }
            set
            {
                this.NameField = value;
            }
        }

        /// <summary>
        /// TODO
        /// </summary>
        public string Street
        {
            get
            {
                return this.StreetField;
            }
            set
            {
                this.StreetField = value;
            }
        }

        /// <summary>
        /// TODO
        /// </summary>
        public string PostalCode
        {
            get
            {
                return this.PostalCodeField;
            }
            set
            {
                this.PostalCodeField = value;
            }
        }

        /// <summary>
        /// TODO
        /// </summary>
        public string City
        {
            get
            {
                return this.CityField;
            }
            set
            {
                this.CityField = value;
            }
        }

        /// <summary>
        /// TODO
        /// </summary>
        public string CountryISO
        {
            get
            {
                return this.CountryISOField;
            }
            set
            {
                this.CountryISOField = value;
            }
        }

        public AddressInfo()
        {

        }

        internal static CustomerAddressEntity GetCustomerAddressEntityFromAddressInfo(AddressInfo ai)
        {
            CustomerAddressEntity cae = new CustomerAddressEntity();

            cae.AddressTypeCode = (Crm.Schema.Generated.customeraddress_addresstypecode?)ai.TypeCode;
            cae.Name = ai.Street;
            cae.Line2 = ai.Street;
            cae.PostalCode = ai.PostalCode;
            cae.City = ai.City;
            cae.Country = ai.CountryISO;
            cae.ed_CoAddress = ai.CO;

            return cae;
        }

        internal static bool GetChangedCustomerAddressEntity(CustomerAddressEntity oldAddress, AddressInfo addressInfo, ref CustomerAddressEntity newAddress)
        {
            bool isChanged = false;
            newAddress.Id = oldAddress.Id;

            if ((int?)oldAddress.AddressTypeCode != addressInfo.TypeCode)
            {
                newAddress.AddressTypeCode = (Crm.Schema.Generated.customeraddress_addresstypecode?)addressInfo.TypeCode;
                isChanged = true;
            }

            if (oldAddress.Name != addressInfo.Name)
            {
                newAddress.Name = addressInfo.Name;
                isChanged = true;
            }

            if (oldAddress.Line2 != addressInfo.Street)
            {
                newAddress.Line2 = addressInfo.Street;
                isChanged = true;
            }

            if (oldAddress.PostalCode != addressInfo.PostalCode)
            {
                newAddress.PostalCode = addressInfo.PostalCode;
                isChanged = true;
            }

            if (oldAddress.City != addressInfo.City)
            {
                newAddress.City = addressInfo.City;
                isChanged = true;
            }

            if (oldAddress.Country != addressInfo.CountryISO)
            {
                newAddress.Country = addressInfo.CountryISO;
                isChanged = true;
            }

            if (oldAddress.ed_CoAddress != addressInfo.CO)
            {
                newAddress.ed_CoAddress = addressInfo.CO;
                isChanged = true;
            }

            return isChanged;
        }

        internal static AddressInfo GetAddressInfoFromCustomerAddress(CustomerAddressEntity cae)
        {
            AddressInfo addressInfo = new AddressInfo();

            addressInfo.TypeCode = (int?)cae.AddressTypeCode;
            addressInfo.Name = cae.Name;
            addressInfo.Street = cae.Line2;
            addressInfo.PostalCode = cae.PostalCode;
            addressInfo.City = cae.City;
            addressInfo.CountryISO = cae.Country;
            addressInfo.CO = cae.ed_CoAddress;

            return addressInfo;
        }
    }
}