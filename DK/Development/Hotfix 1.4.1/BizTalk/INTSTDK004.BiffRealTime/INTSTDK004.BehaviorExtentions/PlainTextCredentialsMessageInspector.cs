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


namespace INTSTDK004.BehaviorExtentions
{
    public class PlainTextCredentialsMessageInspector : IClientMessageInspector
    {
        public PlainTextCredentialsMessageInspector(string username, string password, string wsaAction, string wsaTo)
        {
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
            var security = new WSSESecurity(_username, _password);

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
