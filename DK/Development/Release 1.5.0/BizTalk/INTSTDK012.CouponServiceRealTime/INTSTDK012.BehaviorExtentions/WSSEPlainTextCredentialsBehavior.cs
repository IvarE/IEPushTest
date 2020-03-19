using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel.Description;
using System.ServiceModel.Channels;
using System.ServiceModel.Configuration;
using System.Configuration;
using System.ServiceModel.Dispatcher;

namespace INTSTDK012.BehaviorExtentions
{
    
    public class INTSTDK012_WSSEPlainTextCredentialsBehavior : IEndpointBehavior
    {
        private String UserName;
        private String Password;
        private String WsaTo;
        private String WsaAction;

        public INTSTDK012_WSSEPlainTextCredentialsBehavior(string userName, string password, string wsaAction, string wsaTo)
        {
            System.Diagnostics.Trace.WriteLine("[INTSTDK012][BehaviorExtentions][INTSTDK012_WSSEPlainTextCredentialsBehavior] INTSTDK012_WSSEPlainTextCredentialsBehavior(" + userName + "," + password + "," + wsaAction + "," + wsaTo + ")");
            this.UserName = userName;
            this.Password = password;
            this.WsaAction = wsaAction;
            this.WsaTo = wsaTo;
        }

        #region IEndpointBehavior Members

        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
            System.Diagnostics.Trace.WriteLine("[INTSTDK012][BehaviorExtentions][INTSTDK012_WSSEPlainTextCredentialsBehavior] AddBindingParameters()");
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            System.Diagnostics.Trace.WriteLine("[INTSTDK012][BehaviorExtentions][INTSTDK012_WSSEPlainTextCredentialsBehavior] ApplyClientBehavior()");
            clientRuntime.MessageInspectors.Add(new INTSTDK012_PlainTextCredentialsMessageInspector(this.UserName, this.Password, this.WsaAction,this.WsaTo));
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
            System.Diagnostics.Trace.WriteLine("[INTSTDK012][BehaviorExtentions][INTSTDK012_WSSEPlainTextCredentialsBehavior] ApplyDispatchBehavior()");
        }

        public void Validate(ServiceEndpoint endpoint)
        {
            System.Diagnostics.Trace.WriteLine("[INTSTDK012][BehaviorExtentions][INTSTDK012_WSSEPlainTextCredentialsBehavior] Validate()");
        }
        #endregion

    }

}
