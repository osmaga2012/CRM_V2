# 1. RUNTIME
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# 2. SDK para compilación
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia los proyectos (ajusta los nombres si son distintos)
COPY ["CRM.App.Web/CRM.App.Web.csproj", "CRM.App.Web/"]
COPY ["CRM.App.Shared/CRM.App.Shared.csproj", "CRM.App.Shared/"]

RUN dotnet restore "CRM.App.Web/CRM.App.Web.csproj"
COPY . .
WORKDIR "/src/CRM.App.Web"
RUN dotnet build "CRM.App.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CRM.App.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

# 3. IMAGEN FINAL
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CRM.App.Web.dll"]