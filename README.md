# 🏥 MediCoreX – Healthcare Management REST API

MediCoreX is a production-style backend system built using ASP.NET Core and MySQL.  
It demonstrates secure authentication, role-based authorization, layered architecture, and scalable backend design principles.

---

## 🚀 Features

- 🔐 JWT Authentication
- 👥 Role-Based Authorization (Admin / User)
- ⚙️ Configurable Admin Limit (via appsettings.json)
- 🧱 Layered Architecture (Controller → Service → DbContext)
- 📦 DTO Pattern + AutoMapper
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

## 🔑 Role Behavior

### Admin
- Can view all patient data
- Can manage users
- Restricted by configurable admin limit

### User
- Standard access
- Cannot self-assign admin role

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
