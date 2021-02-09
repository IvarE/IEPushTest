using CRM_Connection;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;

namespace UpdateCustomerFromEpiServer
{
    class Program
    {
        static OrganizationServiceProxy service;
        const string CONTACT = "CONTACT";
        const string ACCOUNT = "ACCOUNT";
        const string CGI_HASEPISERVERACCOUNT = "cgi_hasepiserveraccount";
        const string CONTACTVALUE = "0";

        // TODO CHANGE SKIP LINE VALUE IF NEEDED
        // TODO CHANGE contactToUpdate[CGI_HASEPISERVERACCOUNT] if needed
        const string CSVPATH = @"C:\Users\AJIPER\Documents\Contacts.csv"; //TODO CHANGE AT PROD TIME
        const string SERVERURL  ="http://v-dkcrm-utv/Skanetrafiken"; //TODO CHANGE AT PROD TIME
        

        static void Main(string[] args)
        {
            xRMConnection concrm;
            List<Tuple<Guid, string, bool>> contactsFromCSV = new List<Tuple<Guid, string, bool>>();

            if (args==null || args.Count() < 2)
            {
                Console.Write("Please supply username as argument one and password as argument two, ie executable username password");
                return;
            }

            try
            {
                concrm = CreateConnection(args);
                service = concrm.GetService();

                ParseCSV(contactsFromCSV);

                UpdateRecords(contactsFromCSV);

                // Also update those recors that where not present in data file
                UpdateRecords_SetNotUpdatedContactsOrAccountsFieldHasEpiserverAccountToFalse(CONTACT);
                UpdateRecords_SetNotUpdatedContactsOrAccountsFieldHasEpiserverAccountToFalse(ACCOUNT);

                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.Write("There was an error: " + ex.ToString());
                Console.ReadKey();
            }
        }

        private static xRMConnection CreateConnection(string[] args)
        {
            xRMConnection concrm;
            concrm = new xRMConnection(SERVERURL);
            concrm.Authenticationtype = AuthenticationType.User; // NOTE: this musst be user otherwise below username/pwd/domain is not used....
            concrm.Crmtype = CRMType.OnPremis;
            concrm.Username = args[0];
            concrm.Password = args[1];
            concrm.Domain = "D1";
            return concrm;
        }

        private static void ParseCSV(List<Tuple<Guid, string, bool>> contactsFromCSV)
        {
            using (TextFieldParser csvParser = new TextFieldParser(CSVPATH))
            {
                csvParser.CommentTokens = new string[] { "#" };
                csvParser.SetDelimiters(new string[] { ";" });
                csvParser.HasFieldsEnclosedInQuotes = false;
                // Skip the row with the column names
                csvParser.ReadLine();

                while (!csvParser.EndOfData)
                {
                    // Read current line fields, pointer moves to the next line.
                    string[] fields = csvParser.ReadFields();
                    Guid id = Guid.Parse(fields[0]);
                    string type = fields[1];
                    bool hasEpiServerAccount = true;//bool.Parse(fields[2]);
                    contactsFromCSV.Add(new Tuple<Guid, string, bool>(id, type, hasEpiServerAccount));
                }
            }
        }


        private static void UpdateRecords(List<Tuple<Guid, string, bool>> entityDataList)
        {
           int recordIndexCount = 0;
           int entityDataListCount = entityDataList.Count;
            do
            {
                List<Tuple<Guid, string, bool>> batchRecords = new List<Tuple<Guid, string, bool>>();

                int currentMax = recordIndexCount != 0 ? recordIndexCount + 999 : 999; // max batchsize is 1000 if you do not change it in CRM. ( think of max message size if you do )

                Console.WriteLine("Processing {0} to {1}", recordIndexCount, currentMax);

                while (recordIndexCount <= currentMax && recordIndexCount < entityDataListCount) // NOTE: entityDataListCount is not 0 index based
                {
                    batchRecords.Add(entityDataList[recordIndexCount]);
                    recordIndexCount++;
                }

                UpdateRecordsByBatchMode(batchRecords);

            } while (recordIndexCount < entityDataListCount);
            Console.WriteLine("All {0} from datafile processed. Processed in loop was {1}. Date and time is {2}.", recordIndexCount, entityDataListCount, DateTime.Now);
        }

        private static void UpdateRecords_SetNotUpdatedContactsOrAccountsFieldHasEpiserverAccountToFalse(string entityName)
        {
            entityName = entityName.ToLower();
            Console.WriteLine("Updating remaining {0} that was not in data file", entityName);
            // Query for active accounts
            QueryByAttribute querybyexpression = new QueryByAttribute(entityName);
            querybyexpression.ColumnSet = new ColumnSet("statecode");
            querybyexpression.Attributes.Add("statecode");
            querybyexpression.Values.Add(0);
            querybyexpression.Attributes.Add("cgi_hasepiserveraccount");
            querybyexpression.Values.Add(null);

            //Query passed to the service proxy    
            EntityCollection records = service.RetrieveMultiple(querybyexpression);
            
            int retrievedContactsCount = records.Entities.Count;
            Console.WriteLine("Retrieved {0} {1} that has hasepiserveraccount null ", retrievedContactsCount, entityName);

            List<Tuple<Guid, string, bool>> contactsToUpdate = new List<Tuple<Guid,string,bool>>();
            for (int i = 0; i < records.Entities.Count; i++)
            {
                Entity contactToUpdate = records.Entities[i];

                contactsToUpdate.Add(new Tuple<Guid, string, bool>(contactToUpdate.Id, entityName, false));
            }

            UpdateRecords(contactsToUpdate);

            Console.WriteLine("Done updating those {0}s", entityName);
        }

        private static void UpdateRecordsByBatchMode(List<Tuple<Guid,string,bool>> records)
        {
            Console.WriteLine("Processing {0} at {1}", records.Count, DateTime.Now);

            Microsoft.Xrm.Sdk.Messages.ExecuteMultipleRequest requestWithResults = new Microsoft.Xrm.Sdk.Messages.ExecuteMultipleRequest()
            {
                // Assign settings that define execution behavior: continue on error, return responses. 
                Settings = new ExecuteMultipleSettings()
                {
                    ContinueOnError = true,
                    ReturnResponses = true
                },
                // Create an empty organization request collection.
                Requests = new OrganizationRequestCollection()
            };

            // TODO  do not have watch insisde method?
            // Create new stopwatch.
            Stopwatch stopwatch = new Stopwatch();
            Console.WriteLine("Starting watch");
            // Begin timing.
            stopwatch.Start();
            for (int i = 0; i < records.Count; i++)
            {
                Entity contactToUpdate;
                if (records[i].Item2.ToUpper() == CONTACTVALUE)
                {
                    contactToUpdate = new Entity(CONTACT.ToLower());
                }
                else
                {
                    contactToUpdate = new Entity(ACCOUNT.ToLower());
                }
                contactToUpdate.Id = records[i].Item1;
                contactToUpdate[CGI_HASEPISERVERACCOUNT] = records[i].Item3;
                Microsoft.Xrm.Sdk.Messages.UpdateRequest updateRequest = new Microsoft.Xrm.Sdk.Messages.UpdateRequest { Target = contactToUpdate };
                requestWithResults.Requests.Add(updateRequest);
            }

            // Using this multiple update for speed, if in the future, we have cases that are in many queues at once.
            Microsoft.Xrm.Sdk.Messages.ExecuteMultipleResponse responseWithResults = (Microsoft.Xrm.Sdk.Messages.ExecuteMultipleResponse)service.Execute(requestWithResults);

            stopwatch.Stop();
            Console.WriteLine("Stopping watch");
            Console.WriteLine("Time elapsed {0}", stopwatch.Elapsed);
        }

    }
}
