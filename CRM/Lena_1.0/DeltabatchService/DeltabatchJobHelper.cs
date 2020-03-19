using Common.Logging;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;
using Quartz;
using Skanetrafiken.Crm.Entities;
using System;
using System.IO;
using Tamir.SharpSsh;
using System.Xml.Linq;
using Endeavor.Crm.UnitTest;
using Skanetrafiken.Crm.Schema.Generated;
using Microsoft.Crm.Sdk.Messages;
using System.Collections.Generic;

namespace Endeavor.Crm.DeltabatchService
{
    public class DeltabatchJobHelper
    {
        public static readonly ColumnSet deltabatchQueueColumnSet = new ColumnSet(
               DeltabatchQueueEntity.Fields.ed_ContactGuid,
               DeltabatchQueueEntity.Fields.ed_ContactNumber,
               DeltabatchQueueEntity.Fields.ed_DeltabatchOperation,
               DeltabatchQueueEntity.Fields.ed_DeltabatchQueueId,
               DeltabatchQueueEntity.Fields.ed_name,
               DeltabatchQueueEntity.Fields.CreatedOn
            );
        
        public static Sftp CreateSftpConnectionToCreditsafe(ILog _log)
        {
            //string username = null, password = null;
            //string credentialFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Endeavor\\CreditsafeCredential.xml");
            if (File.Exists(DeltabatchService.CreditsafeCredentialFilePath))
            {
                //XDocument doc = XDocument.Load(DeltabatchService.CreditsafeCredentialFilePath);
                //username = DeltabatchService.CreditsafeUsername;
                //password = Configuration.ToInsecureString(Configuration.DecryptString(doc.Root.Element("Password").Value));

                //_log.Debug($"Connecting to Creditsafe({Properties.Settings.Default.CreditsafeIP}) with {username},{password}");
                ////_log.Debug($"Connecting to Creditsafe({Properties.Settings.Default.CreditsafeIP}) with {username},{password}({doc.Root.Element("Password").Value})");
                return new Sftp(DeltabatchService.CreditsafeIP, DeltabatchService.CreditsafeUsername, CrmConnection.LoadCredentials(DeltabatchService.CreditsafeCredentialFilePath, DeltabatchService.Entropy));
            }
            else
            {
                throw new Exception($"Could not find Credentials file at: {DeltabatchService.CredentialFilePath}. Please create with Endeavor NUnit Credentials Manager");
            }
        }

        public static Plugin.LocalPluginContext GenerateLocalContext()
        {
            // Connect to the CRM web service using a connection string.
            CrmServiceClient conn = new CrmServiceClient(CrmConnection.GetCrmConnectionString(DeltabatchService.CredentialFilePath, DeltabatchService.Entropy));

            // Cast the proxy client to the IOrganizationService interface.
            IOrganizationService serviceProxy = (IOrganizationService)conn.OrganizationWebProxyClient != null ? (IOrganizationService)conn.OrganizationWebProxyClient : (IOrganizationService)conn.OrganizationServiceProxy;

            Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), serviceProxy, null, new TracingService());

            return localContext;
        }

        internal static void SendErrorMailToDev(Plugin.LocalPluginContext localContext, Exception e)
        {
            SystemUserEntity toUser = XrmRetrieveHelper.RetrieveFirst<SystemUserEntity>(localContext, new ColumnSet(false),
                new FilterExpression
                {
                    Conditions =
                    {
                        new ConditionExpression(SystemUserEntity.Fields.DomainName, ConditionOperator.Equal, @"D1\CRMAdmin")
                    }
                });

            ActivityParty toParty = new ActivityParty
            {
                PartyId = new EntityReference(SystemUserEntity.EntityLogicalName, toUser.Id),
                AddressUsed = Properties.Settings.Default.DeveloperMailAddress
            };
        
            // Get a system user to send the email (From: field)
            WhoAmIRequest systemUserRequest = new WhoAmIRequest();
            WhoAmIResponse systemUserResponse = (WhoAmIResponse)localContext.OrganizationService.Execute(systemUserRequest);
            Guid _userId = systemUserResponse.UserId;

            // Create the 'From:' activity party for the email
            ActivityParty fromParty = new ActivityParty
            {
                PartyId = new EntityReference(SystemUserEntity.EntityLogicalName, _userId)
            };

            EmailEntity errorMail = new EmailEntity
            {
                To = new List<ActivityParty> { toParty },
                From = new List<ActivityParty> { fromParty },
                Subject = "Exception in Deltabatch run",
                Description = $"Exception Message:\n\n{e.Message}",
                DirectionCode = true
            };
            errorMail.Id = XrmHelper.Create(localContext, errorMail);

            // Use the SendEmail message to send an e-mail message.
            SendEmailRequest sendEmailreq = new SendEmailRequest
            {
                EmailId = errorMail.Id,
                TrackingToken = "",
                IssueSend = true
            };

            SendEmailResponse sendEmailresp = (SendEmailResponse)localContext.OrganizationService.Execute(sendEmailreq);
            Console.WriteLine("Sent the error-mail message.");
        }
    }
}
