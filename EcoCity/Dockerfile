# 1. Etapa de Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia os ficheiros de projeto e restaura as dependências
COPY ["EcoCity.csproj", "./"]
RUN dotnet restore "EcoCity.csproj"

# Copia o resto do código
COPY . .
WORKDIR "/src/"

# Compila
RUN dotnet publish "EcoCity.csproj" -c Release -o /app/publish

# 2. Etapa de Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_HTTP_PORTS=8080 
EXPOSE 8080

ENTRYPOINT ["dotnet", "EcoCity.dll"]