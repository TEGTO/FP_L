#!/bin/bash

cd src

dotnet sonarscanner begin /k:"FP_L" /d:sonar.login="sqp_918c39aafd85c5def3d6add0b0477f256a31442d" /d:sonar.host.url="http://localhost:9000" /d:sonar.cs.vscoveragexml.reportsPaths=coverage.xml
dotnet build --no-incremental
dotnet-coverage collect dotnet test -f xml  -o coverage.xml
dotnet sonarscanner end /d:sonar.login="sqp_918c39aafd85c5def3d6add0b0477f256a31442d"