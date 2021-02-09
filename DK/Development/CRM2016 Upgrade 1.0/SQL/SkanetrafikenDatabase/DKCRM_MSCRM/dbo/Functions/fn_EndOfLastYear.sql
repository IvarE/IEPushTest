

create function dbo.fn_EndOfLastYear ( 
  @TodayUTC         datetime
)
returns datetime
as
begin
  return dbo.fn_BeginOfYear(@TodayUTC)
end
