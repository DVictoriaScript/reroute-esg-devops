# Etapa de build - compila e publica a API
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ReRoute.Api.sln ./
COPY ReRoute.Api/ReRoute.Api.csproj ReRoute.Api/
RUN dotnet restore ReRoute.Api/ReRoute.Api.csproj

COPY ReRoute.Api/ ReRoute.Api/
RUN dotnet publish ReRoute.Api/ReRoute.Api.csproj -c Release -o /app/publish --no-restore

# Etapa de runtime - imagem enxuta para execução
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# A API escuta na porta 8080. Banco e JWT são passados por variável de ambiente:
# docker run -p 8080:8080 -e OracleConnection__ConnectionString="..." -e Jwt__SecretKey="..." reroute-api
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ReRoute.Api.dll"]
