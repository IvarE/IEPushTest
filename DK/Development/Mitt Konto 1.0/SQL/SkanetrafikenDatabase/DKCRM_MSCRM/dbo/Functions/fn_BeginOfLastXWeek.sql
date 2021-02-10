

create function dbo.fn_BeginOfLastXWeek ( 
  @TodayUTC     datetime,
  @X		int
)
returns datetime
as
begin
  return dbo.fn_BeginOfDay(dateadd(dd, -7*@X, @TodayUTC))
end
