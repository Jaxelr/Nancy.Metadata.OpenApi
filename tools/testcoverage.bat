cd ..
set "destination=testcoverage"

rmdir /q /s %destination%
mkdir %destination%
cd tests
dotnet test /p:AltCover=true /p:AltCoverAssemblyExcludeFilter="xunit"
mv coverage.xml ../%destination%/coverage.xml

cd ../%destination%

set "reportgenerator=%UserProfile%\.nuget\packages\reportgenerator\4.2.10\tools\net47\ReportGenerator.exe"
set "targetdir=."

"%reportGenerator%" -reports:coverage.xml -reporttypes:HtmlInline -targetdir:%targetdir%