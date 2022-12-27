ARG VERSION=6.0-alpine

FROM mcr.microsoft.com/dotnet/sdk:$VERSION AS build-env
WORKDIR /src

COPY *.csproj ./
RUN dotnet restore

COPY . .
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:$VERSION
WORKDIR /app
COPY --from=build-env /app/publish .
EXPOSE 3001
ENV ASPNETCORE_URLS=http://+:3001
ENTRYPOINT ["dotnet", "Demo.AspNetCore.MicroFrontendsInAction.Decide.dll"]