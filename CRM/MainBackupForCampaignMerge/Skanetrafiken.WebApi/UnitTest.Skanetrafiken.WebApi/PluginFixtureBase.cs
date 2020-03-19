using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using NUnit.Framework;
using Rhino.Mocks;

namespace Endeavor.Crm.UnitTest
{
    public class PluginFixtureBase
    {
        private string _organizationName;

        private Guid _initiatingUserId;

        protected IPluginExecutionContext PluginExecutionContext;

        protected ITracingService TracingService;

        protected EntityImageCollection PreImage;

        protected EntityImageCollection PostImage;

        /// <summary>
        /// Stores the organization service proxy.
        /// </summary>
        protected OrganizationServiceProxy _serviceProxy;

        public string LastTraceMessage { get; set; }

        protected Guid InitiatingUserId
        {
            get
            {
                if (_initiatingUserId != Guid.Empty)
                {
                    return _initiatingUserId;
                }

                if (_serviceProxy != null)
                {
                    WhoAmIResponse whoAmI = (WhoAmIResponse)_serviceProxy.Execute(new WhoAmIRequest());
                    _initiatingUserId = whoAmI.UserId;
                    return _initiatingUserId;
                }

                return Guid.Empty;
            }
            set
            {
                _initiatingUserId = value;
            }
        }

        protected string OrganizationName
        {
            get
            {
                if (!string.IsNullOrEmpty(_organizationName))
                {
                    return _organizationName;
                }

                if (_serviceProxy != null)
                {
                    WhoAmIResponse whoAmI = (WhoAmIResponse)_serviceProxy.Execute(new WhoAmIRequest());

                    RetrieveVersionRequest versionRequest = new RetrieveVersionRequest();
                    RetrieveVersionResponse versionResponse = (RetrieveVersionResponse)_serviceProxy.Execute(versionRequest);

                    Version version = new Version(versionResponse.Version);

                    if (version.Major >= 7)
                    {
                        RetrieveCurrentOrganizationRequest organizationRequest = new RetrieveCurrentOrganizationRequest();
                        RetrieveCurrentOrganizationResponse organizationResponse = (RetrieveCurrentOrganizationResponse)_serviceProxy.Execute(versionRequest);

                        if(organizationResponse != null)
                        {
                            _organizationName = organizationResponse.Detail.UniqueName;
                        }
                    }

                    if (!string.IsNullOrEmpty(_organizationName))
                    {
                        return _organizationName;
                    }

                    QueryExpression qe = new QueryExpression("organization");
                    qe.ColumnSet = new ColumnSet(new string[] { "name" });
                    qe.Criteria.AddCondition("organizationid", ConditionOperator.Equal, whoAmI.OrganizationId);

                    EntityCollection orgs = _serviceProxy.RetrieveMultiple(qe);

                    if (orgs != null && orgs.Entities.Count > 0)
                    {
                        var org = orgs[0];
                        if (org.Contains("name"))
                        {
                            _organizationName = org["name"].ToString();
                            return _organizationName;
                        }
                    }
                }
                return null;
            }

            set
            {
                _organizationName = value;
            }
        }

        [SetUp]
        public void SetupPluginContext()
        {
            // IPluginExecutionContext
            this.PluginExecutionContext = MockRepository.GenerateStub<IPluginExecutionContext>();
            this.PluginExecutionContext.Stub(x => x.PreEntityImages).IgnoreArguments().Return(this.PreImage);
            this.PluginExecutionContext.Stub(x => x.PostEntityImages).IgnoreArguments().Return(this.PostImage);
            this.PluginExecutionContext.Stub(x => x.InitiatingUserId)
                .IgnoreArguments()
                .Return(this.InitiatingUserId)
                .WhenCalled(call => { call.ReturnValue = this.InitiatingUserId; });
            this.PluginExecutionContext.Stub(x => x.OrganizationName)
                .IgnoreArguments()
                .Return(this.OrganizationName)
                .WhenCalled(call => { call.ReturnValue = this.OrganizationName; });

            // ITracingService
            this.TracingService = MockRepository.GenerateStub<ITracingService>();
            this.TracingService.Stub(x => x.Trace(null, null)).IgnoreArguments().Do((Action<string, object[]>)delegate (string f, object[] o)
            {
                this.LastTraceMessage = f;
                Debug.WriteLine(f, o);
            });
        }

        [TearDown]
        public void TearDownPluginContextAndServices()
        {
            this.PluginExecutionContext = null;
            this.TracingService = null;
            PreImage = null;
            PostImage = null;
        }
    }
}
