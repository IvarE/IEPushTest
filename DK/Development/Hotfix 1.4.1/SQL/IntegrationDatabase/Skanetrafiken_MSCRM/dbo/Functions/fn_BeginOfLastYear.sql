

create function dbo.fn_BeginOfLastYear ( 
  @TodayUTC         datetime
)
returns datetime
as
begin
  return dbo.fn_BeginOfYear(dateadd(yy, -1, @TodayUTC))
end
