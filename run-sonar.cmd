@echo off
:: Navigate to the src directory
cd /d "%~dp0src"

:: Begin Sonar Scanner
dotnet sonarscanner begin ^
    /k:"FP_L" ^
    /d:sonar.login="sqp_918c39aafd85c5def3d6add0b0477f256a31442d" ^
    /d:sonar.host.url="http://localhost:9000" ^
    /d:sonar.cs.vscoveragexml.reportsPaths="coverage.xml"

:: Build without incremental builds
dotnet build --no-incremental

:: Run tests and collect code coverage
dotnet-coverage collect ^
    dotnet test --no-build -f xml -o coverage.xml

:: End Sonar Scanner
dotnet sonarscanner end ^
    /d:sonar.login="sqp_918c39aafd85c5def3d6add0b0477f256a31442d"
