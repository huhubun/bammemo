FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /source

COPY src/Bammemo.Data/*.csproj ./src/Bammemo.Data/
COPY src/Bammemo.Service.Abstractions/*.csproj ./src/Bammemo.Service.Abstractions/
COPY src/Bammemo.Service.Server/*.csproj ./src/Bammemo.Service.Server/
RUN dotnet restore "/source/src/Bammemo.Service.Server/Bammemo.Service.Server.csproj"

COPY src/Bammemo.WebApi/*.csproj ./src/Bammemo.WebApi/
RUN dotnet restore "/source/src/Bammemo.WebApi/Bammemo.WebApi.csproj"

COPY src/. ./src/
WORKDIR /source/src/Bammemo.WebApi
RUN dotnet publish --configuration Release --runtime linux-x64 -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "Bammemo.WebApi.dll"]
