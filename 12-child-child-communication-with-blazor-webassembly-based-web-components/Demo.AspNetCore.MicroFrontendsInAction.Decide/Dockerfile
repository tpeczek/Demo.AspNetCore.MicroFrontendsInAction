FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /src

COPY *.csproj ./
RUN dotnet restore

COPY . .
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build-env /app/publish .
EXPOSE 3001
ENV ASPNETCORE_URLS=http://+:3001
ENTRYPOINT ["dotnet", "Demo.AspNetCore.MicroFrontendsInAction.Decide.dll"]