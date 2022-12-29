ARG VERSION=6.0-alpine

FROM mcr.microsoft.com/dotnet/sdk:$VERSION AS build-env
WORKDIR /src

COPY *.csproj ./
RUN dotnet restore

COPY . .
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:$VERSION
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
ENTRYPOINT ["dotnet", "Demo.AspNetCore.MicroFrontendsInAction.Inspire.dll"]