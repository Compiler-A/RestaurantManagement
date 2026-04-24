# 🍽️ Restaurant Management System — Backend API

A production-ready RESTful backend API for managing full restaurant operations, built with **ASP.NET Core 8** and **SQL Server**. The system implements a clean **N-Layer architecture** with JWT authentication, role-based authorization, rate limiting, BCrypt hashing, and comprehensive separation of concerns.

> **Platform:** Windows only — uses Windows Event Log for logging.

---

## 📋 Table of Contents

- [Tech Stack](#-tech-stack)
- [Architecture Overview](#-architecture-overview)
- [Design Patterns](#-design-patterns)
- [Security Features](#-security-features)
- [API Endpoints Reference](#-api-endpoints-reference)
- [Role-Based Access Control](#-role-based-access-control)
- [Unified Response Format](#-unified-response-format)
- [Error Handling](#-error-handling)
- [Rate Limiting](#-rate-limiting)
- [Setup & Installation](#-setup--installation)
- [Project Structure](#-project-structure)
- [Version History](#-version-history)

---

## 🛠️ Tech Stack

| Category | Technology |
|---|---|
| Language | C# (.NET 8) |
| Framework | ASP.NET Core Web API |
| Database | SQL Server (Stored Procedures only) |
| Authentication | JWT Bearer Tokens |
| Password Hashing | BCrypt.Net (with salt) |
| API Documentation | Swagger / OpenAPI (with JWT support) |
| Logging | Windows Event Log |
| Platform | Windows |

---

## 🏗️ Architecture Overview

The solution is organized into **four independent projects**, each with a single, clearly defined responsibility. Layers only communicate with the layer directly below through **interfaces**, ensuring loose coupling and easy testability.

```
RestaurantManagement/
│
├── APILayer/                       ← Presentation Layer
│   ├── Controllers/                   HTTP endpoints (12 controllers)
│   ├── Authorization/                 Custom policy handlers
│   ├── Extensions/
│   │   ├── Configuration/             JWT & app settings setup
│   │   └── Security/                  Auth, CORS, Rate Limiting, Swagger
│   ├── Filters/                       Model validation, Swagger filters
│   ├── Middleware/                    Global exception handler
│   └── Program.cs                     App bootstrap
│
├── BusinessLayerRestaurant/        ← Business Logic Layer
│   ├── Classes/                       Service implementations
│   └── Interfaces/                    Service contracts
│
├── ContractsLayerRestaurant/       ← Contracts Layer (Shared DTOs)
│   └── DTOs/                          All request/response objects
│       ├── Auth/
│       ├── Employees/
│       ├── Orders/
│       ├── MenuItems/
│       ├── Tables/
│       └── ...
│
└── DataLayerRestaurant/            ← Data Access Layer
    ├── Classes/                       Repository implementations
    └── Interfaces/                    Repository contracts
```

### Layer Communication Flow

```
HTTP Request
    │
    ▼
[APILayer — Controller]
    │  calls interface
    ▼
[BusinessLayerRestaurant — Service]
    │  calls interface
    ▼
[DataLayerRestaurant — Repository]
    │  executes stored procedure
    ▼
[SQL Server Database]
```

---

## 🧩 Design Patterns

### Repository Pattern
Every entity has its own repository class that handles all direct database operations. The repository is the **only** place in the codebase that talks to SQL Server. Each repository is accessed via an interface, keeping the business layer completely database-agnostic.

### Reader / Writer Separation
Each repository and service is split into two distinct classes:
- **Reader** — handles all `GET` operations (read-only queries)
- **Writer** — handles all `POST`, `PUT`, `DELETE` operations (mutations)

Both implement a shared interface that is exposed to the layer above. This enforces a clean separation between read and write responsibilities.

### Composition Pattern
Related data loading is handled via composable loader classes. For example, when fetching an `Order`, a `CompositionLoader` automatically triggers loaders for `Employee`, `Table`, and `StatusOrder` — loading each related object into the DTO without the repository needing to know about any of it. New loaders can be added without changing existing code.

```
clsOrdersService
    └── clsCompositionOrdersLoader
            ├── clsEmployeeLoader
            ├── clsTableLoader
            └── clsStatusOrderLoader
```

### Dependency Injection via Extension Methods
All service and repository registrations are extracted from `Program.cs` into dedicated Extension Method files under `Extensions/Services/`. Each entity has its own file (e.g. `EmployeesServiceExtension.cs`), keeping `Program.cs` clean and minimal.

---

## 🔒 Security Features

### JWT Authentication
- Bearer token authentication using `Microsoft.AspNetCore.Authentication.JwtBearer`
- Tokens are signed with HMAC-SHA256
- Tokens carry claims: `NameIdentifier` (employee ID), `Name` (username), `Role`
- Configurable via `appsettings.json`: issuer, audience, secret key, expiration

### Refresh Token System
- On login, both an **access token** (short-lived) and a **refresh token** (7-day) are issued
- Refresh tokens are stored as **BCrypt hashes** in the database — the raw token is never stored
- Token **rotation** is enforced: each `/refresh` call issues a brand new refresh token and invalidates the old one
- Logout revokes the refresh token by recording a `RevokedAt` timestamp

### Role-Based Authorization
Endpoints are protected using `[Authorize(Roles = "...")]`. The system supports the following roles:

| Role | Description |
|---|---|
| `Manager` | Full administrative access |
| `Chef` | Kitchen-level access |
| `Sous Chef` | Assistant kitchen access |
| `Waiter` | Table and order access |

### Custom Authorization Policies
Beyond roles, the API enforces **resource-level ownership** through three custom `IAuthorizationHandler` policies:

| Policy | Rule |
|---|---|
| `EmployeeOwnerOrAdmin` | Manager can access any employee record; others can only access their own |
| `EmployeeByUserNameOwnerOrAdmin` | Same rule but scoped to username-based lookups |
| `WaiterOwnerOrAdmin` | Manager, Chef, Sous Chef can manage any order; Waiters can only manage orders they created |

### BCrypt Password Hashing
All passwords and refresh tokens are hashed using BCrypt, which automatically handles salt generation and stores it as part of the hash. Verification uses `BCrypt.Verify()`.

### CORS Policy
A named CORS policy (`RMApiCorsPolicy`) restricts API access to allowed origins, configured in `CorsSecurityExtension.cs`.

---

## 📡 API Endpoints Reference

> All endpoints require a valid JWT Bearer token unless marked **Public**.

### 🔑 Auth — `/api/Auth`

| Method | Route | Access | Description |
|---|---|---|---|
| POST | `/login` | Public | Authenticate and receive access + refresh tokens |
| POST | `/refresh` | Public | Rotate refresh token and get new access token |
| POST | `/logout` | Authenticated | Revoke refresh token |

### 👤 Employees — `/api/Employees`

| Method | Route | Roles | Description |
|---|---|---|---|
| GET | `/` | Manager | Get all employees (paginated) |
| GET | `/{ID}` | Owner or Manager | Get employee by ID |
| GET | `/user-name/{userName}` | Owner or Manager | Get employee by username |
| POST | `/` | Manager | Create new employee |
| PUT | `/` | Manager | Update employee |
| DELETE | `/{ID}` | Manager | Delete employee |
| POST | `/changed-password` | Self only | Change own password |

### 📦 Orders — `/api/Orders`

| Method | Route | Roles | Description |
|---|---|---|---|
| GET | `/` | Manager, Chef, Sous Chef, Waiter | Get all orders (paginated) |
| GET | `/{ID}` | Manager, Chef, Sous Chef, Waiter | Get order by ID |
| GET | `/filter` | Manager, Chef, Sous Chef, Waiter | Filter orders by table, employee, or status |
| POST | `/` | Manager, Waiter (own orders) | Create new order |
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
| DELETE | `/{ID}` | Authenticated | Remove item from order |

### 🍕 Menu Items — `/api/MenuItems`

| Method | Route | Access | Description |
|---|---|---|---|
| GET | `/` | **Public** | Get all menu items (paginated) |
| GET | `/{ID}` | **Public** | Get menu item by ID |
| GET | `/all-availables` | **Public** | Get all currently available items |
| GET | `/all-filters` | **Public** | Filter menu items by type, status, price range |
| POST | `/` | Authenticated | Add new menu item |
| PUT | `/` | Authenticated | Update menu item |
| DELETE | `/{ID}` | Authenticated | Delete menu item |

### 🪑 Tables — `/api/Tables`

| Method | Route | Access | Description |
|---|---|---|---|
| GET | `/` | **Public** | Get all tables (paginated) |
| GET | `/{ID}` | **Public** | Get table by ID |
| GET | `/all-nopagination` | **Public** | Get all tables (no pagination) |
| GET | `/all-availables` | **Public** | Get all available tables |
| GET | `/table-name` | **Public** | Find table by name |
| GET | `/allfilter-seats` | **Public** | Filter tables by seat count |
| GET | `/allfilter-statustables` | **Public** | Filter tables by status |
| GET | `/allfilter-global` | **Public** | Filter tables by status and seat count |
| POST | `/` | Authenticated | Add new table |
| PUT | `/` | Authenticated | Update table |
| DELETE | `/{ID}` | Authenticated | Delete table |

### 🔧 Lookup / Reference Endpoints

| Base Route | Description |
|---|---|
| `/api/JobRoles` | Full CRUD for employee job roles |
| `/api/TypeItems` | Full CRUD for menu item categories |
| `/api/StatusOrders` | Full CRUD for order status values |
| `/api/StatusMenus` | Full CRUD for menu availability statuses |
| `/api/StatusTables` | Full CRUD for table status values |
| `/api/Settings` | Manage system settings (e.g. rows per page) |

---

## 🎭 Role-Based Access Control

| Action | Manager | Chef | Sous Chef | Waiter |
|---|:---:|:---:|:---:|:---:|
| View all employees | ✅ | ❌ | ❌ | ❌ |
| Manage employees | ✅ | ❌ | ❌ | ❌ |
| View own profile | ✅ | ✅ | ✅ | ✅ |
| View all orders | ✅ | ✅ | ✅ | ✅ |
| Create/update own order | ✅ | ❌ | ❌ | ✅ |
| Delete orders | ✅ | ✅ | ✅ | ❌ |
| Manage menu items | ✅ | ✅ | ✅ | ✅ |
| Browse menu (public) | — | — | — | — |

---

## 📐 Unified Response Format

Every endpoint returns the same JSON envelope, making it simple for any frontend or client to handle responses consistently:

```json
{
  "statusCode": 200,
  "message": "Found Successfully!",
  "data": { }
}
```

For list responses:
```json
{
  "statusCode": 200,
  "message": "Row: 12",
  "data": [ ]
}
```

For auth responses:
```json
{
  "statusCode": 200,
  "message": "Login Successfully!",
  "data": {
    "accessToken": "eyJhbGciOiJIUzI1NiIs...",
    "refreshToken": "aGFzaGVkUmVmcmVzaFRva2Vu..."
  }
}
```

---

## 🚨 Error Handling

A `GlobalExceptionMiddleware` catches all unhandled exceptions across the entire application and maps them to the appropriate HTTP status code before returning them in the unified response format. No stack traces are leaked to the client.

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
| Any other exception | 500 Internal Server Error |

All exceptions are also written to the **Windows Event Log** via `IMyLogger`.

Model validation errors are caught before the action even executes by a `ValidateModelAttribute` action filter, returning a 400 with a descriptive error message for every failing field.

---

## ⏱️ Rate Limiting

All endpoints are protected by a **fixed-window rate limiter** scoped per client IP address. Different operation types have different limits to protect sensitive endpoints while keeping read-heavy routes performant:

| Policy | Permit Limit | Window |
|---|---|---|
| `Auth` (login/refresh) | 5 requests | 1 minute |
| `GetAll` (list endpoints) | 30 requests | 1 minute |
| `GetOne` (single record) | 60 requests | 1 minute |
| `Add` (POST) | 10 requests | 1 minute |
| `Update` (PUT) | 15 requests | 1 minute |
| `Delete` (DELETE) | 5 requests | 1 minute |

Exceeding any limit returns `HTTP 429 Too Many Requests`.

---

## ⚙️ Setup & Installation

### Prerequisites

- [Visual Studio 2022+](https://visualstudio.microsoft.com/)
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- SQL Server (any edition)
- Windows OS

### Steps

**1. Clone the repository**
```bash
git clone https://github.com/Compiler-A/RestaurantManagement.git
```

**2. Open the solution**

Open `RestaurantProjectv2.slnx` in Visual Studio 2022.

**3. Configure the database connection**

Edit `APILayer/appsettings.json`:
```json
{
  "MySettings": {
    "ConnectionString": "Server=.;Database=RestaurantManager;User Id=sa;Password=YOUR_PASSWORD;Encrypt=False;TrustServerCertificate=True;",
    "RowsPerPage": 12
  },
  "JwtSettings": {
    "SecretKey": "YOUR_SECRET_KEY_MIN_32_CHARS",
    "Issuer": "RestaurantManagementAPI",
    "Audience": "RestaurantManagementClient",
    "ExpirationMinutes": 60
  }
}
```

**4. Set up the database**

Run the SQL scripts to create the database schema and all stored procedures on your SQL Server instance.

**5. Run the project**

Press `F5` in Visual Studio. Swagger UI will be available at:
```
https://localhost:{port}/swagger
```

**6. Authenticate in Swagger**

1. Call `POST /api/Auth/login` with valid credentials
2. Copy the `accessToken` from the response
3. Click **Authorize** in Swagger UI and enter: `Bearer <your_token>`
4. All protected endpoints are now accessible

---

## 📁 Project Structure

```
RestaurantManagement/
│
├── APILayer/
│   ├── Controllers/
│   │   ├── BaseController.cs                  ← Shared CreateResponse<T> helper
│   │   ├── AuthController.cs                  ← Login, Refresh, Logout
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
│   │   │   ├── EmployeeOwnerOrAdminHandler.cs
│   │   │   ├── EmployeeOwnerOrAdminRequirement.cs
│   │   │   ├── EmployeeUserNameOwnerOrAdminHandler.cs
│   │   │   └── EmployeeUserNameOwnerOrAdminRequirement.cs
│   │   └── Order/
│   │       ├── WaiterOwnerOrAdminHandler.cs
│   │       └── WaiterOwnerOrAdminRequirement.cs
│   │
│   ├── Extensions/
│   │   ├── Configuration/
│   │   │   ├── JwtSettingConfigurationExtension.cs
│   │   │   └── MySettingConfigurationExtension.cs
│   │   └── Security/
│   │       ├── AuthenticationSecurityExtension.cs
│   │       ├── AuthorizationSecurityExtension.cs
│   │       ├── CorsSecurityExtension.cs
│   │       ├── RateLimitingSecurityExtension.cs
│   │       └── SwaggerGenSecurityExtension.cs
│   │
│   ├── Filters/
│   │   ├── ValidateModelAttribute.cs          ← Model validation action filter
│   │   ├── DefaultResponsesOperationFilter.cs ← Swagger response docs
│   │   └── NameRateLimitPolicies.cs           ← Rate limit policy name constants
│   │
│   ├── Middleware/
│   │   └── GlobalExceptionMiddleware.cs
│   │
│   ├── ApiResponse.cs                         ← Generic wrapper: { statusCode, message, data }
│   └── Program.cs
│
├── BusinessLayerRestaurant/
│   ├── Classes/
│   │   ├── JwtSettings.cs                     ← JWT config model
│   │   ├── clsHashingService.cs               ← BCrypt + SHA256 implementations
│   │   ├── clsMyLogger.cs                     ← Windows Event Log writer
│   │   ├── clsLoginService.cs                 ← Auth, token generation, refresh rotation
│   │   ├── clsEmployeesService.cs
│   │   ├── clsOrdersService.cs
│   │   ├── clsMenuItemsService.cs
│   │   └── ... (one service class per entity)
│   │
│   └── Interfaces/
│       ├── IHashingService.cs
│       ├── ILoginService.cs
│       ├── IEmployeesService.cs
│       └── ... (one interface per service)
│
├── ContractsLayerRestaurant/
│   └── DTOs/
│       ├── Auth/         DTOLoginRequest, DTOTokenResponse, DTORefreshRequest, DTOLogoutRequest
│       ├── Employees/    DTOEmployees, DTOEmployeesCRequest, DTOEmployeesURequest, DTOEmployeesChangedPassword
│       ├── Orders/       DTOOrders, DTOOrderCRequest, DTOOrderURequest, DTOOrderFilterRequest
│       ├── MenuItems/    DTOMenuItems, DTOMenuItemsCRequest, DTOMenuItemsURequest, DTOMenuItemsFilterRequest
│       ├── Tables/       DTOTables + multiple filter request DTOs
│       └── ...
│
└── DataLayerRestaurant/
    ├── clsDataAccessLayer.cs                  ← Base interfaces: IReadable, IWritable, IComposition
    ├── clsMySettings.cs                       ← Connection string + RowsPerPage config model
    ├── Classes/
    │   ├── clsOrdersRepository.cs             ← Reader + Writer + Repository (facade)
    │   ├── clsEmployeesRepository.cs
    │   └── ... (one repository per entity)
    └── Interfaces/
        └── ... (one interface per repository)
```

---

## 📜 Version History

| Version | Highlights |
|---|---|
| **v1.0** | Initial release — N-Layer architecture, full CRUD, async/await, Stored Procedures only |
| **v2.0** | BCrypt password hashing with salt, hashing logic moved to Business Layer, fixed naming conventions (I-prefix interfaces), standardized API response messages |
| **v3.0** | JWT authentication, refresh token with rotation, BCrypt-hashed refresh tokens, role-based authorization, custom resource-level authorization policies (Owner or Admin), rate limiting per IP per action type, CORS policy, Swagger with JWT support, Windows Event Log logging integrated throughout |