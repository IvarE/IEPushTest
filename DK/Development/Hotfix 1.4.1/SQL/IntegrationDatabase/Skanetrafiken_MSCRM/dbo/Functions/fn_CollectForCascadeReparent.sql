CREATE function [dbo].[fn_CollectForCascadeReparent]
(
  @root_id uniqueidentifier,
  @root_otc int,
  @isoffline int,
  @old_owner uniqueidentifier,
  @old_owner_type int
)
returns @t table
( 	
   o uniqueidentifier,
   t int,
   u uniqueidentifier,
   v int,
   q uniqueidentifier,
   s int,
   l int
) 
as
begin
insert into @t values(@root_id,@root_otc,@old_owner, @old_owner_type, N'00000000-0000-0000-0000-000000000000', 0, 0)
if(exists(select * from @t where t=1))begin insert into @t(o,t,u,v,q,s,l) select o.AccountId,1,o.OwnerId, o.OwnerIdType, c.o, c.t, c.l +1 from Account o,@t c where o.ParentAccountId=c.o and c.t=1
while(@@rowcount <> 0)if(exists(select * from @t where t=1))insert into @t(o,t,u,v,q,s,l) select o.AccountId,1,o.OwnerId,o.OwnerIdType,c.o,c.t,c.l +1 from Account o,@t c where o.ParentAccountId=c.o and c.t=1 and o.AccountId not in(select o from @t where o=o.AccountId and t=1) end
if(exists(select * from @t where t=9100))begin insert into @t(o,t,u,v,q,s,l) select o.ReportId,9100,o.OwnerId, o.OwnerIdType, c.o, c.t, c.l +1 from Report o,@t c where o.ParentReportId=c.o and c.t=9100
while(@@rowcount <> 0)if(exists(select * from @t where t=9100))insert into @t(o,t,u,v,q,s,l) select o.ReportId,9100,o.OwnerId,o.OwnerIdType,c.o,c.t,c.l +1 from Report o,@t c where o.ParentReportId=c.o and c.t=9100 and o.ReportId not in(select o from @t where o=o.ReportId and t=9100) end
if(exists(select * from @t where t=4400))begin insert into @t(o,t,u,v,q,s,l) select o.ActivityId,4402,o.OwnerId, o.OwnerIdType, c.o, c.t, c.l +1 from CampaignActivity o,@t c where o.RegardingObjectId=c.o and c.t=4400 end
if(exists(select * from @t where t in(2,1)))begin insert into @t(o,t,u,v,q,s,l) select o.ContactId,2,o.OwnerId, o.OwnerIdType, c.o, c.t, c.l +1 from Contact o,@t c where o.ParentCustomerId=c.o and c.t in(2,1)
while(@@rowcount <> 0)if(exists(select * from @t where t=2))insert into @t(o,t,u,v,q,s,l) select o.ContactId,2,o.OwnerId,o.OwnerIdType,c.o,c.t,c.l +1 from Contact o,@t c where o.ParentCustomerId=c.o and c.t=2 and o.ContactId not in(select o from @t where o=o.ContactId and t=2) end
if(exists(select * from @t where t in(2,1)))begin insert into @t(o,t,u,v,q,s,l) select o.SocialProfileId,99,o.OwnerId, o.OwnerIdType, c.o, c.t, c.l +1 from SocialProfile o,@t c where o.CustomerId=c.o and c.t in(2,1) end
if(exists(select * from @t where t in(2,1)))begin insert into @t(o,t,u,v,q,s,l) select o.LeadId,4,o.OwnerId, o.OwnerIdType, c.o, c.t, c.l +1 from Lead o,@t c where o.CustomerId=c.o and c.t in(2,1) end
if(exists(select * from @t where t in(2,1)))begin insert into @t(o,t,u,v,q,s,l) select o.QuoteId,1084,o.OwnerId, o.OwnerIdType, c.o, c.t, c.l +1 from Quote o,@t c where o.CustomerId=c.o and c.t in(2,1) end
if(exists(select * from @t where t=1084))begin insert into @t(o,t,u,v,q,s,l) select o.ActivityId,4211,o.OwnerId, o.OwnerIdType, c.o, c.t, c.l +1 from QuoteClose o,@t c where o.QuoteId=c.o and c.t=1084 end
if(exists(select * from @t where t in(2,1)))begin insert into @t(o,t,u,v,q,s,l) select o.IncidentId,112,o.OwnerId, o.OwnerIdType, c.o, c.t, c.l +1 from Incident o,@t c where o.CustomerId=c.o and c.t in(2,1) end
if(exists(select * from @t where t=112))begin insert into @t(o,t,u,v,q,s,l) select o.cgi_passtravelinformationId,10037,o.OwnerId, o.OwnerIdType, c.o, c.t, c.l +1 from cgi_passtravelinformation o,@t c where o.cgi_IncidentId=c.o and c.t=112 end
if(exists(select * from @t where t=112))begin insert into @t(o,t,u,v,q,s,l) select o.cgi_filelinkId,10055,o.OwnerId, o.OwnerIdType, c.o, c.t, c.l +1 from cgi_filelink o,@t c where o.cgi_IncidentId=c.o and c.t=112 end
if(exists(select * from @t where t in(2,1)))begin insert into @t(o,t,u,v,q,s,l) select o.SalesOrderId,1088,o.OwnerId, o.OwnerIdType, c.o, c.t, c.l +1 from SalesOrder o,@t c where o.CustomerId=c.o and c.t in(2,1) end
if(exists(select * from @t where t=1088))begin insert into @t(o,t,u,v,q,s,l) select o.ActivityId,4209,o.OwnerId, o.OwnerIdType, c.o, c.t, c.l +1 from OrderClose o,@t c where o.SalesOrderId=c.o and c.t=1088 end
if(exists(select * from @t where t in(2,1)))begin insert into @t(o,t,u,v,q,s,l) select o.OpportunityId,3,o.OwnerId, o.OwnerIdType, c.o, c.t, c.l +1 from Opportunity o,@t c where o.CustomerId=c.o and c.t in(2,1) end
if(exists(select * from @t where t=3))begin insert into @t(o,t,u,v,q,s,l) select o.CustomerOpportunityRoleId,4503,o.OwnerId, o.OwnerIdType, c.o, c.t, c.l +1 from CustomerOpportunityRole o,@t c where o.OpportunityId=c.o and c.t=3 end
if(exists(select * from @t where t=3))begin insert into @t(o,t,u,v,q,s,l) select o.ActivityId,4208,o.OwnerId, o.OwnerIdType, c.o, c.t, c.l +1 from OpportunityClose o,@t c where o.OpportunityId=c.o and c.t=3 end
if(exists(select * from @t where t=2020))begin insert into @t(o,t,u,v,q,s,l) select o.MailboxId,9606,o.OwnerId, o.OwnerIdType, c.o, c.t, c.l +1 from Mailbox o,@t c where o.RegardingObjectId=c.o and c.t=2020 end
if(exists(select * from @t where t in(4406,4400)))begin insert into @t(o,t,u,v,q,s,l) select o.ActivityId,4401,o.OwnerId, o.OwnerIdType, c.o, c.t, c.l +1 from CampaignResponse o,@t c where o.RegardingObjectId=c.o and c.t in(4406,4400) end
if(exists(select * from @t where t in(2,1)))begin insert into @t(o,t,u,v,q,s,l) select o.ContractId,1010,o.OwnerId, o.OwnerIdType, c.o, c.t, c.l +1 from Contract o,@t c where o.CustomerId=c.o and c.t in(2,1) end
if(exists(select * from @t where t=112))begin insert into @t(o,t,u,v,q,s,l) select o.ActivityId,4206,o.OwnerId, o.OwnerIdType, c.o, c.t, c.l +1 from IncidentResolution o,@t c where o.IncidentId=c.o and c.t=112 end
if(exists(select * from @t where t in(2,1)))begin insert into @t(o,t,u,v,q,s,l) select o.InvoiceId,1090,o.OwnerId, o.OwnerIdType, c.o, c.t, c.l +1 from Invoice o,@t c where o.CustomerId=c.o and c.t in(2,1) end
if(exists(select * from @t where t in(1090,10027,4700,10000,1010,4406,10063,10014,3,1088,2,9700,10059,1,1084,112,10058,10025,4)))begin insert into @t(o,t,u,v,q,s,l) select o.ActivityId,4216,o.OwnerId, o.OwnerIdType, c.o, c.t, c.l +1 from SocialActivity o,@t c where o.RegardingObjectId=c.o and c.t in(1090,10027,4700,10000,1010,4406,10063,10014,3,1088,2,9700,10059,1,1084,112,10058,10025,4) end
if(exists(select * from @t where t in(1090,10027,10000,1010,4406,10063,10014,3,1088,2,4400,9700,10059,1,1084,112,10058,4402,10025,4)))begin insert into @t(o,t,u,v,q,s,l) select o.ActivityId,4207,o.OwnerId, o.OwnerIdType, c.o, c.t, c.l +1 from Letter o,@t c where o.RegardingObjectId=c.o and c.t in(1090,10027,10000,1010,4406,10063,10014,3,1088,2,4400,9700,10059,1,1084,112,10058,4402,10025,4) end
if(exists(select * from @t where t in(1090,10027,10000,1010,4406,10063,10014,3,1088,2,4400,9700,10059,1,1084,112,10058,4402,10025,4)))begin insert into @t(o,t,u,v,q,s,l) select o.ActivityId,4210,o.OwnerId, o.OwnerIdType, c.o, c.t, c.l +1 from PhoneCall o,@t c where o.RegardingObjectId=c.o and c.t in(1090,10027,10000,1010,4406,10063,10014,3,1088,2,4400,9700,10059,1,1084,112,10058,4402,10025,4) end
if(exists(select * from @t where t in(1090,10027,4700,10000,1010,4406,10063,10014,3,1088,2,4400,9700,10059,1,1084,112,10058,4402,10025,4)))begin insert into @t(o,t,u,v,q,s,l) select o.ActivityId,4202,o.OwnerId, o.OwnerIdType, c.o, c.t, c.l +1 from Email o,@t c where o.RegardingObjectId=c.o and c.t in(1090,10027,4700,10000,1010,4406,10063,10014,3,1088,2,4400,9700,10059,1,1084,112,10058,4402,10025,4) end
if(exists(select * from @t where t in(1090,10027,10000,1010,10063,10014,3,1088,2,4400,9700,10059,1,1084,112,10058,10025,4)))begin insert into @t(o,t,u,v,q,s,l) select o.ActivityId,4214,o.OwnerId, o.OwnerIdType, c.o, c.t, c.l +1 from ServiceAppointment o,@t c where o.RegardingObjectId=c.o and c.t in(1090,10027,10000,1010,10063,10014,3,1088,2,4400,9700,10059,1,1084,112,10058,10025,4) end
if(exists(select * from @t where t in(1090,10027,10000,1010,10063,10014,3,1088,2,4400,9700,10059,1,1084,112,10058,4402,10025,4)))begin insert into @t(o,t,u,v,q,s,l) select o.ActivityId,4212,o.OwnerId, o.OwnerIdType, c.o, c.t, c.l +1 from Task o,@t c where o.RegardingObjectId=c.o and c.t in(1090,10027,10000,1010,10063,10014,3,1088,2,4400,9700,10059,1,1084,112,10058,4402,10025,4) end
if(exists(select * from @t where t in(1090,10027,10000,1010,4406,10063,10014,3,1088,2,4400,9700,10059,1,1084,112,10058,4402,10025,4)))begin insert into @t(o,t,u,v,q,s,l) select o.ActivityId,4201,o.OwnerId, o.OwnerIdType, c.o, c.t, c.l +1 from Appointment o,@t c where o.RegardingObjectId=c.o and c.t in(1090,10027,10000,1010,4406,10063,10014,3,1088,2,4400,9700,10059,1,1084,112,10058,4402,10025,4) end
if(exists(select * from @t where t in(1090,10027,10000,1010,4406,10063,10014,3,1088,2,4400,9700,10059,1,1084,112,10058,4402,10025,4)))begin insert into @t(o,t,u,v,q,s,l) select o.ActivityId,4204,o.OwnerId, o.OwnerIdType, c.o, c.t, c.l +1 from Fax o,@t c where o.RegardingObjectId=c.o and c.t in(1090,10027,10000,1010,4406,10063,10014,3,1088,2,4400,9700,10059,1,1084,112,10058,4402,10025,4) end
if(exists(select * from @t where t in(1090,10027,10000,1010,4406,10063,10014,3,1088,2,4400,9700,10059,1,1084,112,10058,4402,10025,4)))begin insert into @t(o,t,u,v,q,s,l) select o.ActivityId,4251,o.OwnerId, o.OwnerIdType, c.o, c.t, c.l +1 from RecurringAppointmentMaster o,@t c where o.RegardingObjectId=c.o and c.t in(1090,10027,10000,1010,4406,10063,10014,3,1088,2,4400,9700,10059,1,1084,112,10058,4402,10025,4) end
if(exists(select * from @t where t in(1090,10027,10000,1010,4406,10063,10014,3,1088,2,4400,10059,1,1084,112,10058,4402,10025,4)))begin insert into @t(o,t,u,v,q,s,l) select o.ActivityId,10009,o.OwnerId, o.OwnerIdType, c.o, c.t, c.l +1 from cgi_callguidefacebook o,@t c where o.RegardingObjectId=c.o and c.t in(1090,10027,10000,1010,4406,10063,10014,3,1088,2,4400,10059,1,1084,112,10058,4402,10025,4) end
if(exists(select * from @t where t in(1090,10027,10000,1010,4406,10063,10014,3,1088,2,4400,10059,1,1084,112,10058,4402,10025,4)))begin insert into @t(o,t,u,v,q,s,l) select o.ActivityId,10008,o.OwnerId, o.OwnerIdType, c.o, c.t, c.l +1 from cgi_callguidechat o,@t c where o.RegardingObjectId=c.o and c.t in(1090,10027,10000,1010,4406,10063,10014,3,1088,2,4400,10059,1,1084,112,10058,4402,10025,4) end
if(exists(select * from @t where t in(10008,10009,4251,1090,4204,10027,4202,10000,4206,9605,1010,10063,4300,10019,3,9606,4703,4401,4207,4209,9300,4201,9750,1088,4216,4208,2,4400,4414,8181,9700,9507,9600,4211,4212,10059,1,1084,112,10058,4402,4214,10025,4,4210)))begin insert into @t(o,t,u,v,q,s,l) select o.AnnotationId,5,o.OwnerId, o.OwnerIdType, c.o, c.t, c.l +1 from Annotation o,@t c where o.ObjectId=c.o and c.t in(10008,10009,4251,1090,4204,10027,4202,10000,4206,9605,1010,10063,4300,10019,3,9606,4703,4401,4207,4209,9300,4201,9750,1088,4216,4208,2,4400,4414,8181,9700,9507,9600,4211,4212,10059,1,1084,112,10058,4402,4214,10025,4,4210) end
return
end
