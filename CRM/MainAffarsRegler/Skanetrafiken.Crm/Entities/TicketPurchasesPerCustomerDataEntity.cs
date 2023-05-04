using System;
using System.Collections.Generic;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Endeavor.Crm.Extensions;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Crm.Sdk;
using Microsoft.Crm.Sdk.Messages;
using System.Net;
using System.Net.Http;
using Generated = Skanetrafiken.Crm.Schema.Generated;
using Endeavor.Crm;
using System.Globalization;
using System.IdentityModel;
using System.Linq;

namespace Skanetrafiken.Crm.Entities
{
    public class TicketPurchasesPerCustomerDataEntity : Generated.ed_ticketpurchasespercustomerdata
    {
        public static void HandlePostTicketPurchasesPerCustomerDataEntityCreate(Plugin.LocalPluginContext localContext, TicketPurchasesPerCustomerDataEntity tppcData)
        {
            UpdateTicketInfoWithTicketPurchaseData(localContext, tppcData, false);
        }

        public static void HandlePostTicketPurchasesPerCustomerDataEntityUpdate(Plugin.LocalPluginContext localContext, TicketPurchasesPerCustomerDataEntity tppcData, TicketPurchasesPerCustomerDataEntity preImage)
        {
            tppcData.CombineAttributes(preImage);
            UpdateTicketInfoWithTicketPurchaseData(localContext, tppcData, false);
        }

        public static void HandlePostTicketPurchasesPerCustomerDataEntityDelete(Plugin.LocalPluginContext localContext, TicketPurchasesPerCustomerDataEntity preImage)
        {
            UpdateTicketInfoWithTicketPurchaseData(localContext, preImage, true);
        }

        public static void UpdateTicketInfoWithTicketPurchaseData(Plugin.LocalPluginContext localContext, TicketPurchasesPerCustomerDataEntity tppcData, bool isDelete)
        {
            TicketInfoEntity ticketInfo = null;

            if (tppcData.ed_MKLid != null)
            {
                ticketInfo = XrmRetrieveHelper.RetrieveFirst<TicketInfoEntity>(localContext, new ColumnSet(
                   TicketInfoEntity.Fields.ed_CRMNumber,
                   TicketInfoEntity.Fields.ed_MklId,
                   TicketInfoEntity.Fields.ed_name,
                   TicketInfoEntity.Fields.ed_NumberofTickets,
                   TicketInfoEntity.Fields.ed_Offer_Name,
                   TicketInfoEntity.Fields.ed_OfferName,
                   TicketInfoEntity.Fields.ed_TotalSum),
                    new FilterExpression
                    {
                        Conditions =
                               {
                                new ConditionExpression(TicketInfoEntity.Fields.ed_MklId, ConditionOperator.Equal, tppcData.ed_MKLid),
                                new ConditionExpression(TicketInfoEntity.Fields.ed_Offer_Name, ConditionOperator.Equal, tppcData.ed_OfferName)
                               }
                    });
            }

            if (ticketInfo != null) // Existing Ticket Info found
            {

                if (isDelete) //Ticket info data deleted, delete corresponding ticket info record. 
                {

                    EntityReference ticketInfoEntityRefrence = new EntityReference(TicketInfoEntity.EntityLogicalName, ticketInfo.Id);

                    XrmHelper.Delete(localContext, ticketInfoEntityRefrence);
                }
                else //Update ticket info
                {

                    if (tppcData.ed_NumberOfTickets != null)
                        ticketInfo.ed_NumberofTickets = tppcData.ed_NumberOfTickets;

                    if (tppcData.ed_TotalSumTicketOffer != null)
                        ticketInfo.ed_TotalSum = new Money(tppcData.ed_TotalSumTicketOffer.Value);

                    if (ticketInfo.ed_OfferName == null)
                        ticketInfo.ed_OfferName = setOfferNameOptionset(localContext, tppcData);

                    XrmHelper.Update(localContext, ticketInfo);
                }
            }
            else // No ticket info found - Creating new ticket info
            {
                TicketInfoEntity createTicketInfo = new TicketInfoEntity 
                {
                    
                    ed_CRMNumber = tppcData.ed_ContactNumber,
                    ed_MklId = tppcData.ed_MKLid,
                    ed_name = tppcData.ed_ContactNumber + "_" + tppcData.ed_OfferName,
                    ed_NumberofTickets = tppcData.ed_NumberOfTickets,
                    ed_Offer_Name = tppcData.ed_OfferName,
                    ed_TotalSum = new Money(tppcData.ed_TotalSumTicketOffer.Value)
                };

                createTicketInfo.ed_OfferName = setOfferNameOptionset(localContext, tppcData);

                XrmHelper.Create(localContext, createTicketInfo);
            }

        }

        private static Generated.st_singaporetickettype setOfferNameOptionset(Plugin.LocalPluginContext localContext, TicketPurchasesPerCustomerDataEntity tppcData)
        {
            Generated.st_singaporetickettype offerName = (Generated.st_singaporetickettype)(-1);

            if (tppcData.ed_OfferName == "30-dagarsbiljett")
            {
                offerName = Generated.st_singaporetickettype.dagarsbiljett;
            }
            else if (tppcData.ed_OfferName == "30 dagar + Metro")
            {
                offerName = Generated.st_singaporetickettype.dagarPlusMetro;
            }
            else if (tppcData.ed_OfferName == "10/30")
            {
                offerName = Generated.st_singaporetickettype._1030;
            }
            else if (tppcData.ed_OfferName == "10/30 DK ink Metro")
            {
                offerName = Generated.st_singaporetickettype.DKinkMetro;
            }
            else if (tppcData.ed_OfferName == "Enkelbiljett")
            {
                offerName = Generated.st_singaporetickettype.Enkelbiljett;
            }
            else if (tppcData.ed_OfferName == "10 enkla 5%")
            {
                offerName = Generated.st_singaporetickettype.enkla5;
            }
            else if (tppcData.ed_OfferName == "Dygnsbiljett")
            {
                offerName = Generated.st_singaporetickettype.Dygnsbiljett;
            }
            else if (tppcData.ed_OfferName == "10 enkla app 5%")
            {
                offerName = Generated.st_singaporetickettype.enklaapp5;
            }
            else if (tppcData.ed_OfferName == "Sommarbiljett")
            {
                offerName = Generated.st_singaporetickettype.Sommarbiljett;
            }

            return offerName;
        }

    }
}
