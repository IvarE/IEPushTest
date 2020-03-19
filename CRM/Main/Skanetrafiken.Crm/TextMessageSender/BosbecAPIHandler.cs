using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Skanetrafiken.Crm.TextMessageSender
{
    public class BosbecAPIHandler
    {
        public string apiKey, workflowId, baseUrlStr;
        private readonly string callWorkflowUrlStr = "/2/workflows/";
        private readonly string deliveryReportUrlStr = "/2/messages/";


        public TextMessageResponseBase SendTextMessage(string phoneNumber, string senderName, string messageText)
        {
            Uri sendUri = new Uri(baseUrlStr + callWorkflowUrlStr);
            HttpWebRequest httpWebRequest = CreateRequest(sendUri);
            httpWebRequest.Method = "POST";

            string InputJSON = CreateInputJSON(phoneNumber, senderName, messageText);

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(InputJSON);
                streamWriter.Flush();
                streamWriter.Close();
            }

            try
            {
                return GetAPIResponse<TextMessageResponseBase>(httpWebRequest);
            }
            catch (WebException)
            {
                throw;
            }

        }

        public T GetDeliveryReport<T>(string Id) where T : class, new()
        {
            HttpWebRequest httpWebRequest = CreateRequest(new Uri(baseUrlStr + deliveryReportUrlStr + Id));
            httpWebRequest.Method = "GET";

            try
            {
                return GetAPIResponse<T>(httpWebRequest);
            }
            catch (WebException)
            {
                throw;
            }
        }

        public int NumberOfSentMessagesByProcess(string processId)
        {
            HttpWebRequest httpWebRequest = CreateRequest(new Uri(baseUrlStr + callWorkflowUrlStr + processId));
            httpWebRequest.Method = "GET";

            HttpWebResponse httpResponse;
            try
            {
                httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            }
            catch (WebException)
            {
                throw;
            }

            string response;
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                response = streamReader.ReadToEnd();
            }

            string numberOfMessageAttrStr = "numberOfMessagePartsSent";
            if (response.Contains(numberOfMessageAttrStr))
            {

                int index = response.IndexOf(numberOfMessageAttrStr);
                string subStr = response.Substring(index + numberOfMessageAttrStr.Length + 2);
                string numberStr = subStr.Split(',')[0];

                int number;
                if (Int32.TryParse(numberStr, out number))
                {
                    return number;
                }
            }

            return 0;
        }

        private HttpWebRequest CreateRequest(Uri uri)
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(uri);
            httpWebRequest.Headers.Add("api-key", this.apiKey);
            httpWebRequest.ContentType = "application/json";
            return httpWebRequest;
        }

        private static T GetAPIResponse<T>(HttpWebRequest httpWebRequest) where T : class, new()
        {
            HttpWebResponse httpResponse;

            try
            {
                httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            }
            catch (WebException)
            {
                throw;
            }

            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                string responseJson = streamReader.ReadToEnd();

                T responseObj = new T();
                using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(responseJson)))
                {
                    DataContractJsonSerializer ser = new DataContractJsonSerializer(responseObj.GetType());
                    responseObj = ser.ReadObject(ms) as T;
                    ms.Close();
                }
                return responseObj;
            }
        }

        private string CreateInputJSON(string recipient, string sender, string message_text)
        {
            SendMessageRequest sendMessageRequest = new SendMessageRequest()
            {
                workflowid = this.workflowId,
                metadata = new Metadata()
                {
                    recipient = recipient,
                    sender = sender,
                    message_text = message_text
                }
            };
            DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(SendMessageRequest));
            MemoryStream msObj = new MemoryStream();
            js.WriteObject(msObj, sendMessageRequest);
            msObj.Position = 0;
            StreamReader sr = new StreamReader(msObj);
            string json = sr.ReadToEnd();
            sr.Close();
            msObj.Close();

            return json.Replace("_","-");
        }

        [DataContract]
        public class TextMessageResponseBase
        {
            public TextMessageResponseBase() { }

            [DataMember]
            public string processId { get; set; }
                
            [DataMember]
            public string created { get; set; }

        }

        [DataContract]
        public class DeliveryReportResponse : TextMessageResponseBase
        {
            [DataMember]
            public List<ResponseMessage> messages { get; set; }
        }

        [DataContract]
        public class ResponseMessage
        {
            [DataMember]
            public string messageId { get; set; }
            [DataMember]
            public string to { get; set; }
            [DataMember]
            public string status { get; set; }
            [DataMember]
            public string statusType { get; set; }
        }

        [DataContract]
        public class Metadata
        {
            [DataMember]
            public string recipient { get; set; }
            [DataMember]
            public string sender { get; set; }
            [DataMember]
            public string message_text { get; set; }
}

        [DataContract]
        public class SendMessageRequest
        {
            [DataMember]
            public string workflowid { get; set; }
            [DataMember]
            public Metadata metadata { get; set; }
        }
    }
}
