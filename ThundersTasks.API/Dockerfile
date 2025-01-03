#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ThundersTasks.API/ThundersTasks.API.csproj", "ThundersTasks.API/"]
COPY ["ThundersTasks.API.Application/ThundersTasks.API.Application.csproj", "ThundersTasks.API.Application/"]
COPY ["ThundersTasks.API.Domain/ThundersTasks.API.Domain.csproj", "ThundersTasks.API.Domain/"]
COPY ["ThundersTasks.API.Infra.Data/ThundersTasks.API.Infra.Data.csproj", "ThundersTasks.API.Infra.Data/"]
RUN dotnet restore "./ThundersTasks.API/ThundersTasks.API.csproj"
COPY . .
WORKDIR "/src/ThundersTasks.API"
RUN dotnet build "./ThundersTasks.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ThundersTasks.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ThundersTasks.API.dll"]
#
#
#

## Etapa de construção - Usando a imagem do SDK do .NET 8
#FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
#
## Defina o diretório de trabalho para o contexto da construção
#WORKDIR /src
#
## Copiar os arquivos .csproj dos quatro projetos
#COPY ["ThundersTasks.API/ThundersTasks.API.csproj", "ThundersTasks.API/"]
#COPY ["ThundersTasks.API.Application/ThundersTasks.API.Application.csproj", "ThundersTasks.API.Application/"]
#COPY ["ThundersTasks.API.Domain/ThundersTasks.API.Domain.csproj", "ThundersTasks.API.Domain/"]
#COPY ["ThundersTasks.API.Infra.Data/ThundersTasks.API.Infra.Data.csproj", "ThundersTasks.API.Infra.Data/"]
#
## Restaurar as dependências para os quatro projetos
#RUN dotnet restore "ThundersTasks.API/ThundersTasks.API.csproj"
#
## Copiar o restante dos arquivos para o diretório de construção
#COPY . .
#
## Construir a aplicação
#WORKDIR "/src/ThundersTasks.API"
#RUN dotnet build "ThundersTasks.API.csproj" -c Release -o /app/build
#
## Etapa de publicação - Publicar a aplicação
#FROM build AS publish
#RUN dotnet publish "ThundersTasks.API.csproj" -c Release -o /app/publish /p:UseAppHost=false
#
## Etapa final - Usar a imagem do ASP.NET para rodar a aplicação
#FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
#WORKDIR /app
#EXPOSE 8080
#EXPOSE 8081
#
## Copiar os arquivos publicados do container anterior
#COPY --from=publish /app/publish .
#
## Definir o ponto de entrada da aplicação
#ENTRYPOINT ["dotnet", "ThundersTasks.API.dll"]
