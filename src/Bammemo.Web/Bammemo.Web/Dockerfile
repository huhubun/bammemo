ARG VERSION=0.0.0

FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build
ARG VERSION
ENV APP_VERSION=${VERSION}
RUN echo "App Version: $APP_VERSION"
RUN apk add --upgrade --no-cache python3
RUN dotnet workload install wasm-tools
WORKDIR /source

COPY src/Bammemo.Data/*.csproj ./src/Bammemo.Data/
COPY src/Bammemo.Service.Abstractions/*.csproj ./src/Bammemo.Service.Abstractions/
COPY src/Bammemo.Service/*.csproj ./src/Bammemo.Service/
RUN dotnet restore --runtime linux-musl-x64 "/source/src/Bammemo.Service/Bammemo.Service.csproj"

COPY src/Bammemo.Web/Bammemo.Web/*.csproj ./src/Bammemo.Web/Bammemo.Web/
COPY src/Bammemo.Web/Bammemo.Web.Client/*.csproj ./src/Bammemo.Web/Bammemo.Web.Client/
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
    -p:DebugType=none \
    -p:BlazorWebAssemblyOmitDebugProxyOutput=true \
    -o /app

FROM alpine:3.21 AS final
RUN apk add --upgrade --no-cache ca-certificates-bundle libgcc libssl3 libstdc++ zlib
ENV ASPNETCORE_HTTP_PORTS=8080
WORKDIR /app
COPY --from=publish /app ./
ENTRYPOINT ["./Bammemo.Web"]