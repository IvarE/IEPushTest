param (
	[string]
$PathSuffix

)

$BasePath = "D:\Crm\"


	New-Item -ItemType Directory -Path @BasePath + "CGIXrmServices" + $PathSuffix +"\CGIXrmCreateCaseService" -Force
	New-Item -ItemType Directory -Path @BasePath + "CGIXrmServices" + $PathSuffix +"\CGIXrmTravelcardService" -Force
	New-Item -ItemType Directory -Path @BasePath + "CGIXrmServices" + $PathSuffix +"\CGIXrmGetOrdersService" -Force
	
	New-Item -ItemType Directory -Path @BasePath + "CrmEAIConnectorService" + $PathSuffix  -Force
	New-Item -ItemType Directory -Path @BasePath + "CrmExtConnectorService" + $PathSuffix  -Force
	New-Item -ItemType Directory -Path @BasePath + "CrmPortalService" + $PathSuffix  -Force
				
