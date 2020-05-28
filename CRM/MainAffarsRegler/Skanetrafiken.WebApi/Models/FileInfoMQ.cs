using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Skanetrafiken.Crm.Models
{
    public class FileInfoMQ
    {

        private string orderId;
        private string fileName;

        /// <summary>
        /// Order Number in CRM
        /// </summary>
        public string OrderId
        {
            get
            {
                return this.orderId;
            }
            set
            {
                this.orderId = value;
            }
        }

        /// <summary>
        /// Report Name File
        /// </summary>
        public string FileName
        {
            get
            {
                return this.fileName;
            }
            set
            {
                this.fileName = value;
            }
        }
    }
}