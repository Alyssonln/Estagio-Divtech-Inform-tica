# 🏢 FornecedoresApp

Sistema web desenvolvido em **ASP.NET Core MVC** para **cadastro e gerenciamento de fornecedores**.  
Projeto criado como parte de um processo de estágio, aplicando práticas de **desenvolvimento full stack com C# e Entity Framework Core**.

---

## ⚙️ Tecnologias

- ASP.NET Core MVC (.NET 9.0 / C#)
- Entity Framework Core (Code First)
- SQL Server LocalDB
- Bootstrap 5
- JavaScript (API ViaCEP)

---

## 💡 Funcionalidades

- CRUD completo de fornecedores
- Upload de imagem (PNG)
- Preenchimento automático de endereço (API ViaCEP)
- Validações de CNPJ e CEP
- Interface simples e responsiva

---

## 🚀 Como executar

### Pré-requisitos

- [.NET SDK 9.0+](https://dotnet.microsoft.com/download)
- SQL Server LocalDB

### Passos

```bash
# Clonar o projeto
git clone https://github.com/SEUUSUARIO/FornecedoresApp.git
cd FornecedoresApp

# Restaurar dependências
dotnet restore

# Criar o banco de dados
dotnet ef database update

# Executar o sistema
dotnet run
```

🔗 Acesse: http://localhost:5104/Fornecedores
