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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]    
    public interface IPortalService
    {

        // TODO: Add your service operations here

        #region Customer Methods

        [OperationContract]
        CheckCustomerExistResponse CheckCustomerExist(string customerEmail,Guid callerId);

        [OperationContract]        
        CreateCustomerResponse CreateCustomer(Customer customer,Guid callerId);

        [OperationContract]
        GetCustomerResponse GetCustomer(Guid customerId,AccountCategoryCode accountType, Guid callerId);

        
        /*For this method the client application has to send all the information back inorder to avoid null being updated back*/
        [OperationContract]
        UpdateCustomerResponse UpdateCustomer(Guid customerId, Customer customer, Guid callerId);
        

        //[OperationContract]
        //bool DeactivateCustomer(Guid customerId, Guid callerId);

        #endregion
        
        #region [Travel Card]
        [OperationContract]
        RegisterTravelCardResponse RegisterTravelCard(TravelCard travelCard, Guid callerId);                
        
        //[OperationContract]
        //TravelCard GetTravelCardDetails(Guid customerId, string cardNumber, Guid callerId);

        [OperationContract]
        UpdateTravelCardResponse UpdateTravelCard(TravelCard travelCard, Guid callerId);  
        
        [OperationContract]
        GetCardsForCustomerResponse GetCardsForCustomer(Guid customerId,AccountCategoryCode customerType,Guid callerId);
        
        //[OperationContract]
        //bool BlockTravelCard(Guid customerId, string cardNumber, Guid callerId);
        
        //[OperationContract]
        //bool RemoveTravelCard(Guid customerId, string cardNumber, Guid callerId);

        [OperationContract]
        UnRegisterTravelCardResponse UnregisterTravelCard(Guid customerId, AccountCategoryCode customerType,string travelCardNumber, Guid callerId);
        #endregion

        #region [Case]
        //[OperationContract]
        //CreateCaseResponse CreateCase(Case customerCase, Guid callerId, Note Attachment = null);
        #endregion



    }
}
