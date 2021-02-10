

create function dbo.fn_UTCToLocalTime(	@UTCTime  datetime )
returns datetime
as 
begin
	declare @timezonecode 	int
	declare @ResultDateTime datetime

	select top 1 @timezonecode = us.TimeZoneCode
	from UserSettingsBase as us WHERE us.SystemUserId = dbo.fn_FindUserGuid()

	select @ResultDateTime = dbo.fn_UTCToTzCodeSpecificLocalTime(@UTCTime,
						@timezonecode)

	return @ResultDateTime

end