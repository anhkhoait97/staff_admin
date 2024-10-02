# Example Dockerfile for Gateway Service
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/VnvcStaffAdmin.Gateway/VnvcStaffAdmin.Gateway.csproj", "src/VnvcStaffAdmin.Gateway/"]
RUN dotnet restore "src/VnvcStaffAdmin.Gateway/VnvcStaffAdmin.Gateway.csproj"
COPY . .
WORKDIR "/src/src/VnvcStaffAdmin.Gateway"
RUN dotnet build "VnvcStaffAdmin.Gateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "VnvcStaffAdmin.Gateway.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Set permissions for /app directory
RUN chmod -R 755 /app

# Set permissions for the ocelot.json file
RUN chmod 644 /app/ocelot.json

ENTRYPOINT ["dotnet", "VnvcStaffAdmin.Gateway.dll"]
