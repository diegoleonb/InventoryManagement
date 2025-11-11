FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project file and restore dependencies
COPY ["InventoryAPI.csproj", "."]
RUN dotnet restore "InventoryAPI.csproj"

# Copy everything else and build
COPY . .
RUN dotnet publish "InventoryAPI.csproj" -c Release -o /app

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app .

# Expose the port the app runs on
EXPOSE 80

# Set environment variables
ENV ASPNETCORE_ENVIRONMENT=Development
ENV ASPNETCORE_URLS=http://+:80

ENTRYPOINT ["dotnet", "InventoryAPI.dll"]