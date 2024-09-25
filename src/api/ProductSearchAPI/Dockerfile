ARG arch
FROM mcr.microsoft.com/dotnet/aspnet:8.0-noble-amd64 AS base
USER app
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0-noble-amd64 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ProductSearchAPI.csproj", "."]
RUN dotnet restore "./ProductSearchAPI.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "./ProductSearchAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ProductSearchAPI.csproj" --no-restore -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "ProductSearchAPI.dll"]