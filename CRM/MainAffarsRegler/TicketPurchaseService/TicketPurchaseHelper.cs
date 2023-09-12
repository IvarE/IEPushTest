using Endeavor.Crm;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;
using Skanetrafiken.Crm.Entities;

namespace TicketPurchaseService
{
    public class TicketPurchaseHelper
    {
        public static readonly ColumnSet ticketInfoDataColumnSet = new ColumnSet(
                   TicketPurchasesPerCustomerDataEntity.Fields.ed_MKLid,
                   TicketPurchasesPerCustomerDataEntity.Fields.ed_ContactNumber,
                   TicketPurchasesPerCustomerDataEntity.Fields.ed_NumberOfTickets,
                   TicketPurchasesPerCustomerDataEntity.Fields.ed_OfferName,
                   TicketPurchasesPerCustomerDataEntity.Fields.ed_TotalSumTicketOffer
                );

        public static Plugin.LocalPluginContext GenerateLocalContext()
        {
            // Connect to the CRM web service using a connection string.
            CrmServiceClient conn = new CrmServiceClient(CrmConnection.GetCrmConnectionString(TicketPurchaseService.CredentialFilePath, TicketPurchaseService.Entropy));

            // Cast the proxy client to the IOrganizationService interface.
            IOrganizationService serviceProxy = (IOrganizationService)conn.OrganizationWebProxyClient != null ? (IOrganizationService)conn.OrganizationWebProxyClient : (IOrganizationService)conn.OrganizationServiceProxy;

            Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), serviceProxy, null, new TracingService());

            return localContext;
        }


    }
}
