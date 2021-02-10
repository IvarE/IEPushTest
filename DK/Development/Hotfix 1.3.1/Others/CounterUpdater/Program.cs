using CRM_Connection;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CounterUpdater
{
    class Program
    {
        private static xRMConnection _concrm;
        private static OrganizationServiceProxy _service;
        private const string CONTACT = "CONTACT";
        private const string CGI_CONTACTNUMBER = "cgi_contactnumber";
        private const string CONTACTVALUE = "0";
        private static string[] _commandLineArguments = new string[4];
        private static EntityCollection _records;
        private static int _retrievedContactsCount;
        private static int _duplicateCount;
        private static int _counterValueToIncrease;
    
        static void Main(string[] args)
        {
            _commandLineArguments = args;
            
            if (_commandLineArguments == null || _commandLineArguments.Count() < 2)
            {
                Console.WriteLine("Please supply server as argument one, username as argument two and password as argument three.");
                Console.WriteLine("Server could be: http://v-dkcrm-utv/Skanetrafiken  or http://v-dkcrm-tst/DKCRM or https://sekunduat.skanetrafiken.se/DKCRMUAT or https://sekund.skanetrafiken.se/DKCRM");
                Console.WriteLine("Log will be written to CounterUpdater.txt in same location as executable is run. Be sure to have write access to this location.");
                Console.WriteLine("Hit key to exit");
                Console.ReadKey();
                return;
            }

            SetupLogging();

            Trace.WriteLine(string.Format("Starting new processing at {0} for server {1}", DateTime.Now, _commandLineArguments[0]));

            CreateConnectionAndService();
            GetAllActiveContacts();
            CountDuplicates();
            UpdateCounterValueToReflectDuplicatesThatWillBeUpdated();
            UpdateContactsThatAreDuplicates();
            CheckIfAllDuplicatesAreGone();

            Trace.WriteLine(string.Format("End of processing at {0}", DateTime.Now));
            Trace.WriteLine("All done.");
            Trace.WriteLine("");
        }

        private static void SetupLogging()
        {
            Trace.Listeners.Clear();

            TextWriterTraceListener textWriterTraceListerner = new TextWriterTraceListener("./CounterUpdater.txt");
            textWriterTraceListerner.Name = "TextLogger";
            textWriterTraceListerner.TraceOutputOptions = TraceOptions.DateTime;

            ConsoleTraceListener consoleTraceListerner = new ConsoleTraceListener(false);
            consoleTraceListerner.TraceOutputOptions = TraceOptions.DateTime;

            Trace.Listeners.Add(textWriterTraceListerner);
            Trace.Listeners.Add(consoleTraceListerner);
            Trace.AutoFlush = true;
        }

        private static void CheckIfAllDuplicatesAreGone()
        {
            Trace.WriteLine("Checking if all duplicates are gone");
            Trace.WriteLine("Getting all active contacts");
            QueryByAttribute querybyexpression = new QueryByAttribute(CONTACT.ToLower());
            querybyexpression.ColumnSet = new ColumnSet("cgi_contactnumber");
            querybyexpression.Attributes.Add("statecode");
            querybyexpression.Values.Add(0); // 0 is active value

            //Query passed to the service proxy    
            EntityCollection contactRecords = _service.RetrieveMultiple(querybyexpression);

            Trace.WriteLine(string.Format("Retrieved {0} {1}  ", contactRecords.Entities.Count, CONTACT.ToLower()));

            HashSet<string> nonDuplicates = new HashSet<string>();
            bool areThereDuplicates = false;
            int duplicateCount = 0;
            for (int i = 0; i < contactRecords.Entities.Count; i++) // TODO warning length vs count?? 0 based?
            {
                Entity dbRecord = contactRecords.Entities[i];
                string cgi_contactNumber = dbRecord["cgi_contactnumber"].ToString();
                if (nonDuplicates.Contains(cgi_contactNumber) == false)  /*nonDuplictes.ContainsKey(dbRecord["cgi_contactnumber"].ToString()) == false*/
                {
                    //nonDuplictes.Add(dbRecord["cgi_contactnumber"].ToString(), dbRecord.Id);
                    nonDuplicates.Add(cgi_contactNumber);
                }
                else
                {
                    duplicateCount++;
                    areThereDuplicates = true;
                }
            }
            if (areThereDuplicates==true)
            {
                Trace.WriteLine(string.Format("I'm sorry but there are still {0} duplicates. Troubleshooting is needed. Program seems faulty or just a few didn't update.",duplicateCount));
            }
            else
            {
                Trace.WriteLine("Excellent, there are no duplictes");
            }
        }

        private static void UpdateContactsThatAreDuplicates()
        {
            if (_duplicateCount < 1) return;

            HashSet<string> nonDuplicates = new HashSet<string>();            
            for (int i = 0; i < _records.Entities.Count; i++) // TODO warning length vs count?? 0 based?
            {
                Entity dbRecord = _records.Entities[i];
                string cgi_contactNumber = dbRecord["cgi_contactnumber"].ToString();
                if (nonDuplicates.Contains(cgi_contactNumber) == true) 
                {
                    Entity contact = new Entity();
                    contact.Id = dbRecord.Id;
                    contact.LogicalName = dbRecord.LogicalName;
                    contact["cgi_contactnumber"] = _counterValueToIncrease.ToString();
                    try
                    {
                        _service.Update(contact);
                    }
                    catch (Exception ex)
                    {
                        Trace.WriteLine(string.Format("Failed to update a contact with contactid {0} and it had original cgi_customernumber {1} and we tried to update it to {2}", dbRecord.Id, dbRecord["cgi_contactnumber"].ToString(), _counterValueToIncrease));
                        continue; // lets not leave a gap, but instead try with the next contact with the same number
                    }
                    Trace.WriteLine(string.Format("Updated a contact with contactid {0} and it had original cgi_customernumber {1} and we updated it to {2}", dbRecord.Id, dbRecord["cgi_contactnumber"].ToString(), _counterValueToIncrease));
                    _counterValueToIncrease++;
                    System.Threading.Thread.Sleep(1000); // INFO: let user be able to stop program, dont work to fast
                }
                else
                {
                    nonDuplicates.Add(cgi_contactNumber);
                }
            }
        }

        private static void CountDuplicates()
        {
            HashSet<string> nonDuplicates = new HashSet<string>();
            //Dictionary<string, Guid> nonDuplictes = new Dictionary<string, Guid>();
            for (int i = 0; i < _records.Entities.Count; i++) // TODO warning length vs count?? 0 based?
            {
                Entity dbRecord = _records.Entities[i];
                string cgi_contactNumber = dbRecord["cgi_contactnumber"].ToString();
                if (nonDuplicates.Contains(cgi_contactNumber)==false)  /*nonDuplictes.ContainsKey(dbRecord["cgi_contactnumber"].ToString()) == false*/
                {
                    //nonDuplictes.Add(dbRecord["cgi_contactnumber"].ToString(), dbRecord.Id);
                    nonDuplicates.Add(cgi_contactNumber);
                }
                else
                {
                    _duplicateCount++;
                }
            }
            Trace.WriteLine(string.Format("Counted {0} duplicates",_duplicateCount));
        }

        private static void UpdateCounterValueToReflectDuplicatesThatWillBeUpdated()
        {
            if (_duplicateCount < 1) return;

            QueryByAttribute querybyexpression = new QueryByAttribute("cgi_autonumber");
            querybyexpression.ColumnSet = new ColumnSet("cgi_lastused");
            querybyexpression.Attributes.Add("statecode");
            querybyexpression.Values.Add(0); // 0 is active value
            querybyexpression.Attributes.Add("cgi_entity");
            querybyexpression.Values.Add("contact"); // 0 is active value

            //Query passed to the service proxy    
            EntityCollection counterRecords = _service.RetrieveMultiple(querybyexpression);

                Entity dbRecord = counterRecords.Entities.First();

                string lastused = dbRecord.Attributes["cgi_lastused"].ToString();
                Int32 nextnumberToUse = Convert.ToInt32(lastused) + _duplicateCount;

                _counterValueToIncrease = Convert.ToInt32(lastused);
                Trace.WriteLine(string.Format("Original counter value is {0}", _counterValueToIncrease));
                Trace.WriteLine(string.Format("I will create a gap of {0} in counter", _duplicateCount));
                Trace.WriteLine(string.Format("Updated final counter value is {0}", nextnumberToUse.ToString()));

                Entity counter = new Entity();
                counter.Id = dbRecord.Id;
                counter.LogicalName = dbRecord.LogicalName;
                counter["cgi_lastused"] = nextnumberToUse.ToString();

                _service.Update(counter);
        }

        // TODO paging cookie???
        private static void GetAllActiveContacts()
        {
            Trace.WriteLine("Getting all active contacts");
            QueryByAttribute querybyexpression = new QueryByAttribute(CONTACT.ToLower());
            querybyexpression.ColumnSet = new ColumnSet("cgi_contactnumber");
            querybyexpression.Attributes.Add("statecode");
            querybyexpression.Values.Add(0); // 0 is active value

            //Query passed to the service proxy    
            _records = _service.RetrieveMultiple(querybyexpression);

            _retrievedContactsCount = _records.Entities.Count;
            Trace.WriteLine(string.Format("Retrieved {0} {1}  ", _retrievedContactsCount, CONTACT.ToLower()));
        }

        private static void CreateConnectionAndService()
        {
            xRMConnection concrm;
            string serverUrl = _commandLineArguments[0];
            concrm = new xRMConnection(serverUrl);
            concrm.Authenticationtype = AuthenticationType.User; // NOTE: this must be user otherwise below username/pwd/domain is not used....
            concrm.Crmtype = CRMType.OnPremis;
            concrm.Username = _commandLineArguments[1];
            concrm.Password = _commandLineArguments[2];
            concrm.Domain = "D1";
            _concrm = concrm;
            _service = _concrm.GetService();
        }

    }
}
