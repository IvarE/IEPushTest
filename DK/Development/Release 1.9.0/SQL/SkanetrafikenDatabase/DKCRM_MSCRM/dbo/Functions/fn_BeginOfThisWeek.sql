

create function dbo.fn_BeginOfThisWeek ( 
  @TodayUTC	datetime
)
returns datetime
as
begin
  return dbo.fn_BeginOfWeek(@TodayUTC)
end
