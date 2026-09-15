# 🏥 MediCoreX – Healthcare Management System

MediCoreX is a full-stack healthcare management system built using **ASP.NET Core Web API, Angular, MySQL, and Docker**.

The project demonstrates practical backend development concepts including secure authentication, role-based authorization, layered architecture, patient management, search, filtering, sorting, pagination, validation, global exception handling, structured logging, unit testing, and containerization.

---

## 🚀 Features

### 🔐 Authentication & Security

- JWT Authentication
- Access Token + Refresh Token
- Refresh Token Rotation
- Secure password hashing
- Role-Based Authorization
- Admin / User roles
- Secure Admin Seeding
- Admin credentials managed using .NET User Secrets
- Protected patient-management APIs
- Automatic access-token refresh from Angular frontend

### 👨‍⚕️ Patient Management

- Create Patient
- View All Patients
- View Patient by ID
- Update Patient
- Delete Patient
- Search patients by name
- Filter patients by gender
- Sort patients by name or age
- Ascending / Descending sorting
- Pagination
- Combined Search + Filter + Sort + Pagination

### 🛠 Backend

- ASP.NET Core Web API
- Layered Architecture
- Dependency Injection
- Entity Framework Core
- MySQL
- LINQ
- DTO Pattern
- AutoMapper
- FluentValidation
- Global Exception Handling Middleware
- Custom Exceptions
- Structured Logging
- EF Core Code First
- EF Core Migrations
- Swagger / OpenAPI

### 🧪 Testing

- Unit Testing with xUnit
- Mocking with Moq
- EF Core InMemory
- FluentValidation TestHelper
- 47 automated tests

### 🐳 Containerization

- Docker
- Docker Compose
- Containerized ASP.NET Core API
- Containerized MySQL database

### 🖥️ Angular Frontend

- Login
- Registration
- JWT Authentication
- Refresh Token Handling
- Authentication Guard
- HTTP Interceptor
- Dashboard
- Patient Management
- Add / Edit / Delete Patient
- Search
- Filtering
- Sorting
- Pagination
- Reactive Form Validation
- Responsive UI

---

# 🏗️ Architecture

MediCoreX follows a layered architecture to maintain separation of concerns and make the application easier to maintain, test, and extend.

```text
                    Angular Client
                         │
                         ▼
                    HTTP / REST
                         │
                         ▼
                 ASP.NET Controllers
                         │
                         ▼
                    Service Layer
                         │
                         ▼
              Entity Framework Core
                    DbContext
                         │
                         ▼
                  MySQL Database
```

### Backend Request Flow

```text
Client / Swagger
       ↓
Controller
       ↓
Service Layer
       ↓
DbContext
       ↓
MySQL Database
```

The Service Layer contains the application/business logic, while Entity Framework Core is responsible for database access through `DbContext`.

---

# 📁 Project Structure

```text
MediCoreX
│
├── MediCoreX.Api
│   │
│   ├── Controllers
│   │   ├── AuthController.cs
│   │   └── PatientsController.cs
│   │
│   ├── Services
│   │   ├── AuthService.cs
│   │   ├── PatientService.cs
│   │   └── TokenService.cs
│   │
│   ├── DTOs
│   │   ├── Auth DTOs
│   │   └── Patient DTOs
│   │
│   ├── Models
│   │   ├── User.cs
│   │   └── Patient.cs
│   │
│   ├── Data
│   │   └── MediCoreXDbContext.cs
│   │
│   ├── Middleware
│   │   └── ExceptionMiddleware.cs
│   │
│   ├── Validators
│   │   ├── RegisterDtoValidator.cs
│   │   ├── CreatePatientDtoValidator.cs
│   │   └── UpdatePatientDtoValidator.cs
│   │
│   ├── Mappings
│   │   └── AutoMapper Profiles
│   │
│   ├── Migrations
│   │
│   ├── Program.cs
│   └── appsettings.json
│
├── MediCoreX.Tests
│   │
│   ├── AuthServiceTests
│   ├── PatientServiceTests
│   ├── TokenServiceTests
│   └── ValidatorTests
│
├── Angular Frontend
│   │
│   └── src
│       └── app
│           ├── core
│           │   ├── models
│           │   ├── services
│           │   ├── guards
│           │   └── interceptors
│           │
│           └── features
│               ├── login
│               ├── register
│               ├── dashboard
│               └── patients
│
├── Dockerfile
├── docker-compose.yml
└── README.md
```

---

# 👥 Roles & Authorization

MediCoreX supports two roles:

## 🔴 Admin

Admin users can:

- View patient records
- Create patient records
- Update patient records
- Delete patient records
- Access admin-only endpoints

The Admin account is created through startup seeding.

Sensitive Admin configuration is managed using **.NET User Secrets**.

---

## 🔵 User

Regular users can:

- Register
- Login
- Receive access and refresh tokens

Regular users cannot:

- Access patient-management APIs
- Self-assign the Admin role
- Access Admin-only endpoints

The role is controlled by the backend and is not accepted from the public registration request.

---

# 🔐 Authentication Flow

MediCoreX uses **JWT-based authentication**.

```text
User
 │
 │ Login
 ▼
Auth API
 │
 │ Validate Email + Password
 ▼
Generate Tokens
 │
 ├──────────────► Access Token
 │
 └──────────────► Refresh Token
```

The access token is sent with protected API requests.

```text
Angular Client
      │
      │ Authorization: Bearer <AccessToken>
      ▼
ASP.NET Core API
      │
      ▼
JWT Authentication
      │
      ▼
Role Authorization
      │
      ▼
Protected Controller
```

---

# 🔄 Refresh Token Flow

MediCoreX implements **Refresh Token Rotation**.

### Flow

```text
1. User Login
       ↓
2. API validates credentials
       ↓
3. Access Token + Refresh Token generated
       ↓
4. Client uses Access Token
       ↓
5. Access Token expires
       ↓
6. Client sends Refresh Token
       ↓
7. API validates Refresh Token
       ↓
8. New Access Token generated
       ↓
9. New Refresh Token generated
       ↓
10. Old Refresh Token replaced
```

Refresh endpoint:

```http
POST /api/auth/refresh
```

The API validates the refresh token against the database and checks its expiry before generating a new token pair.

---

# 📡 API Endpoints

## Authentication APIs

| Method | Endpoint | Access | Description |
|---|---|---|---|
| POST | `/api/auth/register` | Public | Register a new user |
| POST | `/api/auth/login` | Public | Login and receive tokens |
| POST | `/api/auth/refresh` | Public | Refresh access token |

---

## Patient APIs

| Method | Endpoint | Access | Description |
|---|---|---|---|
| GET | `/api/patients` | Admin | Get all patients |
| GET | `/api/patients/{id}` | Admin | Get patient by ID |
| GET | `/api/patients/above-age/{age}` | Admin | Get patients above a specific age |
| GET | `/api/patients/gender/{gender}` | Admin | Filter patients by gender |
| GET | `/api/patients/search?name=Amit` | Admin | Search patients by name |
| GET | `/api/patients/sort?asc=true` | Admin | Sort patients by age |
| GET | `/api/patients/filter` | Admin | Search, filter, sort and paginate |
| GET | `/api/patients/paged` | Admin | Get paginated patient records |
| GET | `/api/patients/admin-data` | Admin | Admin-only endpoint |
| POST | `/api/patients` | Admin | Create a patient |
| PUT | `/api/patients/{id}` | Admin | Update a patient |
| DELETE | `/api/patients/{id}` | Admin | Delete a patient |

---

# 🔎 Search, Filtering, Sorting & Pagination

The main patient query endpoint supports multiple query parameters.

```http
GET /api/patients/filter
```

### Supported Parameters

```text
Page
PageSize
Search
Gender
SortBy
SortOrder
```

### Example

```http
GET /api/patients/filter?Page=1&PageSize=5&Search=Amit&Gender=Male&SortBy=age&SortOrder=asc
```

### Example Response

```json
{
  "page": 1,
  "pageSize": 5,
  "totalRecords": 2,
  "totalPages": 1,
  "data": [
    {
      "id": 11,
      "fullName": "Amit Singh",
      "age": 26,
      "gender": "Male"
    }
  ]
}
```

The API builds the query using LINQ and applies filtering, sorting, counting, `Skip()` and `Take()` before retrieving the requested records.

---

# 🛡️ Validation

MediCoreX uses **FluentValidation** for request validation.

Validators are implemented for:

- Register DTO
- Create Patient DTO
- Update Patient DTO

Validation includes:

- Required fields
- Minimum / maximum field lengths
- Email format
- Password length
- Valid age range
- Allowed gender values

ASP.NET Core `[ApiController]` model validation is also used for request parameter validation.

---

# 🛡️ Global Exception Handling

The application includes global exception-handling middleware.

Instead of handling exceptions separately in every controller, unexpected exceptions are processed centrally.

```text
Request
   ↓
Controller
   ↓
Service
   ↓
Exception
   ↓
Global Exception Middleware
   ↓
Error Response
```

Custom exceptions are also used for application-specific error scenarios.

---

# 📝 Structured Logging

MediCoreX uses `ILogger` for structured application logging.

Logging helps with:

- Tracking application events
- Debugging
- Error investigation
- Understanding service operations
- Production issue analysis

---

# 🧩 DTO Pattern & AutoMapper

DTOs are used to control the data exchanged between the client and API.

The application separates API request/response models from database entities.

Example:

```text
Client
   ↓
CreatePatientDto
   ↓
PatientService
   ↓
Patient Entity
   ↓
Entity Framework Core
   ↓
MySQL
```

AutoMapper is used for mapping between DTOs and entity models where appropriate.

This helps keep API contracts separate from database entities.

---

# 💉 Dependency Injection

MediCoreX uses ASP.NET Core's built-in Dependency Injection container.

Services such as:

- `IPatientService`
- `IAuthService`
- `ITokenService`

are registered and injected where required.

Example:

```text
PatientsController
       ↓
IPatientService
       ↓
PatientService
       ↓
MediCoreXDbContext
```

Dependency Injection improves:

- Separation of concerns
- Testability
- Maintainability
- Loose coupling

---

# 🗄️ Database

MediCoreX uses **MySQL** with **Entity Framework Core**.

### Database Technologies

- MySQL
- Entity Framework Core
- Code First
- EF Core Migrations
- LINQ

Database schema changes are managed through EF Core migrations.

### Create Migration

```bash
dotnet ef migrations add MigrationName
```

### Apply Migration

```bash
dotnet ef database update
```

---

# 🧪 Unit Testing

MediCoreX contains automated tests covering services, token generation, and request validation.

## Testing Technologies

- xUnit
- Moq
- Entity Framework Core InMemory
- FluentValidation TestHelper

## Current Test Suite

```text
47 Tests
```

---

## 🔐 AuthService Tests

The following scenarios are covered:

- Login with valid credentials
- Login with invalid email
- Login with invalid password
- Register a valid user
- Register with duplicate email
- Refresh token with valid token
- Refresh token with invalid token
- Refresh token with expired token

---

## 👨‍⚕️ PatientService Tests

The following scenarios are covered:

- Get patient by ID
- Get all patients
- Filter patients by age
- Filter patients by gender
- Search patients by name
- Sort patients by age
- Pagination
- Add patient
- Update existing patient
- Update non-existing patient
- Delete existing patient
- Delete non-existing patient

---

## ✅ Validator Tests

Validators are tested for:

- Register DTO
- Create Patient DTO
- Update Patient DTO

Validation scenarios include:

- Required fields
- Maximum field length
- Email format
- Password length
- Valid age range
- Allowed gender values

---

## 🔑 TokenService Tests

The following scenarios are covered:

- Refresh token generation
- Unique refresh token generation
- JWT creation
- JWT claims
- JWT expiry

---

## ▶️ Run Unit Tests

From the project root:

```bash
dotnet test MediCoreX.Tests/MediCoreX.Tests.csproj
```

Or:

```bash
dotnet test
```

---

# 🖥️ Angular Frontend

MediCoreX includes an Angular frontend that consumes the ASP.NET Core REST APIs.

## Frontend Features

### 🔐 Authentication

- Login
- Registration
- JWT access token handling
- Refresh token handling
- Automatic token refresh
- Logout
- Authentication Guard
- HTTP Interceptor

### 📊 Dashboard

- Total patient count
- Male patient count
- Female patient count
- Recent patients
- View All Patients navigation

### 👨‍⚕️ Patient Management

- Patient listing
- Add patient
- Edit patient
- Delete patient
- Search by name
- Gender filtering
- Sorting
- Pagination
- Reactive form validation

---

# 🔒 Angular Authentication Flow

The Angular frontend sends the access token with protected API requests.

When an API request returns `401 Unauthorized` because the access token has expired, the HTTP interceptor attempts to refresh the token.

```text
Angular Request
      ↓
Access Token
      ↓
ASP.NET Core API
      ↓
401 Unauthorized
      ↓
HTTP Interceptor
      ↓
Refresh Token
      ↓
/api/auth/refresh
      ↓
New Access Token
      ↓
Retry Original Request
```

If the refresh token is invalid or expired, the user is logged out and redirected to the login page.

---

# 🧭 Angular Route Protection

Protected pages use an authentication guard.

```text
User
  ↓
Angular Route
  ↓
Auth Guard
  │
  ├── Token exists → Allow access
  │
  └── Token missing → Redirect to Login
```

This provides frontend-level route protection while the backend remains responsible for actual API authorization.

---

# 🐳 Docker

MediCoreX is containerized using Docker.

The project includes:

- `Dockerfile`
- `docker-compose.yml`

Docker Compose is used to run the ASP.NET Core API and MySQL database together.

```text
Docker Compose
      │
      ├── MediCoreX API
      │
      └── MySQL Database
```

---

## ▶️ Run with Docker Compose

From the project root:

```bash
docker compose up --build
```

---

## ▶️ Run in Detached Mode

```bash
docker compose up -d --build
```

---

## 🔍 Check Running Containers

```bash
docker compose ps
```

---

## 🛑 Stop Containers

```bash
docker compose down
```

---

## 🔄 Rebuild Containers

After making backend changes:

```bash
docker compose down
docker compose up --build
```

---

# 🛠️ Tech Stack

## Backend

- C#
- ASP.NET Core Web API
- .NET 9
- Entity Framework Core
- MySQL
- LINQ
- Dependency Injection

## Authentication & Security

- JWT Authentication
- Access Tokens
- Refresh Tokens
- Refresh Token Rotation
- Role-Based Authorization
- Password Hashing
- .NET User Secrets

## Validation & Mapping

- FluentValidation
- AutoMapper
- DTO Pattern

## Error Handling & Logging

- Global Exception Handling Middleware
- Custom Exceptions
- `ILogger` Structured Logging

## Frontend

- Angular
- TypeScript
- HTML
- SCSS
- Angular Router
- Reactive Forms
- HttpClient
- HTTP Interceptor
- Route Guards

## Testing

- xUnit
- Moq
- EF Core InMemory
- FluentValidation TestHelper

## Database

- MySQL
- Entity Framework Core
- EF Core Migrations

## DevOps

- Docker
- Docker Compose

## API Documentation

- Swagger
- OpenAPI

---

# ▶️ How to Run Locally

## Prerequisites

Make sure the following are installed:

- .NET 9 SDK
- Node.js
- Angular CLI
- Docker
- Docker Compose
- MySQL (only required when running the database outside Docker)

---

# Option 1: Run Using Docker

Clone the repository:

```bash
git clone https://github.com/sumitdy/MediCoreX.git
```

Navigate to the project:

```bash
cd MediCoreX
```

Start the backend and database:

```bash
docker compose up --build
```

This starts the ASP.NET Core API and MySQL database through Docker Compose.

---

# Option 2: Run Backend Without Docker

Restore .NET dependencies:

```bash
dotnet restore
```

Configure the MySQL connection string according to your local environment.

Apply EF Core migrations:

```bash
dotnet ef database update
```

Run the API:

```bash
dotnet run --project MediCoreX.Api
```

Swagger will be available at the URL displayed in the terminal after the API starts.

---

# Option 3: Run Angular Frontend

Navigate to the Angular project directory:

```bash
cd medicorex-ui
```

Install dependencies:

```bash
npm install
```

Start the Angular development server:

```bash
ng serve
```

The frontend will be available at the local URL shown by Angular CLI.

The Angular application communicates with the ASP.NET Core API using the configured API base URL.

---

# 📖 API Documentation

Swagger / OpenAPI is included for API exploration and testing.

After starting the backend, open the Swagger URL displayed in the terminal.

Swagger can be used to:

- View available endpoints
- Inspect request models
- Send API requests
- Test authentication
- Test authorization
- Test patient CRUD operations
- Test search, filtering, sorting and pagination

---

# 🔐 Configuration & Secrets

Sensitive configuration should not be committed to source control.

For local development, **.NET User Secrets** can be used to store sensitive values such as Admin credentials and other development secrets.

Initialize User Secrets:

```bash
dotnet user-secrets init
```

Example:

```bash
dotnet user-secrets set "AdminSettings:Email" "admin@example.com"
```

Sensitive credentials should never be hardcoded or committed to GitHub.

---

# 🧠 Learning Highlights

This project demonstrates practical experience with:

- ASP.NET Core Web API
- REST API development
- Layered Architecture
- Dependency Injection
- Service Layer
- Entity Framework Core
- MySQL
- LINQ
- Code First approach
- EF Core Migrations
- DTO Pattern
- AutoMapper
- JWT Authentication
- Access Tokens
- Refresh Tokens
- Refresh Token Rotation
- Role-Based Authorization
- Password Hashing
- FluentValidation
- Global Exception Handling
- Custom Exceptions
- Structured Logging
- Search
- Filtering
- Sorting
- Pagination
- Angular
- Reactive Forms
- Route Guards
- HTTP Interceptors
- Unit Testing
- xUnit
- Moq
- EF Core InMemory
- Docker
- Docker Compose
- Swagger / OpenAPI

---

# 🎯 Project Purpose

MediCoreX was developed as an **interview-focused production-style project** to demonstrate practical experience with modern .NET backend development.

The project focuses on implementing real-world backend concepts such as:

- Secure authentication
- Role-based authorization
- Database-driven APIs
- Business logic separation
- Request validation
- Global error handling
- Structured logging
- Unit testing
- API documentation
- Containerization

The Angular frontend provides a practical interface for consuming and demonstrating the backend APIs.

---

# 📊 Project Summary

| Area | Technology |
|---|---|
| Backend | ASP.NET Core Web API |
| Framework | .NET 9 |
| Language | C# |
| Frontend | Angular |
| Database | MySQL |
| ORM | Entity Framework Core |
| Authentication | JWT |
| Token Management | Access + Refresh Tokens |
| Authorization | Role-Based Authorization |
| Validation | FluentValidation |
| Mapping | AutoMapper |
| Testing | xUnit + Moq |
| Test Database | EF Core InMemory |
| API Documentation | Swagger / OpenAPI |
| Containerization | Docker + Docker Compose |
| Architecture | Layered Architecture |

---

# 📌 Key Implementation Highlights

## Authentication

```text
Register
   ↓
Password Hashing
   ↓
Login
   ↓
JWT Access Token
+
Refresh Token
```

## Authorization

```text
JWT Token
   ↓
Role Claim
   ↓
[Authorize(Roles = "Admin")]
   ↓
Protected Patient APIs
```

## Patient Query

```text
Search
   ↓
Gender Filter
   ↓
Sorting
   ↓
Count
   ↓
Pagination
   ↓
Database
```

## Error Handling

```text
Controller
   ↓
Service
   ↓
Exception
   ↓
Global Middleware
   ↓
Error Response
```

## Testing

```text
Service / Validator
       ↓
Unit Test
       ↓
xUnit
       ↓
Moq / EF Core InMemory
       ↓
Automated Test Result
```

---

# 🔗 Repository

GitHub Repository:

https://github.com/sumitdy/MediCoreX

---

# 👨‍💻 Author

## Sumit Dubey

**Full Stack Developer | .NET & Angular**

Focused on building secure, maintainable, and scalable web applications using ASP.NET Core, Angular, Entity Framework Core, and MySQL.
