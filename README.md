# Restaurant Management System — Backend API

A RESTful backend API for managing restaurant operations, built with ASP.NET Core and SQL Server. The project follows a clean N-Layer architecture with full separation of concerns across four independent layers.

---

## Tech Stack

- **Language:** C# / .NET
- **Framework:** ASP.NET Core Web API
- **Database:** SQL Server (Stored Procedures)
- **Password Hashing:** BCrypt.Net
- **Documentation:** Swagger / OpenAPI
- **Platform:** Windows

---

## Architecture

The solution is divided into four projects:

```
RestaurantManagement/
├── APILayer                  # Controllers, Middleware, Filters, Extensions
├── BusinessLayerRestaurant   # Business logic, Services, Hashing
├── ContractsLayerRestaurant  # DTOs (Data Transfer Objects)
└── DataLayerRestaurant       # Repositories, Database access
```

Each layer communicates only with the layer directly below it through interfaces, ensuring loose coupling and easy testability.

### Design Patterns Used

- **Repository Pattern** — separates data access logic from business logic
- **Composition Pattern** — loads related data (e.g. loading employee info inside an order)
- **Reader / Writer Separation** — read and write operations are split into separate classes
- **Dependency Injection** — all dependencies are injected via interfaces, registered in `Program.cs` using Extension Methods

---

## Features

- Full CRUD operations for all entities
- Pagination support on all list endpoints
- Filter endpoints for Orders, Menu Items, and Tables
- Global Exception Handling Middleware with proper HTTP status codes
- Model validation using Data Annotations and a custom `ValidateModelAttribute` filter
- Password hashing with BCrypt (includes salt automatically)
- Windows Event Log logging for all operations
- Unified API response format: `{ statusCode, message, data }`

---

## API Endpoints

### Employees — `/api/Employees`
| Method | Route | Description |
|--------|-------|-------------|
| GET | `/` | Get all employees (paginated) |
| GET | `/{ID}` | Get employee by ID |
| GET | `/user-name/{userName}` | Get employee by username |
| POST | `/` | Add new employee |
| POST | `/login` | Employee login |
| POST | `/changed-password` | Change password |
| PUT | `/` | Update employee |
| DELETE | `/{ID}` | Delete employee |

### Orders — `/api/Orders`
| Method | Route | Description |
|--------|-------|-------------|
| GET | `/` | Get all orders (paginated) |
| GET | `/{ID}` | Get order by ID |
| GET | `/filter` | Filter orders by table, employee, or status |
| POST | `/` | Create new order |
| PUT | `/` | Update order |
| DELETE | `/{ID}` | Delete order |

### Order Details — `/api/OrderDetails`
| Method | Route | Description |
|--------|-------|-------------|
| GET | `/` | Get all order details (paginated) |
| GET | `/{ID}` | Get order detail by ID |
| GET | `/all-orderid/{orderID}` | Get all details for a specific order |
| POST | `/` | Add order detail |
| PUT | `/` | Update order detail |
| DELETE | `/{ID}` | Delete order detail |

### Menu Items — `/api/MenuItems`
| Method | Route | Description |
|--------|-------|-------------|
| GET | `/` | Get all menu items (paginated) |
| GET | `/{ID}` | Get menu item by ID |
| GET | `/all-availables` | Get all available items |
| GET | `/all-filters` | Filter menu items |
| POST | `/` | Add menu item |
| PUT | `/` | Update menu item |
| DELETE | `/{ID}` | Delete menu item |

### Tables — `/api/Tables`
| Method | Route | Description |
|--------|-------|-------------|
| GET | `/` | Get all tables (paginated) |
| GET | `/{ID}` | Get table by ID |
| GET | `/all-nopagination` | Get all tables without pagination |
| GET | `/all-availables` | Get available tables |
| GET | `/table-name` | Get table by name |
| GET | `/allfilter-seats` | Filter by seat count |
| GET | `/allfilter-statustables` | Filter by status |
| GET | `/allfilter-global` | Filter by status and seats |
| POST | `/` | Add table |
| PUT | `/` | Update table |
| DELETE | `/{ID}` | Delete table |

### Other Endpoints
- `/api/JobRoles` — CRUD for job roles
- `/api/TypeItems` — CRUD for item types
- `/api/StatusOrders` — CRUD for order statuses
- `/api/StatusMenus` — CRUD for menu statuses
- `/api/StatusTables` — CRUD for table statuses
- `/api/Settings` — CRUD for system settings (pagination size, etc.)

---

## Setup

### Requirements
- Visual Studio 2022+
- .NET 8 SDK
- SQL Server
- Windows OS

### Steps

1. Clone the repository:
```bash
git clone https://github.com/Compiler-A/RestaurantManagement.git
```

2. Open `RestaurantProjectv2.slnx` in Visual Studio.

3. Update the connection string in `APILayer/appsettings.json`:
```json
"MySettings": {
  "ConnectionString": "Server=.;Database=RestaurantManager;User Id=sa;Password=YOUR_PASSWORD;Encrypt=False;",
  "RowsPerPage": 12
}
```

4. Set up the database by running the SQL scripts (Stored Procedures) on your SQL Server instance.

5. Run the project — Swagger UI will be available at `https://localhost:{port}/swagger`.

---

## API Response Format

All endpoints return a unified response:

```json
{
  "statusCode": 200,
  "message": "Found Successfully!",
  "data": { }
}
```

---

## Versioning

| Version | Highlights |
|---------|------------|
| v1.0 | Initial release — N-Layer architecture, CRUD, async/await, Stored Procedures |
| v2.0 | BCrypt hashing with salt, Hashing moved to Business Layer, improved naming conventions, standardized API messages |

---

## Planned for v3.0

- JWT Authentication
- Role-based Authorization
