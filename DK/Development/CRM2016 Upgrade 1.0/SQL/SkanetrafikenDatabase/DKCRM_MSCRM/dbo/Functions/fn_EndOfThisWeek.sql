

create function dbo.fn_EndOfThisWeek ( 
  @TodayUTC         datetime
)
returns datetime
as
begin
  return dbo.fn_BeginOfWeek(dateadd(dd, 7 , @TodayUTC))
end
