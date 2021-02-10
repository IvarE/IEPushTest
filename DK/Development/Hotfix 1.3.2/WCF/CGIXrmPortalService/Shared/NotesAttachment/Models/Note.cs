using System.Runtime.Serialization;
using System;
using System.Xml.Serialization;

namespace CGICRMPortalService
{
    [DataContract] 
    public class Note
    {
        [DataMember]
        string _title = string.Empty;
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        [DataMember]
        string _noteDescription = string.Empty;
        public string NoteDescription
        {
            get { return _noteDescription; }
            set { _noteDescription = value; }
        }

        [DataMember]
        bool _hasAttachements = false;
        public bool HasAttachements
        {
            get { return _hasAttachements; }
            set { _hasAttachements = value; }
        }

        [DataMember]
        string _fileName = string.Empty;
        public string FileName
        {
            get { return _fileName; }
            set { _fileName = value; }
        }

        [DataMember]
        string _fileType = string.Empty;
        public string FileType
        {
            get { return _fileType; }
            set { _fileType = value; }
        }

        [DataMember]
        byte[] _attachmentBody;
        public byte[] AttachmentBody
        {
            get { return _attachmentBody; }
            set { _attachmentBody = value; }
        }
    }
}