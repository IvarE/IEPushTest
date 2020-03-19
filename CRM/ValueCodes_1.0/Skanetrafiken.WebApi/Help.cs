using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Skanetrafiken.Crm
{
    public class Help
    {
        public enum errorCodes  { ContactNotFound = 1, OpportunityModifiedOnIsEmpty=2, StatusIsEmpty =3,  OpportunityClosed =4, ServerHasNewerOpportunity=5, ServerHasNewerQuoteDetail=6 ,
                                        DetailModifiedOnIsEmpty =7, UpdateOpportunityFailed =8, UpdateQuoteDetailFailed = 9, OpportunityIdIsEmpty = 10,
            DeleteQuoteFailed = 11
        }

        public static Dictionary<int, string> longErrorMessage = new Dictionary<int, string>() 
        {
           {1, "The contact with the id {0} could not be found"},

           {2, "The ModifiedOn date of the opportunity cannot be empty"}, 

           {3, "The status of the opportunity cannot be empty"}, 

           {4, "The Opportunity has been already closed"}, 

           {5, "The server has a newer version of the opportunity"}, 

           {6, "The server has a newer version of the quote detail {0}"}, 

           {7, "The ModifiedOn of the quote detail {0} cannot be empty"}, 

           {9, "The update of the quote detail {0} failed"} , 

           {10,"The opportunity id cannot be empty"} ,

            {11,"The delete request for an Opportunity Detail resulted in an error:\n{0}" }

        };


    }
}