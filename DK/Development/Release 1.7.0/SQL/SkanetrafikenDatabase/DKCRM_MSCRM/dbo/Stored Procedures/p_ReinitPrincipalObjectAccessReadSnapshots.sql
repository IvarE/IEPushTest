

CREATE     procedure [dbo].[p_ReinitPrincipalObjectAccessReadSnapshots](@minimumCount int) as
begin
	SET NOCOUNT ON

	-- Create a temp table to store the new data in - we'll then copy the new data over to the existing table.  We
	-- do it in steps so as to avoid dropping all of the data in this table, which leaves a window where we could
	-- end up getting no data for the cache.
	create table #POASnapshotData(PrincipalId uniqueidentifier, ObjectTypeCode int not null, Count bigint not null)
	CREATE CLUSTERED INDEX ndx_POASnapshotData ON #POASnapshotData(PrincipalId, ObjectTypeCode)

	-- first insert data for non-team principals
	insert into #POASnapshotData(PrincipalId, ObjectTypeCode, Count)
	select PrincipalId, ObjectTypeCode, COUNT(PrincipalObjectAccessId)
	from PrincipalObjectAccess POA with (nolock)
	where ((POA.AccessRightsMask|POA.InheritedAccessRightsMask) & 1) = 1
	and POA.PrincipalTypeCode <> 9	-- skip team
	group by PrincipalId, ObjectTypeCode
	having COUNT(PrincipalObjectAccessId) > @minimumCount

	-- insert *user* data corresponding to their team principals
	MERGE #POASnapshotData AS Target
	USING
	(select s.SystemUserId, p.ObjectTypeCode, COUNT(p.ObjectId) AS TeamShareCount from
		PrincipalObjectAccess p	 WITH (nolock)	
		inner join SystemUserPrincipals s WITH (nolock)
		on s.PrincipalId = p.PrincipalId
		where p.PrincipalTypeCode = 9
		AND ((p.AccessRightsMask|p.InheritedAccessRightsMask) & 1) = 1
		group by s.SystemUserId, p.ObjectTypeCode
		having COUNT(p.ObjectId) > @minimumCount
		)  AS Source
	ON Source.SystemUserId = Target.PrincipalId AND Source.ObjectTypeCode = Target.ObjectTypeCode
	WHEN MATCHED THEN
		UPDATE SET Target.Count = Source.TeamShareCount + Target.Count
	WHEN NOT MATCHED BY TARGET THEN
		INSERT (PrincipalId, ObjectTypeCode, Count) VALUES 
		(Source.SystemUserId, Source.ObjectTypeCode, Source.TeamShareCount);

	-- Delete anything that's not in #POASnapshotData
	delete from PrincipalObjectAccessReadSnapshot where PrincipalObjectAccessReadSnapshotId in
	(select PrincipalObjectAccessReadSnapshotId from PrincipalObjectAccessReadSnapshot poars
	left join #POASnapshotData psd on poars.PrincipalId = psd.PrincipalId and poars.ObjectTypeCode = psd.ObjectTypeCode
	where psd.ObjectTypeCode is null)

	-- Update rows that are in both
	update poars set poars.Count = psd.Count
	from PrincipalObjectAccessReadSnapshot poars
	join #POASnapshotData psd on poars.PrincipalId = psd.PrincipalId and poars.ObjectTypeCode = psd.ObjectTypeCode

	-- Insert any new rows
	insert into PrincipalObjectAccessReadSnapshot (PrincipalObjectAccessReadSnapshotId, PrincipalId, Count, ObjectTypeCode)
	select NEWID(), psd.PrincipalId, psd.Count, psd.ObjectTypeCode
	from #POASnapshotData psd
	left join PrincipalObjectAccessReadSnapshot poars on poars.PrincipalId = psd.PrincipalId and poars.ObjectTypeCode = psd.ObjectTypeCode
	where poars.PrincipalId is null

	drop table #POASnapshotData
end