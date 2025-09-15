FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["TrueCode.MigrationService/TrueCode.MigrationService.csproj", "TrueCode.MigrationService/"]
RUN dotnet restore "TrueCode.MigrationService/TrueCode.MigrationService.csproj"
COPY . .
WORKDIR "/src/TrueCode.MigrationService"
RUN dotnet build "./TrueCode.MigrationService.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TrueCode.MigrationService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TrueCode.MigrationService.dll"]
