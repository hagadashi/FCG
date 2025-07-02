
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

# Instala o Datadog .NET Tracer
RUN apt-get update \
    && apt-get install -y curl dpkg \
    && mkdir -p /opt/datadog \
    && mkdir -p /var/log/datadog \
    && TRACER_VERSION=$(curl -s https://api.github.com/repos/DataDog/dd-trace-dotnet/releases/latest | grep tag_name | cut -d '"' -f 4 | cut -c2-) \
    && curl -LO https://github.com/DataDog/dd-trace-dotnet/releases/download/v${TRACER_VERSION}/datadog-dotnet-apm_${TRACER_VERSION}_amd64.deb \
    && dpkg -i ./datadog-dotnet-apm_${TRACER_VERSION}_amd64.deb \
    && rm ./datadog-dotnet-apm_${TRACER_VERSION}_amd64.deb

# Define variáveis de ambiente para ativar o tracer
ENV CORECLR_ENABLE_PROFILING=1
ENV CORECLR_PROFILER={846F5F1C-F9AE-4B07-969E-05C26BC060D8}
ENV CORECLR_PROFILER_PATH=/opt/datadog/Datadog.Trace.ClrProfiler.Native.so
ENV DD_DOTNET_TRACER_HOME=/opt/datadog

# Copia a aplicação publicada
COPY --from=build /app/publish .

# Define porta e entrada
ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80
ENTRYPOINT ["dotnet", "FCG.API.dll"]