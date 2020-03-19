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
    public class INTSTDK012_WSSEPlainTextCredentialsBehaviorExt : BehaviorExtensionElement
    {
        [ConfigurationProperty("UserName")]
        public string UserName { get { return (string)base["UserName"]; } set { base["UserName"] = value; } }
        [ConfigurationProperty("Password")]
        public string Password { get { return (string)base["Password"]; } set { base["Password"] = value; } }
        [ConfigurationProperty("WSAAction")]
        public string WsaAction { get { return (string)base["WSAAction"]; } set { base["WSAAction"] = value; } }
        [ConfigurationProperty("WSATo")]
        public string WsaTo { get { return (string)base["WSATo"]; } set { base["WSATo"] = value; } }

        public override Type BehaviorType
        {
            get { return typeof(INTSTDK012_WSSEPlainTextCredentialsBehavior); }
        }

        protected override object CreateBehavior()
        {
            System.Diagnostics.Trace.WriteLine("[INTSTDK012][BehaviorExtentions][INTSTDK012_WSSEPlainTextCredentialsBehaviorExt] CreateBehavior()");
            return new INTSTDK012_WSSEPlainTextCredentialsBehavior(this.UserName, this.Password,this.WsaAction,this.WsaTo);
        }
    }

}
