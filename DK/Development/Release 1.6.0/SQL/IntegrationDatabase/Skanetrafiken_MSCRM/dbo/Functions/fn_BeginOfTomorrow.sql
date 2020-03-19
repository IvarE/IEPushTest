


create function dbo.fn_BeginOfTomorrow(
  @TodayUTC	datetime
)
returns datetime
as
begin
  return dbo.fn_EndOfToday(@TodayUTC)
end
