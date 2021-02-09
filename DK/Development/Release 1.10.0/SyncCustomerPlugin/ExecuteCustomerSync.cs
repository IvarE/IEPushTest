using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SyncCustomerPlugin
{
    static class ExecuteCustomerSync
    {
        static public void Sync(string id, string filePath)
        {
            SyncFromCrmtoEPiRequestParameters customerUpdateRequest = new SyncFromCrmtoEPiRequestParameters();
            customerUpdateRequest.CustomerId = new CustomerId();
            customerUpdateRequest.CustomerId.Text = id;

            var serializerCustomer = new XmlSerializer(typeof(SyncFromCrmtoEPiRequestParameters));
            using (var stream = new StreamWriter(filePath + customerUpdateRequest.CustomerId.Text + ".xml"))
                serializerCustomer.Serialize(stream, customerUpdateRequest);

        }
    }


}
