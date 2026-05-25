# KM-Control

Sistema web para controle de veículos, abastecimentos, consumo médio, custo por km e histórico de uso.

O projeto foi desenvolvido como uma solução web para organizar informações de veículos e abastecimentos, aplicando conceitos de desenvolvimento full stack com C#, ASP.NET Core, Entity Framework Core, banco de dados relacional e uma interface web consumindo a API.

## Screenshot

### Dashboard

![Dashboard do KM-Control](docs/images/dashboard.png)

## Objetivo do projeto

O KM-Control surgiu a partir de uma necessidade prática: controlar melhor o consumo de combustível de veículos que não possuem cálculo automático de média de km/l.

A ideia do sistema é permitir o cadastro de veículos e abastecimentos, calculando informações úteis como:

- média de consumo em km/l;
- valor por litro;
- custo por km rodado;
- histórico de abastecimentos;
- controle de veículos cadastrados.

## Tecnologias utilizadas

### Back-end

- C#
- ASP.NET Core Web API
- Entity Framework Core
- MySQL
- Pomelo Entity Framework Core MySQL
- Swagger

### Front-end

- HTML
- CSS
- JavaScript
- Consumo de API REST

### Ferramentas

- Git e GitHub
- Visual Studio Code
- MySQL Workbench
- Postman

## Funcionalidades implementadas

- Cadastro de veículos
- Listagem de veículos
- Cadastro de abastecimentos
- Cálculo de média de consumo
- Cálculo de valor por litro
- Cálculo de custo por km
- Integração entre front-end e API
- Organização do front-end em arquivos de serviços, utilitários e interface

## Estrutura do projeto

KM-Control/
│
├── KmControl.Api/
│   ├── Controllers/
│   ├── DTOs/
│   ├── Models/
│   ├── Migrations/
│   ├── Program.cs
│   └── appsettings.json
│
├── KmControl.web/
│   ├── css/
│   ├── js/
│   │   ├── services/
│   │   ├── ui/
│   │   └── utils/
│   └── index.html
│
├── KM-Control.sln
├── .gitignore
└── README.md

## Conceitos aplicados

- criação de APIs REST com ASP.NET Core;
- organização de controllers, models e DTOs;
- integração com banco de dados usando Entity Framework Core;
- criação e aplicação de migrations;
- consumo de API com JavaScript;
- separação entre back-end e front-end;
- versionamento de código com Git;
- estruturação de um projeto com foco em evolução futura.

## Status do projeto

Projeto em desenvolvimento.

A versão atual já possui a base principal da API e da interface web, mas ainda pode receber melhorias como:

- autenticação de usuários;
- dashboard com gráficos;
- filtros por período;
- edição e exclusão de registros;
- melhoria visual da interface;
- deploy da API e do front-end;
- testes automatizados.

## Como executar o projeto

### Pré-requisitos

- .NET 8 SDK instalado
- MySQL instalado e em execução
- Visual Studio Code ou Visual Studio
- Git

### Passos básicos

Clone o repositório:

git clone https://github.com/DanielMeirelesSi/KM-Control.git

Acesse a pasta do projeto:

cd KM-Control

Configure a string de conexão no arquivo:

KmControl.Api/appsettings.json

Execute a API:

cd KmControl.Api
dotnet run

Depois, abra o front-end localizado em:

KmControl.web/index.html

## Observação

Este é um projeto pessoal em desenvolvimento, criado para aplicar conceitos de desenvolvimento web, APIs, banco de dados e organização de código em uma solução baseada em uma necessidade real de controle de veículos e abastecimentos.