FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/VnvcStaffAdmin.Authen/VnvcStaffAdmin.Authen.csproj", "src/VnvcStaffAdmin.Authen/"]
COPY ["src/VnvcStaffAdmin.Domain/VnvcStaffAdmin.Domain.csproj", "src/VnvcStaffAdmin.Domain/"]
COPY ["src/VnvcStaffAdmin.Identity/VnvcStaffAdmin.Identity.csproj", "src/VnvcStaffAdmin.Identity/"]
RUN dotnet restore "./src/VnvcStaffAdmin.Authen/VnvcStaffAdmin.Authen.csproj"
COPY . .
WORKDIR "/src/src/VnvcStaffAdmin.Authen"
RUN dotnet build "./VnvcStaffAdmin.Authen.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./VnvcStaffAdmin.Authen.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "VnvcStaffAdmin.Authen.dll"]