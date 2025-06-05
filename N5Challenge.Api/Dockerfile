FROM mcr.microsoft.com/dotnet/aspnet:8.0-noble AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0-noble AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copiamos TODOS los proyectos .csproj dentro de src
COPY ["src/N5Challenge.Api/N5Challenge.Api.csproj", "src/N5Challenge.Api/"]
COPY ["src/N5Challenge.Api.Application/N5Challenge.Api.Application.csproj", "src/N5Challenge.Api.Application/"]
COPY ["src/N5Challenge.Api.Application.Interfaces/N5Challenge.Api.Application.Interfaces.csproj", "src/N5Challenge.Api.Application.Interfaces/"]
COPY ["src/N5Challenge.Api.Domain/N5Challenge.Api.Domain.csproj", "src/N5Challenge.Api.Domain/"]
COPY ["src/N5Challenge.Api.Infraestructure.SQL/N5Challenge.Api.Infraestructure.SQL.csproj", "src/N5Challenge.Api.Infraestructure.SQL/"]
COPY ["src/N5Challenge.Api.Infraestructure.Services.ElasticSearch/N5Challenge.Api.Infraestructure.Services.ElasticSearch.csproj", "src/N5Challenge.Api.Infraestructure.Services.ElasticSearch/"]
COPY ["src/N5Challenge.Api.Infraestructure.Services.Kafka/N5Challenge.Api.Infraestructure.Services.Kafka.csproj", "src/N5Challenge.Api.Infraestructure.Services.Kafka/"]

RUN dotnet restore "src/N5Challenge.Api/N5Challenge.Api.csproj"

COPY . .

WORKDIR "/src/src/N5Challenge.Api"
RUN dotnet build "N5Challenge.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "N5Challenge.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "N5Challenge.Api.dll"]
