

create function dbo.fn_BeginOfLastXDay ( 
  @TodayUTC     datetime,
  @X		int
)
returns datetime
as
begin
  return dbo.fn_BeginOfDay(dateadd(dd, -@X, @TodayUTC))
end
