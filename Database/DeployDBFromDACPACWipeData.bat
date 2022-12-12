Title Publish Main Database from DACPAC
@ECHO OFF

REM *** Prepare parameters for executing ***
set deployenv="DEV"
set SQLServerSolutionRootUrl="C:\crawl-project\Database\DatabaseDeployment"
set MSBuild="C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe"
set SQLServerSolution="%SQLServerSolutionRootUrl%\DatabaseDeployment.sln"
set CoreData_DACPAC="%SQLServerSolutionRootUrl%\CoreData\bin\Local\CoreData.dacpac"

:: Reference to url for more information https://docs.microsoft.com/en-us/sql/tools/sqlpackage?view=sql-server-ver15#publish-parameters-properties-and-sqlcmd-variables
set SqlPackage="C:\Program Files\Microsoft SQL Server\160\DAC\bin\SqlPackage.exe"
:: Drop & create new database or just migration True/False
set CreateNewDatabase=true
set BlockOnPossibleDataLoss=False
:: Wipe Data In All Table True/False
set WipeData=true
:: SQL Server information
:: In case you don't setup SQL Server with username/password
:: Please remove:
:: - -U %SQLServerUsername% -P %SQLServerPassword% in sqlcmd Enable contained database
:: - /TargetUser:%SQLServerUsername% /TargetPassword:%SQLServerPassword% in all publish command
set SQLServerInstance=localhost,1433
set SQLServerUsername=sa
set SQLServerPassword=1405
REM *** End Prepare parameters for executing ***

ECHO Begin build SQL Server solution
%MSBuild% %SQLServerSolution% -nologo -nr:false -t:rebuild -p:Configuration=Local
ECHO End build SQL Server Projects

@REM ECHO Enable contained database
@REM sqlcmd -S %SQLServerInstance% -d master -U %SQLServerUsername% -P %SQLServerPassword% -Q "sp_configure 'contained database authentication', 1; GO RECONFIGURE; GO" -o LogWipeData.log
@REM ECHO End Enable contained database

ECHO Begin publish to CoreData DB
%SqlPackage% /TargetDatabaseName:CoreData /TargetServerName:%SQLServerInstance% /TargetUser:%SQLServerUsername% /TargetPassword:%SQLServerPassword% /Action:Publish /p:CreateNewDatabase=%CreateNewDatabase% /p:BlockOnPossibleDataLoss=%BlockOnPossibleDataLoss% /SourceFile:%CoreData_DACPAC% /v:deployenv=%deployenv% /v:WipeData=$WipeData
ECHO End publish to CoreData DB

@pause
REM exit
