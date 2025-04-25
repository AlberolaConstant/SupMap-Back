# route-service/Dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

# Étape de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore
RUN dotnet publish -c Release -o /app/publish

# Étape finale
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
COPY .env .

ENTRYPOINT ["dotnet", "RoutesService.dll"]
