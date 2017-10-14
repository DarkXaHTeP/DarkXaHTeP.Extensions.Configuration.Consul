#!/usr/bin/env bash

dotnet pack ./ConsulConfiguration/ConsulConfiguration.csproj -c release -o ../release

dotnet nuget push ./release/*.nupkg -k $NUGET_API_KEY -s NuGetFeed