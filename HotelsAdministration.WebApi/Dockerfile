FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["HotelsAdministration.WebApi.Server/HotelsAdministration.WebApi.csproj", "HotelsAdministration.WebApi.Server/"]
RUN dotnet restore "HotelsAdministration.WebApi.Server/HotelsAdministration.WebApi.csproj"
COPY . .
WORKDIR "/src/HotelsAdministration.WebApi.Server"
RUN dotnet build "HotelsAdministration.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "HotelsAdministration.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HotelsAdministration.WebApi.dll"]
