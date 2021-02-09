

create function dbo.fn_EndOfNextWeek ( 
  @TodayUTC         datetime
)
returns datetime
as
begin
  return dbo.fn_BeginOfWeek(dateadd(dd, 14, @TodayUTC))
end
