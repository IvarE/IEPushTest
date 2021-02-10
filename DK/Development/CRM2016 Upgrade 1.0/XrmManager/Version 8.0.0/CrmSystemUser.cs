using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ServiceModel;


namespace CGIXrmWin
{
    [DataContract]
    public class SystemUser
    {
        private string pFirstName;
        [DataMember]
        public string FirstName
        {
            get { return pFirstName; }
            set
            {
                if (pFirstName != value)
                {
                    pFirstName = value;
                }
            }
        }

        private string pLastName;
        [DataMember]
        public string LastName
        {
            get { return pLastName; }
            set
            {
                if (pLastName != value)
                {
                    pLastName = value;
                }
            }
        }

        private string pFulllName;
        [DataMember]
        public string FullName
        {
            get { return pFulllName; }
            set
            {
                if (pFulllName != value)
                {
                    pFulllName = value;
                }
            }
        }

        private Guid pSystemUserId;
        [DataMember]
        public Guid SystemUserId
        {
            get { return pSystemUserId; }
            set
            {
                if (pSystemUserId != value)
                {
                    pSystemUserId = value;
                }
            }
        }

    }


}
