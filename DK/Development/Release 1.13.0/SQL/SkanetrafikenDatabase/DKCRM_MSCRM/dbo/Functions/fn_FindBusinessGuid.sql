

create function dbo.fn_FindBusinessGuid ()
returns uniqueidentifier
as
begin

	declare @userGuid uniqueidentifier
	declare @businessguid uniqueidentifier

	--- test whether the query is runing by priviledged user with user role of CRMReaderRole
	--- if it is dbo, we trust it as well. 
	--- There is an issue in SQL. If the user is a dbo, if it not member of any role
	if (is_member('CRMReaderRole') | is_member('db_owner')) = 1
	begin
		select @userGuid = cast(context_info() as uniqueidentifier)

		if @userGuid is not null
		begin		
			select @businessguid = s.BusinessUnitId
				from SystemUserBase s
					where s.SystemUserId = @userGuid
			return @businessguid
		end
	end

	select @businessguid = s.BusinessUnitId
	from SystemUserBase s
	where s.DomainName = SUSER_SNAME()
	return @businessguid
end


