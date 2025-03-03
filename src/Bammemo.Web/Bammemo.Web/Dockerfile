ARG VERSION=0.0.0

FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build
ARG VERSION
ENV APP_VERSION=${VERSION}
RUN echo "App Version: $APP_VERSION"
WORKDIR /source

COPY src/Bammemo.Data/*.csproj ./src/Bammemo.Data/
COPY src/Bammemo.Service.Abstractions/*.csproj ./src/Bammemo.Service.Abstractions/
COPY src/Bammemo.Service.Server/*.csproj ./src/Bammemo.Service.Server/
RUN dotnet restore --runtime linux-musl-x64 "/source/src/Bammemo.Service.Server/Bammemo.Service.Server.csproj"

COPY src/Bammemo.Web/Bammemo.Web/*.csproj ./src/Bammemo.Web/Bammemo.Web/
COPY src/Bammemo.Web/Bammemo.Web.Client/*.csproj ./src/Bammemo.Web/Bammemo.Web.Client/
RUN apk update
RUN apk add python3
RUN dotnet workload install wasm-tools
RUN dotnet restore --runtime linux-musl-x64 \
    -p:UseMonoRuntime=false \
    "/source/src/Bammemo.Web/Bammemo.Web/Bammemo.Web.csproj"

COPY src/. ./src/
WORKDIR /source/src/Bammemo.Web/Bammemo.Web
RUN dotnet build --configuration Release \
    --runtime linux-musl-x64 \
    --no-restore 

FROM build AS publish
RUN dotnet publish --configuration Release \
    --runtime linux-musl-x64 \
    --no-restore \
    -p:VersionPrefix=$APP_VERSION \
    -o /app

FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine AS final
WORKDIR /app
COPY --from=publish /app ./
ENTRYPOINT ["./Bammemo.Web"]