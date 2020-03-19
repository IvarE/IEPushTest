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
    public class INTSTDK012_WSSESecurity : MessageHeader
    {
        private String UserName;
        private String Password;

        public INTSTDK012_WSSESecurity(string userName, string password)
        {
            System.Diagnostics.Trace.WriteLine("[INTSTDK012][BehaviorExtentions][INTSTDK012_WSSESecurity] INTSTDK012_WSSESecurity("+userName +"," +password+")");
            this.UserName = userName;
            this.Password = password;
        }

        protected override void OnWriteStartHeader(System.Xml.XmlDictionaryWriter writer, MessageVersion messageVersion)
        {
            System.Diagnostics.Trace.WriteLine("[INTSTDK012][BehaviorExtentions][INTSTDK012_WSSESecurity] OnWriteStartHeader()");
            base.OnWriteStartHeader(writer, messageVersion);
           
            writer.WriteAttributeString("xmlns:wsse", WsseNamespaceToken);
        }

        protected override void OnWriteHeaderContents(System.Xml.XmlDictionaryWriter writer, MessageVersion messageVersion)
        {
            System.Diagnostics.Trace.WriteLine("[INTSTDK012][BehaviorExtentions][INTSTDK012_WSSESecurity] OnWriteHeaderContents()");
            string sHeader = "<wsse:UsernameToken><wsse:Username>{USERNAME}</wsse:Username><wsse:Password Type=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordText\">{PASSWORD}</wsse:Password></wsse:UsernameToken>";
            writer.WriteRaw(sHeader.Replace("{USERNAME}", this.UserName).Replace("{PASSWORD}", this.Password));
        }

        public override string Name
        {
            get { return "wsse:Security"; }
        }

        public override string Namespace
        {
            get { return ""; }
        }
    
        public override bool MustUnderstand
        {
            get
            {
                return false;
            }
        }

        private const string WsseNamespaceToken = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd";
    }



    public class WSATo : MessageHeader
    {
        private String WsaTo;

        public WSATo(string wsaTo)
        {
            this.WsaTo = wsaTo;
        }

        protected override void OnWriteStartHeader(System.Xml.XmlDictionaryWriter writer, MessageVersion messageVersion)
        {
            base.OnWriteStartHeader(writer, messageVersion);
            writer.WriteAttributeString("xmlns:wsa", WsaNamespaceToken);
        }

        protected override void OnWriteHeaderContents(System.Xml.XmlDictionaryWriter writer, MessageVersion messageVersion)
        {
            string sHeader = "{WSATO}";

            writer.WriteRaw(sHeader.Replace("{WSATO}", this.WsaTo));
        }

        public override string Name
        {
            get { return "wsa:To"; }
        }

        public override string Namespace
        {
            get { return ""; }
        }
        private const string WsaNamespaceToken = "http://schemas.xmlsoap.org/ws/2004/08/addressing";
    }

    public class WSAAction : MessageHeader
    {
        private String WsaAction;

        public WSAAction(string wsaAction)
        {
            this.WsaAction = wsaAction;
        }

        protected override void OnWriteStartHeader(System.Xml.XmlDictionaryWriter writer, MessageVersion messageVersion)
        {
            base.OnWriteStartHeader(writer, messageVersion);
            writer.WriteAttributeString("xmlns:wsa", WsaNamespaceToken);
        }

        protected override void OnWriteHeaderContents(System.Xml.XmlDictionaryWriter writer, MessageVersion messageVersion)
        {
            string sHeader = "{WSAACTION}";

            writer.WriteRaw(sHeader.Replace("{WSAACTION}", this.WsaAction));
        }

        public override string Name
        {
            get { return "wsa:Action"; }
        }

        public override string Namespace
        {
            get { return ""; }
        }
        private const string WsaNamespaceToken = "http://schemas.xmlsoap.org/ws/2004/08/addressing";
    }
}
