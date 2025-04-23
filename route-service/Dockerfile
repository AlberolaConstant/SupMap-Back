# auth-service/Dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

# �tape de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore
RUN dotnet publish -c Release -o /app/publish

# �tape finale
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
COPY .env . 

ENTRYPOINT ["dotnet", "RouteService.dll"]
