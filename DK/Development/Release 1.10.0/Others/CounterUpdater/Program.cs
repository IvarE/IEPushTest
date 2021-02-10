using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace CounterUpdater
{
    class Program
    {
        #region Declarations
        private static xRMConnection _concrm;
        private static OrganizationServiceProxy _service;
        private const string Contact = "CONTACT";
        private const string CgiContactnumber = "cgi_contactnumber";
        private const string Contactvalue = "0";
        private static string[] _commandLineArguments = new string[4];
        private static List<Entity> _records;
        private static int _retrievedContactsCount;
        private static int _duplicateCount;
        private static int _counterValueToIncrease;
        #endregion

        #region Main
        static void Main(string[] args)
        {
            _commandLineArguments = args;

            if (_commandLineArguments == null || _commandLineArguments.Length < 2)
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
        #endregion

        #region Private Methods
        private static void SetupLogging()
        {
            Trace.Listeners.Clear();

            TextWriterTraceListener textWriterTraceListerner = new TextWriterTraceListener("./CounterUpdater.txt")
            {
                Name = "TextLogger",
                TraceOutputOptions = TraceOptions.DateTime
            };

            ConsoleTraceListener consoleTraceListerner = new ConsoleTraceListener(false)
            {
                TraceOutputOptions = TraceOptions.DateTime
            };

            Trace.Listeners.Add(textWriterTraceListerner);
            Trace.Listeners.Add(consoleTraceListerner);
            Trace.AutoFlush = true;
        }

        private static void CheckIfAllDuplicatesAreGone()
        {
            Trace.WriteLine("Checking if all duplicates are gone");
            Trace.WriteLine("Getting all active contacts, 5000 per call. Please wait a while for query to run.");

            List<Entity> contactRecords = new List<Entity>();

            // Query using the paging cookie.
            // Define the paging attributes.
            // The number of records per page to retrieve.
            int queryCount = 5000;

            // Initialize the page number.
            int pageNumber = 1;

            // Define the condition expression for retrieving records.
            ConditionExpression pagecondition = new ConditionExpression
            {
                AttributeName = "statecode",
                Operator = ConditionOperator.Equal
            };
            pagecondition.Values.Add(0);

            // Create the query expression and add condition.
            QueryExpression pagequery = new QueryExpression
            {
                EntityName = Contact.ToLower()
            };
            pagequery.Criteria.AddCondition(pagecondition);
            pagequery.ColumnSet.AddColumns("cgi_contactnumber");

            // Assign the pageinfo properties to the query expression.
            pagequery.PageInfo = new PagingInfo
            {
                Count = queryCount,
                PageNumber = pageNumber,
                PagingCookie = null
            };

            // The current paging cookie. When retrieving the first page, 
            // pagingCookie should be null.

            while (true)
            {
                Trace.Write(".");

                //Query passed to the service proxy    
                EntityCollection results = _service.RetrieveMultiple(pagequery);
                contactRecords.AddRange(results.Entities);

                if (results.MoreRecords)
                {
                    // Increment the page number to retrieve the next page.
                    pagequery.PageInfo.PageNumber++;

                    // Set the paging cookie to the paging cookie returned from current results.
                    pagequery.PageInfo.PagingCookie = results.PagingCookie;
                }
                else
                {
                    break;
                }
            }
            Trace.WriteLine("");
            Trace.WriteLine(string.Format("Retrieved {0} {1}  ", contactRecords.Count, Contact.ToLower()));

            HashSet<string> nonDuplicates = new HashSet<string>();
            bool areThereDuplicates = false;
            int duplicateCount = 0;
            foreach (Entity dbRecord in contactRecords)
            {
                if (dbRecord.Attributes.ContainsKey("cgi_contactnumber"))
                {
                    string cgiContactNumber = dbRecord["cgi_contactnumber"].ToString();
                    if (nonDuplicates.Contains(cgiContactNumber) == false)
                    {
                        nonDuplicates.Add(cgiContactNumber);
                    }
                    else
                    {
                        duplicateCount++;
                        areThereDuplicates = true;
                    }
                }
                else
                {
                    Console.WriteLine("Missing cgi_contactnumber attribute");
                }
            }
            if (areThereDuplicates)
            {
                Trace.WriteLine(string.Format("I'm sorry but there are still {0} duplicates. Troubleshooting is needed. Program seems faulty or just a few didn't update.", duplicateCount));
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
            foreach (Entity dbRecord in _records)
            {
                if (dbRecord.Attributes.ContainsKey("cgi_contactnumber"))
                {
                    string cgiContactNumber = dbRecord["cgi_contactnumber"].ToString();
                    if (nonDuplicates.Contains(cgiContactNumber))
                    {
                        Entity contact = new Entity
                        {
                            Id = dbRecord.Id,
                            LogicalName = dbRecord.LogicalName
                        };
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
                        nonDuplicates.Add(cgiContactNumber);
                    }
                }
                else
                {
                    Console.WriteLine("Missing cgi_contactnumber attribute");
                }
            }
        }

        private static void CountDuplicates()
        {
            HashSet<string> nonDuplicates = new HashSet<string>();
            foreach (Entity dbRecord in _records)
            {
                if (dbRecord.Attributes.ContainsKey("cgi_contactnumber"))
                {
                    string cgiContactNumber = dbRecord["cgi_contactnumber"].ToString();
                    if (nonDuplicates.Contains(cgiContactNumber) == false)
                    {
                        nonDuplicates.Add(cgiContactNumber);
                    }
                    else
                    {
                        _duplicateCount++;
                    }
                }
                else
                {
                    Console.WriteLine("Missing cgi_contactnumber attribute");
                }
            }
            Trace.WriteLine(string.Format("Counted {0} duplicates", _duplicateCount));
        }

        private static void UpdateCounterValueToReflectDuplicatesThatWillBeUpdated()
        {
            if (_duplicateCount < 1) return;

            QueryByAttribute querybyexpression = new QueryByAttribute("cgi_autonumber")
            {
                ColumnSet = new ColumnSet("cgi_lastused")
            };
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

            Entity counter = new Entity
            {
                Id = dbRecord.Id,
                LogicalName = dbRecord.LogicalName
            };
            counter["cgi_lastused"] = nextnumberToUse.ToString();

            _service.Update(counter);
            _counterValueToIncrease++;
        }

        // TODO paging cookie???
        private static void GetAllActiveContacts()
        {
            Trace.WriteLine("Getting all active contacts, 5000 per call. Please wait a while for query to run.");
            _records = new List<Entity>();

            // Query using the paging cookie.
            // Define the paging attributes.
            // The number of records per page to retrieve.
            int queryCount = 5000;

            // Initialize the page number.
            int pageNumber = 1;

            // Define the condition expression for retrieving records.
            ConditionExpression pagecondition = new ConditionExpression
            {
                AttributeName = "statecode",
                Operator = ConditionOperator.Equal
            };
            pagecondition.Values.Add(0);

            // Create the query expression and add condition.
            QueryExpression pagequery = new QueryExpression
            {
                EntityName = Contact.ToLower()
            };
            pagequery.Criteria.AddCondition(pagecondition);
            pagequery.ColumnSet.AddColumns("cgi_contactnumber");

            // Assign the pageinfo properties to the query expression.
            pagequery.PageInfo = new PagingInfo
            {
                Count = queryCount,
                PageNumber = pageNumber,
                PagingCookie = null
            };

            // The current paging cookie. When retrieving the first page, 
            // pagingCookie should be null.

            while (true)
            {
                Trace.Write(".");

                //Query passed to the service proxy    
                EntityCollection results = _service.RetrieveMultiple(pagequery);
                _records.AddRange(results.Entities);

                if (results.MoreRecords)
                {
                    // Increment the page number to retrieve the next page.
                    pagequery.PageInfo.PageNumber++;

                    // Set the paging cookie to the paging cookie returned from current results.
                    pagequery.PageInfo.PagingCookie = results.PagingCookie;
                }
                else
                {
                    break;
                }
            }

            _retrievedContactsCount = _records.Count;
            Trace.WriteLine("");
            Trace.WriteLine(string.Format("Retrieved {0} {1}  ", _retrievedContactsCount, Contact.ToLower()));
        }

        private static void CreateConnectionAndService()
        {
            string serverUrl = _commandLineArguments[0];
            var concrm = new xRMConnection(serverUrl)
            {
                Authenticationtype = AuthenticationType.User,
                Crmtype = CRMType.OnPremis,
                Username = _commandLineArguments[1],
                Password = _commandLineArguments[2],
                Domain = "D1"
            };
            // NOTE: this must be user otherwise below username/pwd/domain is not used....
            _concrm = concrm;
            _service = _concrm.GetService();
        }
        #endregion
    }
}
