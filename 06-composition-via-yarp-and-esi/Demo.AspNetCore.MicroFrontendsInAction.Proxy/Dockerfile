ARG VERSION=8.0-alpine

FROM mcr.microsoft.com/dotnet/sdk:$VERSION AS build-env
WORKDIR /src

COPY *.csproj ./
RUN dotnet restore

COPY . .
RUN dotnet publish \
  --runtime linux-musl-x64 \
  --configuration Release \
  --self-contained true \
  --output /app/publish \
  /p:PublishSingleFile=true

FROM mcr.microsoft.com/dotnet/runtime-deps:$VERSION
USER app
WORKDIR /app
COPY --from=build-env /app/publish .
ENV \
  DOTNET_RUNNING_IN_CONTAINER=true \
  ASPNETCORE_URLS=http://+:3000
EXPOSE 3000
ENTRYPOINT ["./Demo.AspNetCore.MicroFrontendsInAction.Proxy", "--urls", "http://0.0.0.0:3000"]