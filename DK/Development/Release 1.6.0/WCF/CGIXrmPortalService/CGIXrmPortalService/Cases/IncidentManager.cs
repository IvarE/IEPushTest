using System;
using Generic=System.Collections.Generic;
using System.Linq;
using System.Web;
using CGIXrmWin;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System.Collections.ObjectModel;
using System.Threading;
using System.Configuration;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Messages;
using System.Data.SqlClient;
using System.Xml;
using System.Xml.Serialization;
using System.Threading.Tasks;
using System.IO;
using CGICRMPortalService.Models;


namespace CGICRMPortalService
{
    public class IncidentManager
    {
        private XrmManager xrmMgr;
        private XrmHelper xrmHelper;
        public IncidentManager()
        {

            xrmHelper = new XrmHelper();
            xrmMgr = xrmHelper.GetXrmManagerFromAppSettings(Guid.Empty);
        }

        public IncidentManager(Guid callerId)
        {
            xrmHelper = new XrmHelper();
            xrmMgr = xrmHelper.GetXrmManagerFromAppSettings(callerId);
        }
        #region [Private Method]
        private Case GetCasefromIncident(Incident incident)
        {
            Case customerCase = new Case();
            customerCase.AccountNumber = incident.AccountNumber;
            customerCase.CaseNumber = incident.CaseNumber;
            customerCase.CaseStatus = incident.CaseStatusText;
            customerCase.CaseType = incident.CaseTypeValue;
            customerCase.CustomerId = incident.Account.Id;
            customerCase.Description = incident.Description;
            customerCase.Email = incident.Email;
            customerCase.CaseId = incident.IncidentId;
            customerCase.TelephoneNumber = incident.TelephoneNumber;
            customerCase.Title = incident.Title;
            customerCase.TravelCardNumber = incident.TravelCardNumber;
            return customerCase;
        }
        private Incident GetIncidentromCustomerCase(Case customerCase)
        {
            Incident incident = new Incident();
            incident.Account = customerCase.CustomerId!=Guid.Empty?new EntityReference("account", customerCase.CustomerId):null;
            incident.AccountNumber = customerCase.AccountNumber;
            incident.CaseOrigin = new OptionSetValue();
            incident.CaseType = customerCase.CaseType > 0 ? new OptionSetValue(customerCase.CaseType) : null;
            incident.TelephoneNumber = customerCase.TelephoneNumber;
            incident.Title = customerCase.Title;
            incident.Description = customerCase.Description;
            incident.Email = customerCase.Email;
            Guid travelCardId=xrmHelper.GetIdByValue(customerCase.TravelCardNumber, "cgi_travelcardnumber", "cgi_travelcard", xrmMgr);
            if(travelCardId!=Guid.Empty)
                incident.TravelCard = new EntityReference("cgi_travelcardnumber",travelCardId);            
            
            return incident;
        }
        private string GetCaseNumber(Guid caseId)
        {
            string retValue = string.Empty;
            Incident incident = xrmMgr.Get<Incident>(caseId, new string[] { "ticketnumber" });
            if (incident != null)
                retValue = incident.CaseNumber;

            return retValue;
        }
        #endregion
        #region [Public Method]
        internal CreateCaseResponse CreateCase(Case customerCase)
        {
            CreateCaseResponse response = null;
            Incident incident = GetIncidentromCustomerCase(customerCase);
            Entity entity=xrmMgr.Create<Incident>(incident);
            if (entity != null)
            {
                response.CaseNumber = entity.Attributes.ContainsKey("ticketnumber") ? entity.GetAttributeValue<string>("ticketnumber") : GetCaseNumber(entity.Id);
                response.CaseId = entity.Id;
            }
            return response;
        }
        #endregion

    }
}