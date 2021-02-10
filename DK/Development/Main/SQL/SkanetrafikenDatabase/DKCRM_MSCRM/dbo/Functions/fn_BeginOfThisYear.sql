

create function dbo.fn_BeginOfThisYear ( 
  @TodayUTC         datetime
)
returns datetime
as
begin
  return dbo.fn_BeginOfYear(@TodayUTC)
end
