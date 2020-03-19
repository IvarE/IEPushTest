using Microsoft.Xrm.Sdk.Query;
using Skanetrafiken.Crm.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Endeavor.Crm.UnitTest
{
    /// <summary>
    /// Contains methods for interacting with CRM, that includes get/create/update/delete entities.
    /// </summary>
    public static class TestDataHelper
    {
        #region General
        private const string cFirstName = "FirstName1";
        private const string cLastName = "LastName1";

        /// <summary>
        /// Contact: Mitt Konto-Id - ed_mklid
        /// </summary>
        private const string contact_MklId = "4687935157";

        private const string cEmail = "o1046308@nwytg.com";
        private const string cMobile = "0705726487235311";

        private const string cTravelCardNumber = "72369659165";
        private const string cTravelCardName = "UnitTestCard";
        private const string cTravelCardCVC = "516";

        #endregion

        #region Get Entity Methods

        public static ContactEntity GetContactByFullName(Plugin.LocalPluginContext localContext, string firstName, string lastName)
        {
            if (string.IsNullOrEmpty(firstName))
                throw new ArgumentNullException($"{nameof(firstName)} cannot be null or empty.");

            if (string.IsNullOrEmpty(lastName))
                throw new ArgumentNullException($"{nameof(lastName)} cannot be null or empty.");

            var query = new QueryExpression()
            {
                EntityName = ContactEntity.EntityLogicalName,
                ColumnSet = new ColumnSet(true),
                Criteria =
                    {
                        Conditions =
                        {
                            new ConditionExpression(ContactEntity.Fields.FirstName, ConditionOperator.Equal, firstName),
                            new ConditionExpression(ContactEntity.Fields.LastName, ConditionOperator.Equal, lastName),
                        }
                    }
            };

            try
            {
                var contact = XrmRetrieveHelper.RetrieveFirst<ContactEntity>(localContext, query);

                return contact;
            }
            catch (Exception ex)
            {
                PrintGenericErrorMessage("Get", nameof(ContactEntity), ex.Message);
                throw;
            }

        }

        public static ContactEntity GetContactByMklId(Plugin.LocalPluginContext localContext, string mklId)
        {
            if (string.IsNullOrEmpty(mklId))
                throw new ArgumentNullException($"{nameof(mklId)} cannot be null or empty.");

            var query = new QueryExpression()
            {
                EntityName = ContactEntity.EntityLogicalName,
                ColumnSet = new ColumnSet(true),
                Criteria =
                    {
                        Conditions =
                        {
                            new ConditionExpression(ContactEntity.Fields.ed_MklId, ConditionOperator.Equal, mklId)
                        }
                    }
            };

            try
            {
                var contact = XrmRetrieveHelper.RetrieveFirst<ContactEntity>(localContext, query);

                return contact;
            }
            catch (Exception ex)
            {
                PrintGenericErrorMessage("Get", nameof(ContactEntity), ex.Message);
                throw;
            }
               
        }

        public static TravelCardEntity GetTravelCardByCardNumber(Plugin.LocalPluginContext localContext, string travelCardNumber)
        {
            if (string.IsNullOrEmpty(travelCardNumber))
                throw new ArgumentNullException($"{nameof(travelCardNumber)} cannot be null or empty.");

            var query = new QueryExpression()
            {
                EntityName = TravelCardEntity.EntityLogicalName,
                ColumnSet = new ColumnSet(true),
                Criteria =
                    {
                        Conditions =
                        {
                            new ConditionExpression(TravelCardEntity.Fields.cgi_travelcardnumber, ConditionOperator.Equal, travelCardNumber)
                        }
                    }
            };

            try
            {
                var travelCard = XrmRetrieveHelper.RetrieveFirst<TravelCardEntity>(localContext, query);
                return travelCard;
            }
            catch (Exception ex)
            {
                PrintGenericErrorMessage("Get", nameof(TravelCardEntity), ex.Message);
                throw;
            }

        }

        /// <summary>
        /// Get vlue code based on name.
        /// </summary>
        /// <param name="localContext"></param>
        /// <param name="name">Name is shares same value has CodeId.</param>
        /// <returns></returns>
        public static ValueCodeEntity GetValueCodeByName(Plugin.LocalPluginContext localContext, string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException($"Argument {nameof(name)} is null.");


            var query = new QueryExpression()
            {
                EntityName = ValueCodeEntity.EntityLogicalName,
                ColumnSet = new ColumnSet(),
                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(ValueCodeEntity.Fields.ed_name, ConditionOperator.Equal, name)
                    }
                }
            };

            try
            {
                var valueCode = XrmRetrieveHelper.RetrieveFirst<ValueCodeEntity>(localContext, query);
                return valueCode;
            }
            catch (Exception ex)
            {
                PrintGenericErrorMessage("Get", nameof(ValueCodeEntity), ex.Message);
                throw;
            }

        }

        #endregion

        #region Create Entity Methods

        /// <summary>
        /// Creates a contact.
        /// </summary>
        /// <param name="localContext"></param>
        /// <param name="email"></param>
        /// <param name="mobileNumber"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="mklId"></param>
        /// <returns></returns>
        public static ContactEntity CreateContact(Plugin.LocalPluginContext localContext, 
            string lastName, string firstName = null, string mklId = null, string mobileNumber = null, string email = null)
        {

            var contact = new ContactEntity()
            {
                LastName = lastName,
                ed_MklId = mklId,
                ed_InformationSource = Skanetrafiken.Crm.Schema.Generated.ed_informationsource.AdmAndraKund
            };

            if (!string.IsNullOrEmpty(firstName))
                contact.FirstName = firstName;
            if (!string.IsNullOrEmpty(email))
                contact.EMailAddress1 = email;
            if (!string.IsNullOrEmpty(mobileNumber))
                contact.MobilePhone = mobileNumber;

            try
            {
                contact.Id = XrmHelper.Create(localContext, contact);
                return contact;
            }
            catch (Exception ex)
            {
                PrintGenericErrorMessage("Create", nameof(ContactEntity), ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Creates a travel card.
        /// </summary>
        /// <param name="localContext"></param>
        /// <param name="travelCardNumber"></param>
        /// <param name="contact"></param>
        /// <param name="account"></param>
        /// <param name="validFrom"></param>s
        /// <param name="validTo"></param>
        /// <returns></returns>
        public static TravelCardEntity CreateTravelCard(Plugin.LocalPluginContext localContext, string travelCardNumber, string CVC, ContactEntity contact = null, AccountEntity account = null,
            DateTime? validFrom = null, DateTime? validTo = null)
        {

            var travelCard = new TravelCardEntity() { cgi_travelcardnumber = travelCardNumber, cgi_TravelCardCVC = CVC };

            if (validFrom != null)
                travelCard.cgi_ValidTo = validTo;
            if (validTo != null)
                travelCard.cgi_ValidFrom = validFrom;
            if (contact != null)
                travelCard.cgi_Contactid = contact.ToEntityReference();
            if (account != null)
                travelCard.cgi_Accountid = account.ToEntityReference();

            try
            {
                travelCard.Id = XrmHelper.Create(localContext, travelCard);
                return travelCard;
            }
            catch (Exception ex)
            {
                PrintGenericErrorMessage("Create", nameof(TravelCardEntity), ex.Message);
                throw;
            }
        }


        #endregion

        #region Update Entity Methods

        #endregion

        #region Delete Entity Methods


        public static void DeleteValueCodeApprovalsByContact(Plugin.LocalPluginContext localContext, ContactEntity contact)
        {

            if(contact == null)
            {
                PrintGenericErrorMessage("Delete", nameof(ValueCodeApprovalEntity), $"Argument {nameof(contact)} is null.");
                return;
            }

            var query = new QueryExpression()
            {
                EntityName = ValueCodeApprovalEntity.EntityLogicalName,
                ColumnSet = new ColumnSet(ValueCodeApprovalEntity.Fields.ed_Contact),
                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(ValueCodeApprovalEntity.Fields.ed_Contact, ConditionOperator.Equal, contact.Id)
                    }
                }
            };

            var valueCodeApprovals = XrmRetrieveHelper.RetrieveMultiple<ValueCodeApprovalEntity>(localContext, query);

            if(valueCodeApprovals == null || valueCodeApprovals.Count < 1)
            {
                PrintGenericErrorMessage("Delete", nameof(ValueCodeApprovalEntity), $"XrmRetrieveHelper returned null or 0");
                return;
            }

            foreach (var valueCodeApproval in valueCodeApprovals)
                XrmHelper.Delete(localContext, valueCodeApproval.ToEntityReference());

            PrintGenericSuccessMessage("Delete", nameof(ValueCodeApprovalEntity));
        }

        public static void DeleteValueCodeApprovalsByTravelCardNumber(Plugin.LocalPluginContext localContext, string travelCardNumber)
        {
            if (string.IsNullOrEmpty(travelCardNumber))
            {
                PrintGenericErrorMessage("Delete", nameof(ValueCodeApprovalEntity), $"Argument {nameof(travelCardNumber)} is null.");
                return;
            }

            var query = new QueryExpression()
            {
                EntityName = ValueCodeApprovalEntity.EntityLogicalName,
                ColumnSet = new ColumnSet(),
                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(ValueCodeApprovalEntity.Fields.ed_TravelCardNumber, ConditionOperator.Equal, travelCardNumber)
                    }
                }
            };

            try
            {
                var valueCodeApprovals = XrmRetrieveHelper.RetrieveMultiple<ValueCodeApprovalEntity>(localContext, query);
                foreach (var valueCodeApproval in valueCodeApprovals)
                    XrmHelper.Delete(localContext, valueCodeApproval.ToEntityReference());

                PrintGenericSuccessMessage("Delete", nameof(ValueCodeApprovalEntity));
            }
            catch (Exception ex)
            {
                PrintGenericErrorMessage("Delete", nameof(TravelCardEntity), ex.Message);
                return;
            }
        }

        /// <summary>
        /// Deletes a value code
        /// </summary>s
        /// <param name="localContext"></param>
        /// <param name="contact"></param>
        public static void DeleteValueCodesByContact(Plugin.LocalPluginContext localContext, ContactEntity contact)
        {
            if(contact == null)
            {
                PrintGenericErrorMessage("Delete", nameof(ValueCodeEntity), $"Argument {nameof(contact)} is null.");
                return;
            }

            var query = new QueryExpression()
            {
                EntityName = ValueCodeEntity.EntityLogicalName,
                ColumnSet = new ColumnSet(),
                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(ValueCodeEntity.Fields.ed_Contact, ConditionOperator.Equal, contact.Id)
                    }
                }
            };

            var valueCodes = XrmRetrieveHelper.RetrieveMultiple<ValueCodeEntity>(localContext, query);

            if (valueCodes == null || valueCodes.Count < 1)
            {
                PrintGenericErrorMessage("Delete", nameof(ValueCodeEntity), "There are no value codes.");
                return;
            }

            foreach (var valCode in valueCodes)
                XrmHelper.Delete(localContext, valCode.ToEntityReference());

            PrintGenericSuccessMessage("Delete", nameof(ValueCodeEntity));

        }

        public static void DeleteContactByMklId(Plugin.LocalPluginContext localContext, string mklId)
        {
            if (string.IsNullOrEmpty(mklId))
            {
                PrintGenericErrorMessage("Delete", nameof(ContactEntity), $"Argument {nameof(mklId)} is null.");
                return;
            }


            var query = new QueryExpression()
            {
                EntityName = ContactEntity.EntityLogicalName,
                ColumnSet = new ColumnSet(),
                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(ContactEntity.Fields.ed_MklId, ConditionOperator.Equal, mklId)
                    }
                }
            };
            
            try
            {
                var contact = XrmRetrieveHelper.RetrieveFirst<ContactEntity>(localContext, query);
                XrmHelper.Delete(localContext, contact.ToEntityReference());

                PrintGenericSuccessMessage("Delete", nameof(ContactEntity));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"");
                throw;
            }

        }

        /// <summary>
        /// Deletes a contact
        /// </summary>
        /// <param name="localContext"></param>
        /// <param name="id">ed_contactId</param>
        public static void DeleteContactById(Plugin.LocalPluginContext localContext, Guid? id)
        {
            if (id == null || id == Guid.Empty)
            {
                PrintGenericErrorMessage("Delete", nameof(ContactEntity), $"Argument {nameof(id)} is null.");
                return;
            }

            try
            {

                var contact = XrmRetrieveHelper.Retrieve<ContactEntity>(localContext, id.Value, 
                    new ColumnSet(ContactEntity.Fields.StatusCode, ContactEntity.Fields.StateCode));
                if (contact.StatusCode.Value == Skanetrafiken.Crm.Schema.Generated.contact_statuscode.Active &&
                    contact.StateCode.Value == Skanetrafiken.Crm.Schema.Generated.ContactState.Active)
                {
                    contact.StateCode = Skanetrafiken.Crm.Schema.Generated.ContactState.Inactive;
                    contact.StatusCode = Skanetrafiken.Crm.Schema.Generated.contact_statuscode.Inactive;
                    XrmHelper.Update(localContext, contact);
                }
                    

                XrmHelper.Delete(localContext, 
                    new Microsoft.Xrm.Sdk.EntityReference(ContactEntity.EntityLogicalName, id.Value));
                PrintGenericSuccessMessage("Delete", nameof(ContactEntity));
            }
            catch (Exception ex)
            {
                PrintGenericErrorMessage("Delete", nameof(ContactEntity), ex.Message);
                throw;
            }
        }


        public static void DeleteValueCodesRelatedToAnonymousContact(Plugin.LocalPluginContext localContext)
        {
            var query = new QueryExpression()
            {
                EntityName = ContactEntity.EntityLogicalName,
                ColumnSet = new ColumnSet(ContactEntity.Fields.cgi_ContactNumber),
                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(ContactEntity.Fields.cgi_ContactNumber, ConditionOperator.Equal, "1")
                    }
                }
            };

            var contact = XrmRetrieveHelper.RetrieveFirst<ContactEntity>(localContext, query);

            if (contact == null)
            {
                PrintGenericErrorMessage("Get", nameof(ContactEntity), "Could not find contact.");
                return;
            }

            query = new QueryExpression()
            {
                EntityName = ValueCodeEntity.EntityLogicalName,
                ColumnSet = new ColumnSet(ValueCodeEntity.Fields.ed_Contact),
                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(ValueCodeEntity.Fields.ed_Contact, ConditionOperator.Equal, contact.ContactId)
                    }
                }
            };

            var valueCodes = XrmRetrieveHelper.RetrieveMultiple<ValueCodeEntity>(localContext, query);

            foreach (var vc in valueCodes)
            {
                XrmHelper.Delete(localContext, vc.ToEntityReference());
            }

            PrintGenericSuccessMessage("Delete", nameof(ValueCodeEntity));
        }

        public static void DeleteContactByName(Plugin.LocalPluginContext localContext, string lastName)
        {
            if (string.IsNullOrEmpty(lastName))
            {
                PrintGenericErrorMessage("Delete", nameof(ContactEntity), $"Argument {nameof(lastName)} is null.");
                return;
            }


            var query = new QueryExpression()
            {
                EntityName = ContactEntity.EntityLogicalName,
                ColumnSet = new ColumnSet(),
                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(ContactEntity.Fields.LastName, ConditionOperator.Equal, lastName)
                    }
                }
            };

            var contact = XrmRetrieveHelper.RetrieveFirst<ContactEntity>(localContext, query);

            if(contact == null)
            {
                PrintGenericErrorMessage("Delete", nameof(ContactEntity), "Could not find contact.");
                return;
            }

            //Contact has to be deactivated before deleting.
            DeactivateContact(localContext, new ContactEntity() { Id = contact.Id });

            XrmHelper.Delete(localContext, contact.ToEntityReference());
            PrintGenericSuccessMessage("Delete", nameof(ContactEntity));

        }

        public static void DeleteTravelCardByTravelCardNumber(Plugin.LocalPluginContext localContext, string number)
        {
            if (string.IsNullOrEmpty(number))
            {
                PrintGenericErrorMessage("Delete", nameof(TravelCardEntity), $"Argument {nameof(number)} is null.");
                return;
            }


            var query = new QueryExpression()
            {
                EntityName = TravelCardEntity.EntityLogicalName,
                ColumnSet = new ColumnSet(),
                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(TravelCardEntity.Fields.cgi_travelcardnumber, ConditionOperator.Equal, number)
                    }
                }
            };

            try
            {
                var travelCard = XrmRetrieveHelper.RetrieveFirst<TravelCardEntity>(localContext, query);
                XrmHelper.Delete(localContext, travelCard.ToEntityReference());

                PrintGenericSuccessMessage("Delete", nameof(TravelCardEntity));
            }
            catch (Exception ex)
            {
                PrintGenericErrorMessage("Delete", nameof(TravelCardEntity), ex.Message);
                return;
            }

        }

        public static void DeleteTravelCardsByContact(Plugin.LocalPluginContext localContext, ContactEntity contact)
        {
            if (contact == null)
            {
                PrintGenericErrorMessage("Delete", nameof(TravelCardEntity), $"Argument {nameof(contact)} is null.");
                return;
            }


            var query = new QueryExpression()
            {
                EntityName = TravelCardEntity.EntityLogicalName,
                ColumnSet = new ColumnSet(),
                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(TravelCardEntity.Fields.cgi_Contactid, ConditionOperator.Equal, contact.Id)
                    }
                }
            };

            try
            {
                var travelCards = XrmRetrieveHelper.RetrieveMultiple<TravelCardEntity>(localContext, query);

                foreach (var travelCard in travelCards)
                    XrmHelper.Delete(localContext, travelCard.ToEntityReference());
                PrintGenericSuccessMessage("Delete", nameof(TravelCardEntity));

            }
            catch (Exception ex)
            {
                PrintGenericErrorMessage("Delete", nameof(TravelCardEntity), ex.Message);
                throw;
            }
        }
        #endregion

        #region Helper


        private static void DeactivateContact(Plugin.LocalPluginContext localContext, ContactEntity contact)
        {
            contact.StateCode = Skanetrafiken.Crm.Schema.Generated.ContactState.Inactive;
            XrmHelper.Update(localContext, contact);
        }

        /// <summary>
        /// Prints error message.
        /// </summary>
        /// <param name="method"></param>
        /// <param name="nameofEntity"></param>
        /// <param name="exceptionMessage"></param>
        /// <param name="nameOfMethod">CallerMemberName fetches the name of the caller.</param>
        private static void PrintGenericErrorMessage(string CRUD, string nameofEntity, string exceptionMessage, [CallerMemberName] string nameOfMethod = null)
        {
            Console.WriteLine($"Method {nameOfMethod} did not {CRUD.ToLower()} {nameofEntity}. Error: {exceptionMessage}");
        }

        /// <summary>
        /// Prints success message.
        /// </summary>
        /// <param name="CRUD"></param>
        /// <param name="nameofEntity"></param>
        /// <param name="nameOfMethod"></param>
        private static void PrintGenericSuccessMessage(string CRUD, string nameofEntity, [CallerMemberName] string nameOfMethod = null)
        {
            Console.WriteLine($"Method {nameOfMethod} successfully performed CRUD operation ({CRUD.ToLower()}) on entity {nameofEntity}.");
        }

        /// <summary>
        /// Read file from disk
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        internal static string ReadTestFile(string fileName)
        {
            using (StreamReader r = new StreamReader(fileName))
            {
                return r.ReadToEnd();
            }

        }

        #endregion
    }

}
