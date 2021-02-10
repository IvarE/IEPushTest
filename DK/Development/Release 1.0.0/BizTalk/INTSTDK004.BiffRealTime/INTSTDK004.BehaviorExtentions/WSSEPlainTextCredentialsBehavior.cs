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

namespace INTSTDK004.BehaviorExtentions
{
    
    public class WSSEPlainTextCredentialsBehavior : IEndpointBehavior
    {
        private String UserName;
        private String Password;
        private String WsaTo;
        private String WsaAction;

        public WSSEPlainTextCredentialsBehavior(string userName, string password, string wsaAction, string wsaTo)
        {
            this.UserName = userName;
            this.Password = password;
            this.WsaAction = wsaAction;
            this.WsaTo = wsaTo;
        }

        #region IEndpointBehavior Members

        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            clientRuntime.MessageInspectors.Add(new PlainTextCredentialsMessageInspector(this.UserName, this.Password, this.WsaAction,this.WsaTo));
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
        }

        public void Validate(ServiceEndpoint endpoint)
        {
        }
        #endregion

    }

}
