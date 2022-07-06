using Endeavor.Crm;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

using System;
using System.Linq;
using Skanetrafiken.Crm.Schema.Generated;
using System.Collections.Generic;

namespace Skanetrafiken.Crm.Entities
{
    public class TicketInfoEntity : ed_ticketinfo
    {
        internal void HandlePostTicketPurchasesCreateAsync(Plugin.LocalPluginContext localContext)
        {
            try
            {
                bool needsUpdate = false;
                ed_ticketinfo uTicketInfo = new ed_ticketinfo();

                string mklId = ed_MklId;
                string nName = ed_CRMNumber + "_" + FormattedValues["ed_offername"];
                if (ed_name != nName)
                {
                    uTicketInfo.ed_name = nName;
                    needsUpdate = true;
                }

                if (!string.IsNullOrEmpty(mklId))
                {
                    var lContacts = SingaporeTicketEntity.GetActiveContactQuery(localContext, mklId);
                    localContext.Trace($"Found {lContacts.Count} Active Contacts with ed_MklId {mklId}");
                    if (lContacts.Count == 1)
                    {
                        var eContact = lContacts.FirstOrDefault();
                        var erContact = new EntityReference(eContact.LogicalName, eContact.Id);
                        uTicketInfo.ed_Contact = erContact;
                        needsUpdate = true;
                    }
                }

                if (needsUpdate)
                {
                    uTicketInfo.Id = Id;
                    XrmHelper.Update(localContext, uTicketInfo);
                }
            }
            catch (Exception e)
            {
                localContext.Trace($"HandlePostTicketPurchasesCreateAsync threw an unexpected exception: {e.Message}");
                throw e;
            }
        }

        internal void HandlePostTicketPurchasesUpdateAsync(Plugin.LocalPluginContext localContext, ed_ticketinfo preImage)
        {
            try
            {
                bool needsUpdate = false;
                ed_ticketinfo uTicketInfo = new ed_ticketinfo();

                string mklId = string.IsNullOrEmpty(ed_MklId) ? preImage.ed_MklId : ed_MklId;
                string contactNumber = string.IsNullOrEmpty(this.ed_CRMNumber) ? preImage.ed_CRMNumber : this.ed_CRMNumber;
                string offerName = string.IsNullOrEmpty(this.FormattedValues["ed_offername"]) ? preImage.FormattedValues["ed_offername"] : this.FormattedValues["ed_offername"];

                string nName = contactNumber + "_" + offerName;
                if (preImage.ed_name != nName)
                {
                    uTicketInfo.ed_name = nName;
                    needsUpdate = true;
                }

                if (!string.IsNullOrEmpty(mklId))
                {
                    var lContacts = SingaporeTicketEntity.GetActiveContactQuery(localContext, mklId);
                    localContext.Trace($"Found {lContacts.Count} Active Contacts with ed_MklId {mklId}");
                    if (lContacts.Count == 1)
                    {
                        var eContact = lContacts.FirstOrDefault();
                        if (eContact.Id != preImage.ed_Contact?.Id)
                        {
                            uTicketInfo.ed_Contact = eContact.ToEntityReference();
                            needsUpdate = true;
                        }
                    }
                }

                if (needsUpdate)
                {
                    uTicketInfo.Id = Id;
                    XrmHelper.Update(localContext, uTicketInfo);
                }
            }
            catch (Exception e)
            {
                localContext.Trace($"HandlePostTicketPurchasesUpdateAsync threw an unexpected exception: {e.Message}");
                throw e;
            }
        }

        public static void HandlePostTicketInfoEntityCreate(Plugin.LocalPluginContext localContext, TicketInfoEntity ticketInfo)
        {
            string mklId = ticketInfo.ed_MklId;

            if (!string.IsNullOrEmpty(mklId))
            {
                CalculateClassification(localContext, mklId);
            }
        }

        public static void HandlePostTicketInfoEntityUpdate(Plugin.LocalPluginContext localContext, TicketInfoEntity ticketInfo, TicketInfoEntity preImage)
        {
            string mklId = null;

            if(ticketInfo.ed_MklId != null)
            {
                mklId = ticketInfo.ed_MklId;
            }
            else if(preImage.ed_MklId != null)
            {
                mklId = preImage.ed_MklId;
            }

            if (!string.IsNullOrEmpty(mklId))
            {
                CalculateClassification(localContext, mklId);
            }

        }

        public static void HandlePostTicketInfoEntityDelete(Plugin.LocalPluginContext localContext, TicketInfoEntity preImage)
        {
            string mklId = preImage.ed_MklId;

            if (!string.IsNullOrEmpty(mklId))
            {
                CalculateClassification(localContext, mklId);
            }
        }

        public static void CalculateClassification(Plugin.LocalPluginContext localContext, string mklId)
        {
            var lContacts = GetActiveContactMklIdQuery(localContext, mklId);
            if (lContacts.Count == 1)
            {
                var eContact = lContacts.FirstOrDefault();

                if (eContact.Id != null)
                {
                    var everyTicketInfo = GetEveryTicketInfoQuery(localContext, eContact.Id);
                    //var ticketListWithAmount = new List<Tuple<int, int>>();
                    var trettioDagarsbiljett = 0; //206290000
                    var trettioDagarsbiljettMetro = 0; // 	899310002
                    var tioTrettioDagarsbiljett = 0; // 899310001
                    var tioTrettioDagarsbiljettDKMetro = 0; // 	899310003
                    var enkelbiljett = 0; // 	206290002
                    var tioEnklaFem = 0; //  	206290003 //motsvarar 10 enkelbiljetter
                    var dygnsbiljett = 0; // 	206290001
                    

                    var isSallanresenar = false;
                    var isVaxlare = false;
                    var isPendlare = false;
                    var isForloradKund = false; 

                    foreach (TicketInfoEntity ticket in everyTicketInfo)
                    {
                        var ticketOfferNameValue = Convert.ToInt32(ticket.ed_OfferName.Value);

                        if (ticketOfferNameValue == 206290000)
                        {
                            trettioDagarsbiljett = Convert.ToInt32(ticket.ed_NumberofTickets);
                        }
                        else if(ticketOfferNameValue == 899310002)
                        {
                            trettioDagarsbiljettMetro = Convert.ToInt32(ticket.ed_NumberofTickets);
                        }
                        else if (ticketOfferNameValue == 899310001)
                        {
                            tioTrettioDagarsbiljett = Convert.ToInt32(ticket.ed_NumberofTickets);
                        }
                        else if (ticketOfferNameValue == 899310003)
                        {
                            tioTrettioDagarsbiljettDKMetro = Convert.ToInt32(ticket.ed_NumberofTickets);
                        }
                        else if (ticketOfferNameValue == 206290002)
                        {
                            enkelbiljett = Convert.ToInt32(ticket.ed_NumberofTickets);
                        }
                        else if (ticketOfferNameValue == 206290003)
                        {
                            tioEnklaFem = Convert.ToInt32(ticket.ed_NumberofTickets);
                        }
                        else if (ticketOfferNameValue == 206290001) 
                        {
                            dygnsbiljett = Convert.ToInt32(ticket.ed_NumberofTickets);
                        }
                        
                    }

                    //Sällanresnär
                    if((enkelbiljett > 1 || dygnsbiljett > 1 || tioEnklaFem > 1 || enkelbiljett > 10) && trettioDagarsbiljett == 0 || tioTrettioDagarsbiljett == 0)
                    {
                        isSallanresenar = true; 
                    }

                    //Växlare
                    var manadsbiljetter = trettioDagarsbiljett + tioTrettioDagarsbiljett + tioTrettioDagarsbiljettDKMetro;
                    if (manadsbiljetter != 0 && manadsbiljetter < 7)
                    {
                        isVaxlare = true;
                    }

                    //Pendlare
                    if(manadsbiljetter > 7)
                    {
                        isPendlare = true;
                    }

                    //Förlorad kund
                    if(trettioDagarsbiljett == 0 && trettioDagarsbiljettMetro == 0 && tioTrettioDagarsbiljett == 0 && tioTrettioDagarsbiljettDKMetro == 0 && enkelbiljett == 0 && tioEnklaFem == 0 && dygnsbiljett == 0)
                    {
                        isForloradKund = true;
                    }

                    if(isForloradKund)
                    {
                        eContact.ed_Kundresan = new OptionSetValue((int)899310004);
                    }
                    else if(isPendlare)
                    {
                        eContact.ed_Kundresan = new OptionSetValue((int)899310003);
                    }
                    else if(isVaxlare)
                    {
                        eContact.ed_Kundresan = new OptionSetValue((int)899310002);
                    }
                    else if(isSallanresenar)
                    {
                        eContact.ed_Kundresan = new OptionSetValue((int)899310001);
                    }

                    XrmHelper.Update(localContext, eContact);
                }

            }

        }

        public static List<Contact> GetActiveContactMklIdQuery(Plugin.LocalPluginContext localContext, string mklId)
        {
            QueryExpression queryContacts = new QueryExpression(Contact.EntityLogicalName);
            queryContacts.NoLock = true;
            queryContacts.ColumnSet = new ColumnSet(Contact.Fields.ContactId, Contact.Fields.ed_Kundresan);
            queryContacts.Criteria.AddCondition(Contact.Fields.ed_MklId, ConditionOperator.Equal, mklId);
            queryContacts.Criteria.AddCondition(Contact.Fields.StateCode, ConditionOperator.Equal, (int)ContactState.Active);
            return XrmRetrieveHelper.RetrieveMultiple<Contact>(localContext, queryContacts);
        }

        public static List<TicketInfoEntity> GetEveryTicketInfoQuery(Plugin.LocalPluginContext localContext, Guid contactId)
        {
            QueryExpression queryTicketInfo = new QueryExpression(TicketInfoEntity.EntityLogicalName);
            queryTicketInfo.NoLock = true;
            queryTicketInfo.ColumnSet = new ColumnSet(TicketInfoEntity.Fields.ed_OfferName, TicketInfoEntity.Fields.ed_NumberofTickets);
            queryTicketInfo.Criteria.AddCondition(TicketInfoEntity.Fields.ed_Contact, ConditionOperator.Equal, contactId);
            return XrmRetrieveHelper.RetrieveMultiple<TicketInfoEntity>(localContext, queryTicketInfo);
        }
    }
}
