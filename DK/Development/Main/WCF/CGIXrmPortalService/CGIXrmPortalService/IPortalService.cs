using System;
using System.ServiceModel;
using CGICRMPortalService.Customer.Models;
using CGICRMPortalService.Shared.Models;
using CGICRMPortalService.TravelCard.Models;

namespace CGICRMPortalService
{
    [ServiceContract]    
    public interface IPortalService
    {
        #region Customer Methods

        /// <summary>
        /// Method returning if either a customer exists in CRM database or not, based
        /// on given e-mail address.
        /// </summary>
        /// <param name="customerEmail">E-mail address</param>
        /// <param name="callerId">Unique Identifier of CRM user</param>
        /// <returns></returns>
        [OperationContract]
        CheckCustomerExistResponse CheckCustomerExist(string customerEmail,Guid callerId);


        /// <summary>
        /// Method creating a customer based on given parameters.
        /// </summary>
        /// <param name="customer">Customer Entity</param>
        /// <param name="callerId">Unique Identifier of CRM user</param>
        /// <returns></returns>
        [OperationContract]        
        CreateCustomerResponse CreateCustomer(Customer.Models.Customer customer,Guid callerId);


        /// <summary>
        /// Method retrieving a customer based on given parameters.
        /// </summary>
        /// <param name="customerId">Unique Identifier of Customer Entity</param>
        /// <param name="accountType">Customer Account Type: Organization or Private</param>
        /// <param name="callerId">Unique Identifier of CRM user</param>
        /// <returns></returns>
        [OperationContract]
        GetCustomerResponse GetCustomer(Guid customerId,AccountCategoryCode accountType, Guid callerId);


        /// <summary>
        /// Method updating an existing customer based on given parameters.
        /// </summary>
        /// <param name="customerId">Unique Identifier of Customer Entity</param>
        /// <param name="customer">Customer Entity</param>
        /// <param name="callerId">Unique Identifier of CRM user</param>
        /// <returns></returns>
        [OperationContract]
        UpdateCustomerResponse UpdateCustomer(Guid customerId, Customer.Models.Customer customer, Guid callerId);

        /// <summary>
        /// Method updating existing customer field cgi_myaccount_lastlogin.
        /// </summary>
        /// <param name="customerId">Unique Identifier of Customer Entity</param>
        /// <param name="isCompany">Identifies type of customer as account or contact</param>
        /// <param name="myAccountLastLogin">Date and time of last login in My Account web system</param>
        /// /// <param name="callerId">Unique Identifier of CRM user</param>
        /// <returns>UpdateCustomerResponse object with Succes and Message property</returns>
        [OperationContract]
        UpdateCustomerResponse UpdateMyAccountLastLogin(Guid customerId, bool isCompany, DateTime myAccountLastLogin, Guid callerId);
        
        #endregion

        #region Public Methods
        #region Travel Card

        /// <summary>
        /// Method registering a travel card based on given parameters.
        /// </summary>
        /// <param name="travelCard">Travel Card Entity</param>
        /// <param name="callerId">Unique Identifier of CRM User</param>
        /// <returns></returns>
        [OperationContract]
        RegisterTravelCardResponse RegisterTravelCard(TravelCard.Models.TravelCard travelCard, Guid callerId);


        /// <summary>
        /// Method updating an existing Travel Card Entity based on given parameters.
        /// </summary>
        /// <param name="travelCard">Travel Card Entity</param>
        /// <param name="callerId">Unique Identifier of CRM User</param>
        /// <returns></returns>
        [OperationContract]
        UpdateTravelCardResponse UpdateTravelCard(TravelCard.Models.TravelCard travelCard, Guid callerId);


        /// <summary>
        /// Method retrieving multiple Travel Cards bound to a specific customer, 
        /// based on given parameters.
        /// </summary>
        /// <param name="customerId">Unique Identifier of Customer Entity in CRM</param>
        /// <param name="customerType">Customer Type: Organization of Private</param>
        /// <param name="callerId">Unique Identifier of CRM User</param>
        /// <returns></returns>
        [OperationContract]
        GetCardsForCustomerResponse GetCardsForCustomer(Guid customerId,AccountCategoryCode customerType,Guid callerId);


        /// <summary>
        /// Method unregistering an existing Travel Card entity bound to a Customer Entity in CRM.
        /// </summary>
        /// <param name="customerId">Unique Identifier of Customer Entity in CRM</param>
        /// <param name="customerType">Customer Type: Organization or Private</param>
        /// <param name="travelCardNumber">Unique Travel Card Number ID</param>
        /// <param name="callerId">Unique Identifier of CRM User</param>
        /// <returns></returns>
        [OperationContract]
        UnRegisterTravelCardResponse UnregisterTravelCard(Guid customerId, AccountCategoryCode customerType,string travelCardNumber, Guid callerId);
        #endregion
        #endregion
    }
}
