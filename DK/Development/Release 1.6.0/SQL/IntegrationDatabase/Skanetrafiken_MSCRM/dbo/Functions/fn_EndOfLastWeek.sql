

create function dbo.fn_EndOfLastWeek ( 
  @TodayUTC         datetime
)
returns datetime
as
begin
  return dbo.fn_BeginOfWeek(@TodayUTC)
end
