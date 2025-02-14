/****** Script for SelectTopNRows command from SSMS  ******/
SELECT TOP (1000) [Last_Name]
      ,[First_Name]
      ,[Mkl_Id]
      ,[Social_Security_Number]
      ,[Modified_On]
      ,[Created_On]
      ,[Email]
      ,[Information_Source]
      ,[Contact]
      ,TRIM(STR([Social_Security_Number], 25, 0))
  FROM [SkaneDuplicates].[dbo].[Contact-20190628.2]
  where 199811170654 = [Social_Security_Number]

  
  select [Social_Security_Number], MAX([First_Name]), COUNT(*), max(a.[Created_On])
  , (Select b.[Contact] 
	 from [SkaneDuplicates].[dbo].[Contact-20190628.2] as b
	 where a.[Social_Security_Number] = b.[Social_Security_Number]
	   and b.[Created_On] = max(a.[Created_On])
	    ) as 'from.ContactGuid'
  , (Select b.[Contact] 
	 from [SkaneDuplicates].[dbo].[Contact-20190628.2] as b
	 where a.[Social_Security_Number] = b.[Social_Security_Number]
	   and b.[Created_On] <> max(a.[Created_On])
	    ) as 'to.ContactGuid'

    from [SkaneDuplicates].[dbo].[Contact-20190628.2] as a
  where LEN( TRIM(STR([Social_Security_Number], 25, 0)) ) > 10
  --and [Social_Security_Number] NOT IN ('193110152836', '198812183567', '197504181798', '197207280269')
  --and 199811170654 = [Social_Security_Number]
  group by [Social_Security_Number]
  having count(*) = 2
  order by 1

