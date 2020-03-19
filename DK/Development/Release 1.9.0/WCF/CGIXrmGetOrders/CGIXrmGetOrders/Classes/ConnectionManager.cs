using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CGIXrmGetOrders.GetOrdersServiceReference;
using CGIXrmGetOrders.CreditOrderServiceReference;
using System.Diagnostics;
using CGIXrmWin;
using System.Configuration;

namespace CGIXrmGetOrders
{
    class ConnectionManager
    {

        public GetOrdersServiceReference.BizTalkServiceInstance GetOrdersServiceClient()
        {
            try
            {

                GetOrdersServiceReference.BizTalkServiceInstance client = new GetOrdersServiceReference.BizTalkServiceInstance();

                client.Url = ConfigurationManager.AppSettings["GetOrdersService"];

                return client;

            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("CGIXrmGetOrders", "Error occured in ConnectionManager.GetOrdersServiceClient.\n" + ex.Message + "\n" + ex.StackTrace, EventLogEntryType.Error);
                throw;
            }
        }

        public CreditOrderServiceReference.BizTalkServiceInstance CreditOrderServiceClient()
        {
            try
            {

                CreditOrderServiceReference.BizTalkServiceInstance client = new CreditOrderServiceReference.BizTalkServiceInstance();

                client.Url = ConfigurationManager.AppSettings["CreditOrderService"];

                return client;

            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("CGIXrmGetOrders", "Error occured in ConnectionManager.CreditOrderServiceClient.\n" + ex.Message + "\n" + ex.StackTrace, EventLogEntryType.Error);
                throw;
            }
        }
    }
}
