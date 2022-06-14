#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/azure-functions/dotnet:4 AS base
WORKDIR /home/site/wwwroot
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["FundooMicroservices.FunctionApp.Notes/FundooMicroservices.FunctionApp.Notes.csproj", "FundooMicroservices.FunctionApp.Notes/"]
COPY ["FundooMicroservices.Models/FundooMicroservices.Models.csproj", "FundooMicroservices.Models/"]
COPY ["FunctionApps/FundooMicroservices.FunctionApp.Users/FundooMicroservices.FunctionApp.Users.csproj", "FunctionApps/FundooMicroservices.FunctionApp.Users/"]
COPY ["FundooMicroservices.Shared/FundooMicroservices.Shared.csproj", "FundooMicroservices.Shared/"]
RUN dotnet restore "FundooMicroservices.FunctionApp.Notes/FundooMicroservices.FunctionApp.Notes.csproj"
COPY . .
WORKDIR "/src/FundooMicroservices.FunctionApp.Notes"
RUN dotnet build "FundooMicroservices.FunctionApp.Notes.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FundooMicroservices.FunctionApp.Notes.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /home/site/wwwroot
COPY --from=publish /app/publish .
ENV AzureWebJobsScriptRoot=/home/site/wwwroot \
    AzureFunctionsJobHost__Logging__Console__IsEnabled=true