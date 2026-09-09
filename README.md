# 🏥 MediCoreX – Healthcare Management REST API

MediCoreX is a production-style backend system built using ASP.NET Core and MySQL.  
It demonstrates secure authentication, role-based authorization, layered architecture, and scalable backend design principles.

---

## 🚀 Features

- 🔐 JWT Authentication
- 👥 Role-Based Authorization (Admin / User)
- 👤 Secure Admin Seeding using .NET User Secrets
- 🛡 Admin-only patient management
- ➕ Create Patient API
- ✏️ Update Patient API
- 📦 DTO Pattern + AutoMapper
- ✅ FluentValidation for registration and patient requests
- 🛡 Global Exception Handling Middleware
- 📊 Pagination Support
- 📝 Structured Logging
- 🐬 MySQL with Entity Framework Core

---

## 🏗 Architecture Overview

Controller  
   ↓  
Service Layer  
   ↓  
Entity Framework Core (DbContext)  
   ↓  
MySQL Database  

---

### Admin

- Can view, create, update, and delete patient records
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
| GET | `/api/patients` | Admin |
| POST | `/api/patients` | Admin |
| PUT | `/api/patients/{id}` | Admin |
| DELETE | `/api/patients/{id}` | Admin |
| POST | `/api/auth/refresh` | Public |

---

## 🛠 Tech Stack

- ASP.NET Core (.NET 9)
- Entity Framework Core
- MySQL
- Docker
- JWT
- AutoMapper

---

## ▶️ How to Run Locally

1. Clone the repository
2. Start MySQL using Docker
3. Update `appsettings.json` connection string if needed
4. Run the project:

dotnet restore  
dotnet run  

Swagger will be available at:

https://localhost:{port}/swagger  

Replace `{port}` with the port shown in your terminal after running the application.

---

## 📌 Learning Highlights

This project demonstrates:

- Clean separation of concerns
- Dependency Injection usage
- Secure password hashing
- Config-driven business rules
- Production-style error handling

---

## 👨‍💻 Author

Sumit Dubey  
Backend Developer  
Focused on secure and scalable API development.
