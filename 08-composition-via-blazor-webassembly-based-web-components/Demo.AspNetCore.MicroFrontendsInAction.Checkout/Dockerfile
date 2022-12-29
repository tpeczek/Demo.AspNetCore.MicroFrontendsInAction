ARG VERSION=6.0-alpine

FROM mcr.microsoft.com/dotnet/sdk:$VERSION AS build-env
WORKDIR /src

COPY Frontend/*.csproj ./Frontend/
COPY Service/*.csproj ./Service/
RUN dotnet restore Service

COPY . .
RUN dotnet publish Service \
  --runtime alpine-x64 \
  --configuration Release \
  --self-contained true \
  --output /app/publish

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
  ASPNETCORE_URLS=http://+:3003
EXPOSE 3003
ENTRYPOINT ["./Demo.AspNetCore.MicroFrontendsInAction.Checkout", "--urls", "http://0.0.0.0:3003"]