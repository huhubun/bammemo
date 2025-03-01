FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /source

COPY *.sln .
COPY src/Bammemo.Data/*.csproj ./src/Bammemo.Data/
COPY src/Bammemo.Service/*.csproj ./src/Bammemo.Service/
COPY src/Bammemo.Service.Abstractions/*.csproj ./src/Bammemo.Service.Abstractions/
COPY src/Bammemo.Service.Server/*.csproj ./src/Bammemo.Service.Server/
COPY src/Bammemo.WebApi/*.csproj ./src/Bammemo.WebApi/
COPY src/Bammemo.Web/Bammemo.Web/*.csproj ./src/Bammemo.Web/Bammemo.Web/
COPY src/Bammemo.Web/Bammemo.Web.Client/*.csproj ./src/Bammemo.Web/Bammemo.Web.Client/
RUN dotnet restore

COPY src/. ./src/
WORKDIR /source/src/Bammemo.WebApi
RUN dotnet publish -c Release -o /app
