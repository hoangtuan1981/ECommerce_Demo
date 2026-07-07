# ==========================
# Build Stage
# ==========================
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

# Copy project files để tận dụng Docker layer cache
COPY src/Services/Identity/Identity.API/Identity.API.csproj src/Services/Identity/Identity.API/
COPY src/Services/Identity/Identity.Application/Identity.Application.csproj src/Services/Identity/Identity.Application/
COPY src/Services/Identity/Identity.Infrastructure/Identity.Infrastructure.csproj src/Services/Identity/Identity.Infrastructure/

# Restore packages
RUN dotnet restore src/Services/Identity/Identity.API/Identity.API.csproj

# Copy toàn bộ source
COPY . .

# Publish
RUN dotnet publish src/Services/Identity/Identity.API/Identity.API.csproj \
    -c Release \
    -o /app/publish \
    --no-restore

# ==========================
# Runtime Stage
# ==========================
FROM mcr.microsoft.com/dotnet/aspnet:10.0

WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "Identity.API.dll"]