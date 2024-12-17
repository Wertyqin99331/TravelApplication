FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["JourneyApp.WebApi/JourneyApp.WebApi.csproj", "JourneyApp.WebApi/"]
COPY ["JourneyApp.Application/JourneyApp.Application.csproj", "JourneyApp.Application/"]
COPY ["JourneyApp.Core/JourneyApp.Core.csproj", "JourneyApp.Core/"]
COPY ["JourneyApp.Infrastructure/JourneyApp.Infrastructure.csproj", "JourneyApp.Infrastructure/"]
COPY ["JourneyApp.sln", "./"]

RUN dotnet restore "JourneyApp.WebApi/JourneyApp.WebApi.csproj" --disable-parallel --no-cache

COPY . .

WORKDIR "/src/JourneyApp.WebApi"
RUN dotnet build "JourneyApp.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build --no-restore

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "JourneyApp.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false --no-restore

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "JourneyApp.WebApi.dll"]
