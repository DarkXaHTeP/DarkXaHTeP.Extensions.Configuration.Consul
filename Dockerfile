FROM microsoft/dotnet:2.0-sdk
WORKDIR /app

# copy csproj and restore as distinct layers
COPY ConsulConfiguration/*.csproj ./ConsulConfiguration/
COPY ConsulConfiguration.Test/*.csproj ./ConsulConfiguration.Test/
COPY *.sln ./
RUN dotnet restore

# copy everything else and build
COPY . ./

RUN chmod +x ./test.sh
RUN chmod +x ./publish.sh
