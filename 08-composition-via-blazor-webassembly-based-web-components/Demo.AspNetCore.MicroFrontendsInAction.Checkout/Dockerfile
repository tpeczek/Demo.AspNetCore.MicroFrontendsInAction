FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine AS build-env
WORKDIR /src

COPY Frontend/*.csproj ./Frontend/
COPY Service/*.csproj ./Service/
RUN dotnet restore Service

COPY . .
RUN dotnet publish Service -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine
WORKDIR /app
COPY --from=build-env /app/publish .
EXPOSE 3001
ENV ASPNETCORE_URLS=http://+:3003
ENTRYPOINT ["dotnet", "Demo.AspNetCore.MicroFrontendsInAction.Checkout.dll"]