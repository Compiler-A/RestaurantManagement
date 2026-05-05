# 🍽️ Restaurant Management — Backend API

> **Portfolio Project** | A full-featured RESTful backend API built to demonstrate practical skills in backend architecture, security, and clean code design using **C# / ASP.NET Core 8 / SQL Server**.

[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![C#](https://img.shields.io/badge/C%23-Latest-239120?logo=csharp)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![SQL Server](https://img.shields.io/badge/SQL_Server-2019+-CC2927?logo=microsoftsqlserver)](https://www.microsoft.com/sql-server)
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
| ORM / Data Access | Raw ADO.NET — Stored Procedures only (no ORM) |
| Authentication | JWT Bearer Tokens (HMAC-SHA256) |
| Password Hashing | BCrypt.Net (salted) |
| API Documentation | Swagger / OpenAPI with JWT support |
| Logging | Windows Event Log (`System.Diagnostics`) |
| Platform | Windows |

---

## 🏗️ Architecture Overview

The solution is split into **five independent projects**, each with a single, clearly defined responsibility. Layers only communicate with the layer directly below — always through an **interface**, never through a concrete class. This enforces loose coupling and makes each layer independently replaceable.

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
[DataLayerRestaurant] — Repository executes a Stored Procedure
    │ SQL ↓
[SQL Server Database]
    │ result ↑
[DataLayerRestaurant] — Maps SqlDataReader → Domain Entity
    │ ↑
[BusinessLayerRestaurant] — Applies business logic on entity
    │ ↑
[APILayer] — Maps entity → DTO Response → Wrapped in ApiResponse<T>
    │
    ▼
HTTP Response (JSON)
```

---

## 🧩 Design Patterns Applied

### Repository Pattern
Every entity has its own dedicated repository class that is the **only** place in the entire codebase that executes SQL. The business layer has zero awareness of SQL Server — it only knows the repository interface. Swapping the database engine would require changing only the repository classes.

### Reader / Writer Separation
Every repository and every service class is split into **two distinct classes**:

- `Reader` — handles all read operations (`GET`, `SELECT`)
- `Writer` — handles all mutation operations (`POST`, `PUT`, `DELETE`)

Both expose a single combined interface to the layer above. This enforces the **Single Responsibility Principle** at the class level — a reader class can never accidentally modify data.

```
clsEmployeesRepository
    ├── clsEmployeesRepositoryReader  (GetDataAsync, GetAllDataAsync)
    └── clsEmployeesRepositoryWriter  (CreateDataAsync, UpdateDataAsync, DeleteDataAsync)
```

### Composition Pattern
Related data loading (navigation properties) is handled through composable loader classes rather than SQL JOINs. When fetching an `Order`, a composition loader independently fetches and attaches the related `Employee`, `Table`, and `StatusOrder` objects. New relationships can be added without modifying existing SQL or repository code.

```
clsOrdersService
    └── Composition Loader
            ├── Employee Loader
            ├── Table Loader
            └── StatusOrder Loader
```

### Container / Facade Pattern
Each service and repository exposes a **Container class** that holds references to both the Reader and Writer. The layer above receives one clean interface without needing to know how reading and writing are internally separated.

### Dependency Injection via Extension Methods
All DI registrations are extracted out of `Program.cs` into dedicated Extension Method classes under `Extensions/Services/`. Each entity has its own file. `Program.cs` stays minimal and readable — it only wires things together at a high level.

### Base Controller
All controllers inherit from `BaseController`, which provides a generic `CreateResponse<T>()` helper that produces the unified JSON envelope for every response.

---

## 🔒 Security Layer

### JWT Authentication
- Bearer token authentication using `Microsoft.AspNetCore.Authentication.JwtBearer`
- Tokens signed with **HMAC-SHA256**
- Each token carries three claims: `NameIdentifier` (employee ID), `Name` (username), `Role` (job role)
- Expiration time, issuer, and audience are configurable via `appsettings.json`
- The secret key is loaded from an **environment variable** — it is never stored in source code or config files

### Refresh Token System with Rotation
- On login, the API issues **two tokens**: a short-lived access token and a 7-day refresh token
- The raw refresh token is **never stored** — only its BCrypt hash is persisted in the database
- Every `/refresh` call issues a **brand new refresh token** and invalidates the previous one (rotation), preventing replay attacks
- Logout revokes the token by recording a `RevokedAt` timestamp

### Role-Based Authorization (RBAC)
The system defines four roles with distinct permission levels:

| Role | Description |
|---|---|
| `Manager` | Full administrative access across all resources |
| `Chef` | Kitchen-level access — menu and order management |
| `Sous Chef` | Assistant kitchen access |
| `Waiter` | Table and order access, limited to own records |

### Custom Resource-Level Authorization Policies
Beyond role checks, three custom `IAuthorizationHandler` implementations enforce **ownership-level access control**:

| Policy Name | Rule |
|---|---|
| `EmployeeOwnerOrAdmin` | A Manager can access any employee record. Any other role can only access their own record. |
| `EmployeeByUserNameOwnerOrAdmin` | Same ownership rule, applied to username-based lookups. |
| `WaiterOwnerOrAdmin` | Managers, Chefs, and Sous Chefs can manage any order. A Waiter can only create or update orders they personally created. |

### BCrypt Password Hashing
All passwords (and refresh token hashes) use BCrypt, which generates a unique salt per hash automatically. Plain SHA-256 hashing is also available in `clsHashingService` but is not used for authentication — BCrypt is the active standard. Verification uses `BCrypt.Verify()` — the raw password is never compared directly.

### Rate Limiting (Per IP, Fixed Window)
Every endpoint is assigned a rate limit policy. Limits are scoped per client IP address:

| Policy | Limit | Window |
|---|---|---|
| `Auth` — login, refresh | 5 requests | 1 minute |
| `GetAll` — paginated list endpoints | 30 requests | 1 minute |
| `GetOne` — single record endpoints | 60 requests | 1 minute |
| `Add` — POST endpoints | 10 requests | 1 minute |
| `Update` — PUT endpoints | 15 requests | 1 minute |
| `Delete` — DELETE endpoints | 5 requests | 1 minute |

Returns `HTTP 429 Too Many Requests` when exceeded.

### CORS Policy
A named CORS policy (`RMApiCorsPolicy`) restricts cross-origin requests to a configured list of allowed origins, preventing unauthorized frontends from consuming the API.

---

## 📡 API Endpoints

> All endpoints require a valid `Authorization: Bearer <token>` header unless marked as **Public**.

### 🔑 Authentication — `/api/Auth`

| Method | Route | Access | Description |
|---|---|---|---|
| POST | `/login` | Public | Authenticate with username + password. Returns access token + refresh token. |
| POST | `/refresh` | Public | Exchange a valid refresh token for a new access + refresh token pair. |
| POST | `/logout` | Authenticated | Revoke the current refresh token. |

### 👤 Employees — `/api/Employees`

| Method | Route | Roles | Description |
|---|---|---|---|
| GET | `/` | Manager | Get all employees (paginated) |
| GET | `/{ID}` | Owner or Manager | Get employee by ID |
| GET | `/user-name/{userName}` | Owner or Manager | Get employee by username |
| POST | `/` | Manager | Create new employee |
| PUT | `/` | Manager | Update employee |
| DELETE | `/{ID}` | Manager | Delete employee |
| POST | `/changed-password` | Self only | Change own password (validates current password first) |

### 📦 Orders — `/api/Orders`

| Method | Route | Roles | Description |
|---|---|---|---|
| GET | `/` | Manager, Chef, Sous Chef, Waiter | Get all orders (paginated) |
| GET | `/{ID}` | Manager, Chef, Sous Chef, Waiter | Get order by ID |
| GET | `/filter` | Manager, Chef, Sous Chef, Waiter | Filter orders by table, employee, or status |
| POST | `/` | Manager, Waiter (own orders only) | Create new order |
| PUT | `/` | Manager, Chef, Sous Chef, Waiter (own orders) | Update order |
| DELETE | `/{ID}` | Manager, Chef, Sous Chef | Delete order |

### 📋 Order Details — `/api/OrderDetails`

| Method | Route | Access | Description |
|---|---|---|---|
| GET | `/` | Authenticated | Get all order details (paginated) |
| GET | `/{ID}` | Authenticated | Get order detail by ID |
| GET | `/all-orderid/{orderID}` | Authenticated | Get all items for a specific order |
| POST | `/` | Authenticated | Add item to an order |
| PUT | `/` | Authenticated | Update order detail |
| DELETE | `/{ID}` | Authenticated | Remove item from an order |

### 🍕 Menu Items — `/api/MenuItems`

| Method | Route | Access | Description |
|---|---|---|---|
| GET | `/` | **Public** | Get all menu items (paginated) |
| GET | `/{ID}` | **Public** | Get menu item by ID |
| GET | `/all-availables` | **Public** | Get all currently available items |
| GET | `/all-filters` | **Public** | Filter by type, availability status, price range |
| POST | `/` | Manager, Chef | Add new menu item |
| PUT | `/` | Manager, Chef, Sous Chef | Update menu item |
| DELETE | `/{ID}` | Manager, Chef | Delete menu item |

### 🪑 Tables — `/api/Tables`

| Method | Route | Access | Description |
|---|---|---|---|
| GET | `/` | **Public** | Get all tables (paginated) |
| GET | `/{ID}` | **Public** | Get table by ID |
| GET | `/all-nopagination` | **Public** | Get all tables (no pagination) |
| GET | `/all-availables` | **Public** | Get all currently available tables |
| GET | `/table-name` | **Public** | Find a table by its name/number |
| GET | `/allfilter-seats` | **Public** | Filter tables by seat count |
| GET | `/allfilter-statustables` | **Public** | Filter tables by status |
| GET | `/allfilter-global` | **Public** | Filter tables by status AND seat count combined |
| POST | `/` | Manager | Add new table |
| PUT | `/` | Manager, Cleaner | Update table |
| DELETE | `/{ID}` | Manager | Delete table |

### 🔧 Reference / Lookup Data

| Base Route | Description |
|---|---|
| `/api/JobRoles` | Full CRUD for employee job roles (Manager, Chef, Waiter, etc.) |
| `/api/TypeItems` | Full CRUD for menu item categories |
| `/api/StatusOrders` | Full CRUD for order status values (e.g. Pending, Preparing, Delivered) |
| `/api/StatusMenus` | Full CRUD for menu item availability statuses |
| `/api/StatusTables` | Full CRUD for table statuses (Available, Occupied, Reserved, etc.) |
| `/api/Settings` | System settings management (e.g. rows per page for pagination) |

---

## 🎭 Role-Based Access Summary

| Action | Manager | Chef | Sous Chef | Waiter |
|---|:---:|:---:|:---:|:---:|
| View all employees | ✅ | ❌ | ❌ | ❌ |
| Manage employees (create/update/delete) | ✅ | ❌ | ❌ | ❌ |
| View own profile | ✅ | ✅ | ✅ | ✅ |
| Change own password | ✅ | ✅ | ✅ | ✅ |
| View all orders | ✅ | ✅ | ✅ | ✅ |
| Create order | ✅ | ❌ | ❌ | ✅ (own only) |
| Update order | ✅ | ✅ | ✅ | ✅ (own only) |
| Delete order | ✅ | ✅ | ✅ | ❌ |
| Add/delete menu items | ✅ | ✅ | ❌ | ❌ |
| Update menu items | ✅ | ✅ | ✅ | ❌ |
| Manage tables | ✅ | ❌ | ❌ | ❌ |
| Browse menu & tables (no login needed) | — | — | — | — |

---

## 📐 Unified Response Format

Every endpoint — success or failure — returns the same JSON envelope. No endpoint returns a bare object or an inconsistent shape.

**Success:**
```json
{
  "statusCode": 200,
  "message": "Found Successfully!",
  "data": { }
}
```

**List response:**
```json
{
  "statusCode": 200,
  "message": "Row: 12",
  "data": [ ]
}
```

**Auth response:**
```json
{
  "statusCode": 200,
  "message": "Login Successfully!",
  "data": {
    "accessToken": "eyJhbGciOiJIUzI1NiIs...",
    "refreshToken": "randomly-generated-64-byte-token..."
  }
}
```

**Error:**
```json
{
  "statusCode": 404,
  "message": "Not Found!",
  "data": null
}
```

---

## 🚨 Global Exception Handling

`GlobalExceptionMiddleware` wraps the entire request pipeline. Every unhandled exception is caught here, mapped to the correct HTTP status code, and returned in the unified JSON format. No raw stack traces are ever exposed to the client. All exceptions are also written to the Windows Event Log via `IMyLogger`.

| Exception Type | HTTP Status |
|---|---|
| `ArgumentNullException` | 400 Bad Request |
| `ArgumentException` | 400 Bad Request |
| `ArgumentOutOfRangeException` | 400 Bad Request |
| `FormatException` | 400 Bad Request |
| `AuthenticationException` | 401 Unauthorized |
| `UnauthorizedAccessException` | 403 Forbidden |
| `KeyNotFoundException` | 404 Not Found |
| `InvalidOperationException` | 409 Conflict |
| Any unrecognized exception | 500 Internal Server Error |

Model validation errors are caught even earlier — by the `ValidateModelAttribute` action filter — before the controller action runs. The response includes a descriptive message for every failing field.

---

## ⚙️ Setup & Installation

### Prerequisites

- [Visual Studio 2022+](https://visualstudio.microsoft.com/)
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- SQL Server (any edition — Express works fine)
- Windows OS

### Steps

**1. Clone the repository**
```bash
git clone https://github.com/Compiler-A/RestaurantManagement.git
```

**2. Open the solution**

Open `RestaurantProjectv2.slnx` in Visual Studio 2022.

**3. Configure the database**

Edit `APILayer/appsettings.json` and update the connection string:
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
  }
}
```

**4. Set the JWT secret key as an environment variable**

The secret key is **not stored in any file** — it must be set as a system environment variable named `JWT_SECRET_KEY`. It must be at least 32 characters long.

```powershell
# PowerShell (run as Administrator)
[System.Environment]::SetEnvironmentVariable("JWT_SECRET_KEY", "your-very-secure-secret-key-here", "Machine")
```

**5. Run the database scripts**

Execute the SQL scripts to create the database schema, tables, stored procedures, and initial seed data on your SQL Server instance.

**6. Run the project**

Press `F5` in Visual Studio. Swagger UI is available at:
```
https://localhost:{port}/swagger
```

**7. Authenticate in Swagger**

1. Call `POST /api/Auth/login` with valid credentials
2. Copy the `accessToken` from the response `data` field
3. Click **Authorize** in Swagger UI and enter: `Bearer <your_token>`
4. All protected endpoints are now accessible for the duration of the token

---

## 📁 Project Structure

```
RestaurantManagement/
│
├── APILayer/                                    ← ASP.NET Core Web API project
│   ├── Controllers/
│   │   ├── BaseController.cs                   ← Generic CreateResponse<T>() helper
│   │   ├── AuthController.cs                   ← Login, Refresh, Logout
│   │   ├── APIEmployees.cs
│   │   ├── APIOrders.cs
│   │   ├── APIOrderDetails.cs
│   │   ├── APIMenuItems.cs
│   │   ├── APITables.cs
│   │   ├── APIJobRoles.cs
│   │   ├── APITypeItems.cs
│   │   ├── APIStatusOrders.cs
│   │   ├── APIStatusMenus.cs
│   │   ├── APIStatusTables.cs
│   │   └── APISettings.cs
│   │
│   ├── Authorization/
│   │   ├── Employee/
│   │   │   ├── EmployeeOwnerOrAdminRequirement.cs
│   │   │   ├── EmployeeOwnerOrAdminHandler.cs
│   │   │   ├── EmployeeUserNameOwnerOrAdminRequirement.cs
│   │   │   └── EmployeeUserNameOwnerOrAdminHandler.cs
│   │   └── Order/
│   │       ├── WaiterOwnerOrAdminRequirement.cs
│   │       └── WaiterOwnerOrAdminHandler.cs
│   │
│   ├── Extensions/
│   │   ├── Configuration/
│   │   │   ├── JwtSettingConfigurationExtension.cs
│   │   │   └── MySettingConfigurationExtension.cs
│   │   └── Security/
│   │       ├── AuthenticationSecurityExtension.cs  ← JWT Bearer setup
│   │       ├── AuthorizationSecurityExtension.cs   ← Policy registration
│   │       ├── CorsSecurityExtension.cs
│   │       ├── RateLimitingSecurityExtension.cs
│   │       └── SwaggerGenSecurityExtension.cs
│   │
│   ├── Filters/
│   │   ├── ValidateModelAttribute.cs              ← Model validation action filter
│   │   ├── DefaultResponsesOperationFilter.cs     ← Swagger standard response docs
│   │   └── NameRateLimitPolicies.cs               ← Rate limit policy name constants
│   │
│   ├── Middleware/
│   │   └── GlobalExceptionMiddleware.cs
│   │
│   ├── ApiResponse.cs                             ← Generic wrapper { statusCode, message, data }
│   └── Program.cs                                 ← App bootstrap (minimal, clean)
│
├── BusinessLayerRestaurant/                       ← Business logic project
│   ├── Classes/
│   │   ├── JwtSettings.cs                         ← JWT config model
│   │   ├── clsHashingService.cs                   ← BCrypt + SHA256 hashing
│   │   ├── clsMyLogger.cs                         ← Windows Event Log writer
│   │   ├── clsLoginService.cs                     ← Token generation, refresh rotation, logout
│   │   ├── clsEmployeesService.cs                 ← Employee business logic + password hashing
│   │   ├── clsOrdersService.cs
│   │   ├── clsMenuItemsService.cs
│   │   ├── clsTablesService.cs
│   │   └── ... (one service class per entity, each split into Reader + Writer)
│   └── Interfaces/
│       ├── IHashingService.cs
│       ├── ILoginService.cs
│       ├── IEmployeesService.cs
│       └── ... (one interface per service)
│
├── ContractsLayerRestaurant/                      ← Shared DTOs project
│   ├── DTORequest/
│   │   ├── Auth/       DTOLoginRequest, DTORefreshRequest, DTOLogoutRequest, DTOAuthCURequest
│   │   ├── Employees/  DTOEmployeesCRequest, DTOEmployeesURequest, DTOEmployeesChangedPassword
│   │   ├── Orders/     DTOOrderCRequest, DTOOrderURequest, DTOOrderFilterRequest
│   │   ├── MenuItems/  DTOMenuItemsCRequest, DTOMenuItemsURequest, DTOMenuItemsFilterRequest
│   │   ├── Tables/     DTOTablesCRequest, DTOTablesURequest + 3 filter request DTOs
│   │   └── ...
│   └── DTOResponse/
│       ├── DTOTokenResponse.cs
│       ├── DTOEmployeeResponse.cs
│       ├── DTOOrderResponse.cs
│       └── ... (one response DTO per entity)
│
├── DataLayerRestaurant/                           ← Data access project
│   ├── clsDataAccessLayer.cs                      ← Base generic interfaces (IRepositoryReader, IRepositoryWriter, ICompositionDataBase)
│   ├── clsMySettings.cs                           ← Config model: ConnectionString + RowsPerPage
│   ├── Classes/
│   │   ├── clsEmployeesRepository.cs              ← Reader + Writer + Repository facade
│   │   ├── clsOrdersRepository.cs
│   │   ├── clsLoginRepository.cs
│   │   └── ... (one repository per entity)
│   ├── Interfaces/
│   │   └── ... (one interface per repository)
│   └── Mapper/
│       └── ... (SqlDataReader → Entity mappers, one per entity)
│
└── DomainLayer/                                   ← Domain entities project
    └── Entities/
        ├── Employee.cs
        ├── Order.cs
        ├── OrderDetail.cs
        ├── MenuItem.cs
        ├── Table.cs
        ├── JobRole.cs
        └── ... (one entity per database table)
```

---

## 🧠 Key Technical Decisions

**Why Raw ADO.NET instead of Entity Framework?**
The choice to use raw SQL with Stored Procedures was deliberate — to understand what ORMs abstract away, to have full control over query performance, and to practice writing structured data access code without relying on code generation.

**Why separate Reader and Writer classes?**
Following the Single Responsibility Principle at the class level ensures that a class that reads data cannot accidentally write, and vice versa. It also makes each class smaller, more focused, and easier to reason about independently.

**Why a dedicated Contracts layer?**
Keeping all DTOs in a separate project (`ContractsLayerRestaurant`) means neither the Business Layer nor the Data Layer are coupled to each other's types — they only share the contract types. This also makes the contracts reusable if a second consumer (e.g. a frontend BFF or a gRPC service) were ever added.

**Why BCrypt for refresh token storage?**
Even though refresh tokens are randomly generated (not user-chosen passwords), storing them hashed means that a database breach does not expose live tokens. The same security principle that applies to passwords applies to any secret stored at rest.

---

## 📜 Version History

| Version | Changes |
|---|---|
| **v1.0** | Initial build — N-Layer architecture, full CRUD for all entities, async/await throughout, Stored Procedures only |
| **v2.0** | Migrated password hashing from SHA-256 to BCrypt, moved hashing logic to Business Layer, fixed interface naming convention (capital `I` prefix), standardized response messages |
| **v3.0** | Full JWT authentication with refresh token rotation, BCrypt-hashed refresh token storage, role-based authorization, custom resource-level authorization policies (Owner or Admin), per-IP rate limiting, CORS policy, Swagger with JWT support, Windows Event Log integration |

---

## 👨‍💻 Author

Built by [Compiler-A](https://github.com/Compiler-A) as a backend portfolio project.
