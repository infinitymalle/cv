# --- Build stage: full SDK, thrown away after publishing ---
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Restore first (cached layer: only re-runs when project files change)
COPY global.json Directory.Build.props Directory.Packages.props ./
COPY src/Cv.Domain/Cv.Domain.csproj src/Cv.Domain/
COPY src/Cv.Application/Cv.Application.csproj src/Cv.Application/
COPY src/Cv.Infrastructure/Cv.Infrastructure.csproj src/Cv.Infrastructure/
COPY src/Cv.Api/Cv.Api.csproj src/Cv.Api/
RUN dotnet restore src/Cv.Api/Cv.Api.csproj

COPY src/ src/
RUN dotnet publish src/Cv.Api/Cv.Api.csproj -c Release -o /app --no-restore -p:OpenApiGenerateDocuments=false

# --- Runtime stage: "chiseled" image = no shell, no package manager, runs as non-root ---
FROM mcr.microsoft.com/dotnet/aspnet:10.0-noble-chiseled
WORKDIR /app
COPY --from=build /app .
EXPOSE 8080
ENTRYPOINT ["dotnet", "Cv.Api.dll"]
