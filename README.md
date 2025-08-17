# BonnyBabyStore
BonnyBabyStore is a web application built with ASP.NET Core Razor Pages (.NET 8) for managing an online baby products store. It features user authentication, product management, shopping cart functionality, and an admin area for managing categories, products, orders, and user roles.

## Features

- User registration, login, and profile management
- Product catalog with categories
- Shopping cart and checkout
- Admin area for managing products, categories, orders, users, and roles
- Role-based access control
- Entity Framework Core integration with SQL Server

## Technologies

- ASP.NET Core Razor Pages (.NET 8)
- Entity Framework Core 8 (SQL Server)
- Microsoft Visual Studio Web Code Generation
- Areas for admin functionality
- Responsive layouts with Razor views

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- Visual Studio 2022

### Setup

1. **Clone the repository:**
   
2. **Configure the database connection:**
   - Update the connection string in `appsettings.json` to point to your SQL Server instance.

3. **Apply migrations and update the database:**
   
4. **Run the application:**
      Or use __Start Debugging__ in Visual Studio.

### Folder Structure

- `Areas/Admin/`: Admin controllers, views, and models
- `Controllers/`: Main application controllers
- `Models/`: Entity and view models
- `Views/`: Razor views for user and admin interfaces
- `wwwroot/`: Static files (images, CSS, JS)

## Contributing

Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

## License

This project is licensed under the MIT License.

---

**Note:** For code generation and scaffolding, the project uses `Microsoft.VisualStudio.Web.CodeGeneration.Design`. Entity Framework Core is used for data access.

