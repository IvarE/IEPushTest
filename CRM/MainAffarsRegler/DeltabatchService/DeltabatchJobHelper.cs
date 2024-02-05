using Common.Logging;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;
using Skanetrafiken.Crm.Entities;
using System;
using System.IO;

using Renci.SshNet;
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
        
        public static SftpClient CreateSftpConnectionToCreditsafe(ILog _log)
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


                return new SftpClient(DeltabatchService.CreditsafeIP, Properties.Settings.Default.CreditsafeFtpPort, DeltabatchService.CreditsafeUsername, CrmConnection.LoadCredentials(DeltabatchService.CreditsafeCredentialFilePath, DeltabatchService.Entropy));
                //return new Sftp(DeltabatchService.CreditsafeIP, DeltabatchService.CreditsafeUsername, CrmConnection.LoadCredentials(DeltabatchService.CreditsafeCredentialFilePath, DeltabatchService.Entropy));
            }
            else
            {
                throw new Exception($"Could not find Credentials file at: {DeltabatchService.CreditsafeCredentialFilePath}. Please create with Endeavor NUnit Credentials Manager");
            }
        }

        public static Plugin.LocalPluginContext GenerateLocalContext()
        {
            // Connect to the CRM web service using a connection string.
            CrmServiceClient conn = new CrmServiceClient(CrmConnection.GetCrmConnectionString(DeltabatchService.CredentialFilePath, DeltabatchService.Entropy));
            if (conn.OrganizationWebProxyClient != null)
                conn.OrganizationWebProxyClient.InnerChannel.OperationTimeout = new TimeSpan(10, 0, 0);
            // Cast the proxy client to the IOrganizationService interface.
            IOrganizationService serviceProxy = conn.OrganizationWebProxyClient != null ? conn.OrganizationWebProxyClient : (IOrganizationService)conn.OrganizationServiceProxy;

            Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), serviceProxy, null, new TracingService());

            return localContext;
        }

        internal static void SendErrorMailToDev(Plugin.LocalPluginContext localContext, Exception e)
        {
            SystemUserEntity mls = XrmRetrieveHelper.RetrieveFirst<SystemUserEntity>(localContext, new ColumnSet(SystemUser.Fields.InternalEMailAddress),
                new FilterExpression
                {
                    Conditions =
                    {
                        new ConditionExpression(SystemUserEntity.Fields.DomainName, ConditionOperator.Equal, @"D1\EVSA") // Marie-Louise Sandberg
                    }
                });

            ActivityParty toParty = new ActivityParty
            {
                PartyId = new EntityReference(SystemUserEntity.EntityLogicalName, mls.Id),
                AddressUsed = mls.InternalEMailAddress
            };

            // Create the 'From:' activity party for the email
            ActivityParty fromParty = new ActivityParty
            {
                PartyId = mls.ToEntityReference()
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

            SendEmailRequest sendEmailreq2 = new SendEmailRequest
            {
                EmailId = errorMail.Id,
                TrackingToken = "",
                IssueSend = true
            };

            localContext.OrganizationService.Execute(sendEmailreq2);

            Console.WriteLine("Sent the error-mail message.");
        }
    }
}
