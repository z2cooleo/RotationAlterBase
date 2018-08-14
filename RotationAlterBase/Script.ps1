##################################################################################################
# Скрипт резервного копирования БД PostgreSQL
##################################################################################################
#set-item -force -path env:FGST_CURRENT_DB_ID -value "b47a8176-c638-46dc-873c-a0eda60368c7"
#set-item -force -path env:FGST_CURRENT_DB_TYPE -value "pgsql"
#set-item -force -path env:FGST_CURRENT_DB_CONNECTION_STRING -value "port=5432 host=localhost user=postgres password=Aa147963 dbname=2"
#set-item -force -path env:FGST_PREVIOUS_DB_ID -value "d6ba7603-1340-4153-adb3-163d4025777f"
#set-item -force -path env:FGST_PREVIOUS_DB_TYPE -value "pgsql"
#set-item -force -path env:FGST_PREVIOUS_DB_CONNECTION_STRING -value "port=5432 host=localhost user=postgres password=Aa147963 dbname=1"
# при значение auto - определяется автоматически
$fastTableSpace = "auto" # Имя Таблспэйса на быстром носителе
$slowTableSpace = "auto" # Имя таблспэ	с на медленной хранилке
$fastTableSpacePath = "auto" # Путь для хранения быстрого индекса
$slowTableSpacePath = "auto" # Путь для хранения индекса, на медленном насителе.
$maintenanceDB = "postgres" # БД для обслуживания
#------------------------------------------------------------------------------------
$fgStConfigXML = "C:\ProgramData\Falcongaze SecureTower\FgStDPConfig.xml"
$fgStConfigYaml = "C:\ProgramData\Falcongaze SecureTower\SecureTowerServer\conf\config.yaml"
$logPath = "C:\ProgramData\Falcongaze SecureTower\logs\"
$app = "C:\ProgramData\Falcongaze SecureTower\RotationWorker\RotationAlterBase.exe"

################################################################################## Переменные устанавливаемые автоматически

Get-Childitem env:
Get-Childitem env:computername
$currdbType = [Environment]::GetEnvironmentVariable("FGST_CURRENT_DB_TYPE");
$currDbID = [Environment]::GetEnvironmentVariable("FGST_CURRENT_DB_ID");
$currDbConnString = [Environment]::GetEnvironmentVariable("FGST_CURRENT_DB_CONNECTION_STRING");
$currDbConnString = $currDbConnString.Replace("port", "cPort");
$currDbConnString = $currDbConnString.Replace("host", "cHost");
$currDbConnString = $currDbConnString.Replace("user", "cUser");
$currDbConnString = $currDbConnString.Replace("password", "cPassword");
$currDbConnString = $currDbConnString.Replace("dbname", "cDbName");
$prevDbType = [Environment]::GetEnvironmentVariable("FGST_PREVIOUS_DB_TYPE");
$prevDbID = [Environment]::GetEnvironmentVariable("FGST_PREVIOUS_DB_ID");
$prevDbConnString = [Environment]::GetEnvironmentVariable("FGST_PREVIOUS_DB_CONNECTION_STRING");



############################################################################


	$args = "logPath="+$logPath.Replace(' ','?')
	$args = $args + " currDbID="+$currDbID.Replace(' ','?')
	$args = $args + " currdbType="+$currdbType.Replace(' ','?')
	$args = $args + " "+$currDbConnString
	$args = $args + " prevDbID="+$prevDbID.Replace(' ','?')
	$args = $args + " prevDbType="+$prevDbType.Replace(' ','?')
	$args = $args + " fgStConfigXML="+$fgStConfigXML.Replace(' ','?')
	$args = $args + " fgStConfigYaml="+$fgStConfigYaml.Replace(' ','?')
	$args = $args + " fastTableSpacePath="+$fastTableSpacePath.Replace(' ','?')
	$args = $args + " slowTableSpacePath="+$slowTableSpacePath.Replace(' ','?')
	$args = $args + " postgresPass="+$postgresPass.Replace(' ','?')
	$args = $args + " " +$prevDbConnString;
	$args = $args + " fastTableSpace="+$fastTableSpace.Replace(' ','?')
	$args = $args + " slowTableSpace="+$slowTableSpace.Replace(' ','?')
	$args = $args + " maintenanceDB"+$maintenanceDB
	#$args | Out-File $logPath"1.txt"

	Start-Process -FilePath $app -ArgumentList $args -Verb runAs -WindowStyle Normal -Wait;
	exit
	
	

