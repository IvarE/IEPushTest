using System;

namespace CGICRMPortalService.Models
{
    public class Case 
    {
        Guid _CaseId;
      
        public Guid CaseId
        {
            get { return _CaseId; }
            set { _CaseId = value; }
        }
        string _CaseNumber;        
        public string CaseNumber
        {
            get { return _CaseNumber; }
            set { _CaseNumber = value; }
        }
        string _Title;
        public string Title
        {
            get { return _Title; }
            set { _Title = value; }
        }

        int _CaseType;
        public int CaseType
        {
            get { return _CaseType; }
            set { _CaseType = value; }
        }

        int _CaseCatergory;
        public int CaseCatergory
        {
            get { return _CaseCatergory; }
            set { _CaseCatergory = value; }
        }
        string _AccountNumber;        
        public string AccountNumber
        {
            get { return _AccountNumber; }
            set { _AccountNumber = value; }
        }
        
        Guid _CustomerId;
        public Guid CustomerId
        {
            get { return _CustomerId; }
            set { _CustomerId = value; }
        }
        
        string _Email;
        public string Email
        {
            get { return _Email; }
            set { _Email = value; }
        }

        string _TelephoneNumber;
        public string TelephoneNumber
        {
            get { return _TelephoneNumber; }
            set { _TelephoneNumber = value; }
        }
                
        string _TravelCardNumber;        
        public string TravelCardNumber
        {
            get { return _TravelCardNumber; }
            set { _TravelCardNumber = value; }
        }
        string _Description;
        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }
        string _CaseStatus;
        public string CaseStatus
        {
            get { return _CaseStatus; }
            set { _CaseStatus = value; }
        }

        
    }

}