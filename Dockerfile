FROM microsoft/dotnet:2.0-sdk

ARG NUGET_API_KEY

ENV NUGET_API_KEY $NUGET_API_KEY

WORKDIR /app

COPY . ./

RUN dotnet restore

RUN chmod +x ./test.sh ./publish.sh
