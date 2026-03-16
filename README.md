# 🛒 ECommerce ITI - ASP.NET Core MVC Project

A full-featured **E-Commerce web application** built with **ASP.NET Core MVC (.NET 9)** following **Clean Architecture** principles. Developed as an ITI (Information Technology Institute) graduation project.

---

## 📋 Table of Contents

- [About](#about)
- [Architecture](#architecture)
- [Tech Stack](#tech-stack)
- [Domain Entities](#domain-entities)
- [Project Structure](#project-structure)
- [Features](#features)
- [Getting Started](#getting-started)
- [Configuration](#configuration)
- [Screenshots](#screenshots)
- [Contributing](#contributing)
- [License](#license)

---

## 📖 About

**ECommerce ITI** is a modern e-commerce web application that allows users to browse products, manage a shopping cart via sessions, place orders, and manage their accounts. The project demonstrates best practices in ASP.NET Core MVC development with a layered Clean Architecture approach.

---

## 🏗️ Architecture

The solution follows **Clean Architecture** with clear separation of concerns across four projects:

```
EcommerceIti.sln
├── Domain          → Core entities & business models
├── Application     → Interfaces, ViewModels & business logic contracts
├── Infrastructure  → Data access, EF Core, Identity, Services & Seeding
└── Web             → ASP.NET Core MVC presentation layer
```

| Layer              | Project                        | Responsibility                                      |
|--------------------|--------------------------------|-----------------------------------------------------|
| **Domain**         | `EcommerceIti.Domain`          | Entities, enums, and core business models            |
| **Application**    | `EcommerceIti.Application`     | Interfaces, ViewModels, and service contracts        |
| **Infrastructure** | `EcommerceIti.Infrastructure`  | EF Core DbContext, Configurations, DI, Seed Data    |
| **Web**            | `EcommerceIti.Web`             | Controllers, Views, Areas, Extensions, Static Assets |

---

## 🛠️ Tech Stack

| Technology                  | Version / Details           |
|-----------------------------|-----------------------------|
| **.NET**                    | 9.0                         |
| **ASP.NET Core MVC**        | With Razor Pages support    |
| **Entity Framework Core**   | 9.0.12                      |
| **ASP.NET Core Identity**   | Authentication & Authorization |
| **SQL Server**              | Via EF Core SqlServer provider |
| **C#**                      | Primary language (~56%)     |
| **HTML (Razor Views)**      | UI markup (~41%)            |
| **CSS**                     | Styling (~2%)               |
| **JavaScript**              | Client-side interactivity   |
| **Session Management**      | Cart persistence via distributed memory cache |

---

## 📦 Domain Entities

| Entity         | Description                                      |
|----------------|--------------------------------------------------|
| `AppUser`      | Custom user extending ASP.NET Core Identity       |
| `Product`      | Product catalog with details and pricing          |
| `Category`     | Product categorization                            |
| `Order`        | Customer order with status tracking               |
| `OrderItem`    | Individual items within an order                  |
| `OrderStatus`  | Enum for order lifecycle states                   |
| `Address`      | User shipping/billing address                     |

---

## 📁 Project Structure

```
ECommerce_MVC_Project_ITI/
│
├── Domain/
│   ├── Entities/
│   │   ├── Address.cs
│   │   ├── AppUser.cs
│   │   ├── Category.cs
│   │   ├── Order.cs
│   │   ├── OrderItem.cs
│   │   ├── OrderStatus.cs
│   │   └── Product.cs
│   └── EcommerceIti.Domain.csproj
│
├── Application/
│   ├── Interfaces/
│   ├── ViewModels/
│   └── EcommerceIti.Application.csproj
│
├── Infrastructure/
│   ├── Configurations/        # EF Core entity configurations
│   ├── Data/                  # DbContext
│   ├── Seed/                  # Database seeding logic
│   ├── Services/              # Service implementations
│   ├── DependencyInjection.cs # DI registration
│   └── EcommerceIti.Infrastructure.csproj
│
├── Web/
│   ├── Areas/                 # Admin/feature areas
│   ├── Controllers/           # MVC controllers
│   ├── Data/                  # Web-layer data
│   ├── Extensions/            # Extension methods
│   ├── Models/                # View-specific models
│   ├── Services/              # Web services (CartSessionService)
│   ├── Views/                 # Razor views
│   ├── wwwroot/               # Static files (CSS, JS, images)
│   ├── Program.cs             # Application entry point
│   ├── appsettings.json       # Configuration
│   └── EcommerceIti.Web.csproj
│
└── EcommerceIti.sln           # Solution file
```

---

## ✨ Features

- 🔐 **User Authentication & Authorization** — ASP.NET Core Identity with role-based access
- 🛍️ **Product Catalog** — Browse products by categories
- 🛒 **Shopping Cart** — Session-based cart management
- 📦 **Order Management** — Place orders with status tracking (Pending, Confirmed, Shipped, Delivered, Cancelled)
- 📍 **Address Management** — User shipping/billing addresses
- 🌱 **Database Seeding** — Auto-seed initial data on startup
- 🏗️ **Area-based Routing** — Organized feature areas (e.g., Admin panel)
- 💉 **Dependency Injection** — Clean DI setup via `AddInfrastructure()` extension

---

## 🚀 Getting Started

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/) (or SQL Server LocalDB)
- [Visual Studio 2022+](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/)

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/01124833532mo/ECommerce_MVC_Project_ITI.git
   cd ECommerce_MVC_Project_ITI
   ```

2. **Restore dependencies**
   ```bash
   dotnet restore
   ```

3. **Update the connection string** in `Web/appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Your_SQL_Server_Connection_String_Here"
     }
   }
   ```

4. **Apply database migrations**
   ```bash
   cd Web
   dotnet ef database update
   ```

5. **Run the application**
   ```bash
   dotnet run --project Web
   ```

6. Open your browser and navigate to `https://localhost:5001` (or the port shown in the console).

> 💡 **Note:** The database will be automatically seeded with initial data on first run via `DbSeeder.SeedAsync()`.

---

## ⚙️ Configuration

Configuration is managed through `appsettings.json` and `appsettings.Development.json` in the `Web` project.

| Setting                | Description                          |
|------------------------|--------------------------------------|
| `ConnectionStrings`    | Database connection string           |
| `Logging`              | Log levels and providers             |
| Session timeout        | Configured to 30 minutes idle        |

---

## 🤝 Contributing

Contributions are welcome! Please follow these steps:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

---

## 📄 License

This project is open source and available for educational purposes.

---

## 👤 Author

- **GitHub:** [@01124833532mo](https://github.com/01124833532mo)

---

> ⭐ If you found this project helpful, please give it a star!
