

create function dbo.fn_EndOfNextMonth ( 
  @TodayUTC         datetime
)
returns datetime
as
begin
  return dbo.fn_BeginOfMonth(dateadd(mm, 2, @TodayUTC))
end
