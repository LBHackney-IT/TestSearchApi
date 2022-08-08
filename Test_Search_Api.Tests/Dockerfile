FROM mcr.microsoft.com/dotnet/core/sdk:3.1

# disable microsoft telematry
ENV DOTNET_CLI_TELEMETRY_OPTOUT='true'

ARG LBHPACKAGESTOKEN
ENV LBHPACKAGESTOKEN=$LBHPACKAGESTOKEN

WORKDIR /app

# Copy csproj and nuget config and restore as distinct layers
COPY ./Test_Search_Api.sln ./
COPY ./Test_Search_Api/Test_Search_Api.csproj ./Test_Search_Api/
COPY ./Test_Search_Api.Tests/Test_Search_Api.Tests.csproj ./Test_Search_Api.Tests/
COPY /NuGet.Config /root/.nuget/NuGet/NuGet.Config

RUN dotnet restore ./Test_Search_Api/Test_Search_Api.csproj
RUN dotnet restore ./Test_Search_Api.Tests/Test_Search_Api.Tests.csproj

# Copy everything else and build
COPY . .

RUN dotnet build -c debug -o out Test_Search_Api.Tests/Test_Search_Api.Tests.csproj

CMD dotnet test
