using System.ServiceModel;

namespace ImportContactsService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService1
    {
        #region Public Methods
        [OperationContract]
        bool CreateContacts();

        [OperationContract]
        bool CreateTravelCards();
        #endregion
    }
}
