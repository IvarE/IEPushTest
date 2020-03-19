using System;
using CGICRMPortalService.Shared.NotesAttachment.Models;
using CGIXrmWin;
using Microsoft.Xrm.Sdk;

namespace CGICRMPortalService.Shared.NotesAttachment
{
    public class NotesManager
    {
        #region Global Variables
        private readonly XrmManager _xrmMgr;
        private readonly XrmHelper _xrmHelper = new XrmHelper();
        #endregion

        #region Constructors
        public NotesManager()
        {
            _xrmMgr = _xrmHelper.GetXrmManagerFromAppSettings(Guid.Empty);
        }

        public NotesManager(Guid callerId)
        {

            _xrmMgr = _xrmHelper.GetXrmManagerFromAppSettings(callerId);
            
        }
        #endregion

        #region Internal Methods
        internal Guid CreateNotes(string entityName,Guid associatedRecordId,Note note)
        {

            Entity noteEntity = new Entity("annotation")
            {
                Attributes = new AttributeCollection
                {
                    {"notetext", note.NoteDescription},
                    {"subject", note.Title},
                    {"objectid", new EntityReference(entityName, associatedRecordId)},
                    {"objecttypecode", entityName}
                }
            };

            if(note.HasAttachements)
            {
                string encodedData = Convert.ToBase64String(note.AttachmentBody);
                noteEntity.Attributes.Add("documentbody", encodedData);
                noteEntity.Attributes.Add("mimetype", note.FileType);
                noteEntity.Attributes.Add("filename",note.FileName);
            }

            return _xrmMgr.Service.Create(noteEntity);
        }

        internal void UpdatNote(Guid noteId, Note note)
        {
            Entity noteEntity = new Entity("annotation")
            {
                Attributes = new AttributeCollection(),
                Id = noteId
            };
            noteEntity.Attributes.Add("notetext", note.NoteDescription);
            if (note.HasAttachements)
            {
                string encodedData = Convert.ToBase64String(note.AttachmentBody);
                noteEntity.Attributes.Add("documentbody", encodedData);
                noteEntity.Attributes.Add("mimetype", note.FileType);
                noteEntity.Attributes.Add("filename", note.FileName);
            }
            _xrmMgr.Service.Update(noteEntity);
        }
        #endregion
    }
}