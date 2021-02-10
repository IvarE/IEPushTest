using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using CGICRMPortalService.Models;

namespace CGICRMPortalService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    public class PortalService : IPortalService
    {   
        XrmHelper xrmHelper = new XrmHelper();


        #region Customer       
        public CheckCustomerExistResponse CheckCustomerExist(string customerEmail, Guid callerId)
        {
            CustomerManager customerManager = new CustomerManager(callerId);
            return customerManager.CheckCustomerExist(customerEmail);

        }
        public CreateCustomerResponse CreateCustomer(Customer customer,Guid callerId)
        {
            CustomerManager customerManager = new CustomerManager(callerId);
            return customerManager.CreateCustomer(customer);
            
        }
        public UpdateCustomerResponse UpdateCustomer(Guid customerId, Customer customer, Guid callerId)
        {
            CustomerManager customerManager = new CustomerManager(callerId);
            return customerManager.UpdateCustomer(customerId, customer);            
        }
        public GetCustomerResponse GetCustomer(Guid customerId,AccountCategoryCode accountType, Guid callerId)
        {
            CustomerManager customerManager = new CustomerManager(callerId);
            return customerManager.GetCustomer(customerId, accountType);
        }
        //public bool DeactivateCustomer(Guid customerId, Guid callerId)
        //{
        //    CustomerManager customerManager = new CustomerManager();
        //    return customerManager.DeactivateCustomer(customerId);
        //}
        #endregion

       

        #region [Travel Card]
        public RegisterTravelCardResponse RegisterTravelCard(TravelCard travelCard, Guid callerId)
        {
            TravelCardManager travelCardManager = new TravelCardManager(callerId);
            return travelCardManager.RegisterTravelCard(travelCard);
            
        }

        public UpdateTravelCardResponse UpdateTravelCard(TravelCard travelCard, Guid callerId)
        {
            TravelCardManager travelCardManager = new TravelCardManager(callerId);
            return travelCardManager.UpdateTravelCard(travelCard);

        }

        //public TravelCard GetTravelCardDetails(Guid customerId, string cardNumber, Guid callerId)
        //{
        //    TravelCardManager travelCardManager = new TravelCardManager();
        //    return new TravelCard(); 
        //}
        //public List<TravelCard> GetCardsForCustomer(Guid customerId, Guid callerId)
        //{
        //    TravelCardManager travelCardManager = new TravelCardManager(callerId);
        //    return travelCardManager.GetCardsForCustomer(customerId).TravelCards;
        //}

        public GetCardsForCustomerResponse GetCardsForCustomer(Guid customerId,AccountCategoryCode customerType, Guid callerId)
        {
            TravelCardManager travelCardManager = new TravelCardManager(callerId);
            return travelCardManager.GetCardsForCustomer(customerId, customerType);
        }
        //public bool BlockTravelCard(Guid customerId, string cardNumber, Guid callerId)
        //{
        //    TravelCardManager travelCardManager = new TravelCardManager();

        //    return travelCardManager;
        //}
        //public bool RemoveTravelCard(Guid customerId, string cardNumber, Guid callerId)
        //{
        //    TravelCardManager travelCardManager = new TravelCardManager();
        //    return true;
        //}
        public UnRegisterTravelCardResponse UnregisterTravelCard(Guid customerId,AccountCategoryCode customerType, string travelCardNumber,Guid callerId)
        {
            TravelCardManager travelCardManager = new TravelCardManager(callerId);
            return travelCardManager.UnregisterTravelCard(customerId, customerType,travelCardNumber);
        }
        #endregion


        //#region[Case]
        //public CreateCaseResponse CreateCase(Case customerCase,Guid callerId,Note Notes=null)
        //{
        //    IncidentManager incidentMgr = new IncidentManager(callerId);
        //    CreateCaseResponse createCaseResponse= incidentMgr.CreateCase(customerCase);
        //    if (Notes != null && createCaseResponse!=null)
        //    {
        //        NotesManager notesMgr = new NotesManager(callerId);
        //        notesMgr.CreateNotes("incident", createCaseResponse.CaseId, Notes);
        //    }
        //    return createCaseResponse;
        //}
        //#endregion
    }
}
 