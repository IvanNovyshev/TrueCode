FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app


FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["TrueCode.FinanceService/TrueCode.FinanceService.csproj", "TrueCode.FinanceService/"]
RUN dotnet restore "TrueCode.FinanceService/TrueCode.FinanceService.csproj"
COPY . .
WORKDIR "/src/TrueCode.FinanceService"
RUN dotnet build "./TrueCode.FinanceService.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TrueCode.FinanceService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TrueCode.FinanceService.dll"]
