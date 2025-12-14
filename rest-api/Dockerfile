# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy solution and project files
COPY ["DailyYield.sln", "."]
COPY ["src/DailyYield.Api/DailyYield.Api.csproj", "src/DailyYield.Api/"]
COPY ["src/DailyYield.Application/DailyYield.Application.csproj", "src/DailyYield.Application/"]
COPY ["src/DailyYield.Domain/DailyYield.Domain.csproj", "src/DailyYield.Domain/"]
COPY ["src/DailyYield.Adapter.Database/DailyYield.Adapter.Database.csproj", "src/DailyYield.Adapter.Database/"]
COPY ["src/DailyYield.Integration/DailyYield.Integration.csproj", "src/DailyYield.Integration/"]

# Restore dependencies
RUN dotnet restore "src/DailyYield.Api/DailyYield.Api.csproj"

# Copy everything else
COPY . .

# Build the application
WORKDIR "/src/src/DailyYield.Api"
RUN dotnet build "DailyYield.Api.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "DailyYield.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

# Create non-root user
RUN adduser --disabled-password --gecos '' appuser && chown -R appuser:appuser /app
USER appuser

# Copy published application
COPY --from=publish /app/publish .

# Expose port
EXPOSE 8080

# Set environment variables
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

# Health check
HEALTHCHECK --interval=30s --timeout=3s --start-period=5s --retries=3 \
  CMD curl -f http://localhost:8080/health || exit 1

# Run the application
ENTRYPOINT ["dotnet", "DailyYield.Api.dll"]