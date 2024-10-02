FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/VnvcStaffAdmin.WebApi/VnvcStaffAdmin.WebApi.csproj", "src/VnvcStaffAdmin.WebApi/"]
COPY ["src/VnvcStaffAdmin.Application/VnvcStaffAdmin.Application.csproj", "src/VnvcStaffAdmin.Application/"]
COPY ["src/VnvcStaffAdmin.Infrastructure/VnvcStaffAdmin.Infrastructure.csproj", "src/VnvcStaffAdmin.Infrastructure/"]
COPY ["src/VnvcStaffAdmin.Domain/VnvcStaffAdmin.Domain.csproj", "src/VnvcStaffAdmin.Domain/"]
COPY ["src/VnvcStaffAdmin.Identity/VnvcStaffAdmin.Identity.csproj", "src/VnvcStaffAdmin.Identity/"]
RUN dotnet restore "./src/VnvcStaffAdmin.WebApi/VnvcStaffAdmin.WebApi.csproj"
COPY . .
WORKDIR "/src/src/VnvcStaffAdmin.WebApi"
RUN dotnet build "./VnvcStaffAdmin.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./VnvcStaffAdmin.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "VnvcStaffAdmin.WebApi.dll"]