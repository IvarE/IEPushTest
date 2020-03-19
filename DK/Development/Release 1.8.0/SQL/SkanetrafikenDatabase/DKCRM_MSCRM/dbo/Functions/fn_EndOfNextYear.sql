

create function dbo.fn_EndOfNextYear ( 
  @TodayUTC         datetime
)
returns datetime
as
begin
  return dbo.fn_BeginOfYear(dateadd(yy, 2, @TodayUTC))
end
