

/*
This proc is used to update root site collection of given SharePoint record and its children
A valid SharePoint record can be from one of the following entities:
SharePointSite
SharePointDocumentLocation
*/
CREATE procedure [dbo].[p_UpdateRootSite](
 @objectId uniqueidentifier ,@rootSiteId uniqueidentifier) 
as
begin

--- Constants defining objecttype codes of entities.
declare @sharePointSite int
declare @sharePointDocumentLocation int
select @sharePointSite = 9502
select @sharePointDocumentLocation = 9508
declare @objectTypeCode int
declare @rowNum int
declare @count int

set @rowNum = 1
-- Variables
declare @childId uniqueidentifier
declare @Results table (childId uniqueidentifier not null, objectTypeCode int, processed int identity)

-- Set SiteCollectionID for object Id
update SharePointSite set SiteCollectionId = @rootSiteId where SharePointSiteId = @objectId and SharePointSiteId <> @rootSiteId
update SharePointDocumentLocation set SiteCollectionId = @rootSiteId where SharePointDocumentLocationId = @objectId 

insert into @Results(childId , objectTypeCode) select SharePointSiteId, @sharePointSite from SharePointSite where ParentSite = @objectId and IsNull(IsGridPresent, 0) <> 1
insert into @Results(childId , objectTypeCode) select SharePointDocumentLocationId, @sharePointDocumentLocation from SharePointDocumentLocation where ParentSiteOrLocation = @objectId

select @count = COUNT(*) from @Results;

while(@rowNum <= @count)
begin	
	select @childId = childId , @objectTypeCode = objectTypeCode from @Results where processed = @rowNum
	if(@objectTypeCode = @sharePointSite)
	begin
--		Set SiteCollectionID for child
		exec p_UpdateRootSite @childId, @rootSiteId
	end
	else if(@objectTypeCode = @sharePointDocumentLocation)
	begin
--		Set SiteCollectionID for child
		exec p_UpdateRootSite @childId, @rootSiteId	
	end
	
	set @rowNum = @rowNum + 1
end
end