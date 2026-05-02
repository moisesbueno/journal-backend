#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Journal.Api/Journal.Api.csproj", "Journal.Api/"]
COPY ["Journal.Data/Journal.Data.csproj", "Journal.Data/"]
COPY ["Journal.MessageBus/Journal.MessageBus.csproj", "Journal.MessageBus/"]
RUN dotnet restore "./Journal.Api/Journal.Api.csproj"
COPY . .
WORKDIR "/src/Journal.Api"
RUN dotnet build "./Journal.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Journal.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Journal.Api.dll"]