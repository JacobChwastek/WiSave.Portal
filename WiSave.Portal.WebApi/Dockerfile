FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8082
EXPOSE 8083

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["WiSave.Portal.WebApi/WiSave.Portal.WebApi.csproj", "WiSave.Portal.WebApi/"]
RUN dotnet restore "WiSave.Portal.WebApi/WiSave.Portal.WebApi.csproj"
COPY . .
WORKDIR "/src/WiSave.Portal.WebApi"
RUN dotnet build "./WiSave.Portal.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./WiSave.Portal.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WiSave.Portal.WebApi.dll"]
