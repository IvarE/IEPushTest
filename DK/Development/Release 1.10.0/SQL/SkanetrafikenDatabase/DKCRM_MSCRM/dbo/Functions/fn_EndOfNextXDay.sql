

create function dbo.fn_EndOfNextXDay ( 
  @TodayUTC     datetime,
  @X		int
)
returns datetime
as
begin
  return dbo.fn_BeginOfDay(dateadd(dd, @X+1, @TodayUTC))
end
