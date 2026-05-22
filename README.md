# 🍽️ Restaurant Management — Backend API

> **Portfolio Project** | A full-featured RESTful backend API built to demonstrate practical skills in backend architecture, security, and clean code design using **C# / ASP.NET Core 8 / SQL Server**.

[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![C#](https://img.shields.io/badge/C%23-Latest-239120?logo=csharp)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![SQL Server](https://img.shields.io/badge/SQL_Server-2019+-CC2927?logo=microsoftsqlserver)](https://www.microsoft.com/sql-server)
[![EF Core](https://img.shields.io/badge/EF_Core-8.0-512BD4?logo=dotnet)](https://docs.microsoft.com/en-us/ef/core/)
[![Platform](https://img.shields.io/badge/Platform-Windows-0078D6?logo=windows)](https://www.microsoft.com/)

---

## 💡 About This Project

This project is a **backend API for a restaurant management system**, built from scratch as a learning and portfolio exercise. The goal was to design and implement a real-world-scale API that goes beyond basic CRUD — applying proper architectural patterns, a full security layer, and professional code organization the same way they would appear in a production codebase.

The system covers the full lifecycle of restaurant operations:

- **Employee management** with role-based access
- **Menu management** with filtering and availability tracking
- **Table management** with multi-filter search
- **Order and order-detail management** with status tracking
- **JWT authentication** with refresh token rotation
- **Reference data management** (job roles, item types, statuses)

> **Platform Note:** Windows only — the logging system uses the Windows Event Log via the `System.Diagnostics` API.

---

## 🛠️ Tech Stack

| Category | Technology |
|---|---|
| Language | C# (.NET 8) |
| Framework | ASP.NET Core Web API |
| Database | Microsoft SQL Server |
| ORM | Entity Framework Core 8 (Fluent API, projections) |
| Raw Data Access | ADO.NET — Stored Procedures |
| Data Access Strategy | **Switchable at runtime** — EF Core or raw SQL, configured in `appsettings.json` |
| Authentication | JWT Bearer Tokens (HMAC-SHA256) |
| Password Hashing | BCrypt.Net (salted) |
| API Documentation | Swagger / OpenAPI with JWT support |
| Logging | Windows Event Log (`System.Diagnostics`) |
| Platform | Windows |

---

## 🏗️ Architecture Overview

The solution is split into **five independent projects**, each with a single, clearly defined responsibility. Layers only communicate with the layer directly below — always through an **interface**, never through a concrete class.

```
RestaurantManagement/
│
├── APILayer/                       ← Presentation Layer (HTTP)
├── BusinessLayerRestaurant/        ← Business Logic Layer
├── ContractsLayerRestaurant/       ← Shared DTOs & Contracts
├── DataLayerRestaurant/            ← Data Access Layer (Repositories)
└── DomainLayer/                    ← Domain Entities
```

### Request Flow

```
HTTP Request
    │
    ▼
[APILayer] — Controller receives and validates the request
    │ calls interface ↓
[BusinessLayerRestaurant] — Service applies business rules, hashing, authorization
    │ calls interface ↓
[DataLayerRestaurant] — Repository executes either EF Core query or Stored Procedure
    │ SQL ↓
[SQL Server Database]
    │ result ↑
[DataLayerRestaurant] — Maps result → Domain Entity
    │ ↑
[BusinessLayerRestaurant] — Applies business logic on entity
    │ ↑
[APILayer] — Maps entity → DTO Response → Wrapped in ApiResponse<T>
    │
    ▼
HTTP Response (JSON)
```

---

## 🔄 Dual Data Access Strategy

One of the defining features of v4 is full support for **two independent data access implementations** that are completely interchangeable at the repository level.

### How It Works

The `appsettings.json` file contains a single switch:

```json
"DataType": {
  "DataAccessStrategy": "EF"
}
```

Setting this to `"EF"` activates the Entity Framework Core implementation for every entity. Setting it to `"SqlServer"` activates the raw ADO.NET + Stored Procedure implementation. The business layer has **zero awareness** of which is active — it always calls the same interface.

### Wiring in DI

Each entity's DI extension method performs the switch:

```csharp
public static IServiceCollection AddEmployeesServices(
    this IServiceCollection Services, string DataAccessStrategy)
{
    if (DataAccessStrategy == "EF")
    {
        Services.AddScoped<IEmployeesRepositoryReader, EmployeesRepositoryReaderEF>();
        Services.AddScoped<IEmployeesRepositoryWriter, EmployeesRepositoryWriterEF>();
    }
    else
    {
        Services.AddScoped<IEmployeesRepositoryReader, EmployeesRepositoryReader>();
        Services.AddScoped<IEmployeesRepositoryWriter, EmployeesRepositoryWriter>();
    }
    // ...
}
```

### Implementation Comparison

| Feature | EF Core (`"EF"`) | Raw ADO.NET (`"SqlServer"`) |
|---|---|---|
| Query definition | LINQ + `.Select()` projections | Stored Procedures |
| Join loading | Projected in query | Composition pattern |
| Change tracking | Disabled (`AsNoTracking`) | N/A |
| Transactions | `BeginTransactionAsync()` | Database-side |
| N+1 prevention | `IOrderDetailBatchLoader` | `IOrderDetailBatchLoader` |
| Type safety | Compile-time | Runtime reader indexing |

Both implementations produce identical `Domain Entity` output. The business layer cannot tell them apart.

---

## 🧩 Design Patterns Applied

### Repository Pattern
Every entity has its own dedicated repository class that is the **only** place in the entire codebase that executes data access logic. The business layer has zero awareness of how data is fetched — it only knows the repository interface.

### Reader / Writer Separation
Every repository and every service class is split into **two distinct classes**:

- `Reader` — handles all read operations (`GET`, `SELECT`)
- `Writer` — handles all mutation operations (`POST`, `PUT`, `DELETE`)

```
EmployeesRepository
    ├── EmployeesRepositoryReader  (EF or SQL)
    └── EmployeesRepositoryWriter  (EF or SQL)
```

### Composition / Batch Loading Pattern
Related data loading is handled through composable loader classes rather than SQL JOINs or EF `.Include()` chains. The `IOrderDetailBatchLoader` fetches all order details for a batch of orders in a single query, then distributes them in memory — eliminating the N+1 problem entirely.

```
GetAllOrders(page)
    │
    ├── Query: fetch paginated orders (without details)
    └── IOrderDetailBatchLoader.LoadBatchAsync(orders)
            └── Query: fetch all details WHERE OrderID IN (...)
            └── Group by OrderID → assign to each Order in memory
```

This pattern is implemented identically for both the EF and SQL strategies.

### Container Pattern
Each service exposes a **Container class** that holds references to both Reader and Writer, as well as any dependent service interfaces. The layer above receives one clean, unified interface.

### Dependency Injection via Extension Methods
All DI registrations are extracted out of `Program.cs` into dedicated Extension Method classes. Each entity has its own file. `Program.cs` stays minimal and readable.

### Base Controller
All controllers inherit from `BaseController`, which provides a generic `CreateResponse<T>()` helper that produces the unified JSON envelope for every response.

---

## 🗃️ EF Core — Entity Configuration

All entity configurations are defined using the **Fluent API** in dedicated `IEntityTypeConfiguration<T>` classes, loaded via `modelBuilder.ApplyConfigurationsFromAssembly()`. No data annotations are used on domain entities.

```csharp
// MenuItemConfiguration.cs (example)
builder.ToTable("tbMenuItems");
builder.HasIndex(e => e.ItemName, "UQ_tbMenuItems_Name").IsUnique();
builder.Property(e => e.Price).HasColumnType("decimal(10,2)");
builder.ToTable(t => t.HasCheckConstraint("CK_MenuItems_Price", "[Price] > 0"));

builder.HasOne(e => e.TypeItem)
       .WithMany(t => t.MenuItems)
       .HasForeignKey(e => e.TypeItemID)
       .OnDelete(DeleteBehavior.Restrict)
       .HasConstraintName("FK_MenuItems_TypeItem");
```

All read queries use `AsNoTracking()` and explicit `.Select()` projections — keeping EF usage lean and predictable.

---

## 🔒 Security Layer

### JWT Authentication
- Bearer token authentication using `Microsoft.AspNetCore.Authentication.JwtBearer`
- Tokens signed with **HMAC-SHA256**
- Each token carries three claims: `NameIdentifier` (employee ID), `Name` (username), `Role` (job role)
- The secret key is loaded from an **environment variable** — never stored in source code or config files

### Refresh Token System with Rotation
- On login: short-lived access token + 7-day refresh token
- Raw refresh token is **never stored** — only its BCrypt hash is persisted
- Every `/refresh` call issues a **brand new refresh token** and invalidates the previous one
- Logout revokes the token via a `RevokedAt` timestamp

### Role-Based Authorization (RBAC)

| Role | Description |
|---|---|
| `Manager` | Full administrative access |
| `Chef` | Kitchen-level access — menu and order management |
| `Sous Chef` | Assistant kitchen access |
| `Waiter` | Table and order access, limited to own records |
| `Cleaner` | Table status updates only |

### Custom Resource-Level Authorization Policies

| Policy Name | Rule |
|---|---|
| `EmployeeOwnerOrAdmin` | Manager can access any employee. Others can only access their own record. |
| `EmployeeByUserNameOwnerOrAdmin` | Same ownership rule, applied to username-based lookups. |
| `WaiterOwnerOrAdmin` | Managers, Chefs, Sous Chefs can manage any order. A Waiter can only touch orders they created. |

### BCrypt Password Hashing
All passwords use BCrypt with automatic per-hash salting. Verification uses `BCrypt.Verify()` — the raw password is never compared directly.

### Rate Limiting (Per IP, Fixed Window)

| Policy | Limit | Window |
|---|---|---|
| `Auth` — login, refresh | 5 requests | 1 minute |
| `GetAll` | 30 requests | 1 minute |
| `GetOne` | 60 requests | 1 minute |
| `Add` | 10 requests | 1 minute |
| `Update` | 15 requests | 1 minute |
| `Delete` | 5 requests | 1 minute |

### CORS Policy
A named CORS policy (`RMApiCorsPolicy`) restricts cross-origin requests to a configured list of allowed origins.

---

## 📡 API Endpoints

> All endpoints require `Authorization: Bearer <token>` unless marked **Public**.

### 🔑 Authentication — `/api/Auth`

| Method | Route | Access | Description |
|---|---|---|---|
| POST | `/login` | Public | Authenticate. Returns access + refresh token. |
| POST | `/refresh` | Public | Exchange refresh token for a new token pair. |
| POST | `/logout` | Authenticated | Revoke the current refresh token. |

### 👤 Employees — `/api/Employees`

| Method | Route | Roles | Description |
|---|---|---|---|
| GET | `/` | Manager | Get all employees (paginated) |
| GET | `/{ID}` | Owner or Manager | Get by ID |
| GET | `/user-name/{userName}` | Owner or Manager | Get by username |
| POST | `/` | Manager | Create employee |
| PUT | `/` | Manager | Update employee |
| DELETE | `/{ID}` | Manager | Delete employee |
| POST | `/changed-password` | Self only | Change own password |

### 📦 Orders — `/api/Orders`

| Method | Route | Roles | Description |
|---|---|---|---|
| GET | `/` | All staff | Get all orders (paginated) |
| GET | `/{ID}` | All staff | Get by ID |
| GET | `/filter` | All staff | Filter by table, employee, or status |
| POST | `/` | Manager, Waiter (own) | Create order |
| PUT | `/` | All staff (waiters: own only) | Update order |
| DELETE | `/{ID}` | Manager, Chef, Sous Chef | Delete order |

### 📋 Order Details — `/api/OrderDetails`

| Method | Route | Access | Description |
|---|---|---|---|
| GET | `/` | All staff | Get all (paginated) |
| GET | `/{ID}` | All staff | Get by ID |
| GET | `/all-orderid/{orderID}` | All staff | Get all items for an order |
| POST | `/` | Manager, Waiter (own orders) | Add item to order |
| PUT | `/` | All staff (waiters: own orders) | Update detail |
| DELETE | `/{ID}` | Manager, Waiter | Remove item |

### 🍕 Menu Items — `/api/MenuItems`

| Method | Route | Access | Description |
|---|---|---|---|
| GET | `/` | **Public** | Get all (paginated) |
| GET | `/{ID}` | **Public** | Get by ID |
| GET | `/all-availables` | **Public** | Get all available items |
| GET | `/all-filters` | **Public** | Filter by type and status |
| POST | `/` | Manager, Chef | Add menu item |
| PUT | `/` | Manager, Chef, Sous Chef | Update menu item |
| DELETE | `/{ID}` | Manager, Chef | Delete menu item |

### 🪑 Tables — `/api/Tables`

| Method | Route | Access | Description |
|---|---|---|---|
| GET | `/` | **Public** | Get all (paginated) |
| GET | `/{ID}` | **Public** | Get by ID |
| GET | `/all-nopagination` | **Public** | Get all (no pagination) |
| GET | `/all-availables` | **Public** | Get available tables |
| GET | `/table-name` | **Public** | Find by name/number |
| GET | `/allfilter-seats` | **Public** | Filter by seat count |
| GET | `/allfilter-statustables` | **Public** | Filter by status |
| GET | `/allfilter-global` | **Public** | Filter by status AND seats |
| POST | `/` | Manager | Add table |
| PUT | `/` | Manager, Cleaner | Update table |
| DELETE | `/{ID}` | Manager | Delete table |

### 🔧 Reference / Lookup Data

| Route | Description |
|---|---|
| `/api/JobRoles` | Full CRUD for job roles |
| `/api/TypeItems` | Full CRUD for menu item categories |
| `/api/StatusOrders` | Full CRUD for order statuses |
| `/api/StatusMenus` | Full CRUD for menu availability statuses |
| `/api/StatusTables` | Full CRUD for table statuses |
| `/api/Settings` | System settings management |

---

## 📐 Unified Response Format

```json
// Success
{ "statusCode": 200, "message": "Found Successfully!", "data": { } }

// List
{ "statusCode": 200, "message": "Row: 12", "data": [ ] }

// Error
{ "statusCode": 404, "message": "Not Found!", "data": null }
```

---

## 🚨 Global Exception Handling

`GlobalExceptionMiddleware` catches every unhandled exception, maps it to the correct HTTP status code, and returns the unified JSON format. No stack traces are ever exposed to the client.

| Exception Type | HTTP Status |
|---|---|
| `ArgumentNullException` / `ArgumentException` / `ArgumentOutOfRangeException` | 400 |
| `FormatException` | 400 |
| `AuthenticationException` | 401 |
| `UnauthorizedAccessException` | 403 |
| `KeyNotFoundException` | 404 |
| `InvalidOperationException` | 409 |
| Any unrecognized exception | 500 |

---

## ⚙️ Setup & Installation

**Prerequisites:** Visual Studio 2022+, .NET 8 SDK, SQL Server, Windows OS.

**1. Clone**
```bash
git clone https://github.com/Compiler-A/RestaurantManagement.git
```

**2.** Open `RestaurantProjectv2.slnx` in Visual Studio 2022.

**3. Configure `APILayer/appsettings.json`**
```json
{
  "MySettings": {
    "ConnectionString": "Server=.;Database=RestaurantManager;User Id=sa;Password=YOUR_PASSWORD;Encrypt=False;TrustServerCertificate=True;",
    "RowsPerPage": 12
  },
  "Jwt": {
    "Issuer": "RMAPI",
    "Audience": "RMAPIEmployees",
    "ExpirationMinutes": 10
  },
  "CORS": {
    "Web1": "https://localhost:7292",
    "Web2": "http://localhost:5223"
  },
  "DataType": {
    "DataAccessStrategy": "EF"
  }
}
```

**4. Set the JWT secret key** (never stored in files — must be an environment variable):
```powershell
# PowerShell — run as Administrator
[System.Environment]::SetEnvironmentVariable("JWT_SECRET_KEY", "your-32-char-minimum-secret-key", "Machine")
```

**5.** Set up the database (run SQL scripts for `"SqlServer"` strategy, or use EF migrations for `"EF"` strategy — both target the same schema).

**6.** Press `F5`. Swagger UI opens at `https://localhost:{port}/swagger`.

**7.** Call `POST /api/Auth/login`, copy the `accessToken`, click **Authorize** in Swagger, enter `Bearer <token>`.

---

## 📁 Project Structure

```
RestaurantManagement/
├── APILayer/
│   ├── Controllers/         ← One per entity + BaseController + AuthController
│   ├── Authorization/       ← Custom IAuthorizationHandler implementations
│   ├── Extensions/          ← DI wiring split by concern (Configuration, Security, Services)
│   ├── Filters/             ← ValidateModelAttribute, Swagger filter, rate limit constants
│   ├── Middleware/          ← GlobalExceptionMiddleware
│   ├── ApiResponse.cs
│   └── Program.cs
│
├── BusinessLayerRestaurant/
│   ├── Operations/          ← Reader, Writer, Container per entity
│   ├── Services/            ← Facade service per entity
│   └── Mapper/              ← Entity → DTO Response mappers
│
├── ContractsLayerRestaurant/
│   ├── DTORequest/          ← Create, Update, Filter DTOs
│   ├── DTOResponse/         ← Response DTOs
│   ├── Configuration/       ← JwtSettings, clsMyLogger
│   └── Interfaces/
│       ├── Repositories/    ← Reader/Writer/combined interfaces per entity
│       └── Services/        ← Reader/Writer/Container interfaces per entity
│
├── DataLayerRestaurant/
│   ├── Classes/
│   │   ├── EF/              ← Entity Framework Core implementations
│   │   ├── SQL/             ← Raw ADO.NET + Stored Procedure implementations
│   │   └── Repository/      ← Composition repositories (delegate to EF or SQL)
│   ├── Data/
│   │   ├── AppDBContext.cs
│   │   └── Configuration/   ← Fluent API entity configurations
│   ├── Mapper/              ← SqlDataReader → Entity (SQL strategy only)
│   └── clsMySettings.cs
│
└── DomainLayer/
    └── Entities/            ← Pure domain classes, no annotations
```

---

## 🧠 Key Technical Decisions

**Why support both EF Core and raw ADO.NET?**
Building the raw SQL implementation first gave full understanding of what happens at the database level. Adding EF Core on top of the same interfaces made the comparison concrete — both strategies live side by side, and switching between them is a one-line config change. This demonstrates understanding of both approaches rather than defaulting to one.

**Why the Batch Loader instead of `.Include()` or lazy loading?**
Lazy loading causes N+1 queries. EF's eager `.Include()` generates a JOIN that can multiply result set size. The batch loader is a third option: fetch parents first (lean), then fetch all children in one `WHERE IN (...)` query, then merge in memory. It scales predictably, is explicit about what hits the database, and works identically for both strategies.

**Why separate Reader and Writer classes?**
Single Responsibility at the class level — a class that reads data cannot accidentally write. Each class is smaller, more focused, and independently testable.

**Why a dedicated Contracts layer?**
Neither Business nor Data layer depend on each other's types — only the shared contract. This makes the system open for a second consumer without changing existing layers.

---

## 📜 Version History

| Version | Changes |
|---|---|
| **v1.0** | N-Layer architecture, full CRUD, async/await, raw SQL + Stored Procedures |
| **v2.0** | BCrypt password hashing (replaced SHA-256), hashing moved to Business Layer |
| **v3.0** | JWT + refresh token rotation, RBAC, resource-level authorization, rate limiting, CORS, Swagger JWT, Windows Event Log |
| **v4.0** | **Entity Framework Core 8** added as a second fully supported data access strategy; switchable via `appsettings.json`; Fluent API entity configurations; `IOrderDetailBatchLoader` pattern eliminates N+1 for both EF and SQL |

---

## 👨‍💻 Author

Built by [Compiler-A](https://github.com/Compiler-A) as a backend portfolio project.