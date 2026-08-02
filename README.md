# DotNet8Learning API

A step-by-step learning project built with **ASP.NET Core Web API in .NET 8** on macOS.

This project demonstrates practical implementation of:

* C# fundamentals
* Object-Oriented Programming
* SOLID principles
* ASP.NET Core routing
* Middleware
* Dependency Injection
* Repository pattern
* Service layer
* Entity Framework Core
* SQL Server
* Async programming
* CRUD operations
* Global exception handling

---

## Project Purpose

The purpose of this project is to understand how a real ASP.NET Core Web API request flows through different layers.

The project manages product data using CRUD operations:

* Create a product
* Get all products
* Get a product by ID
* Update a product
* Delete a product

Product data is stored in SQL Server using Entity Framework Core.

---

## Technology Stack

* .NET 8
* ASP.NET Core Web API
* C#
* Entity Framework Core 8
* SQL Server 2022
* Docker
* Swagger/OpenAPI
* Visual Studio Code
* macOS

---

## Project Architecture

```text
Client / Swagger / Postman
          ↓
Global Exception Middleware
          ↓
ProductsController
          ↓
IProductService
          ↓
ProductService
          ↓
IProductRepository
          ↓
ProductRepository
          ↓
ApplicationDbContext
          ↓
Entity Framework Core
          ↓
SQL Server
```

---

## Folder Structure

```text
DotNet8Learning.Api
│
├── Controllers
│   └── ProductsController.cs
│
├── Data
│   └── ApplicationDbContext.cs
│
├── Middleware
│   └── GlobalExceptionMiddleware.cs
│
├── Models
│   ├── Common
│   │   └── BaseEntity.cs
│   └── Product.cs
│
├── Repositories
│   ├── IProductRepository.cs
│   └── ProductRepository.cs
│
├── Services
│   ├── IProductService.cs
│   └── ProductService.cs
│
├── Migrations
│
├── appsettings.json
├── Program.cs
└── DotNet8Learning.Api.csproj
```

---

## OOP Concepts Used

### Encapsulation

The project uses private fields and public properties.

```csharp
private readonly IProductService _productService;
```

The dependency is protected inside the controller and cannot be replaced after construction.

---

### Abstraction

Interfaces define contracts without exposing implementation details.

```csharp
public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAllAsync();
    Task<Product?> GetByIdAsync(int id);
    Task<Product> CreateAsync(Product product);
    Task<bool> UpdateAsync(int id, Product product);
    Task<bool> DeleteAsync(int id);
}
```

The controller and service depend on abstractions instead of concrete implementations.

---

### Inheritance

The `Product` class inherits common properties from `BaseEntity`.

```csharp
public abstract class BaseEntity
{
    public int Id { get; set; }
}
```

```csharp
public class Product : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public bool IsAvailable { get; set; }
}
```

---

### Polymorphism

The project uses interface references with concrete implementations.

```csharp
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
```

A class requests an interface, while ASP.NET Core provides the registered implementation.

---

## SOLID Principles Used

### Single Responsibility Principle

Each class has one main responsibility.

```text
ProductsController
Handles HTTP requests and responses

ProductService
Handles business rules and validation

ProductRepository
Handles database access

ApplicationDbContext
Handles Entity Framework database communication

GlobalExceptionMiddleware
Handles application-wide exceptions
```

---

### Open/Closed Principle

The project can be extended without heavily modifying existing code.

For example, a new repository implementation can be created:

```csharp
public class CachedProductRepository : IProductRepository
{
}
```

The application registration can then be changed without modifying the controller.

---

### Liskov Substitution Principle

Any class implementing `IProductRepository` should be usable wherever `IProductRepository` is required.

```csharp
IProductRepository repository = new ProductRepository(dbContext);
```

Another valid repository implementation should work without changing the service.

---

### Interface Segregation Principle

The project uses small, focused interfaces.

```csharp
IProductRepository
IProductService
```

The controller does not depend directly on database-specific methods.

---

### Dependency Inversion Principle

High-level classes depend on interfaces instead of concrete classes.

```csharp
public ProductsController(IProductService productService)
{
    _productService = productService;
}
```

```csharp
public ProductService(IProductRepository productRepository)
{
    _productRepository = productRepository;
}
```

---

## API Endpoints

### Get all products

```http
GET /api/Products
```

Response:

```json
[
  {
    "id": 1,
    "name": "Laptop",
    "price": 75000,
    "quantity": 5,
    "isAvailable": true
  }
]
```

---

### Get product by ID

```http
GET /api/Products/1
```

Possible responses:

```text
200 OK
404 Not Found
```

---

### Create a product

```http
POST /api/Products
```

Request:

```json
{
  "name": "Monitor",
  "price": 15000,
  "quantity": 10,
  "isAvailable": true
}
```

Response:

```text
201 Created
```

---

### Update a product

```http
PUT /api/Products/1
```

Request:

```json
{
  "name": "Gaming Monitor",
  "price": 22000,
  "quantity": 5,
  "isAvailable": true
}
```

Response:

```text
204 No Content
```

---

### Delete a product

```http
DELETE /api/Products/1
```

Response:

```text
204 No Content
```

---

## Business Validation

The service layer validates product data.

Rules include:

* Product name is required
* Product price must be greater than zero
* Product quantity cannot be negative
* Product availability is calculated from quantity

Example:

```csharp
if (string.IsNullOrWhiteSpace(product.Name))
{
    throw new ArgumentException("Product name is required.");
}

if (product.Price <= 0)
{
    throw new ArgumentException(
        "Product price must be greater than zero.");
}

if (product.Quantity < 0)
{
    throw new ArgumentException(
        "Product quantity cannot be negative.");
}

product.IsAvailable = product.Quantity > 0;
```

---

## SQL Server with Docker

Run SQL Server using Docker:

```bash
docker run \
  -e "ACCEPT_EULA=Y" \
  -e "MSSQL_SA_PASSWORD=Sql@123456" \
  -p 1433:1433 \
  --name dotnet8-sqlserver \
  -v dotnet8-sql-data:/var/opt/mssql \
  -d \
  mcr.microsoft.com/mssql/server:2022-latest
```

Check whether the container is running:

```bash
docker ps
```

Start the container later:

```bash
docker start dotnet8-sqlserver
```

Stop the container:

```bash
docker stop dotnet8-sqlserver
```

---

## Database Connection

The connection string is configured in `appsettings.json`.

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=DotNet8LearningDb;User Id=sa;Password=YOUR_PASSWORD;TrustServerCertificate=True;"
  }
}
```

For production applications, do not store passwords directly in source control. Use environment variables, user secrets, or a secure secret-management service.

---

## Entity Framework Migrations

Create a migration:

```bash
dotnet ef migrations add InitialCreate
```

Apply migrations:

```bash
dotnet ef database update
```

Remove the latest migration before applying it:

```bash
dotnet ef migrations remove
```

List migrations:

```bash
dotnet ef migrations list
```

---

## Run the Project

Clone the repository:

```bash
git clone https://github.com/macosforward-sudo/DotNetLearning.git
```

Move into the project folder:

```bash
cd DotNetLearning/DotNet8Learning.Api
```

Restore packages:

```bash
dotnet restore
```

Build the project:

```bash
dotnet build
```

Apply database migrations:

```bash
dotnet ef database update
```

Run the API:

```bash
dotnet run
```

Open Swagger using the URL displayed in the terminal:

```text
http://localhost:<port>/swagger
```

---

## Dependency Injection Registrations

The application registers dependencies in `Program.cs`.

```csharp
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString(
            "DefaultConnection")));

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();
```

---

## Async Programming

Database operations use asynchronous EF Core methods.

```csharp
await _dbContext.Products
    .AsNoTracking()
    .ToListAsync();
```

```csharp
await _dbContext.Products.AddAsync(product);
await _dbContext.SaveChangesAsync();
```

This prevents blocking request threads while waiting for database operations.

---

## Future Improvements

Planned enhancements include:

* Registering global exception middleware
* DTOs for request and response models
* FluentValidation
* AutoMapper
* JWT authentication
* Role-based authorization
* Pagination and filtering
* Logging with Serilog
* Unit testing with xUnit
* Generic repository pattern
* Clean Architecture
* Docker Compose
* API versioning

---

## Author

**Mritunjay Sharma**

Senior .NET Developer learning and practising .NET 8 Web API development.
