using Microsoft.Xrm.Sdk.Query;
using Skanetrafiken.Crm.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Endeavor.Crm.UnitTest
{
    public static class TestDataHelper
    {
        #region General

        #endregion

        #region Get Entity Methods

        public static List<CustomerAddressEntity> GetCustomerAddresses_ByParentId(Plugin.LocalPluginContext localContext, Guid parentId, ColumnSet column = null, FilterExpression filter = null)
        {

            var filterAddress = new QueryExpression()
            {
                EntityName = CustomerAddressEntity.EntityLogicalName,
                ColumnSet = column,
                Criteria = filter != null ? filter : new FilterExpression { Conditions = { new ConditionExpression(CustomerAddressEntity.Fields.ParentId, ConditionOperator.Equal, parentId) } }
            };

            var addresses = XrmRetrieveHelper.RetrieveMultiple<CustomerAddressEntity>(localContext, filterAddress);
            return addresses;
        }

        public static List<TravelCardEntity> GetTravelCards_ByAccount(Plugin.LocalPluginContext localContext, Guid parentId, ColumnSet column = null)
        {
            var filterTravelCards = new QueryExpression()
            {
                EntityName = TravelCardEntity.EntityLogicalName,
                ColumnSet = column != null ? column : 
                new ColumnSet(TravelCardEntity.Fields.cgi_travelcardnumber,
                TravelCardEntity.Fields.cgi_TravelCardCVC),
                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(TravelCardEntity.Fields.cgi_Accountid, ConditionOperator.Equal, parentId)
                    }
                }
            };

            var travelCards = XrmRetrieveHelper.RetrieveMultiple<TravelCardEntity>(localContext, filterTravelCards);
            return travelCards;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="localContext"></param>
        /// <param name="parentId"></param>
        /// <param name="column"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static List<AccountEntity> GetChildAccounts_ByParentId(Plugin.LocalPluginContext localContext, Guid parentId, ColumnSet column = null, FilterExpression filter = null)
        {

            var filterChildAccounts = new QueryExpression()
            {
                EntityName = AccountEntity.EntityLogicalName,
                ColumnSet = column != null ? column : new ColumnSet(),
                Criteria = filter != null ? filter  : 
                    new FilterExpression()
                    {
                        Conditions =
                        {
                            new ConditionExpression(AccountEntity.Fields.ParentAccountId, ConditionOperator.Equal, parentId)
                        }
                    }
            };

            var childAccounts = XrmRetrieveHelper.RetrieveMultiple<AccountEntity>(localContext, filterChildAccounts);

            return childAccounts;
        }

        public static AccountEntity GetAccount_ById(Plugin.LocalPluginContext localContext, Guid id, ColumnSet column = null)
        {
            var filterTravelCards = new QueryExpression()
            {
                EntityName = AccountEntity.EntityLogicalName,
                ColumnSet = column,
                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(AccountEntity.Fields.Id, ConditionOperator.Equal, id)
                    }
                }
            };

            var account = XrmRetrieveHelper.RetrieveFirst<AccountEntity>(localContext, filterTravelCards);
            return account;
        }

        #endregion

        #region Create Entity Methods

        #endregion

        #region Update Entity Methods

        #endregion

        #region Delete Entity methods

        /// <summary>
        /// Deletes customer addresses by
        /// </summary>
        /// <param name="localContext"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public static void DeleteCustomerAddress_ByParentId(Plugin.LocalPluginContext localContext, Guid parentId)
        {

            var filterAddress = new QueryExpression()
            {
                EntityName = CustomerAddressEntity.EntityLogicalName,
                ColumnSet = new ColumnSet(true),
                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(CustomerAddressEntity.Fields.ParentId, ConditionOperator.Equal, parentId)
                    }
                }
            };

            var addresses = XrmRetrieveHelper.RetrieveMultiple<CustomerAddressEntity>(localContext, filterAddress);
            foreach (var addr in addresses)
            {
                XrmHelper.Delete(localContext, addr.ToEntityReference());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="localContext"></param>
        /// <param name="accountNames"></param>
        public static void DeleteAccount_ByName(Plugin.LocalPluginContext localContext, params string[] accountNames)
        {
            foreach (var name in accountNames)
            {
                var filterAccount = new QueryExpression()
                {
                    EntityName = AccountEntity.EntityLogicalName,
                    ColumnSet = new ColumnSet(),
                    Criteria =
                    {
                        Conditions =
                        {
                            new ConditionExpression(AccountEntity.Fields.Name, ConditionOperator.Equal, name)
                        }
                    }
                };

                var foundAccs = XrmRetrieveHelper.RetrieveMultiple<AccountEntity>(localContext, filterAccount);
                if (foundAccs.Count > 0)
                {
                    foreach (var acc in foundAccs)
                    {
                        XrmHelper.Delete(localContext, acc.ToEntityReference());
                    }
                } else
                {
                    Console.WriteLine("Could not find any accounts.");
                }
                    
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="localContext"></param>
        /// <param name="accountId"></param>
        /// <param name="filterRelatedTravelCards">Default filters on cgi_AccountId.</param>
        public static void DeleteRelatedTravelCards_ByAccountId(Plugin.LocalPluginContext localContext, Guid accountId, FilterExpression filterRelatedTravelCards = null)
        {

            var filterTravelCards = new QueryExpression()
            {
                EntityName = TravelCardEntity.EntityLogicalName,
                ColumnSet = new ColumnSet(),
                Criteria = filterRelatedTravelCards != null ? filterRelatedTravelCards :
                    new FilterExpression { Conditions = { new ConditionExpression(TravelCardEntity.Fields.cgi_Accountid, ConditionOperator.Equal, accountId) } }
            };
            var travelCards = XrmRetrieveHelper.RetrieveMultiple<TravelCardEntity>(localContext, filterTravelCards);
            foreach (var travelCard in travelCards)
            {
                XrmHelper.Delete(localContext, travelCard.ToEntityReference());
                Console.WriteLine($"Deleted travelcard '{travelCard.cgi_travelcardnumber}'");
            }
        }

        public static void DeleteRelatedTravelCards_ByAccountName(Plugin.LocalPluginContext localContext, string accountName, FilterExpression filterRelatedTravelCards = null)
        {
            var filterAccount = new QueryExpression()
            {
                EntityName = AccountEntity.EntityLogicalName,
                ColumnSet = new ColumnSet(),
                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(AccountEntity.Fields.Name, ConditionOperator.Equal, accountName)
                    }
                }
            };

            var foundAcc = XrmRetrieveHelper.RetrieveFirst<AccountEntity>(localContext, filterAccount);
            if (foundAcc == null)
                return;


            var filterTravelCards = new QueryExpression()
            {
                EntityName = TravelCardEntity.EntityLogicalName,
                ColumnSet = new ColumnSet(),
                Criteria = filterRelatedTravelCards != null ? filterRelatedTravelCards :
                    new FilterExpression { Conditions = { new ConditionExpression(TravelCardEntity.Fields.cgi_Accountid, ConditionOperator.Equal, foundAcc.Id) } }
            };
            var travelCards = XrmRetrieveHelper.RetrieveMultiple<TravelCardEntity>(localContext, filterTravelCards);
            foreach (var travelCard in travelCards)
            {
                XrmHelper.Delete(localContext, travelCard.ToEntityReference());
                Console.WriteLine($"Deleted travelcard '{travelCard.cgi_travelcardnumber}'");
            }
        }

        public static void DeleteTravelCard_ByName(Plugin.LocalPluginContext localContext, string travelCardName)
        {

            var filterTravelCard = new QueryExpression()
            {
                EntityName = TravelCardEntity.EntityLogicalName,
                ColumnSet = new ColumnSet(),
                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(TravelCardEntity.Fields.cgi_TravelCardName, ConditionOperator.Equal, travelCardName)
                    }
                }
            };

            var travelCards = XrmRetrieveHelper.RetrieveMultiple<TravelCardEntity>(localContext, filterTravelCard);
            foreach (var travelCard in travelCards)
            {
                XrmHelper.Delete(localContext, travelCard.ToEntityReference());
            }
                
        }

        public static void DeleteIncidentCaseByTicketNumber(Plugin.LocalPluginContext localContext, string ticketNumber)
        {

            var query = new QueryExpression()
            {
                EntityName = IncidentEntity.EntityLogicalName,
                ColumnSet = new ColumnSet(),
                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(IncidentEntity.Fields.TicketNumber, ConditionOperator.Equal, ticketNumber)
                    }
                }
            };

            try
            {
                var incident = XrmRetrieveHelper.RetrieveFirst<IncidentEntity>(localContext, query);
                if (incident == null)
                {
                    Console.WriteLine("Could not find incident based on given ticketnumber.");
                    return;
                }

                XrmHelper.Delete(localContext, incident.ToEntityReference());
            }
            catch (Exception ex)
            {
                PrintGenericErrorMessage("Delete", nameof(DeleteIncidentCaseByTicketNumber), ex.Message);
            }
        }

        public static void DeleteIncidentAndRelatedRefundByTicketNumber(Plugin.LocalPluginContext localContext, string ticketNumber)
        {
            try
            {
                var query = new QueryExpression()
                {
                    EntityName = IncidentEntity.EntityLogicalName,
                    ColumnSet = new ColumnSet(IncidentEntity.Fields.TicketNumber),
                    Criteria =
                    {
                        Conditions =
                        {
                            new ConditionExpression(IncidentEntity.Fields.TicketNumber, ConditionOperator.Equal, ticketNumber)
                        }
                    },
                };

                var incident = XrmRetrieveHelper.RetrieveFirst<IncidentEntity>(localContext, query);

                if (incident == null)
                {
                    Console.WriteLine("Could not find incident based on given ticketnumber.");
                    return;
                }


                var queryExpression = new QueryExpression()
                {
                    EntityName = RefundEntity.EntityLogicalName,
                    ColumnSet = new ColumnSet(),
                    Criteria =
                    {
                        Conditions =
                        {
                            new ConditionExpression(RefundEntity.Fields.cgi_refundnumber, ConditionOperator.Equal, incident.TicketNumber)
                        }
                    }
                };

                var refunds = XrmRetrieveHelper.RetrieveMultiple<RefundEntity>(localContext, queryExpression);

                foreach (var refund in refunds)
                    XrmHelper.Delete(localContext, refund.ToEntityReference());

                XrmHelper.Delete(localContext, incident.ToEntityReference());
            }
            catch (Exception ex)
            {
                PrintGenericErrorMessage("Delete", nameof(DeleteIncidentAndRelatedRefundByTicketNumber), ex.Message);
            }
        }

        #endregion

        #region Helpers
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
        #endregion
    }
}
