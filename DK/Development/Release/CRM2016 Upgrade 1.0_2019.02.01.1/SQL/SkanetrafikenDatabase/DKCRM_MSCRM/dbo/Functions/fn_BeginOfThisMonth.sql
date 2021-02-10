

create function dbo.fn_BeginOfThisMonth ( 
  @TodayUTC         datetime
)
returns datetime
as
begin
  return dbo.fn_BeginOfMonth(@TodayUTC)
end
