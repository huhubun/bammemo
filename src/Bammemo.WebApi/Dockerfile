ARG VERSION=0.0.0

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG VERSION
ENV APP_VERSION=${VERSION}
RUN echo "App Version: $APP_VERSION"
WORKDIR /source

COPY src/Bammemo.Data/*.csproj ./src/Bammemo.Data/
COPY src/Bammemo.Service.Abstractions/*.csproj ./src/Bammemo.Service.Abstractions/
COPY src/Bammemo.Service.Server/*.csproj ./src/Bammemo.Service.Server/
RUN dotnet restore --runtime linux-x64 "/source/src/Bammemo.Service.Server/Bammemo.Service.Server.csproj"

COPY src/Bammemo.WebApi/*.csproj ./src/Bammemo.WebApi/
RUN dotnet restore --runtime linux-x64 "/source/src/Bammemo.WebApi/Bammemo.WebApi.csproj"

COPY src/. ./src/
WORKDIR /source/src/Bammemo.WebApi
RUN dotnet build --configuration Release \
    --runtime linux-x64 \
    --no-restore 

FROM build AS publish
RUN dotnet publish --configuration Release \
    --runtime linux-x64 \
    --no-restore \
    -p:VersionPrefix=$APP_VERSION \
    -o /app

FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine AS final
WORKDIR /app
COPY --from=publish /app ./
ENTRYPOINT ["dotnet", "Bammemo.WebApi.dll"]
