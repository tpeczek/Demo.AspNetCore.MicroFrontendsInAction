FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /src

COPY *.csproj ./
RUN dotnet restore

COPY . .
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build-env /app/publish .
EXPOSE 3002
ENV ASPNETCORE_URLS=http://+:3002
ENTRYPOINT ["dotnet", "Demo.AspNetCore.MicroFrontendsInAction.Inspire.dll"]