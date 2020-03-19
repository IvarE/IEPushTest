using System;
using CGICRMPortalService.Customer;
using CGICRMPortalService.Customer.Models;
using CGICRMPortalService.Shared;
using CGICRMPortalService.Shared.Models;
using CGICRMPortalService.TravelCard;
using CGICRMPortalService.TravelCard.Models;

namespace CGICRMPortalService
{
    public class PortalService : IPortalService
    {
        #region Global Variables
        XrmHelper _xrmHelper;
        #endregion

        #region Constructors
        public PortalService()
        {
            _xrmHelper = new XrmHelper();
        }
        #endregion

        #region Public Methods
        #region Customer

        /// <summary>
        /// Method returning if either a customer exists in CRM database or not, based
        /// on given e-mail address.
        /// </summary>
        /// <param name="customerEmail">E-mail address</param>
        /// <param name="callerId">Unique Identifier of CRM user</param>
        /// <returns></returns>
        public CheckCustomerExistResponse CheckCustomerExist(string customerEmail, Guid callerId)
        {
            CustomerManager customerManager = new CustomerManager(callerId);
            return customerManager.CheckCustomerExist(customerEmail);

        }


        /// <summary>
        /// Method creating a customer based on given parameters.
        /// </summary>
        /// <param name="customer">Customer Entity</param>
        /// <param name="callerId">Unique Identifier of CRM user</param>
        /// <returns></returns>
        public CreateCustomerResponse CreateCustomer(Customer.Models.Customer customer,Guid callerId)
        {
            CustomerManager customerManager = new CustomerManager(callerId);
            return customerManager.CreateCustomer(customer);
            
        }


        /// <summary>
        /// Method updating an existing customer based on given parameters.
        /// </summary>
        /// <param name="customerId">Unique Identifier of Customer Entity</param>
        /// <param name="customer">Customer Entity</param>
        /// <param name="callerId">Unique Identifier of CRM user</param>
        /// <returns></returns>
        public UpdateCustomerResponse UpdateCustomer(Guid customerId, Customer.Models.Customer customer, Guid callerId)
        {
            CustomerManager customerManager = new CustomerManager(callerId);
            return customerManager.UpdateCustomer(customerId, customer);            
        }


        /// <summary>
        /// Method retrieving a customer based on given parameters.
        /// </summary>
        /// <param name="customerId">Unique Identifier of Customer Entity</param>
        /// <param name="accountType">Customer Account Type: Organization or Private</param>
        /// <param name="callerId">Unique Identifier of CRM user</param>
        /// <returns></returns>
        public GetCustomerResponse GetCustomer(Guid customerId,AccountCategoryCode accountType, Guid callerId)
        {
            CustomerManager customerManager = new CustomerManager(callerId);
            return customerManager.GetCustomer(customerId, accountType);
        }

        public UpdateCustomerResponse UpdateMyAccountLastLogin(Guid customerId, bool isCompany, DateTime myAccountLastLogin, Guid callerId)
        {
            CustomerManager customerManager = new CustomerManager(callerId);
            return customerManager.UpdateMyAccountLastLogin(customerId, isCompany, myAccountLastLogin);
        }

        #endregion   

        #region Travel Card

        /// <summary>
        /// Method registering a travel card based on given parameters.
        /// </summary>
        /// <param name="travelCard">Travel Card Entity</param>
        /// <param name="callerId">Unique Identifier of CRM User</param>
        /// <returns></returns>
        public RegisterTravelCardResponse RegisterTravelCard(TravelCard.Models.TravelCard travelCard, Guid callerId)
        {
            TravelCardManager travelCardManager = new TravelCardManager(callerId);
            return travelCardManager.RegisterTravelCard_CreateOrUpdate(travelCard);
            
        }


        /// <summary>
        /// Method updating an existing Travel Card Entity based on given parameters.
        /// </summary>
        /// <param name="travelCard">Travel Card Entity</param>
        /// <param name="callerId">Unique Identifier of CRM User</param>
        /// <returns></returns>
        public UpdateTravelCardResponse UpdateTravelCard(TravelCard.Models.TravelCard travelCard, Guid callerId)
        {
            TravelCardManager travelCardManager = new TravelCardManager(callerId);
            return travelCardManager.UpdateTravelCard(travelCard);

        }


        /// <summary>
        /// Method retrieving multiple Travel Cards bound to a specific customer, 
        /// based on given parameters.
        /// </summary>
        /// <param name="customerId">Unique Identifier of Customer Entity in CRM</param>
        /// <param name="customerType">Customer Type: Organization of Private</param>
        /// <param name="callerId">Unique Identifier of CRM User</param>
        /// <returns></returns>
        public GetCardsForCustomerResponse GetCardsForCustomer(Guid customerId,AccountCategoryCode customerType, Guid callerId)
        {
            TravelCardManager travelCardManager = new TravelCardManager(callerId);
            return travelCardManager.GetCardsForCustomer(customerId, customerType);
        }


        /// <summary>
        /// Method unregistering an existing Travel Card entity bound to a Customer Entity in CRM.
        /// </summary>
        /// <param name="customerId">Unique Identifier of Customer Entity in CRM</param>
        /// <param name="customerType">Customer Type: Organization or Private</param>
        /// <param name="travelCardNumber">Unique Travel Card Number ID</param>
        /// <param name="callerId">Unique Identifier of CRM User</param>
        /// <returns></returns>
        public UnRegisterTravelCardResponse UnregisterTravelCard(Guid customerId,AccountCategoryCode customerType, string travelCardNumber,Guid callerId)
        {
            TravelCardManager travelCardManager = new TravelCardManager(callerId);
            return travelCardManager.UnregisterTravelCard_RemoveRelationships(customerId, customerType,travelCardNumber);
        }
        #endregion
        #endregion

    }
}
 