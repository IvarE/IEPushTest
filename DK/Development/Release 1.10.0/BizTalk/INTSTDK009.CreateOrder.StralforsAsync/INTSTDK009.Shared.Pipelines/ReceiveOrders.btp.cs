namespace INTSTDK009.Shared.Pipelines
{
    using System;
    using System.Collections.Generic;
    using Microsoft.BizTalk.PipelineOM;
    using Microsoft.BizTalk.Component;
    using Microsoft.BizTalk.Component.Interop;
    
    
    public sealed class ReceiveOrders : Microsoft.BizTalk.PipelineOM.ReceivePipeline
    {
        
        private const string _strPipeline = "<?xml version=\"1.0\" encoding=\"utf-16\"?><Document xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instanc"+
"e\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" MajorVersion=\"1\" MinorVersion=\"0\">  <Description /> "+
" <CategoryId>f66b9f5e-43ff-4f5f-ba46-885348ae1b4e</CategoryId>  <FriendlyName>Receive</FriendlyName>"+
"  <Stages>    <Stage>      <PolicyFileStage _locAttrData=\"Name\" _locID=\"1\" Name=\"Decode\" minOccurs=\""+
"0\" maxOccurs=\"-1\" execMethod=\"All\" stageId=\"9d0e4103-4cce-4536-83fa-4a5040674ad6\" />      <Component"+
"s>        <Component>          <Name>INTSTDK009.Shared.PipelineComponents.Archiver.ArchiverComponent"+
",INTSTDK009.Shared.PipelineComponents.Archiver, Version=1.0.0.0, Culture=neutral, PublicKeyToken=369"+
"6f8b0d65bcb41</Name>          <ComponentName>Archiver</ComponentName>          <Description>This com"+
"ponent saves the file in an archive.</Description>          <Version>1.1</Version>          <Propert"+
"ies>            <Property Name=\"ArchivePath\" />            <Property Name=\"ArchiveFileName\" />      "+
"      <Property Name=\"Enabled\">              <Value xsi:type=\"xsd:boolean\">false</Value>            "+
"</Property>            <Property Name=\"Promote_ARCHIVED_AS_Prop\">              <Value xsi:type=\"xsd:"+
"boolean\">false</Value>            </Property>          </Properties>          <CachedDisplayName>Arc"+
"hiver</CachedDisplayName>          <CachedIsManaged>true</CachedIsManaged>        </Component>      "+
"</Components>    </Stage>    <Stage>      <PolicyFileStage _locAttrData=\"Name\" _locID=\"2\" Name=\"Disa"+
"ssemble\" minOccurs=\"0\" maxOccurs=\"-1\" execMethod=\"FirstMatch\" stageId=\"9d0e4105-4cce-4536-83fa-4a504"+
"0674ad6\" />      <Components>        <Component>          <Name>Microsoft.BizTalk.Component.XmlDasmC"+
"omp,Microsoft.BizTalk.Pipeline.Components, Version=3.0.1.0, Culture=neutral, PublicKeyToken=31bf3856"+
"ad364e35</Name>          <ComponentName>XML disassembler</ComponentName>          <Description>Strea"+
"ming XML disassembler</Description>          <Version>1.0</Version>          <Properties>           "+
" <Property Name=\"EnvelopeSpecNames\">              <Value xsi:type=\"xsd:string\" />            </Prope"+
"rty>            <Property Name=\"EnvelopeSpecTargetNamespaces\">              <Value xsi:type=\"xsd:str"+
"ing\" />            </Property>            <Property Name=\"DocumentSpecNames\">              <Value xs"+
"i:type=\"xsd:string\" />            </Property>            <Property Name=\"DocumentSpecTargetNamespace"+
"s\">              <Value xsi:type=\"xsd:string\" />            </Property>            <Property Name=\"A"+
"llowUnrecognizedMessage\">              <Value xsi:type=\"xsd:boolean\">false</Value>            </Prop"+
"erty>            <Property Name=\"ValidateDocument\">              <Value xsi:type=\"xsd:boolean\">false"+
"</Value>            </Property>            <Property Name=\"RecoverableInterchangeProcessing\">       "+
"       <Value xsi:type=\"xsd:boolean\">false</Value>            </Property>            <Property Name="+
"\"HiddenProperties\">              <Value xsi:type=\"xsd:string\">EnvelopeSpecTargetNamespaces,DocumentS"+
"pecTargetNamespaces</Value>            </Property>          </Properties>          <CachedDisplayNam"+
"e>XML disassembler</CachedDisplayName>          <CachedIsManaged>true</CachedIsManaged>        </Com"+
"ponent>      </Components>    </Stage>    <Stage>      <PolicyFileStage _locAttrData=\"Name\" _locID=\""+
"3\" Name=\"Validate\" minOccurs=\"0\" maxOccurs=\"-1\" execMethod=\"All\" stageId=\"9d0e410d-4cce-4536-83fa-4a"+
"5040674ad6\" />      <Components />    </Stage>    <Stage>      <PolicyFileStage _locAttrData=\"Name\" "+
"_locID=\"4\" Name=\"ResolveParty\" minOccurs=\"0\" maxOccurs=\"-1\" execMethod=\"All\" stageId=\"9d0e410e-4cce-"+
"4536-83fa-4a5040674ad6\" />      <Components />    </Stage>  </Stages></Document>";
        
        private const string _versionDependentGuid = "0ccb3f10-2610-4f93-b588-758016f56999";
        
        public ReceiveOrders()
        {
            Microsoft.BizTalk.PipelineOM.Stage stage = this.AddStage(new System.Guid("9d0e4103-4cce-4536-83fa-4a5040674ad6"), Microsoft.BizTalk.PipelineOM.ExecutionMode.all);
            IBaseComponent comp0 = Microsoft.BizTalk.PipelineOM.PipelineManager.CreateComponent("INTSTDK009.Shared.PipelineComponents.Archiver.ArchiverComponent,INTSTDK009.Shared.PipelineComponents.Archiver, Version=1.0.0.0, Culture=neutral, PublicKeyToken=3696f8b0d65bcb41");;
            if (comp0 is IPersistPropertyBag)
            {
                string comp0XmlProperties = "<?xml version=\"1.0\" encoding=\"utf-16\"?><PropertyBag xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-inst"+
"ance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">  <Properties>    <Property Name=\"ArchivePath\" />"+
"    <Property Name=\"ArchiveFileName\" />    <Property Name=\"Enabled\">      <Value xsi:type=\"xsd:boole"+
"an\">false</Value>    </Property>    <Property Name=\"Promote_ARCHIVED_AS_Prop\">      <Value xsi:type="+
"\"xsd:boolean\">false</Value>    </Property>  </Properties></PropertyBag>";
                PropertyBag pb = PropertyBag.DeserializeFromXml(comp0XmlProperties);;
                ((IPersistPropertyBag)(comp0)).Load(pb, 0);
            }
            this.AddComponent(stage, comp0);
            stage = this.AddStage(new System.Guid("9d0e4105-4cce-4536-83fa-4a5040674ad6"), Microsoft.BizTalk.PipelineOM.ExecutionMode.firstRecognized);
            IBaseComponent comp1 = Microsoft.BizTalk.PipelineOM.PipelineManager.CreateComponent("Microsoft.BizTalk.Component.XmlDasmComp,Microsoft.BizTalk.Pipeline.Components, Version=3.0.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35");;
            if (comp1 is IPersistPropertyBag)
            {
                string comp1XmlProperties = "<?xml version=\"1.0\" encoding=\"utf-16\"?><PropertyBag xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-inst"+
"ance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">  <Properties>    <Property Name=\"EnvelopeSpecNam"+
"es\">      <Value xsi:type=\"xsd:string\" />    </Property>    <Property Name=\"EnvelopeSpecTargetNamesp"+
"aces\">      <Value xsi:type=\"xsd:string\" />    </Property>    <Property Name=\"DocumentSpecNames\">   "+
"   <Value xsi:type=\"xsd:string\" />    </Property>    <Property Name=\"DocumentSpecTargetNamespaces\"> "+
"     <Value xsi:type=\"xsd:string\" />    </Property>    <Property Name=\"AllowUnrecognizedMessage\">   "+
"   <Value xsi:type=\"xsd:boolean\">false</Value>    </Property>    <Property Name=\"ValidateDocument\"> "+
"     <Value xsi:type=\"xsd:boolean\">false</Value>    </Property>    <Property Name=\"RecoverableInterc"+
"hangeProcessing\">      <Value xsi:type=\"xsd:boolean\">false</Value>    </Property>    <Property Name="+
"\"HiddenProperties\">      <Value xsi:type=\"xsd:string\">EnvelopeSpecTargetNamespaces,DocumentSpecTarge"+
"tNamespaces</Value>    </Property>  </Properties></PropertyBag>";
                PropertyBag pb = PropertyBag.DeserializeFromXml(comp1XmlProperties);;
                ((IPersistPropertyBag)(comp1)).Load(pb, 0);
            }
            this.AddComponent(stage, comp1);
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
