using System;
using System.IO;
using System.Collections;
using Microsoft.BizTalk.Message.Interop;
using Microsoft.BizTalk.Component.Interop;
using System.ComponentModel;
using IDC.Shared.PipelineComponentsHelpers;

namespace INTSTDK009.Shared.PipelineComponents.Archiver
{

    /// <summary>
    /// Pipeline-component that saves a message to a selected archive folder.
    /// </summary>
    [ComponentCategory(CategoryTypes.CATID_PipelineComponent)]
    [ComponentCategory(CategoryTypes.CATID_Parser)]
    [System.Runtime.InteropServices.Guid("DDAF6234-35F2-4AD2-BB2F-11AEA776ABEE")]
    public class ArchiverComponent : IBaseComponent, Microsoft.BizTalk.Component.Interop.IComponent,
        IComponentUI, IPersistPropertyBag
    {
        #region Public properties

        private string archivePath;

        /// <summary>
        /// Where to archive files, full path to zip archive. Supports macros %date%, %interface%, %bu%, %messageName%.
        /// </summary>
        [DescriptionAttribute("The path to where the files are to be archived should be declared here.")]
        public string ArchivePath
        {
            get { System.Diagnostics.Trace.WriteLine("INTSTDK009.Shared.PipelineComponents.Archiver:ArchivePath"); return archivePath; }
            set { archivePath = value; }
        }

        private bool enabled = true;

        /// <summary>
        /// This attribute controls if the archive functionality is enabled or not.
        /// </summary>
        [DescriptionAttribute("If archive functionality is enabled or not.")]
        public bool Enabled
        {
            get { System.Diagnostics.Trace.WriteLine("INTSTDK009.Shared.PipelineComponents.Archiver:Enabled"); return enabled; }
            set { enabled = value; }
        }

        private bool promote_ARCHIVED_AS_Prop = true; 
        
        /// <summary>
        /// This attribute controls if the ARCHIVED_AS property is to be promoted or not.
        /// </summary>
        [DescriptionAttribute("If the ARCHIVED_AS property is to be promoted or not.")]
        public bool Promote_ARCHIVED_AS_Prop
        {
            get { System.Diagnostics.Trace.WriteLine("INTSTDK009.Shared.PipelineComponents.Archiver:Promote_ARCHIVED_AS"); return promote_ARCHIVED_AS_Prop; }
            set { promote_ARCHIVED_AS_Prop = value; }
        }

        #endregion

        #region IBaseComponent Members

        /// <summary>
        /// Description of the component.
        /// </summary>
        [Browsable(false)]
        public string Description
        {
            get { System.Diagnostics.Trace.WriteLine("INTSTDK009.Shared.PipelineComponents.Archiver:Description"); return "This component saves the file in an archive."; }
        }

        /// <summary>
        /// Name of the component.
        /// </summary>
        [Browsable(false)]
        public string Name
        {
            get { System.Diagnostics.Trace.WriteLine("INTSTDK009.Shared.PipelineComponents.Archiver:Name"); return "Archiver"; }
        }

        /// <summary>
        /// Version of the component.
        /// </summary>
        [Browsable(false)]
        public string Version
        {
            get { System.Diagnostics.Trace.WriteLine("INTSTDK009.Shared.PipelineComponents.Archiver:Version"); return "1.1"; }
        }

        #endregion

        #region IComponent Members

        /// <summary>
        /// Executed when pipeline-component is triggered.
        /// This method implements the functionality of the component.
        /// Saves a message to the archive.
        /// </summary>
        /// <param name="pipelineContext">Message context.</param>
        /// <param name="inMessage">Message received.</param>
        /// <returns>The actual, modified message.</returns>
        public IBaseMessage Execute(IPipelineContext pipelineContext, IBaseMessage inMessage)
        {
            System.Diagnostics.Trace.WriteLine("INTSTDK009.Shared.PipelineComponents.Archiver:Execute");
            string fullArchivePath = null;
            MessageContextWrapper messageContextWrapper = new MessageContextWrapper(inMessage.Context);

            if (Helpers.IsSendPipeline(pipelineContext))
            {
                // Set context property to null just to make sure that we don't get the receive side ArchivedAs.
                messageContextWrapper.ArchivedAs = null;
            }

            // If this component isn't enabled, just pass on the message to the next component
            if (!this.Enabled)
            {
                return inMessage;
            }

            try
            {
                IBaseMessagePart bodyPart = inMessage.BodyPart;

                if (bodyPart != null)
                {
                    fullArchivePath = GetArchivePathAndFileName(pipelineContext, inMessage, messageContextWrapper);

                    // Make sure that the folder specified exists, and create it if it doesn't.
                    string archiveFolder = Path.GetDirectoryName(fullArchivePath);
                    if (!Directory.Exists(archiveFolder))
                    {
                        Directory.CreateDirectory(archiveFolder);
                    }

                    // Create a file, or overwrite it if it exists previously
                    FileStream fileStream = File.Open(fullArchivePath, FileMode.Create);

                    // Assign the datastream to a SeekableStream that overflows to disk (VirtualStream)
                    Stream data = Helpers.EnsureSeekableStream(bodyPart.Data);

                    // Write to the file
                    byte[] buffer = new byte[1024]; // used to bit by bit read and write through the streams
                    // minimizing the risk to have a large amount of data in the memory

                    int bytesRead = 0;

                    while ((bytesRead = data.Read(buffer, 0, buffer.Length)) != 0)
                    {
                        fileStream.Write(buffer, 0, bytesRead);
                    }

                    fileStream.Flush();
                    fileStream.Close();

                    bodyPart.Data = data;
                    data.Seek(0, SeekOrigin.Begin);

                    if (Promote_ARCHIVED_AS_Prop) // Write?
                    {
                        messageContextWrapper.ArchivedAs = fullArchivePath;
                    }
                }
            }
            catch
            {
                throw;

            }
            

            return inMessage;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// This method looks at the message context to determine if this is a send or receive
        /// and if it's a retry (or resume). Depending on the answers to those questions the
        /// filename is set in different ways.
        /// </summary>
        /// <param name="pContext"></param>
        /// <param name="pInMsg"></param>
        /// <param name="mcw"></param>
        /// <returns></returns>
        private string GetArchivePathAndFileName(IPipelineContext pipelineContext, IBaseMessage inMessage, MessageContextWrapper messageContextWrapper)
        {
            System.Diagnostics.Trace.WriteLine("INTSTDK009.Shared.PipelineComponents.Archiver:GetArchivePathAndFileName");
            string fileName = string.Empty;

            if (Helpers.IsSendPipeline(pipelineContext)) // on a send pipeline
            {
                // Try to get info about the file.
                fileName = messageContextWrapper.ArchivedAs;
            }

            if (string.IsNullOrEmpty(fileName))
            {
                if (Helpers.IsSendPipeline(pipelineContext))   // it's a send pipeline
                {
                    if (new SystemMessageContext(inMessage.Context).OutboundTransportType.ToUpper().Equals("NULL"))
                    {
                        fileName = Path.GetFileName(Helpers.GetReceivedFileName(inMessage));
                    }
                    else
                    {
                        fileName = Helpers.GetSentFileName(inMessage);
                    }

                    if (Path.GetExtension(fileName).ToLower().Equals(".zip"))
                    {
                        fileName = Path.GetFileNameWithoutExtension(fileName);
                    }
                }
                else // or it's a receive pipeline
                {
                    fileName = Path.GetFileName(Helpers.GetReceivedFileName(inMessage));
                }

                // Check if file with same name already exists and if it does get a new filename
                fileName = HandleDuplicatedNames(fileName);
            }

            // Fix the archive name
            return Path.Combine(ArchivePath, fileName);
        }

        /// <summary>
        /// This method looks in the archive to make sure that a file with this name does not already exist.
        /// If it does it renames the new entry so that it wont clash with the existing one.
        /// </summary>
        /// <returns>New name of file.</returns>
        private string HandleDuplicatedNames(string fileName) //this method could be made void, don't know which is best.
        {
            System.Diagnostics.Trace.WriteLine("INTSTDK009.Shared.PipelineComponents.Archiver:HandleDuplicateNames");
            string newFileName = fileName;
            //int entrySeed = 0;
            //while (File.Exists(Path.Combine(ArchivePath, newFileName)))
            //{
            //    // If we have this name (ie "xyz.txt") in the archive already, 
            //    // add a counter to the end of it (ie "xyz (1).txt", "xyz (2).txt"). 
            //    newFileName = String.Format("{0} ({1}){2}",
            //    Path.GetFileNameWithoutExtension(fileName),
            //    ++entrySeed,
            //    Path.GetExtension(fileName));
            //}
            return newFileName;
        }

        #endregion

        #region IComponentUI Members

        /// <summary>
        /// Component icon to use in BizTalk Editor.
        /// </summary>
        [Browsable(false)]
        public IntPtr Icon
        {
            get { System.Diagnostics.Trace.WriteLine("INTSTDK009.Shared.PipelineComponents.Archiver:Icon "); return IntPtr.Zero; }
        }

        /// <summary>
        /// Validates property settings when building projects 
        /// that uses this component or saving settings in BizTalk.
        /// </summary>
        /// <param name="projectSystem"></param>
        /// <returns>Enomerator with errors.</returns>
        public IEnumerator Validate(object projectSystem)
        {
            System.Diagnostics.Trace.WriteLine("INTSTDK009.Shared.PipelineComponents.Archiver:Validate");
            ArrayList errors = new ArrayList();
            if (this.Enabled && String.IsNullOrEmpty(this.ArchivePath))
            {
                errors.Add("Property ArchivePath cannot be empty when Enabled is set to True.");
            }
            return errors.GetEnumerator();
        }

        #endregion

        #region IPersistPropertyBag Members

        /// <summary>
        /// Gets class ID of component for usage from unmanaged code.
        /// </summary>
        /// <param name="classID">Class ID of the component.</param>
        public void GetClassID(out Guid classID)
        {
            classID = new Guid("DDAF6234-35F2-4AD2-BB2F-11AEA776ABEE");
            System.Diagnostics.Trace.WriteLine("INTSTDK009.Shared.PipelineComponents.Archiver:ClassID: " + classID);
        }

        /// <summary>
        /// Not implemented.
        /// </summary>
        public void InitNew()
        {
            System.Diagnostics.Trace.WriteLine("INTSTDK009.Shared.PipelineComponents.Archiver:InitNew");
        }

        /// <summary>
        /// Loads data from property bag and sets property values.
        /// </summary>
        /// <param name="propertyBag">Property bag used.</param>
        /// <param name="errorLog"></param>
        public void Load(IPropertyBag propertyBag, int errorLog)
        {
            System.Diagnostics.Trace.WriteLine("INTSTDK009.Shared.PipelineComponents.Archiver:Load");
            object val = null;

            val = Helpers.ReadPropertyBag(propertyBag, "ArchivePath");
            if (val != null)
            {
                archivePath = (string)val;
            }

            val = Helpers.ReadPropertyBag(propertyBag, "Enabled");
            if (val != null)
            {
                enabled = (bool)val;
            }

            val = Helpers.ReadPropertyBag(propertyBag, "Promote_ARCHIVED_AS_Prop");
            if (val != null)
            {
                promote_ARCHIVED_AS_Prop = (bool)val;
            }
        }

        /// <summary>
        /// Saves property values to property bag.
        /// </summary>
        /// <param name="propertyBag">Property bag used.</param>
        /// <param name="clearDirty"></param>
        /// <param name="saveAllProperties"></param>
        public void Save(IPropertyBag propertyBag, bool clearDirty, bool saveAllProperties)
        {
            System.Diagnostics.Trace.WriteLine("INTSTDK009.Shared.PipelineComponents.Archiver:Save");
            Helpers.WritePropertyBag(propertyBag, "ArchivePath", archivePath);
            Helpers.WritePropertyBag(propertyBag, "Enabled", enabled);
            Helpers.WritePropertyBag(propertyBag, "Promote_ARCHIVED_AS_Prop", promote_ARCHIVED_AS_Prop);
        }

        #endregion
    }
}
