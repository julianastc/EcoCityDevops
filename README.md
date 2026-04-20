# Projeto - Cidades ESGInteligentes

O **EcoCity 360** é uma plataforma focada em cidades inteligentes e práticas ESG (Environmental, Social, and Governance). Esta API gere dados críticos em áreas como consumo de energia, gestão de resíduos e indicadores sociais e de governança.

## Como executar localmente com Docker

A aplicação e a base de dados foram preparadas para serem executadas num ambiente isolado utilizando Docker Compose, garantindo que rodam em qualquer máquina sem necessidade de configurações complexas.

**Passo a passo:**
1. Certifique-se de que o Docker e o Docker Desktop estão instalados e a correr na sua máquina.
2. Abra o terminal na raiz do projeto (onde se encontra o ficheiro `compose.yaml`).
3. Execute o comando de orquestração:
   ```bash
   docker compose up -d --build
4. O Docker irá baixar as imagens, compilar a API, subir o MongoDB e executar automaticamente o script init-mongo.js para criar as 5 collections (energia, resíduos, indicadores, governança e sensores).
5. Após os contentores estarem a correr, acesse no navegador: http://localhost:8080/api/status.

## Pipeline CI/CD
A automação do ciclo de vida da aplicação foi configurada utilizando o GitHub Actions. O pipeline é ativado automaticamente em eventos de push ou pull request para a branch main.

Etapas do Pipeline:

1. Checkout e Setup: Baixa o código fonte e configura o ambiente .NET 8.
2. Build: Restaura dependências e compila o projeto C# em modo Release para garantir que não há erros de sintaxe. 
3. Testes Automatizados: Executa o projeto EcoCity.Tests (xUnit) para validar as regras de negócio. Se um teste falhar, o pipeline quebra. 
4. Containerização (Docker Build): Se os testes passarem, constrói a imagem Docker da API a partir do Dockerfile. 
5. Deploy Simulado: Executa os passos simulados de entrega contínua nos ambientes de Staging (Homologação) e Produção.

## Containerização
A estratégia adotada para a API utiliza um processo de Multi-stage Build para manter a imagem final o mais leve e segura possível.

- Fase 1 (Build): Utiliza a imagem pesada do SDK do .NET (mcr.microsoft.com/dotnet/sdk:8.0) para compilar o código.

- Fase 2 (Runtime): Utiliza apenas a imagem de execução (mcr.microsoft.com/dotnet/aspnet:8.0), descartando os ficheiros de compilação.

Conteúdo do Dockerfile:

1. Etapa de Build
FROM [mcr.microsoft.com/dotnet/sdk:8.0](https://mcr.microsoft.com/dotnet/sdk:8.0) AS build
WORKDIR /src
COPY ["EcoCity.csproj", "./"]
RUN dotnet restore "EcoCity.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet publish "EcoCity.csproj" -c Release -o /app/publish

2. Etapa de Runtime
FROM [mcr.microsoft.com/dotnet/aspnet:8.0](https://mcr.microsoft.com/dotnet/aspnet:8.0) AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "EcoCity.dll"]

A orquestração com o banco de dados é feita via Docker Compose, criando uma rede interna (ecocity-network) para comunicação segura entre a API e o MongoDB.

## Prints do funcionamento

1. Execução Local e Base de Dados (Docker Compose)

2. Execução do Pipeline CI/CD (GitHub Actions)

3. Deploy em Staging e Produção

## Tecnologias utilizadas
- Linguagem: C# 12 / .NET 8 (Minimal APIs)

- Testes Automatizados: xUnit

- Base de Dados: MongoDB (NoSQL)

- Containerização: Docker e Docker Compose

- Integração e Entrega Contínua (CI/CD): GitHub Actions

- IDE: JetBrains Rider

- Controle de Versão: Git e GitHub