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
    public class WSSEPlainTextCredentialsBehaviorExt : BehaviorExtensionElement
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
            get { return typeof(WSSEPlainTextCredentialsBehavior); }
        }

        protected override object CreateBehavior()
        {
            return new WSSEPlainTextCredentialsBehavior(this.UserName, this.Password,this.WsaAction,this.WsaTo);
        }
    }

}
