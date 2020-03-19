

create function dbo.fn_EndOfNextXWeek ( 
  @TodayUTC     datetime,
  @X		int
)
returns datetime
as
begin
  return dbo.fn_BeginOfDay(dateadd(dd, 7*@X+1, @TodayUTC))
end
