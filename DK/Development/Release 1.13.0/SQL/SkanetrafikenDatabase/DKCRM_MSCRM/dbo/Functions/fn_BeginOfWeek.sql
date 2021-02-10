

create function dbo.fn_BeginOfWeek ( 
  @DayUTC         datetime
)
returns datetime
as
begin
  declare @DayUserLocal datetime
  declare @WeekStartDay int
  declare @DayDiffFromStart int 
  declare @BDayUTC datetime
  set @DayUserLocal = dbo.fn_UTCToLocalTime(@DayUTC)

  --- select the first day of the week from orgnization setting table
  select @WeekStartDay= og.WeekStartDayCode
	from OrganizationBase og, SystemUserBase su
	where su.SystemUserId = dbo.fn_FindUserGuid()
		and og.OrganizationId = su.OrganizationId

  --- calculate the first day diff from the first day
  set @DayDiffFromStart = (7 + ( dbo.fn_NTDayOfWeek(@DayUserLocal) - @WeekStartDay)) %7
 
  --- calculate the first day of the week
  set @BDayUTC = dbo.fn_BeginOfDay( dateadd(dd, 0 - @DayDiffFromStart, @DayUTC))
  return @BDayUTC
end
