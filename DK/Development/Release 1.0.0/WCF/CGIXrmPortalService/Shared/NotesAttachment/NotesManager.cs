using System;
using Generic=System.Collections.Generic;
using System.Linq;
using System.Web;
using CGIXrmWin;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System.Collections.ObjectModel;
using System.Threading;
using System.Configuration;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Messages;
using System.Data.SqlClient;
using System.Xml;
using System.Xml.Serialization;
using System.Threading.Tasks;
using System.IO;

namespace CGICRMPortalService
{
    public class NotesManager
    {
        private XrmManager xrmMgr;
        private XrmHelper xrmHelper;
        public NotesManager()
        {
            xrmMgr = xrmHelper.GetXrmManagerFromAppSettings(Guid.Empty);
        }

        public NotesManager(Guid callerId)
        {

            xrmMgr = xrmHelper.GetXrmManagerFromAppSettings(callerId);
            
        }

        #region Notes
        internal Guid CreateNotes(string entityName,Guid associatedRecordId,Note note)
        {

            Entity noteEntity = new Entity("annotation");
            noteEntity.Attributes = new AttributeCollection();
            noteEntity.Attributes.Add("notetext", note.NoteDescription);
            noteEntity.Attributes.Add("subject", note.Title);
            noteEntity.Attributes.Add("objectid", new EntityReference(entityName, associatedRecordId));
            noteEntity.Attributes.Add("objecttypecode", entityName);
 
            if(note.HasAttachements)
            {
                string encodedData = System.Convert.ToBase64String(note.AttachmentBody);
                noteEntity.Attributes.Add("documentbody", encodedData);
                noteEntity.Attributes.Add("mimetype", note.FileType);
                noteEntity.Attributes.Add("filename",note.FileName);
            }

            return xrmMgr.Service.Create(noteEntity);
        }

        internal void UpdatNote(Guid noteId, Note note)
        {
            Entity noteEntity = new Entity("annotation");
            noteEntity.Attributes = new AttributeCollection();
            noteEntity.Id = noteId;
            noteEntity.Attributes.Add("notetext", note.NoteDescription);
            if (note.HasAttachements)
            {
                string encodedData = System.Convert.ToBase64String(note.AttachmentBody);
                noteEntity.Attributes.Add("documentbody", encodedData);
                noteEntity.Attributes.Add("mimetype", note.FileType);
                noteEntity.Attributes.Add("filename", note.FileName);
            }
            xrmMgr.Service.Update(noteEntity);
        }

        //internal EntityCollection GetNotes(string attributeName, object attributeValue)
        //{
        //    QueryByAttribute query = new QueryByAttribute
        //    {
        //        ColumnSet = new ColumnSet(true),
        //        EntityName = "annotation"
        //    };
        //    query.Attributes.Add(attributeName);
        //    query.Values.Add(attributeValue);
        //    EntityCollection notes = xrmMgr.Service.RetrieveMultiple(query);
        //    return notes;
        //}
        #endregion
        
        
    }
}