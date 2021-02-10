using System;
using System.Collections;
using System.ComponentModel;

using Microsoft.BizTalk.Component.Interop;
using IDC.Shared.PipelineComponentsHelpers;
using Microsoft.BizTalk.Message.Interop;
using System.Xml.Linq;
using System.IO;
using Microsoft.BizTalk.Streaming;
using System.Xml;
using System.Xml.XPath;


namespace INTSTDK010.Shared.PipelineComponents
{
    /// <summary>
    /// Writes or promotes a value to a context property. The value is specified by an XPath.
    /// </summary>
    [ComponentCategory(CategoryTypes.CATID_PipelineComponent)]
    [ComponentCategory(CategoryTypes.CATID_Encoder)]
    [System.Runtime.InteropServices.Guid("016a35aa-2c6b-4323-ae1b-3ead849f37b3")]
    public class XPathPromoterComponent : IBaseComponent,
        Microsoft.BizTalk.Component.Interop.IComponent,
        IComponentUI, IPersistPropertyBag
    {
        #region Public Properties

        /// <summary>
        /// This property holds the context property to write to. Format: namespace#name
        /// </summary>
        [DescriptionAttribute("This property holds the context property to write to. Format: namespace#name")]
        public string ContextProperty { get; set; }

        /// <summary>
        /// This property holds the XPaht tthe value that is to be promoted.
        /// </summary>
        [DescriptionAttribute("This property holds the XPaht tthe value that is to be promoted.")]
        public string XPathProperty { get; set; }

        /// <summary>
        /// Whether or not the property is to be promoted. Promoting a property requires it to 
        /// </summary>
        [DescriptionAttribute("This property holds the column name to return. Or the constant to return if no TranslationTable is given.")]
        public bool Promote { get; set; }

        /// <summary>
        /// Control whether or not this component is active or not 
        /// </summary>
        [DescriptionAttribute("Control whether or not this component is active or not")]
        public bool Enabled { get; set; }

        #endregion

        #region IBaseComponent Members

        /// <summary>
        /// Description of the component.
        /// </summary>
        [Browsable(false)]
        public string Description
        {
            get { return "This component promotes a value given by an XPath."; }
        }

        /// <summary>
        /// Name of the component.
        /// </summary>
        [Browsable(false)]
        public string Name
        {
            get { return "XPathPromoterComponent"; }
        }

        /// <summary>
        /// Version of the component.
        /// </summary>
        [Browsable(false)]
        public string Version
        {
            get { return "1.1"; }
        }

        #endregion

        #region IComponent Members

        /// <summary>
        /// Executed when pipeline-component is triggered.
        /// This method implements the functionality of the component.
        /// Writes a context property.
        /// </summary>
        /// <param name="pContext">Message context.</param>
        /// <param name="pInMsg">Message received.</param>
        /// <returns>Same message as received, always.</returns>
        public Microsoft.BizTalk.Message.Interop.IBaseMessage Execute(IPipelineContext pContext, Microsoft.BizTalk.Message.Interop.IBaseMessage pInMsg)
        {
            try
            {
                if (this.Enabled)
                {
                    string value = string.Empty;
                    string[] property = null;
                    property = ContextProperty.Split('#');

                    Stream stream = HelperFunctions.GetMessageStream(pInMsg, pContext);
                    value = HelperFunctions.ExtractDataValueXPath(stream, XPathProperty);


                    if (this.Promote)
                        pInMsg.Context.Promote(property[1], property[0], value);
                }

                return pInMsg;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        #endregion

        #region IComponentUI Members

        /// <summary>
        /// Component icon to use in BizTalk Editor.
        /// </summary>
        [Browsable(false)]
        public IntPtr Icon
        {
            get { return IntPtr.Zero; }
        }

        /// <summary>
        /// Validates property settings when building projects 
        /// that uses this component or saving settings in BizTalk.
        /// </summary>
        /// <param name="projectSystem"></param>
        /// <returns>Enomerator with errors.</returns>
        public System.Collections.IEnumerator Validate(object projectSystem)
        {
            ArrayList errors = new ArrayList();
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
            classID = new Guid("016a35aa-2c6b-4323-ae1b-3ead849f37b3");
        }

        /// <summary>
        /// Not implemented.
        /// </summary>
        public void InitNew()
        {

        }

        /// <summary>
        /// Loads data from property bag and sets property values.
        /// </summary>
        /// <param name="propertyBag">Property bag used.</param>
        /// <param name="errorLog"></param>
        public void Load(IPropertyBag propertyBag, int errorLog)
        {
            object val = null;

            val = Helpers.ReadPropertyBag(propertyBag, "ContextProperty");
            if (val != null)
                this.ContextProperty = (string)val;

            val = Helpers.ReadPropertyBag(propertyBag, "XPathProperty");
            if (val != null)
                this.XPathProperty = (string)val;

            val = Helpers.ReadPropertyBag(propertyBag, "Promote");
            if (val != null)
                this.Promote = (bool)val;

            val = Helpers.ReadPropertyBag(propertyBag, "Enabled");
            if (val != null)
                this.Enabled = (bool)val;
        }

        /// <summary>
        /// Saves property values to property bag.
        /// </summary>
        /// <param name="propertyBag">Property bag used.</param>
        /// <param name="clearDirty"></param>
        /// <param name="saveAllProperties"></param>
        public void Save(IPropertyBag propertyBag, bool clearDirty, bool saveAllProperties)
        {
            Helpers.WritePropertyBag(propertyBag, "ContextProperty", this.ContextProperty);
            Helpers.WritePropertyBag(propertyBag, "XPathProperty", this.XPathProperty);

            Helpers.WritePropertyBag(propertyBag, "Promote", this.Promote);
            Helpers.WritePropertyBag(propertyBag, "Enabled", this.Enabled);
        }

        #endregion

    }

}
