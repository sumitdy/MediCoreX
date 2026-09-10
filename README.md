# 🏥 MediCoreX – Healthcare Management REST API

MediCoreX is a production-style backend system built using ASP.NET Core and MySQL.

It demonstrates secure authentication, role-based authorization, layered architecture, unit testing, containerization, and scalable backend design principles.

---

## 🚀 Features

- 🔐 JWT Authentication
- 🔄 Refresh Token Authentication with token rotation
- 👥 Role-Based Authorization (Admin / User)
- 👤 Secure Admin Seeding using .NET User Secrets
- 🛡 Admin-only patient management
- ➕ Create Patient API
- ✏️ Update Patient API
- 📦 DTO Pattern + AutoMapper
- ✅ FluentValidation for registration and patient requests
- 🛡 Global Exception Handling Middleware
- 📊 Pagination Support
- 🔎 Search, Filtering & Sorting
- 📝 Structured Logging
- 🧪 Unit Testing with xUnit, Moq, and EF Core InMemory
- 🐳 Docker & Docker Compose
- 🐬 MySQL with Entity Framework Core

---

## 🏗 Architecture Overview

```text
Client / Swagger
       ↓
Controllers
       ↓
Service Layer
       ↓
Entity Framework Core (DbContext)
       ↓
MySQL Database
```

The application follows a layered architecture to maintain separation of concerns and make the backend easier to maintain and test.

---

## 👥 Roles & Authorization

### Admin

- Can view patient records
- Can create patient records
- Can update patient records
- Can delete patient records
- Can access Admin-only endpoints
- Is created securely through startup seeding

### User

- Can register and log in
- Cannot self-assign the Admin role
- Cannot access patient-management endpoints

---

## 📡 Main API Endpoints

| Method | Endpoint | Access |
|---|---|---|
| POST | `/api/auth/register` | Public |
| POST | `/api/auth/login` | Public |
| POST | `/api/auth/refresh` | Public |
| GET | `/api/patients` | Admin |
| GET | `/api/patients/{id}` | Admin |
| POST | `/api/patients` | Admin |
| PUT | `/api/patients/{id}` | Admin |
| DELETE | `/api/patients/{id}` | Admin |

---

## 🔄 Refresh Token Flow

1. User logs in with email and password.
2. API validates the credentials.
3. API returns an access token and refresh token.
4. Access token is used for protected API requests.
5. When the access token expires, the client sends the refresh token to:
   `POST /api/auth/refresh`
6. API validates the refresh token and its expiry.
7. API generates a new access token.
8. API generates a new refresh token.
9. The old refresh token is replaced with the new refresh token.

This implements refresh token rotation.

---

## 🧪 Unit Testing

MediCoreX includes unit tests for the service layer, token generation, and request validation.

### Testing Technologies

- xUnit
- Moq
- Entity Framework Core InMemory
- FluentValidation TestHelper

### AuthService Tests

The following scenarios are covered:

- Login with valid credentials
- Login with invalid email
- Login with invalid password
- Register a valid user
- Register with duplicate email
- Refresh token with valid token
- Refresh token with invalid token
- Refresh token with expired token

### PatientService Tests

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

### Validator Tests

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

### TokenService Tests

The following scenarios are covered:

- Refresh token generation
- Unique refresh token generation
- JWT creation
- JWT claims
- JWT expiry

### Run Unit Tests

From the project root:

```bash
dotnet test MediCoreX.Tests/MediCoreX.Tests.csproj
```

Current test suite:

```text
47 Tests
```

---

## 🐳 Docker

MediCoreX is containerized using Docker.

The project includes:

- `Dockerfile` for building the ASP.NET Core API image
- `docker-compose.yml` for running the API and MySQL together

### Run with Docker Compose

From the project root:

```bash
docker compose up --build
```

This starts:

- MediCoreX API
- MySQL database

### Run in Detached Mode

```bash
docker compose up -d --build
```

### Check Running Containers

```bash
docker compose ps
```

### Stop Containers

```bash
docker compose down
```

---

## 🛠 Tech Stack

### Backend

- ASP.NET Core (.NET 9)
- C#
- Entity Framework Core
- MySQL
- LINQ

### Authentication & Security

- JWT Authentication
- Refresh Tokens
- Role-Based Authorization
- Password Hashing
- .NET User Secrets

### Validation & Mapping

- FluentValidation
- AutoMapper

### Testing

- xUnit
- Moq
- EF Core InMemory
- FluentValidation TestHelper

### DevOps / Containerization

- Docker
- Docker Compose

### API Documentation

- Swagger / OpenAPI

---

## ▶️ How to Run Locally

### Prerequisites

Make sure the following are installed:

- .NET 9 SDK
- Docker
- Docker Compose
- MySQL (if running without Docker)

---

### Option 1: Run with Docker

Clone the repository:

```bash
git clone https://github.com/sumitdy/MediCoreX.git
```

Navigate to the project:

```bash
cd MediCoreX
```

Start the application:

```bash
docker compose up --build
```

The API and MySQL database will start together.

---

### Option 2: Run without Docker

Restore dependencies:

```bash
dotnet restore
```

Update the MySQL connection string in `appsettings.json` if required.

Apply EF Core migrations:

```bash
dotnet ef database update
```

Run the API:

```bash
dotnet run --project MediCoreX.Api
```

Swagger will be available at the URL shown in the terminal after starting the API.

---

## 🗄️ Database

The application uses:

- MySQL
- Entity Framework Core
- Code First approach
- EF Core migrations

Database schema changes are managed using EF Core migrations.

---

## 📌 Learning Highlights

This project demonstrates:

- Clean separation of concerns
- Layered architecture
- Dependency Injection
- JWT authentication
- Role-based authorization
- Refresh token rotation
- Secure password hashing
- DTO pattern
- AutoMapper
- FluentValidation
- Global exception handling middleware
- Custom exceptions
- Structured logging
- Pagination
- Searching
- Filtering
- Sorting
- Unit testing
- Mocking with Moq
- EF Core InMemory testing
- Docker containerization
- Docker Compose
- MySQL database integration
- Entity Framework Core migrations

---

## 🎯 Project Purpose

MediCoreX was developed as a production-style backend project to demonstrate practical experience with modern .NET backend development, API security, database integration, testing, and containerization.

---

## 👨‍💻 Author

**Sumit Dubey**

Backend Developer

Focused on building secure, scalable, and maintainable backend APIs.
