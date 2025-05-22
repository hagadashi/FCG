# 🎮 FCG - FIAP Cloud Games

[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?style=flat&logo=.net)](https://dotnet.microsoft.com/)
[![PostgreSQL](https://img.shields.io/badge/PostgreSQL-336791?style=flat&logo=postgresql&logoColor=white)](https://www.postgresql.org/)
[![License](https://img.shields.io/badge/license-MIT-green.svg)](LICENSE)
[![Build Status](https://img.shields.io/badge/build-passing-brightgreen.svg)](https://github.com/your-username/fcg)

Uma plataforma de venda de jogos digitais e gestão de servidores para partidas online, desenvolvida seguindo os princípios de **Domain-Driven Design (DDD)** e **Clean Architecture**.

## 📋 Índice

- [Sobre o Projeto](#-sobre-o-projeto)
- [Funcionalidades](#-funcionalidades)
- [Arquitetura](#-arquitetura)
- [Tecnologias](#-tecnologias)
- [Pré-requisitos](#-pré-requisitos)
- [Instalação](#-instalação)
- [Uso](#-uso)
- [Estrutura do Projeto](#-estrutura-do-projeto)
- [Licença](#-licença)

## 🎯 Sobre o Projeto

A **FIAP Cloud Games (FCG)** é uma plataforma completa para:

- 🛒 Venda de jogos digitais
- 📚 Gestão de biblioteca pessoal de jogos
- 👥 Gerenciamento de usuários com diferentes perfis
- 🎁 Sistema de promoções e descontos
- 🔐 Autenticação e autorização robustas

Feito como projeto de estudos durante o curso de "Arquitetura de Sistemas .NET com Azure"

### Perfis de Usuário

- **Administrador**: Gestão completa da plataforma
- **Usuário**: Compra e acesso aos jogos da biblioteca pessoal

## ✨ Funcionalidades

### Para Administradores
- ✅ Cadastro e gerenciamento de jogos
- ✅ Administração de usuários
- ✅ Criação de promoções e descontos
- ✅ Gestão de categorias de jogos

### Para Usuários
- ✅ Autenticação segura
- ✅ Compra de jogos digitais
- ✅ Biblioteca pessoal de jogos
- ✅ Visualização de promoções ativas

## 🏗️ Arquitetura

O projeto segue os princípios de **Clean Architecture** e **Domain-Driven Design (DDD)**:

```
├── 1.API          # Camada de Apresentação (Controllers, Middleware)
├── 2.Application  # Camada de Aplicação (Use Cases, DTOs)
├── 3.Domain       # Camada de Domínio (Entidades, Regras de Negócio)
├── 4.Infrastructure # Camada de Infraestrutura (Repositórios, EF Core)
└── 5.Tests        # Testes Unitários e de Integração
```

### Domínios Principais

#### 🎮 Plataforma de Jogos
- **Gestão de Jogos** (Core)
- **Biblioteca do Usuário** (Core)
- **Gestão de Promoções** (Supporting)

#### 👤 Gestão de Usuários
- **Autenticação e Autorização** (Generic)
- **Perfis de Usuário** (Supporting)

## 🛠️ Tecnologias

- **Backend**: .NET 8, ASP.NET Core Web API
- **Banco de Dados**: PostgreSQL
- **ORM**: Entity Framework Core
- **Documentação**: Swagger/OpenAPI
- **Testes**: xUnit, Moq
- **Logs**: Serilog
- **Arquitetura**: Clean Architecture, DDD

## 📋 Pré-requisitos

Antes de começar, certifique-se de ter instalado:

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [PostgreSQL](https://www.postgresql.org/download/)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) ou [VS Code](https://code.visualstudio.com/)
- [Git](https://git-scm.com/)

## 🚀 Instalação

### 1. Clone o repositório
```bash
git clone https://github.com/your-username/fcg.git
cd fcg
```

### 2. Configure o banco de dados
```bash
# Configure a string de conexão no appsettings.json
# Exemplo:
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Database=fcg_db;Username=your_user;Password=your_password"
}
```

### 3. Execute as migrations
```bash
cd 1.API/FCG.API
dotnet ef database update
```

### 4. Restaure as dependências
```bash
dotnet restore
```

### 5. Execute a aplicação
```bash
dotnet run
```

## 💻 Uso

### Acessando a API

A aplicação estará disponível em:
- **API**: `https://localhost:7000` ou `http://localhost:5000`
- **Swagger**: `https://localhost:7000/swagger`

### Endpoints Principais

```
GET    /api/games           # Listar jogos
POST   /api/games           # Criar jogo (Admin)
GET    /api/users/library   # Biblioteca do usuário
POST   /api/auth/login      # Autenticação
POST   /api/sales           # Criar promoção (Admin)
```

### Dados Iniciais

O sistema já vem com dados iniciais configurados:
- **Admin**: `admin@fcg.com` / `Admin123!` > Pendente criar
- **Usuário**: `user@fcg.com` / `User123!` -> Pendente criar

## 📁 Estrutura do Projeto

```
FCG/
├── 1.API/
│   └── FCG.API/
│       ├── Controllers/     # Controladores da API
│       ├── Middleware/      # Middleware customizado
│       ├── Program.cs       # Configuração da aplicação
│       └── appsettings.json # Configurações
├── 2.Application/
│   └── FCG.Application/
│       ├── DTOs/           # Data Transfer Objects
│       ├── Services/       # Serviços de aplicação
│       └── UseCases/       # Casos de uso
├── 3.Domain/
│   └── FCG.Domain/
│       ├── Entities/       # Entidades do domínio
│       ├── Enums/          # Enumerações
│       ├── Events/         # Eventos de domínio
│       ├── Exceptions/     # Exceções personalizadas
│       └── Interfaces/     # Contratos e interfaces
├── 4.Infrastructure/
│   └── FCG.Infrastructure/
│       ├── Data/           # Contexto do EF Core
│       ├── Repositories/   # Implementação dos repositórios
│       └── Migrations/     # Migrations do banco
└── 5.Tests/
    └── FCG.Tests/
        ├── Unit/           # Testes unitários
        └── Integration/    # Testes de integração
```

## 🧪 Executando Testes

```bash
# Executar todos os testes
dotnet test

# Executar testes com cobertura
dotnet test --collect:"XPlat Code Coverage"

# Executar testes específicos
dotnet test --filter "Category=Unit"
```

## 📊 Banco de Dados

### Entidades Principais

- **User**: Gerencia informações de usuários
- **Role**: Define níveis de acesso/permissões
- **Game**: Catálogo de jogos disponíveis
- **Library**: Biblioteca pessoal de jogos
- **Category**: Classificação de jogos
- **Sale**: Gerencia promoções e descontos
- **Session**: Controla sessões de usuários

### Migrations

```bash
# Criar nova migration
dotnet ef migrations add NomeDaMigration

# Aplicar migrations
dotnet ef database update

# Reverter migration
dotnet ef database update PreviousMigrationName
```

## 📝 Licença

Distribuído sob a licença MIT. Veja `LICENSE` para mais informações.

---

⭐ **Se este projeto foi útil para você, considere dar uma estrela!**