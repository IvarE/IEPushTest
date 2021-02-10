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
using System.Xml;


namespace INTSTDK012.BehaviorExtentions
{
    public class INTSTDK012_PlainTextCredentialsMessageInspector : IClientMessageInspector
    {
        public INTSTDK012_PlainTextCredentialsMessageInspector(string username, string password, string wsaAction, string wsaTo)
        {
            System.Diagnostics.Trace.WriteLine("[INTSTDK012][BehaviorExtentions][INTSTDK012_PlainTextCredentialsMessageInspector] INTSTDK012_PlainTextCredentialsMessageInspector(" + username + "," + password + "," + wsaAction + "," + wsaTo + ")");
            _username = username;
            _password = password;
            _wsaAction = wsaAction;
            _wsaTo = wsaTo;
        }

        public void AfterReceiveReply(ref System.ServiceModel.Channels.Message reply, object correlationState)
        {
        }

        public object BeforeSendRequest(ref System.ServiceModel.Channels.Message request, System.ServiceModel.IClientChannel channel)
        {
            System.Diagnostics.Trace.WriteLine("[INTSTDK012][BehaviorExtentions][INTSTDK012_PlainTextCredentialsMessageInspector] BeforeSendRequest()");
            var security = new INTSTDK012_WSSESecurity(_username, _password);

            request.Headers.Add(security);

            var wsaTo = new WSATo(_wsaTo);
            request.Headers.Add(wsaTo);

            var wsaAction = new WSAAction(_wsaAction);                      
            request.Headers.Add(wsaAction);
                  
            return null;
        }

        private string _username;
        private string _password;
        private string _wsaTo;
        private string _wsaAction;
    }


}
