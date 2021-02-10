

create function dbo.fn_BeginOfToday ( 
  @TodayUTC         datetime
)
returns datetime
as
begin
  return dbo.fn_BeginOfDay(@TodayUTC)
end
