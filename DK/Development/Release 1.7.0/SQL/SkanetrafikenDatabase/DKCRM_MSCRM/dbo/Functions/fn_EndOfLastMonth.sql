

create function dbo.fn_EndOfLastMonth ( 
  @TodayUTC         datetime
)
returns datetime
as
begin
  return dbo.fn_BeginOfMonth(@TodayUTC)
end
