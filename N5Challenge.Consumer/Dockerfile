# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/N5Challenge.Consumer/N5Challenge.Consumer.csproj", "src/N5Challenge.Consumer/"]
COPY ["src/N5Challenge.Consumer.ElasticSearch/N5Challenge.Consumer.ElasticSearch.csproj", "src/N5Challenge.Consumer.ElasticSearch/"]
RUN dotnet restore "src/N5Challenge.Consumer/N5Challenge.Consumer.csproj"
COPY . .
WORKDIR "/src/src/N5Challenge.Consumer"
RUN dotnet build "N5Challenge.Consumer.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "N5Challenge.Consumer.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "N5Challenge.Consumer.dll"]