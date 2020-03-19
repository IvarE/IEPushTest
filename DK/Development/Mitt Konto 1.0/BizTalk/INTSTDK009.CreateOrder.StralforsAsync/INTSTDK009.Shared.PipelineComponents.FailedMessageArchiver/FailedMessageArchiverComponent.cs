using System;
using System.Xml;
using System.Xml.XPath;
using System.IO;
using System.Collections;
using Microsoft.BizTalk.Message.Interop;
using Microsoft.BizTalk.Component.Interop;
using System.ComponentModel;
using IDC.Shared.PipelineComponentsHelpers;

namespace INTSTDK009.Shared.PipelineComponents.FailedMessageArchiver
{

    /// <summary>
    /// Pipeline component that saves a received message to a specified folder unless the message contains <StatusCode>200</StatusCode>, 
    /// which is the HTTP response status code for OK.
    /// </summary>
    [ComponentCategory(CategoryTypes.CATID_PipelineComponent)]
    [ComponentCategory(CategoryTypes.CATID_Any)]
    [System.Runtime.InteropServices.Guid("B88FF97A-1A9E-4301-B10B-3D5B3973D665")]
    public class FailedMessageArchiverComponent : IBaseComponent, Microsoft.BizTalk.Component.Interop.IComponent,
        IComponentUI, IPersistPropertyBag
    {
        #region Public properties

        private string archivePath;
        private string archiveFileName;
        private bool enabled = true;
        private bool promote_ARCHIVED_AS_Prop = true;

        /// <summary>
        /// Where to archive files, full path to zip archive. Supports macros %date%, %interface%, %bu%, %messageName%.
        /// </summary>
        [DescriptionAttribute("The path to where the files are to be archived should be declared here.")]
        public string ArchivePath
        {
            get { System.Diagnostics.Trace.WriteLine("INTSTDK009.Shared.PipelineComponents.FailedMessageArchiver:ArchivePath"); return archivePath; }
            set { archivePath = value; }
        }

        /// <summary>
        /// The filename to archive under. Supports the macros %date%, %MessageID%,%SourceFileName%"
        /// </summary>
        [DescriptionAttribute("The filename to use when archiving. Supports the macros %date% and %MessageID%")]
        public string ArchiveFileName
        {
            get { System.Diagnostics.Trace.WriteLine("INTSTDK009.Shared.PipelineComponents.FailedMessageArchiver:ArchiveFileName"); return archiveFileName; }
            set { archiveFileName = value; }
        }

        /// <summary>
        /// This attribute controls if the archive functionality is enabled or not.
        /// </summary>
        [DescriptionAttribute("If archive functionality is enabled or not.")]
        public bool Enabled
        {
            get { System.Diagnostics.Trace.WriteLine("INTSTDK009.Shared.PipelineComponents.FailedMessageArchiver:Enabled"); return enabled; }
            set { enabled = value; }
        }

        /// <summary>
        /// This attribute controls if the ARCHIVED_AS property is to be promoted or not.
        /// </summary>
        [DescriptionAttribute("If the ARCHIVED_AS property is to be promoted or not.")]
        public bool Promote_ARCHIVED_AS_Prop
        {
            get { System.Diagnostics.Trace.WriteLine("INTSTDK009.Shared.PipelineComponents.FailedMessageArchiver:Promote_ARCHIVED_AS"); return promote_ARCHIVED_AS_Prop; }
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
            get { System.Diagnostics.Trace.WriteLine("INTSTDK009.Shared.PipelineComponents.FailedMessageArchiver:Description"); return "This component saves a received message to a specified folder unless the message contains <StatusCode>200</StatusCode>."; }
        }

        /// <summary>
        /// Name of the component.
        /// </summary>
        [Browsable(false)]
        public string Name
        {
            get { System.Diagnostics.Trace.WriteLine("INTSTDK009.Shared.PipelineComponents.FailedMessageArchiver:Name"); return "FailedMessageArchiver"; }
        }

        /// <summary>
        /// Version of the component.
        /// </summary>
        [Browsable(false)]
        public string Version
        {
            get { System.Diagnostics.Trace.WriteLine("INTSTDK009.Shared.PipelineComponents.FailedMessageArchiver:Version"); return "1.0"; }
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
            System.Diagnostics.Trace.WriteLine("INTSTDK009.Shared.PipelineComponents.FailedMessageArchiver:Execute");
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
                    // Assign the datastream to a SeekableStream that overflows to disk (VirtualStream)
                    Stream data = Helpers.EnsureSeekableStream(bodyPart.Data);

                    string statusCode = ExtractDataValueXPath(data, "//StatusCode");
                    if (!String.IsNullOrWhiteSpace(statusCode) && statusCode.Equals("200"))
                    {
                        return null;
                    }

                    fullArchivePath = GetArchivePathAndFileName(inMessage);

                    // Make sure that the folder specified exists, and create it if it doesn't.
                    string archiveFolder = Path.GetDirectoryName(fullArchivePath);
                    if (!Directory.Exists(archiveFolder))
                    {
                        Directory.CreateDirectory(archiveFolder);
                    }

                    // Create a file, or overwrite it if it exists previously
                    FileStream fileStream = File.Open(fullArchivePath, FileMode.Create);

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

                    // Throw exception to suspend in BizTalk
                    string exceptionMessage = String.Format("INTSTDK009.Shared.PipelineComponents.FailedMessageArchiver: Response message StatusCode='{0}'", statusCode);
                    throw new Exception(exceptionMessage);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("INTSTDK009.Shared.PipelineComponents.FailedMessageArchiver: Error occurred in pipeline component, exception: " + ex.ToString());
                throw;
            }

            // Always return null since the message received should not be processed further.
            return null;
        }

        #endregion

        #region Private Methods
        private string GetArchivePathAndFileName(IBaseMessage inMessage)
        {
            System.Diagnostics.Trace.WriteLine("INTSTDK009.Shared.PipelineComponents.FailedMessageArchiver:GetArchivePathAndFileName");
            string fileName = string.Empty;

            string newFileName = ArchiveFileName;
            if (!string.IsNullOrEmpty(newFileName))
            {
                newFileName = newFileName.Replace("%date%", DateTime.Now.ToString("yyyy-MM-ddTHHmmssff"));
                newFileName = newFileName.Replace("%MessageID%", inMessage.MessageID.ToString());
                fileName = newFileName;
                System.Diagnostics.Trace.WriteLine("INTSTDK009.Shared.PipelineComponents.Archiver:Execute. FileName = " + fileName);
            }

            // Fix the archive name
            return Path.Combine(ArchivePath, fileName);
        }

        private string ExtractDataValueXPath(Stream MsgStream, string MsgXPath)
        {
            XmlReaderSettings settings = new XmlReaderSettings()
            {
                ConformanceLevel = ConformanceLevel.Document,
                IgnoreWhitespace = true,
                ValidationType = ValidationType.None,
                IgnoreProcessingInstructions = true,
                IgnoreComments = true,
                CloseInput = false
            };
            MsgStream.Seek(0, SeekOrigin.Begin);
            XmlReader reader = XmlReader.Create(MsgStream, settings);
            string strValue = null;
            if (!string.IsNullOrEmpty(MsgXPath))
            {
                if (reader.Read())
                {
                    XPathDocument xPathDoc = new XPathDocument(reader);
                    XPathNavigator xNavigator = xPathDoc.CreateNavigator();
                    XPathNodeIterator xNodes = xNavigator.Select(MsgXPath);
                    if (xNodes.Count != 0 && xNodes.MoveNext())
                    {
                        strValue = xNodes.Current.Value;
                    }
                    MsgStream.Seek(0, SeekOrigin.Begin);
                }
            }
            return strValue;
        }

        #endregion

        #region IComponentUI Members

        /// <summary>
        /// Component icon to use in BizTalk Editor.
        /// </summary>
        [Browsable(false)]
        public IntPtr Icon
        {
            get { System.Diagnostics.Trace.WriteLine("INTSTDK009.Shared.PipelineComponents.FailedMessageArchiver:Icon "); return IntPtr.Zero; }
        }

        /// <summary>
        /// Validates property settings when building projects 
        /// that uses this component or saving settings in BizTalk.
        /// </summary>
        /// <param name="projectSystem"></param>
        /// <returns>Enomerator with errors.</returns>
        public IEnumerator Validate(object projectSystem)
        {
            System.Diagnostics.Trace.WriteLine("INTSTDK009.Shared.PipelineComponents.FailedMessageArchiver:Validate");
            return null;
        }

        #endregion

        #region IPersistPropertyBag Members

        /// <summary>
        /// Gets class ID of component for usage from unmanaged code.
        /// </summary>
        /// <param name="classID">Class ID of the component.</param>
        public void GetClassID(out Guid classID)
        {
            classID = new Guid("B88FF97A-1A9E-4301-B10B-3D5B3973D665");
            System.Diagnostics.Trace.WriteLine("INTSTDK009.Shared.PipelineComponents.FailedMessageArchiver:ClassID: " + classID);
        }

        /// <summary>
        /// Not implemented.
        /// </summary>
        public void InitNew()
        {
            System.Diagnostics.Trace.WriteLine("INTSTDK009.Shared.PipelineComponents.FailedMessageArchiver:InitNew");
        }

        /// <summary>
        /// Loads data from property bag and sets property values.
        /// </summary>
        /// <param name="propertyBag">Property bag used.</param>
        /// <param name="errorLog"></param>
        public void Load(IPropertyBag propertyBag, int errorLog)
        {
            System.Diagnostics.Trace.WriteLine("INTSTDK009.Shared.PipelineComponents.FailedMessageArchiver:Load");
            object val = null;

            try
            {
                val = Helpers.ReadPropertyBag(propertyBag, "ArchivePath");
                if (val != null)
                {
                    archivePath = (string)val;
                }

                val = Helpers.ReadPropertyBag(propertyBag, "ArchiveFileName");
                if (val != null)
                {
                    ArchiveFileName = (string)val;
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
            catch (Exception ex)
            {
                throw new ApplicationException("Error reading propertybag: " + ex.Message);
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
            System.Diagnostics.Trace.WriteLine("INTSTDK009.Shared.PipelineComponents.FailedMessageArchiver:Save");
            Helpers.WritePropertyBag(propertyBag, "ArchivePath", archivePath);
            Helpers.WritePropertyBag(propertyBag, "ArchiveFileName", archiveFileName);
            Helpers.WritePropertyBag(propertyBag, "Enabled", enabled);
            Helpers.WritePropertyBag(propertyBag, "Promote_ARCHIVED_AS_Prop", promote_ARCHIVED_AS_Prop);
        }

        #endregion
    }
}
