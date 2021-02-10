

create function fn_GetGuidsFromString
(
    @idsString nvarchar(max) 
)
returns @tbl table
( 	
     id uniqueidentifier primary key clustered
) 
as
begin

	-- Expected list string for Guids fixed length 
	-- No separator for: speed of parsing and reduce of network data
	-- Example for Guids with value 1 and 3: 
	--    N'00000000-0000-0000-0000-00000000000100000000-0000-0000-0000-000000000003'
	declare @countGuids int
	declare @guidLength int
	declare @currentId int
	declare @currentGuid uniqueidentifier

	select @guidLength = 36	
	select @countGuids = len(@idsString) / @guidLength
	select @currentId = 0
			
	while (@currentId < @countGuids)
	begin
		select @currentGuid = cast(substring(@idsString, @currentId * @guidLength + 1, @guidLength) as uniqueidentifier ) 
		insert into @tbl Values(@currentGuid);
		select @currentId = @currentId +1
	end

return  
end