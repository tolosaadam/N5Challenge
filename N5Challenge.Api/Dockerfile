FROM mcr.microsoft.com/dotnet/aspnet:8.0-noble AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0-noble AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["N5Challenge.Api/src/N5Challenge.Api/N5Challenge.Api.csproj", "N5Challenge.Api/src/N5Challenge.Api/"]
COPY ["N5Challenge.Api/src/N5Challenge.Api.Application/N5Challenge.Api.Application.csproj", "N5Challenge.Api/src/N5Challenge.Api.Application/"]
COPY ["N5Challenge.Api/src/N5Challenge.Api.Application.Interfaces/N5Challenge.Api.Application.Interfaces.csproj", "N5Challenge.Api/src/N5Challenge.Api.Application.Interfaces/"]
COPY ["N5Challenge.Api/src/N5Challenge.Api.Domain/N5Challenge.Api.Domain.csproj", "N5Challenge.Api/src/N5Challenge.Api.Domain/"]
COPY ["N5Challenge.Api/src/N5Challenge.Api.Infraestructure/N5Challenge.Api.Infraestructure.csproj", "N5Challenge.Api/src/N5Challenge.Api.Infraestructure/"]
COPY ["N5Challenge.Api/src/N5Challenge.Api.Infraestructure.SQL/N5Challenge.Api.Infraestructure.SQL.csproj", "N5Challenge.Api/src/N5Challenge.Api.Infraestructure.SQL/"]
COPY ["N5Challenge.Api/src/N5Challenge.Api.Infraestructure.ElasticSearch/N5Challenge.Api.Infraestructure.ElasticSearch.csproj", "N5Challenge.Api/src/N5Challenge.Api.Infraestructure.ElasticSearch/"]
COPY ["N5Challenge.Api/src/N5Challenge.Api.Infraestructure.Services.Kafka/N5Challenge.Api.Infraestructure.Services.Kafka.csproj", "N5Challenge.Api/src/N5Challenge.Api.Infraestructure.Services.Kafka/"]
COPY ["N5Challenge.Common/N5Challenge.Common/N5Challenge.Common.csproj", "N5Challenge.Common/N5Challenge.Common/"]

RUN dotnet restore "N5Challenge.Api/src/N5Challenge.Api/N5Challenge.Api.csproj"

COPY . .

WORKDIR "/src/N5Challenge.Api/src/N5Challenge.Api"
RUN dotnet build "N5Challenge.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "N5Challenge.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "N5Challenge.Api.dll"]
