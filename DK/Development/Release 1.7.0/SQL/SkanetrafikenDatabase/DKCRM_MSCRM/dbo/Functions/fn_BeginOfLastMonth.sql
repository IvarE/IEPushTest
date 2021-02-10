

create function dbo.fn_BeginOfLastMonth ( 
  @TodayUTC         datetime
)
returns datetime
as
begin
  return dbo.fn_BeginOfMonth(dateadd(mm, -1, @TodayUTC))
end
