cd ..
set "destination=testcoverage"

rmdir /q /s %destination%
mkdir %destination%
cd tests
dotnet test /p:AltCover=true
mv coverage.xml ../%destination%/coverage.xml

cd ../%destination%

set "reportgenerator=%UserProfile%\.nuget\packages\reportgenerator\3.1.2\tools\ReportGenerator.exe"
set "targetdir=."

"%reportGenerator%" -reports:coverage.xml -reporttypes:HtmlInline -targetdir:%targetdir%