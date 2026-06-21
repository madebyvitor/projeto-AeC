
## 1. Resumo do Projeto
O projeto é uma aplicação web completa, desenvolvida para gerenciar um catálogo de endereços por usuário. Ele engloba desde a autenticação e controle de sessão até a integração com serviços externos e geração de relatórios, cumprindo todos os requisitos estabelecidos no teste prático.

## 2. Principais Funcionalidades Implementadas
1. **Autenticação e Segurança:** 
    * Telas de Login e Registro de usuários com validação de credenciais.
    * Acesso restrito: apenas usuários logados podem visualizar e gerenciar seus endereços (isolamento de dados por `UsuarioId`).
2. **CRUD de Endereços:** 
    * Interface para Adicionar, Visualizar, Editar e Excluir endereços.
3. **Integração com ViaCEP:** 
    * Preenchimento automático dos campos de logradouro, bairro, cidade e UF a partir da digitação do CEP (implementado via JavaScript `wwwroot/js/enderecos.js`).
4. **Exportação de Dados:** 
    * Funcionalidade que permite ao usuário baixar seus endereços salvos em um arquivo no formato `.csv`.

## 3. Tecnologias Utilizadas
### 3.1. Back-end
* **C# com ASP.NET Core MVC (v10.0):** 
    * *Por quê?* É a tecnologia base sugerida pelo teste e o padrão MVC garante uma excelente separação de responsabilidades. Os dados (`Models`), a interface (`Views`) e as regras de negócio/fluxo (`Controllers`) ficam desacoplados, resultando em um código limpo, legível e de fácil manutenção.
* **Entity Framework Core (EF Core):** 
    * *Por quê?* Ele elimina a necessidade de escrever queries SQL manuais para as operações de CRUD, prevenindo ataques de SQL Injection e facilitando a interação com o banco de dados diretamente através de classes C#.
* **Abordagem Code-First & Migrations:** 
    * *Por quê?* Em vez de modelar o banco de dados primeiro, as tabelas foram geradas a partir das classes C# (`Models/Usuario.cs` e `Models/Endereco.cs`). Isso versiona a estrutura do banco junto com o código-fonte (visível na pasta `Migrations/`).
* **Script das tabelas:**
  * *O script que foi usado para criação das tabelas está em `database/scripts`. 

### 3.2. Banco de Dados
* **SQLite (Desenvolvimento) / SQL Server (Estrutura):**
    * *Por quê?* Embora o arquivo falasse em SQL Server, acho que o uso do SQLite no projeto é uma excelente prática para testes práticos, pois o banco de dados roda embutido na aplicação, não exigindo que o avaliador instale e configure um SQL Server local para rodar o seu código. No entanto, os scripts de criação puros foram gerados para cumprir rigorosamente o critério de entrega do teste.

### 3.3. Front-end
* **Razor Views (`.cshtml`), HTML5 e CSS3:**
    * *Por quê?* Pois permite mesclar C# com HTML perfeitamente, facilitando a renderização de dados do banco de dados na tela de forma dinâmica.
* **Bootstrap 5:**
    * *Por quê?* O Bootstrap é o framework CSS ideal para construir layouts responsivos, estruturados e visualmente agradáveis de forma rápida, permitindo focar mais tempo na lógica do back-end em vez de escrever CSS do zero.
* **jQuery & jQuery Validation:**
    * *Por quê?* Utilizados para a manipulação do DOM (como disparar a requisição para a API do ViaCEP ao sair do campo de CEP) e para realizar validações de formulário do lado do cliente (client-side), melhorando a usabilidade e evitando requisições desnecessárias ao servidor quando um campo obrigatório está vazio.

## 4. Diagrama e Figma
* **Diagrama relacional**
    * *Com o objetivo de deixar o projeto mais completo, criei também o diagrama relacional que pode ser visualizado no arquivo `docs/Diagrama/relational`.
* **Template Figma**
    * *O projeto do front-end feito no Figma também foi anexado, podendo ser visualizado na pasta `docs/Figma/`.
 

## Como executar o projeto

### Pré-requisitos

Antes de iniciar, certifique-se de ter instalado:

- .NET SDK 10.0 ou superior
- Git

### 1. Clone o repositório

```bash
git clone https://github.com/madebyvitor/projeto-AeC.git
```

### 2. Restaurar as dependências

```bash
dotnet restore
```

### 3. Criar o banco de dados

O projeto utiliza **SQLite** juntamente com o **Entity Framework Core**. O banco de dados é criado automaticamente a partir das migrations.

Execute:

```bash
dotnet ef database update
```

Será criado o arquivo:

```text
ProjetoAeC.db
```

### 4. Executar a aplicação

```bash
dotnet run
```

Após a inicialização, acesse:

```text
http://localhost:5212
```
