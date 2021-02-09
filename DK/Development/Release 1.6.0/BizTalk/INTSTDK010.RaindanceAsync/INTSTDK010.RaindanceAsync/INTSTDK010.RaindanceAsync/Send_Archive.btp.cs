namespace INTSTDK010.RaindanceAsync
{
    using System;
    using System.Collections.Generic;
    using Microsoft.BizTalk.PipelineOM;
    using Microsoft.BizTalk.Component;
    using Microsoft.BizTalk.Component.Interop;
    
    
    public sealed class Send_Archive : Microsoft.BizTalk.PipelineOM.SendPipeline
    {
        
        private const string _strPipeline = "<?xml version=\"1.0\" encoding=\"utf-16\"?><Document xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instanc"+
"e\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" MajorVersion=\"1\" MinorVersion=\"0\">  <Description /> "+
" <CategoryId>8c6b051c-0ff5-4fc2-9ae5-5016cb726282</CategoryId>  <FriendlyName>Transmit</FriendlyName"+
">  <Stages>    <Stage>      <PolicyFileStage _locAttrData=\"Name\" _locID=\"1\" Name=\"Pre-Assemble\" minO"+
"ccurs=\"0\" maxOccurs=\"-1\" execMethod=\"All\" stageId=\"9d0e4101-4cce-4536-83fa-4a5040674ad6\" />      <Co"+
"mponents />    </Stage>    <Stage>      <PolicyFileStage _locAttrData=\"Name\" _locID=\"2\" Name=\"Assemb"+
"le\" minOccurs=\"0\" maxOccurs=\"1\" execMethod=\"All\" stageId=\"9d0e4107-4cce-4536-83fa-4a5040674ad6\" />  "+
"    <Components />    </Stage>    <Stage>      <PolicyFileStage _locAttrData=\"Name\" _locID=\"3\" Name="+
"\"Encode\" minOccurs=\"0\" maxOccurs=\"-1\" execMethod=\"All\" stageId=\"9d0e4108-4cce-4536-83fa-4a5040674ad6"+
"\" />      <Components>        <Component>          <Name>INTSTDK010.Shared.PipelineComponents.Archiv"+
"er.ArchiverComponent,INTSTDK010.Shared.PipelineComponents.Archiver, Version=1.0.0.0, Culture=neutral"+
", PublicKeyToken=3696f8b0d65bcb41</Name>          <ComponentName>Archiver</ComponentName>          <"+
"Description>This component saves the file in an archive.</Description>          <Version>1.1</Versio"+
"n>          <Properties>            <Property Name=\"ArchivePath\">              <Value xsi:type=\"xsd:"+
"string\">\\\\clfile\\BizTalk\\INTSTDK010\\</Value>            </Property>            <Property Name=\"Archi"+
"veFileName\">              <Value xsi:type=\"xsd:string\">%MessageID%.txt</Value>            </Property"+
">            <Property Name=\"Enabled\">              <Value xsi:type=\"xsd:boolean\">true</Value>      "+
"      </Property>            <Property Name=\"Promote_ARCHIVED_AS_Prop\">              <Value xsi:type"+
"=\"xsd:boolean\">true</Value>            </Property>          </Properties>          <CachedDisplayNam"+
"e>Archiver</CachedDisplayName>          <CachedIsManaged>true</CachedIsManaged>        </Component> "+
"     </Components>    </Stage>  </Stages></Document>";
        
        private const string _versionDependentGuid = "644aa78c-2327-463b-b6b6-b1ccbfe7e997";
        
        public Send_Archive()
        {
            Microsoft.BizTalk.PipelineOM.Stage stage = this.AddStage(new System.Guid("9d0e4108-4cce-4536-83fa-4a5040674ad6"), Microsoft.BizTalk.PipelineOM.ExecutionMode.all);
            IBaseComponent comp0 = Microsoft.BizTalk.PipelineOM.PipelineManager.CreateComponent("INTSTDK010.Shared.PipelineComponents.Archiver.ArchiverComponent,INTSTDK010.Shared.PipelineComponents.Archiver, Version=1.0.0.0, Culture=neutral, PublicKeyToken=3696f8b0d65bcb41");;
            if (comp0 is IPersistPropertyBag)
            {
                string comp0XmlProperties = "<?xml version=\"1.0\" encoding=\"utf-16\"?><PropertyBag xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-inst"+
"ance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">  <Properties>    <Property Name=\"ArchivePath\">  "+
"    <Value xsi:type=\"xsd:string\">\\\\clfile\\BizTalk\\INTSTDK010\\</Value>    </Property>    <Property Na"+
"me=\"ArchiveFileName\">      <Value xsi:type=\"xsd:string\">%MessageID%.txt</Value>    </Property>    <P"+
"roperty Name=\"Enabled\">      <Value xsi:type=\"xsd:boolean\">true</Value>    </Property>    <Property "+
"Name=\"Promote_ARCHIVED_AS_Prop\">      <Value xsi:type=\"xsd:boolean\">true</Value>    </Property>  </P"+
"roperties></PropertyBag>";
                PropertyBag pb = PropertyBag.DeserializeFromXml(comp0XmlProperties);;
                ((IPersistPropertyBag)(comp0)).Load(pb, 0);
            }
            this.AddComponent(stage, comp0);
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
