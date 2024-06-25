#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Worker.Api/Worker.Api.csproj", "src/Worker.Api/"]
COPY ["src/Core.Application/Core.Application.csproj", "src/Core.Application/"]
COPY ["src/BuildingBlocks.Infrastructure/BuildingBlocks.Infrastructure.csproj", "src/BuildingBlocks.Infrastructure/"]
COPY ["src/BuildingBlocks.API.Core/BuildingBlocks.API.Core.csproj", "src/BuildingBlocks.API.Core/"]
COPY ["src/BuildingBlocks.Eventbus/BuildingBlocks.Eventbus.csproj", "src/BuildingBlocks.Eventbus/"]
COPY ["src/Core.Infraestructure/Core.Infraestructure.csproj", "src/Core.Infraestructure/"]
COPY ["src/Core.Domain/Core.Domain.csproj", "src/Core.Domain/"]
COPY ["src/Crosscutting.EventBus.RabbitMq/Crosscutting.EventBus.RabbitMq.csproj", "src/Crosscutting.EventBus.RabbitMq/"]
RUN dotnet restore "./src/Worker.Api/Worker.Api.csproj"
COPY . .
WORKDIR "/src/src/Worker.Api"
RUN dotnet build "./Worker.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Worker.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Worker.Api.dll"]