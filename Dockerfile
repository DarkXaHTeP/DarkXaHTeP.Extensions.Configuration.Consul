FROM microsoft/dotnet:2.0-sdk

ARG NUGET_SOURCE
ARG NUGET_USERNAME
ARG NUGET_API_KEY

ENV NUGET_API_KEY $NUGET_API_KEY

WORKDIR /app

# copy csproj and restore as distinct layers
COPY ConsulConfiguration/*.csproj ./ConsulConfiguration/
COPY ConsulConfiguration.Test/*.csproj ./ConsulConfiguration.Test/
COPY *.sln ./
RUN dotnet restore

# copy everything else and build
COPY . ./

RUN sed -i "s|{NUGET_SOURCE}|$NUGET_SOURCE|g" NuGet.config
RUN sed -i "s%{NUGET_USERNAME}%$NUGET_USERNAME%g" NuGet.config
RUN sed -i "s%{NUGET_API_KEY}%$NUGET_API_KEY%g" NuGet.config

RUN chmod +x ./test.sh
RUN chmod +x ./publish.sh
