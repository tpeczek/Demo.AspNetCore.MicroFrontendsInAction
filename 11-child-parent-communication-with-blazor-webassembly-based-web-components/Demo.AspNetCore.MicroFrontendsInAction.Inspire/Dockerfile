ARG VERSION=6.0-alpine

FROM mcr.microsoft.com/dotnet/sdk:$VERSION AS build-env
WORKDIR /src

COPY *.csproj ./
RUN dotnet restore

COPY . .
RUN dotnet publish \
  --runtime alpine-x64 \
  --configuration Release \
  --self-contained true \
  --output /app/publish \
  /p:PublishSingleFile=true \
  /p:PublishTrimmed=true

FROM mcr.microsoft.com/dotnet/runtime-deps:$VERSION
RUN adduser \
  --disabled-password \
  --home /app \
  --gecos '' app \
  && chown -R app /app
USER app
WORKDIR /app
COPY --from=build-env /app/publish .
ENV \
  DOTNET_RUNNING_IN_CONTAINER=true \
  ASPNETCORE_URLS=http://+:3002
EXPOSE 3002
ENTRYPOINT ["./Demo.AspNetCore.MicroFrontendsInAction.Inspire", "--urls", "http://0.0.0.0:3002"]