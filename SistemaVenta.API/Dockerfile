#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["SistemaVenta.API/SistemaVenta.API.csproj", "SistemaVenta.API/"]
COPY ["SistemaVenta.BLL/SistemaVenta.BLL.csproj", "SistemaVenta.BLL/"]
COPY ["SistemaVenta.DAL/SistemaVenta.DAL.csproj", "SistemaVenta.DAL/"]
COPY ["SistemaVenta.Model/SistemaVenta.Model.csproj", "SistemaVenta.Model/"]
COPY ["SistemaVenta.DTO/SistemaVenta.DTO.csproj", "SistemaVenta.DTO/"]
COPY ["SistemaVenta.Utility/SistemaVenta.Utility.csproj", "SistemaVenta.Utility/"]
COPY ["SistemaVenta.IOC/SistemaVenta.IOC.csproj", "SistemaVenta.IOC/"]
RUN dotnet restore "SistemaVenta.API/SistemaVenta.API.csproj"
COPY . .
WORKDIR "/src/SistemaVenta.API"
RUN dotnet build "SistemaVenta.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SistemaVenta.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SistemaVenta.API.dll"]