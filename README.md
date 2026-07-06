# 📂 SISTEMA DE GESTÃO | Contatos, Compromissos, Categorias, Despesas e Tarefas 🚀

> **Controle inteligente de contatos, compromissos, finanças e tarefas para organizar sua rotina.**

![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)
![.NET](https://img.shields.io/badge/.NET-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![Razor CSHTML](https://img.shields.io/badge/Razor_CSHTML-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![Arquitetura MVC](https://img.shields.io/badge/Architecture-MVC-blue?style=for-the-badge)
![Desenvolvimento Web](https://img.shields.io/badge/Dev-Web-orange?style=for-the-badge)
![Status](https://img.shields.io/badge/Status-Concluído-brightgreen?style=for-the-badge)

---

![](eagenda.gif)

## 👥 Desenvolvedores

<table>
  <tr>
    <td align="center">
      <a href="https://github.com/pedrohenriquedsdev">
        <img src="https://github.com/pedrohenriquedsdev.png" width="80px" style="border-radius: 50%"/><br/>
        <sub><b>Pedro Henrique</b></sub>
      </a>
    </td>
    <td align="center">
      <a href="https://github.com/Marco-Oliver">
        <img src="https://github.com/Marco-Oliver.png" width="80px" style="border-radius: 50%"/><br/>
        <sub><b>Marco Oliver</b></sub>
      </a>
    </td>
  </tr>
</table>

<br>

## 📋 Sobre o Projeto

O **Sistema de Gestão** é uma aplicação web completa para organizar contatos, agendar compromissos, controlar despesas por categorias e gerenciar tarefas com prioridades e itens individuais — tudo em um único lugar, com validações robustas e regras de negócio bem definidas.

<br>

## ✨ Funcionalidades

### 👤 Módulo de Contatos
- Cadastrar, editar, visualizar e excluir contatos
- Campos obrigatórios: Nome (2–100 caracteres), Email (formato válido) e Telefone (`(XX) XXXX-XXXX` ou `(XX) XXXXX-XXXX`)
- Campos opcionais: Cargo e Empresa
- Não são permitidos dois contatos com o mesmo e-mail ou telefone
- Não é possível excluir um contato que possui compromissos vinculados

### 📅 Módulo de Compromissos
- Cadastrar, editar, visualizar e excluir compromissos
- Tipos suportados: **Remoto** (com link) e **Presencial** (com local)
- Campos obrigatórios: Assunto (2–100 caracteres), Data, Hora de Início e Hora de Término
- Contato vinculado é opcional
- Não são permitidos conflitos de horário entre compromissos

### 🏷️ Módulo de Categorias
- Cadastrar, editar, visualizar e excluir categorias
- Visualizar todas as despesas pertencentes a uma categoria específica
- Título obrigatório (2–100 caracteres) e único
- Não é possível excluir uma categoria com despesas vinculadas

### 💰 Módulo de Despesas
- Cadastrar, editar, visualizar e excluir despesas
- Campos obrigatórios: Descrição (2–100 caracteres), Valor (R$), Forma de Pagamento (À Vista, Crédito ou Débito) e ao menos uma Categoria
- Data de Ocorrência opcional — usa a data de cadastro por padrão

### ✅ Módulo de Tarefas
- Cadastrar, editar, visualizar e excluir tarefas
- Visualizar tarefas **pendentes** e **concluídas** separadamente
- Visualizar tarefas **agrupadas por prioridade** (Baixa, Normal, Alta)
- Campos obrigatórios: Título (2–100 caracteres), Prioridade, Datas de Criação e Conclusão, Status e Percentual Concluído
- Itens da tarefa são opcionais

#### 📌 Itens de Tarefas
- Adicionar e remover itens em uma tarefa
- Concluir itens atualiza automaticamente o percentual (%) de conclusão da tarefa
- Campos obrigatórios: Título (2–100 caracteres), Status de Conclusão e vínculo com a Tarefa

<br>

## 🚀 Como Executar o Projeto

### Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) ou [VS Code](https://code.visualstudio.com/)
- Git

### Passo a passo

```bash
# 1. Clone o repositório
git clone https://github.com/pedrohenriquedsdev/sistema-de-gestao.git

# 2. Acesse a pasta do projeto
cd sistema-de-gestao

# 3. Restaure os pacotes
dotnet restore

# 4. Execute a aplicação
dotnet run --project src/SistemaDeGestao.Web
```

Acesse no navegador: `https://localhost:5001`

> Os dados são persistidos em arquivo local — nenhuma configuração de banco de dados é necessária.

<br>

## 🎬 Demonstração

> *(Adicione aqui um GIF ou vídeo demonstrando as principais telas do sistema)*

<!-- Exemplo:
![Demo da aplicação](docs/demo.gif)
-->

<br>

## 🏗️ Arquitetura

O projeto segue o padrão de **3 camadas** com ASP.NET MVC:

```
SistemaDeGestao/
├── src/
│   ├── SistemaDeGestao.Web/              # Camada de Apresentação (Controllers, Views, ViewModels)
│   │   ├── Controllers/
│   │   │   ├── Contatos/
│   │   │   ├── Compromissos/
│   │   │   ├── Categorias/
│   │   │   ├── Despesas/
│   │   │   └── Tarefas/
│   │   └── Views/
│   │       ├── Contatos/
│   │       ├── Compromissos/
│   │       ├── Categorias/
│   │       ├── Despesas/
│   │       └── Tarefas/
│   ├── SistemaDeGestao.Domain/           # Camada de Domínio (Entidades, Regras de Negócio)
│   │   ├── Entities/
│   │   └── Services/
│   └── SistemaDeGestao.Infrastructure/  # Camada de Infraestrutura (Persistência em Arquivo)
│       └── Repositories/
└── README.md
```

<br>

## 🛠️ Tecnologias Utilizadas

| Tecnologia | Uso |
|---|---|
| ASP.NET MVC (.NET 8) | Framework principal |
| C# | Linguagem de programação |
| Razor / TagHelpers | Renderização de views |
| DataAnnotations | Validações de formulário |
| AutoMapper | Mapeamento entre entidades e ViewModels |
| Injeção de Dependência | Baixo acoplamento entre camadas |
| Serialização em arquivo (JSON) | Persistência de dados |
| Bootstrap | Estilização da interface |

<br>

## ✅ Boas Práticas Aplicadas

- Separação clara em **3 camadas** (Apresentação, Domínio, Infraestrutura)
- **ViewModels** para comunicação com as Views; **Records** para DTOs imutáveis
- **Services** concentrando as regras de negócio
- **Extension Methods** para comportamentos reutilizáveis
- **Delegates, métodos anônimos e Lambdas** para maior legibilidade
- **TempData** para feedback entre requisições
- **ModelState** para validação consistente dos formulários
- Nomenclatura seguindo o padrão **PascalCase / camelCase**
- Tratamento de exceções e validações robustos

<br>

## 📌 Regras de Negócio Principais

- Emails e telefones duplicados entre contatos não são permitidos
- Contatos com compromissos vinculados não podem ser excluídos
- Compromissos com conflito de horário não são permitidos
- Categorias com títulos duplicados não são permitidas
- Categorias com despesas vinculadas não podem ser excluídas
- Toda despesa deve pertencer a ao menos uma categoria
- O percentual de conclusão de uma tarefa é atualizado automaticamente conforme seus itens são concluídos

<br>

## 📄 Licença

Este projeto foi desenvolvido para fins educacionais na **Academia do Programador**.
