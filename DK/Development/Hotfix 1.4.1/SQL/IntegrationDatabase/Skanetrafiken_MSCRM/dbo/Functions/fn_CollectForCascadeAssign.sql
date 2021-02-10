CREATE function [dbo].[fn_CollectForCascadeAssign]
(
  @root_id uniqueidentifier,
  @root_otc int,
  @isoffline int,
  @old_owner uniqueidentifier
)
returns @t table
( 	
   o uniqueidentifier,
   t int,
   u uniqueidentifier,
   q uniqueidentifier,
   s int,
   y bit,
   l int
) 
as
begin
declare @l int
insert into @t values(@root_id,@root_otc,@old_owner,N'00000000-0000-0000-0000-000000000000',0,0,0)
set @l=0
set @l=@l+1 if(exists(select * from @t where t=9100))begin insert into @t(o,t,u,q,s,y,l) select o.ReportId,9100,o.OwnerId,c.o,c.t,1,@l from Report o,@t c where o.ParentReportId=c.o and c.t=9100
while(@@rowcount <> 0) begin set @l=@l+1 if(exists(select * from @t where t=9100))insert into @t(o,t,u,q,s,y,l) select o.ReportId,9100,o.OwnerId,c.o,c.t,1,@l from Report o,@t c where o.ParentReportId=c.o and c.t=9100 and o.ReportId not in(select o from @t where t=9100) end end
set @l=@l+1 if(exists(select * from @t where t=2020))begin insert into @t(o,t,u,q,s,y,l) select o.MailboxId,9606,o.OwnerId,c.o,c.t,1,@l from Mailbox o,@t c where o.RegardingObjectId=c.o and c.t=2020 end
set @l=@l+1 if(exists(select * from @t where t=4200))begin insert into @t(o,t,u,q,s,y,l) select o.RuleId,4250,o.OwnerId,c.o,c.t,1,@l from RecurrenceRule o,@t c where o.ObjectId=c.o and c.t=4200 end
set @l=@l+1 if(exists(select * from @t where t in(4400,4406)))begin insert into @t(o,t,u,q,s,y,l) select o.ActivityId,4401,o.OwnerId,c.o,c.t,1,@l from CampaignResponse o,@t c where o.RegardingObjectId=c.o and c.t in(4400,4406) end
set @l=@l+1 if(exists(select * from @t where t=4400))begin insert into @t(o,t,u,q,s,y,l) select o.ActivityId,4402,o.OwnerId,c.o,c.t,1,@l from CampaignActivity o,@t c where o.RegardingObjectId=c.o and c.t=4400 end
set @l=@l+1 if(exists(select * from @t where t=1))begin insert into @t(o,t,u,q,s,y,l) select o.AccountId,1,o.OwnerId,c.o,c.t,1,@l from Account o,@t c where o.ParentAccountId=c.o and c.t=1
while(@@rowcount <> 0) begin set @l=@l+1 if(exists(select * from @t where t=1))insert into @t(o,t,u,q,s,y,l) select o.AccountId,1,o.OwnerId,c.o,c.t,1,@l from Account o,@t c where o.ParentAccountId=c.o and c.t=1 and o.AccountId not in(select o from @t where t=1) end end
set @l=@l+1 if(exists(select * from @t where t in(2,1)))begin insert into @t(o,t,u,q,s,y,l) select o.ContactId,2,o.OwnerId,c.o,c.t,1,@l from Contact o,@t c where o.ParentCustomerId=c.o and c.t in(2,1)
while(@@rowcount <> 0) begin set @l=@l+1 if(exists(select * from @t where t=2))insert into @t(o,t,u,q,s,y,l) select o.ContactId,2,o.OwnerId,c.o,c.t,1,@l from Contact o,@t c where o.ParentCustomerId=c.o and c.t=2 and o.ContactId not in(select o from @t where t=2) end end
set @l=@l+1 if(exists(select * from @t where t in(2,1)))begin insert into @t(o,t,u,q,s,y,l) select o.SocialProfileId,99,o.OwnerId,c.o,c.t,1,@l from SocialProfile o,@t c where o.CustomerId=c.o and c.t in(2,1) end
set @l=@l+1 if(exists(select * from @t where t in(2,1)))begin insert into @t(o,t,u,q,s,y,l) select o.OpportunityId,3,o.OwnerId,c.o,c.t,1,@l from Opportunity o,@t c where o.CustomerId=c.o and c.t in(2,1) end
set @l=@l+1 if(exists(select * from @t where t=3))begin insert into @t(o,t,u,q,s,y,l) select o.SharePointDocumentId,9507,o.OwnerId,c.o,c.t,1,@l from SharePointDocument o,@t c where o.RegardingObjectId=c.o and c.t=3 end
set @l=@l+1 if(exists(select * from @t where t=3))begin insert into @t(o,t,u,q,s,y,l) select o.SharePointDocumentLocationId,9508,o.OwnerId,c.o,c.t,1,@l from SharePointDocumentLocation o,@t c where o.RegardingObjectId=c.o and c.t=3 end
set @l=@l+1 if(exists(select * from @t where t=3))begin insert into @t(o,t,u,q,s,y,l) select o.ProcessSessionId,4710,o.OwnerId,c.o,c.t,1,@l from ProcessSession o,@t c where o.RegardingObjectId=c.o and c.t=3 end
set @l=@l+1 if(exists(select * from @t where t in(2,1)))begin insert into @t(o,t,u,q,s,y,l) select o.LeadId,4,o.OwnerId,c.o,c.t,1,@l from Lead o,@t c where o.CustomerId=c.o and c.t in(2,1) end
set @l=@l+1 if(exists(select * from @t where t=3))begin insert into @t(o,t,u,q,s,y,l) select o.LeadId,4,o.OwnerId,c.o,c.t,1,@l from Lead o,@t c where o.QualifyingOpportunityId=c.o and c.t=3 and o.LeadId not in(select o from @t where t=4) end
set @l=@l+1 if(exists(select * from @t where t=3))begin insert into @t(o,t,u,q,s,y,l) select o.CustomerOpportunityRoleId,4503,o.OwnerId,c.o,c.t,1,@l from CustomerOpportunityRole o,@t c where o.OpportunityId=c.o and c.t=3 end
set @l=@l+1 if(exists(select * from @t where t in(1,2)))begin insert into @t(o,t,u,q,s,y,l) select o.QuoteId,1084,o.OwnerId,c.o,c.t,1,@l from Quote o,@t c where o.CustomerId=c.o and c.t in(1,2) end
set @l=@l+1 if(exists(select * from @t where t=3))begin insert into @t(o,t,u,q,s,y,l) select o.QuoteId,1084,o.OwnerId,c.o,c.t,1,@l from Quote o,@t c where o.OpportunityId=c.o and c.t=3 and o.QuoteId not in(select o from @t where t=1084) end
set @l=@l+1 if(exists(select * from @t where t=1084))begin insert into @t(o,t,u,q,s,y,l) select o.ActivityId,4211,o.OwnerId,c.o,c.t,1,@l from QuoteClose o,@t c where o.QuoteId=c.o and c.t=1084 end
set @l=@l+1 if(exists(select * from @t where t=3))begin insert into @t(o,t,u,q,s,y,l) select o.ActivityId,4208,o.OwnerId,c.o,c.t,1,@l from OpportunityClose o,@t c where o.OpportunityId=c.o and c.t=3 end
set @l=@l+1 if(exists(select * from @t where t in(2,1)))begin insert into @t(o,t,u,q,s,y,l) select o.ContractId,1010,o.OwnerId,c.o,c.t,1,@l from Contract o,@t c where o.CustomerId=c.o and c.t in(2,1) end
set @l=@l+1 if(exists(select * from @t where t in(2,1)))begin insert into @t(o,t,u,q,s,y,l) select o.InvoiceId,1090,o.OwnerId,c.o,c.t,1,@l from Invoice o,@t c where o.CustomerId=c.o and c.t in(2,1) end
set @l=@l+1 if(exists(select * from @t where t in(2,1)))begin insert into @t(o,t,u,q,s,y,l) select o.SalesOrderId,1088,o.OwnerId,c.o,c.t,1,@l from SalesOrder o,@t c where o.CustomerId=c.o and c.t in(2,1) end
set @l=@l+1 if(exists(select * from @t where t=3))begin insert into @t(o,t,u,q,s,y,l) select o.SalesOrderId,1088,o.OwnerId,c.o,c.t,1,@l from SalesOrder o,@t c where o.OpportunityId=c.o and c.t=3 and o.SalesOrderId not in(select o from @t where t=1088) end
set @l=@l+1 if(exists(select * from @t where t=1088))begin insert into @t(o,t,u,q,s,y,l) select o.ActivityId,4209,o.OwnerId,c.o,c.t,1,@l from OrderClose o,@t c where o.SalesOrderId=c.o and c.t=1088 end
set @l=@l+1 if(exists(select * from @t where t in(1,2)))begin insert into @t(o,t,u,q,s,y,l) select o.IncidentId,112,o.OwnerId,c.o,c.t,1,@l from Incident o,@t c where o.CustomerId=c.o and c.t in(1,2) end
set @l=@l+1 if(exists(select * from @t where t in(10000,4406,4,10058,4700,1010,1,1084,10014,1090,112,10027,10059,10063,9700,1088,2,10025,3)))begin insert into @t(o,t,u,q,s,y,l) select o.ActivityId,4216,o.OwnerId,c.o,c.t,1,@l from SocialActivity o,@t c where o.RegardingObjectId=c.o and c.t in(10000,4406,4,10058,4700,1010,1,1084,10014,1090,112,10027,10059,10063,9700,1088,2,10025,3) end
set @l=@l+1 if(exists(select * from @t where t=112))begin insert into @t(o,t,u,q,s,y,l) select o.cgi_filelinkId,10055,o.OwnerId,c.o,c.t,1,@l from cgi_filelink o,@t c where o.cgi_IncidentId=c.o and c.t=112 end
set @l=@l+1 if(exists(select * from @t where t=112))begin insert into @t(o,t,u,q,s,y,l) select o.cgi_passtravelinformationId,10037,o.OwnerId,c.o,c.t,1,@l from cgi_passtravelinformation o,@t c where o.cgi_IncidentId=c.o and c.t=112 end
set @l=@l+1 if(exists(select * from @t where t in(4402,4406,112,4400,10059,1010,1084,2,10000,10025,1088,10058,3,10063,10027,4,9700,10014,1,1090)))begin insert into @t(o,t,u,q,s,y,l) select o.ActivityId,4210,o.OwnerId,c.o,c.t,1,@l from PhoneCall o,@t c where o.RegardingObjectId=c.o and c.t in(4402,4406,112,4400,10059,1010,1084,2,10000,10025,1088,10058,3,10063,10027,4,9700,10014,1,1090) end
set @l=@l+1 if(exists(select * from @t where t in(10014,1088,4,3,2,4400,1,10027,10063,10025,112,10058,9700,10000,1084,1090,10059,1010)))begin insert into @t(o,t,u,q,s,y,l) select o.ActivityId,4214,o.OwnerId,c.o,c.t,1,@l from ServiceAppointment o,@t c where o.RegardingObjectId=c.o and c.t in(10014,1088,4,3,2,4400,1,10027,10063,10025,112,10058,9700,10000,1084,1090,10059,1010) end
set @l=@l+1 if(exists(select * from @t where t in(10000,112,9700,1090,3,1010,10059,10025,10063,10027,2,4,4400,10058,10014,1088,4402,1,1084)))begin insert into @t(o,t,u,q,s,y,l) select o.ActivityId,4212,o.OwnerId,c.o,c.t,1,@l from Task o,@t c where o.RegardingObjectId=c.o and c.t in(10000,112,9700,1090,3,1010,10059,10025,10063,10027,2,4,4400,10058,10014,1088,4402,1,1084) end
set @l=@l+1 if(exists(select * from @t where t in(10059,1010,4406,1088,4400,4402,10025,3,10014,1,10027,2,1084,1090,4,9700,10058,10063,112,10000)))begin insert into @t(o,t,u,q,s,y,l) select o.ActivityId,4207,o.OwnerId,c.o,c.t,1,@l from Letter o,@t c where o.RegardingObjectId=c.o and c.t in(10059,1010,4406,1088,4400,4402,10025,3,10014,1,10027,2,1084,1090,4,9700,10058,10063,112,10000) end
set @l=@l+1 if(exists(select * from @t where t=112))begin insert into @t(o,t,u,q,s,y,l) select o.ActivityId,4206,o.OwnerId,c.o,c.t,1,@l from IncidentResolution o,@t c where o.IncidentId=c.o and c.t=112 end
set @l=@l+1 if(exists(select * from @t where t in(10063,4,4402,10027,112,1,3,1084,10000,10058,1090,10014,1010,4406,1088,10025,4700,4400,10059,9700,2)))begin insert into @t(o,t,u,q,s,y,l) select o.ActivityId,4202,o.OwnerId,c.o,c.t,1,@l from Email o,@t c where o.RegardingObjectId=c.o and c.t in(10063,4,4402,10027,112,1,3,1084,10000,10058,1090,10014,1010,4406,1088,10025,4700,4400,10059,9700,2) end
set @l=@l+1 if(exists(select * from @t where t in(10059,1084,2,10000,4400,1090,3,10027,1,112,4,1088,10025,4402,9700,1010,10014,10063,4406,10058)))begin insert into @t(o,t,u,q,s,y,l) select o.ActivityId,4204,o.OwnerId,c.o,c.t,1,@l from Fax o,@t c where o.RegardingObjectId=c.o and c.t in(10059,1084,2,10000,4400,1090,3,10027,1,112,4,1088,10025,4402,9700,1010,10014,10063,4406,10058) end
set @l=@l+1 if(exists(select * from @t where t in(10058,1090,112,1088,1084,10027,10025,10063,10059,3,1010,9700,4402,4400,10014,2,1,4,10000,4406)))begin insert into @t(o,t,u,q,s,y,l) select o.ActivityId,4251,o.OwnerId,c.o,c.t,1,@l from RecurringAppointmentMaster o,@t c where o.RegardingObjectId=c.o and c.t in(10058,1090,112,1088,1084,10027,10025,10063,10059,3,1010,9700,4402,4400,10014,2,1,4,10000,4406) end
set @l=@l+1 if(exists(select * from @t where t in(1084,4400,10063,10027,10058,4,10000,4402,10025,3,1,112,10014,9700,4406,1090,10059,1010,1088,2)))begin insert into @t(o,t,u,q,s,y,l) select o.ActivityId,4201,o.OwnerId,c.o,c.t,1,@l from Appointment o,@t c where o.RegardingObjectId=c.o and c.t in(1084,4400,10063,10027,10058,4,10000,4402,10025,3,1,112,10014,9700,4406,1090,10059,1010,1088,2) end
set @l=@l+1 if(exists(select * from @t where t=4251))begin insert into @t(o,t,u,q,s,y,l) select o.ActivityId,4201,o.OwnerId,c.o,c.t,1,@l from Appointment o,@t c where o.SeriesId=c.o and c.t=4251 and o.ActivityId not in(select o from @t where t=4201) end
set @l=@l+1 if(exists(select * from @t where t in(112,1088,1090,1010,1,2,10058,4402,10027,10014,10059,10025,10063,10000,4400,4,4406,1084,3)))begin insert into @t(o,t,u,q,s,y,l) select o.ActivityId,10009,o.OwnerId,c.o,c.t,1,@l from cgi_callguidefacebook o,@t c where o.RegardingObjectId=c.o and c.t in(112,1088,1090,1010,1,2,10058,4402,10027,10014,10059,10025,10063,10000,4400,4,4406,1084,3) end
set @l=@l+1 if(exists(select * from @t where t in(112,1088,1090,1010,1,2,10025,4402,10058,10027,10014,10059,10063,10000,4400,4,4406,1084,3)))begin insert into @t(o,t,u,q,s,y,l) select o.ActivityId,10008,o.OwnerId,c.o,c.t,1,@l from cgi_callguidechat o,@t c where o.RegardingObjectId=c.o and c.t in(112,1088,1090,1010,1,2,10025,4402,10058,10027,10014,10059,10063,10000,4400,4,4406,1084,3) end
set @l=@l+1 if(exists(select * from @t where t in(9300,4204,10000,112,4207,9605,10063,4703,4,10008,4251,10027,1010,4202,10009,4210,4201,9507,1088,9750,4209,10019,1084,4414,4300,10058,4401,4402,4206,10059,4216,1,4211,8181,4208,4214,4212,4400,2,10025,9606,3,9600,1090,9700)))begin insert into @t(o,t,u,q,s,y,l) select o.AnnotationId,5,o.OwnerId,c.o,c.t,1,@l from Annotation o,@t c where o.ObjectId=c.o and c.t in(9300,4204,10000,112,4207,9605,10063,4703,4,10008,4251,10027,1010,4202,10009,4210,4201,9507,1088,9750,4209,10019,1084,4414,4300,10058,4401,4402,4206,10059,4216,1,4211,8181,4208,4214,4212,4400,2,10025,9606,3,9600,1090,9700) end
return
end
