namespace INTSTDK009.Shared.Pipelines
{
    using System;
    using System.Collections.Generic;
    using Microsoft.BizTalk.PipelineOM;
    using Microsoft.BizTalk.Component;
    using Microsoft.BizTalk.Component.Interop;
    
    
    public sealed class SendUpdateOrderParametersRequest : Microsoft.BizTalk.PipelineOM.SendPipeline
    {
        
        private const string _strPipeline = "<?xml version=\"1.0\" encoding=\"utf-16\"?><Document xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instanc"+
"e\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" MajorVersion=\"1\" MinorVersion=\"0\">  <Description /> "+
" <CategoryId>8c6b051c-0ff5-4fc2-9ae5-5016cb726282</CategoryId>  <FriendlyName>Transmit</FriendlyName"+
">  <Stages>    <Stage>      <PolicyFileStage _locAttrData=\"Name\" _locID=\"1\" Name=\"Pre-Assemble\" minO"+
"ccurs=\"0\" maxOccurs=\"-1\" execMethod=\"All\" stageId=\"9d0e4101-4cce-4536-83fa-4a5040674ad6\" />      <Co"+
"mponents />    </Stage>    <Stage>      <PolicyFileStage _locAttrData=\"Name\" _locID=\"2\" Name=\"Assemb"+
"le\" minOccurs=\"0\" maxOccurs=\"1\" execMethod=\"All\" stageId=\"9d0e4107-4cce-4536-83fa-4a5040674ad6\" />  "+
"    <Components>        <Component>          <Name>Microsoft.BizTalk.Component.XmlAsmComp,Microsoft."+
"BizTalk.Pipeline.Components, Version=3.0.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35</Name"+
">          <ComponentName>XML assembler</ComponentName>          <Description>XML assembler componen"+
"t.</Description>          <Version>1.0</Version>          <Properties>            <Property Name=\"En"+
"velopeDocSpecNames\">              <Value xsi:type=\"xsd:string\" />            </Property>            "+
"<Property Name=\"EnvelopeSpecTargetNamespaces\">              <Value xsi:type=\"xsd:string\" />         "+
"   </Property>            <Property Name=\"DocumentSpecNames\">              <Value xsi:type=\"xsd:stri"+
"ng\" />            </Property>            <Property Name=\"DocumentSpecTargetNamespaces\">             "+
" <Value xsi:type=\"xsd:string\" />            </Property>            <Property Name=\"XmlAsmProcessingI"+
"nstructions\" />            <Property Name=\"ProcessingInstructionsOptions\">              <Value xsi:t"+
"ype=\"xsd:int\">0</Value>            </Property>            <Property Name=\"ProcessingInstructionsScop"+
"e\">              <Value xsi:type=\"xsd:int\">0</Value>            </Property>            <Property Nam"+
"e=\"AddXmlDeclaration\">              <Value xsi:type=\"xsd:boolean\">true</Value>            </Property"+
">            <Property Name=\"TargetCharset\">              <Value xsi:type=\"xsd:string\" />           "+
" </Property>            <Property Name=\"TargetCodePage\">              <Value xsi:type=\"xsd:int\">0</V"+
"alue>            </Property>            <Property Name=\"PreserveBom\">              <Value xsi:type=\""+
"xsd:boolean\">true</Value>            </Property>            <Property Name=\"HiddenProperties\">      "+
"        <Value xsi:type=\"xsd:string\">EnvelopeSpecTargetNamespaces,DocumentSpecTargetNamespaces,Targe"+
"tCodePage</Value>            </Property>          </Properties>          <CachedDisplayName>XML asse"+
"mbler</CachedDisplayName>          <CachedIsManaged>true</CachedIsManaged>        </Component>      "+
"</Components>    </Stage>    <Stage>      <PolicyFileStage _locAttrData=\"Name\" _locID=\"3\" Name=\"Enco"+
"de\" minOccurs=\"0\" maxOccurs=\"-1\" execMethod=\"All\" stageId=\"9d0e4108-4cce-4536-83fa-4a5040674ad6\" /> "+
"     <Components>        <Component>          <Name>INTSTDK009.Shared.PipelineComponents.XPathPromot"+
"er.XPathPromoterComponent,INTSTDK009.Shared.PipelineComponents, Version=1.0.0.0, Culture=neutral, Pu"+
"blicKeyToken=3696f8b0d65bcb41</Name>          <ComponentName>XPathPromoterComponent</ComponentName> "+
"         <Description>This component promotes a value given by an XPath.</Description>          <Ver"+
"sion>1.1</Version>          <Properties>            <Property Name=\"ContextProperty\">              <"+
"Value xsi:type=\"xsd:string\">http://schemas.microsoft.com/BizTalk/2003/file-properties#ReceivedFileNa"+
"me</Value>            </Property>            <Property Name=\"XPathProperty\">              <Value xsi"+
":type=\"xsd:string\">/*[local-name()='Orders' and namespace-uri()='']/*[local-name()='Order' and names"+
"pace-uri()='']/*[local-name()='Products' and namespace-uri()='']/*[local-name()='Product' and namesp"+
"ace-uri()=''][1]/*[local-name()='ProductCode' and namespace-uri()='']</Value>            </Property>"+
"            <Property Name=\"Promote\">              <Value xsi:type=\"xsd:boolean\">false</Value>      "+
"      </Property>            <Property Name=\"Enabled\">              <Value xsi:type=\"xsd:boolean\">fa"+
"lse</Value>            </Property>          </Properties>          <CachedDisplayName>XPathPromoterC"+
"omponent</CachedDisplayName>          <CachedIsManaged>true</CachedIsManaged>        </Component>   "+
"     <Component>          <Name>Microsoft.BizTalk.Component.JsonEncoder,Microsoft.BizTalk.Pipeline.C"+
"omponents, Version=3.0.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35</Name>          <Compon"+
"entName>JSON encoder</ComponentName>          <Description>JSON encoder component</Description>     "+
"     <Version>1.0</Version>          <Properties>            <Property Name=\"RemoveOuterEnvelope\">  "+
"            <Value xsi:type=\"xsd:boolean\">false</Value>            </Property>          </Properties"+
">          <CachedDisplayName>JSON encoder</CachedDisplayName>          <CachedIsManaged>true</Cache"+
"dIsManaged>        </Component>        <Component>          <Name>INTSTDK009.Shared.PipelineComponen"+
"ts.Archiver.ArchiverComponent,INTSTDK009.Shared.PipelineComponents.Archiver, Version=1.0.0.0, Cultur"+
"e=neutral, PublicKeyToken=3696f8b0d65bcb41</Name>          <ComponentName>Archiver</ComponentName>  "+
"        <Description>This component saves the file in an archive.</Description>          <Version>1."+
"1</Version>          <Properties>            <Property Name=\"ArchivePath\" />            <Property Na"+
"me=\"ArchiveFileName\" />            <Property Name=\"Enabled\">              <Value xsi:type=\"xsd:boole"+
"an\">false</Value>            </Property>            <Property Name=\"Promote_ARCHIVED_AS_Prop\">      "+
"        <Value xsi:type=\"xsd:boolean\">false</Value>            </Property>          </Properties>   "+
"       <CachedDisplayName>Archiver</CachedDisplayName>          <CachedIsManaged>true</CachedIsManag"+
"ed>        </Component>      </Components>    </Stage>  </Stages></Document>";
        
        private const string _versionDependentGuid = "1d1c0117-9027-490f-938a-1457b237c88a";
        
        public SendUpdateOrderParametersRequest()
        {
            Microsoft.BizTalk.PipelineOM.Stage stage = this.AddStage(new System.Guid("9d0e4107-4cce-4536-83fa-4a5040674ad6"), Microsoft.BizTalk.PipelineOM.ExecutionMode.all);
            IBaseComponent comp0 = Microsoft.BizTalk.PipelineOM.PipelineManager.CreateComponent("Microsoft.BizTalk.Component.XmlAsmComp,Microsoft.BizTalk.Pipeline.Components, Version=3.0.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35");;
            if (comp0 is IPersistPropertyBag)
            {
                string comp0XmlProperties = "<?xml version=\"1.0\" encoding=\"utf-16\"?><PropertyBag xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-inst"+
"ance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">  <Properties>    <Property Name=\"EnvelopeDocSpec"+
"Names\">      <Value xsi:type=\"xsd:string\" />    </Property>    <Property Name=\"EnvelopeSpecTargetNam"+
"espaces\">      <Value xsi:type=\"xsd:string\" />    </Property>    <Property Name=\"DocumentSpecNames\">"+
"      <Value xsi:type=\"xsd:string\" />    </Property>    <Property Name=\"DocumentSpecTargetNamespaces"+
"\">      <Value xsi:type=\"xsd:string\" />    </Property>    <Property Name=\"XmlAsmProcessingInstructio"+
"ns\" />    <Property Name=\"ProcessingInstructionsOptions\">      <Value xsi:type=\"xsd:int\">0</Value>  "+
"  </Property>    <Property Name=\"ProcessingInstructionsScope\">      <Value xsi:type=\"xsd:int\">0</Val"+
"ue>    </Property>    <Property Name=\"AddXmlDeclaration\">      <Value xsi:type=\"xsd:boolean\">true</V"+
"alue>    </Property>    <Property Name=\"TargetCharset\">      <Value xsi:type=\"xsd:string\" />    </Pr"+
"operty>    <Property Name=\"TargetCodePage\">      <Value xsi:type=\"xsd:int\">0</Value>    </Property> "+
"   <Property Name=\"PreserveBom\">      <Value xsi:type=\"xsd:boolean\">true</Value>    </Property>    <"+
"Property Name=\"HiddenProperties\">      <Value xsi:type=\"xsd:string\">EnvelopeSpecTargetNamespaces,Doc"+
"umentSpecTargetNamespaces,TargetCodePage</Value>    </Property>  </Properties></PropertyBag>";
                PropertyBag pb = PropertyBag.DeserializeFromXml(comp0XmlProperties);;
                ((IPersistPropertyBag)(comp0)).Load(pb, 0);
            }
            this.AddComponent(stage, comp0);
            stage = this.AddStage(new System.Guid("9d0e4108-4cce-4536-83fa-4a5040674ad6"), Microsoft.BizTalk.PipelineOM.ExecutionMode.all);
            IBaseComponent comp1 = Microsoft.BizTalk.PipelineOM.PipelineManager.CreateComponent("INTSTDK009.Shared.PipelineComponents.XPathPromoter.XPathPromoterComponent,INTSTDK009.Shared.PipelineComponents, Version=1.0.0.0, Culture=neutral, PublicKeyToken=3696f8b0d65bcb41");;
            if (comp1 is IPersistPropertyBag)
            {
                string comp1XmlProperties = "<?xml version=\"1.0\" encoding=\"utf-16\"?><PropertyBag xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-inst"+
"ance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">  <Properties>    <Property Name=\"ContextProperty"+
"\">      <Value xsi:type=\"xsd:string\">http://schemas.microsoft.com/BizTalk/2003/file-properties#Recei"+
"vedFileName</Value>    </Property>    <Property Name=\"XPathProperty\">      <Value xsi:type=\"xsd:stri"+
"ng\">/*[local-name()='Orders' and namespace-uri()='']/*[local-name()='Order' and namespace-uri()='']/"+
"*[local-name()='Products' and namespace-uri()='']/*[local-name()='Product' and namespace-uri()=''][1"+
"]/*[local-name()='ProductCode' and namespace-uri()='']</Value>    </Property>    <Property Name=\"Pro"+
"mote\">      <Value xsi:type=\"xsd:boolean\">false</Value>    </Property>    <Property Name=\"Enabled\"> "+
"     <Value xsi:type=\"xsd:boolean\">false</Value>    </Property>  </Properties></PropertyBag>";
                PropertyBag pb = PropertyBag.DeserializeFromXml(comp1XmlProperties);;
                ((IPersistPropertyBag)(comp1)).Load(pb, 0);
            }
            this.AddComponent(stage, comp1);
            IBaseComponent comp2 = Microsoft.BizTalk.PipelineOM.PipelineManager.CreateComponent("Microsoft.BizTalk.Component.JsonEncoder,Microsoft.BizTalk.Pipeline.Components, Version=3.0.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35");;
            if (comp2 is IPersistPropertyBag)
            {
                string comp2XmlProperties = "<?xml version=\"1.0\" encoding=\"utf-16\"?><PropertyBag xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-inst"+
"ance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">  <Properties>    <Property Name=\"RemoveOuterEnve"+
"lope\">      <Value xsi:type=\"xsd:boolean\">false</Value>    </Property>  </Properties></PropertyBag>";
                PropertyBag pb = PropertyBag.DeserializeFromXml(comp2XmlProperties);;
                ((IPersistPropertyBag)(comp2)).Load(pb, 0);
            }
            this.AddComponent(stage, comp2);
            IBaseComponent comp3 = Microsoft.BizTalk.PipelineOM.PipelineManager.CreateComponent("INTSTDK009.Shared.PipelineComponents.Archiver.ArchiverComponent,INTSTDK009.Shared.PipelineComponents.Archiver, Version=1.0.0.0, Culture=neutral, PublicKeyToken=3696f8b0d65bcb41");;
            if (comp3 is IPersistPropertyBag)
            {
                string comp3XmlProperties = "<?xml version=\"1.0\" encoding=\"utf-16\"?><PropertyBag xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-inst"+
"ance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">  <Properties>    <Property Name=\"ArchivePath\" />"+
"    <Property Name=\"ArchiveFileName\" />    <Property Name=\"Enabled\">      <Value xsi:type=\"xsd:boole"+
"an\">false</Value>    </Property>    <Property Name=\"Promote_ARCHIVED_AS_Prop\">      <Value xsi:type="+
"\"xsd:boolean\">false</Value>    </Property>  </Properties></PropertyBag>";
                PropertyBag pb = PropertyBag.DeserializeFromXml(comp3XmlProperties);;
                ((IPersistPropertyBag)(comp3)).Load(pb, 0);
            }
            this.AddComponent(stage, comp3);
        }
        
        public override string XmlContent
        {
            get
            {
                return _strPipeline;
            }
        }
        
        public override System.Guid VersionDependentGuid
        {
            get
            {
                return new System.Guid(_versionDependentGuid);
            }
        }
    }
}
