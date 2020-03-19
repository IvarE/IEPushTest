


create function dbo.fn_EndOfYesterday(
  @TodayUTC	datetime
)
returns datetime
as
begin
  return dbo.fn_BeginOfDay(@TodayUTC)
end
