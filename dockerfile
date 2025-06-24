
# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia os arquivos de solução e restaura as dependências
COPY ["1.API/FCG.API/FCG.API.csproj", "1.API/FCG.API/"]
COPY ["2.Application/FCG.Application/FCG.Application.csproj", "2.Application/FCG.Application/"]
COPY ["3.Domain/FCG.Domain/FCG.Domain.csproj", "3.Domain/FCG.Domain/"]
COPY ["4.Infrastructure/FCG.Infrastructure/FCG.Infrastructure.csproj", "4.Infrastructure/FCG.Infrastructure/"]
RUN dotnet restore "1.API/FCG.API/FCG.API.csproj"

# Copia o restante dos arquivos e publica a aplicação
COPY . .
WORKDIR "/src/1.API/FCG.API"
RUN dotnet publish "FCG.API.csproj" -c Release -o /app/publish

# Etapa de runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80
ENTRYPOINT ["dotnet", "FCG.API.dll"]