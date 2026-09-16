# 🏥 MediCoreX – Healthcare Management System

MediCoreX is a full-stack healthcare management system built using **ASP.NET Core Web API, Angular, MySQL, Docker, and Google Gemini GenAI**.

The project demonstrates practical software engineering concepts including secure authentication, role-based authorization, layered architecture, patient management, search, filtering, sorting, pagination, validation, global exception handling, structured logging, unit testing, containerization, and Generative AI integration.

---

## 🚀 Features

### 🤖 AI Patient Summary

- Integrated Google Gemini GenAI for patient record summarization
- Generates concise patient summaries from available patient information
- AI integration implemented through a dedicated `IAiService` and `AiService`
- Gemini API key is securely managed using environment variables / .NET User Secrets
- AI endpoint is protected using Admin role-based authorization
- Prompt is designed to avoid medical diagnosis and treatment recommendations

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

### 🛠️ Backend

- ASP.NET Core Web API
- .NET 9
- Layered Architecture
- Dependency Injection
- Entity Framework Core 9
- MySQL
- LINQ
- DTO Pattern
- AutoMapper
- FluentValidation
- Global Exception Handling Middleware
- Custom Exceptions
- Structured Logging using `ILogger`
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
- MySQL health check
- Environment-based configuration

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
- AI Patient Summary
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

AI Architecture
Angular UI
    │
    │ Generate AI Summary
    ▼
AiController
    │
    ▼
IAiService
    │
    ▼
AiService
    │
    ▼
Google Gemini API
    │
    ▼
AI Generated Summary
    │
    ▼
Angular UI
🔐 Authentication & Authorization

MediCoreX uses JWT-based authentication with role-based authorization.

Authentication Flow
User Login
    ↓
ASP.NET Core validates credentials
    ↓
Password verification
    ↓
JWT Access Token generated
    ↓
Refresh Token generated
    ↓
Tokens returned to Angular
    ↓
Angular sends Access Token with API requests
    ↓
API validates JWT
Refresh Token Flow
Access Token expires
        ↓
API returns 401
        ↓
Angular Interceptor detects 401
        ↓
Refresh Token sent to API
        ↓
API validates Refresh Token
        ↓
New Access Token + Refresh Token generated
        ↓
Original request retried

Refresh tokens are rotated after successful refresh.

👥 Roles

The application supports two roles:

Role	Access
Admin	Patient management + AI summary + protected admin endpoints
User	Authentication-related functionality

Patient APIs and AI functionality are protected using role-based authorization.

Example:

[Authorize(Roles = "Admin")]
🤖 AI Patient Summary

MediCoreX integrates Google Gemini GenAI to generate a concise patient record summary.

The AI feature accepts basic patient information:

Patient Name
Age
Gender

The request is sent through the ASP.NET Core backend to Google Gemini.

API Endpoint
POST /api/Ai/patient-summary
Example Request
POST /api/Ai/patient-summary?fullName=Amit%20Singh&age=26&gender=Male
Example Response
{
  "patientName": "Amit Singh",
  "summary": "Amit Singh is a 26-year-old male patient. No additional medical history, clinical findings, or symptoms were included in the provided record."
}

The AI prompt is intentionally designed to:

Summarize only the provided information
Avoid medical diagnosis
Avoid treatment recommendations
Avoid medication recommendations
Return concise plain text
🔑 AI API Key Security

The Gemini API key is not stored directly in source code.

For local development, .NET User Secrets can be used:

dotnet user-secrets init
dotnet user-secrets set "Gemini:ApiKey" "YOUR_API_KEY"

For Docker, the API key is provided through the environment variable:

GEMINI_API_KEY

Docker Compose maps the environment variable to:

Gemini__ApiKey

This keeps the API key outside the source code and Git repository.

🧩 Patient Management

The system supports:

Create patient
Get all patients
Get patient by ID
Update patient
Delete patient
Search patients by name
Filter patients by gender
Filter patients by age
Sort patients by age
Sort patients by name
Pagination
Combined search + filter + sort + pagination
🔎 Combined Patient Query

The main patient filtering endpoint is:

GET /api/patients/filter

Supported parameters:

Page
PageSize
Search
Gender
SortBy
SortOrder
Example
GET /api/patients/filter?Page=1&PageSize=10&Search=Amit&Gender=Male&SortBy=age&SortOrder=asc

Example response:

{
  "page": 1,
  "pageSize": 10,
  "totalRecords": 1,
  "totalPages": 1,
  "data": [
    {
      "id": 1,
      "fullName": "Amit Singh",
      "age": 26,
      "gender": "Male"
    }
  ]
}

Pagination is implemented using LINQ:

query
    .Skip(skip)
    .Take(pageSize)
🧱 Project Structure
MediCoreX
│
├── MediCoreX.Api
│   │
│   ├── Controllers
│   │   ├── AuthController.cs
│   │   ├── PatientsController.cs
│   │   └── AiController.cs
│   │
│   ├── Services
│   │   ├── AuthService.cs
│   │   ├── PatientService.cs
│   │   ├── TokenService.cs
│   │   ├── AiService.cs
│   │   └── Interfaces
│   │
│   ├── Models
│   ├── DTOs
│   ├── Data
│   ├── Middleware
│   ├── Validators
│   ├── Mappings
│   ├── Migrations
│   ├── Program.cs
│   └── appsettings.json
│
├── MediCoreX.Tests
│   ├── AuthServiceTests
│   ├── PatientServiceTests
│   ├── TokenServiceTests
│   └── ValidatorTests
│
├── medicorex-ui
│   └── Angular Frontend
│
├── Dockerfile
├── docker-compose.yml
├── .gitignore
└── README.md
🌐 API Endpoints
Authentication
Method	Endpoint	Description
POST	/api/auth/register	Register a new user
POST	/api/auth/login	Login user
POST	/api/auth/refresh	Generate new access token
Patients
Method	Endpoint	Description
GET	/api/patients	Get all patients
GET	/api/patients/{id}	Get patient by ID
GET	/api/patients/above-age/{age}	Get patients above age
GET	/api/patients/gender/{gender}	Filter by gender
GET	/api/patients/search	Search patients
GET	/api/patients/sort	Sort patients by age
GET	/api/patients/filter	Search, filter, sort and paginate
GET	/api/patients/paged	Get paginated patients
POST	/api/patients	Create patient
PUT	/api/patients/{id}	Update patient
DELETE	/api/patients/{id}	Delete patient
AI
Method	Endpoint	Description
POST	/api/Ai/patient-summary	Generate AI patient summary
Admin
Method	Endpoint	Description
GET	/api/patients/admin-data	Admin-only endpoint
🛡️ Validation

The application uses FluentValidation for request validation.

Examples:

Required fields
Minimum string length
Valid email format
Password validation
Patient age validation
Gender validation

ASP.NET Core [ApiController] also handles automatic model validation responses.

⚠️ Global Exception Handling

MediCoreX uses custom middleware for centralized exception handling.

Controller
    ↓
Service
    ↓
Exception occurs
    ↓
Exception Middleware
    ↓
Consistent HTTP response

This prevents repetitive try/catch blocks across controllers and provides centralized error handling.

📝 Logging

The application uses ILogger<T> for structured logging.

Logging is implemented for important application events such as:

Authentication
Patient operations
Errors
Exceptions
Service operations
🗄️ Database

MediCoreX uses:

MySQL 8
Entity Framework Core 9
Pomelo.EntityFrameworkCore.MySql

Entity Framework Core manages:

Database connection
Entity mapping
LINQ queries
Database migrations
CRUD operations
💉 Dependency Injection

The application uses ASP.NET Core built-in Dependency Injection.

Examples:

builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IAiService, AiService>();

The service layer receives dependencies through constructor injection.

🧪 Unit Testing

The project contains backend unit tests using xUnit.

Current test coverage includes:

AuthService                 8 tests
PatientService             15 tests
RegisterDtoValidator        7 tests
CreatePatientValidator      6 tests
UpdatePatientValidator      6 tests
TokenService                5 tests
------------------------------------
Total                      47 tests

Run tests using:

dotnet test
🐳 Docker

The application supports Docker-based development.

Docker Compose runs:

Angular
   │
   ▼
ASP.NET Core API
   │
   ▼
MySQL

The backend API runs on:

http://localhost:8080

MySQL is exposed on:

localhost:3307

Start the application:

docker compose up --build

Run in detached mode:

docker compose up --build -d

Stop containers:

docker compose down

Check containers:

docker compose ps
⚙️ Local Backend Setup

Clone the repository:

git clone https://github.com/sumitdy/MediCoreX.git

Move into the project:

cd MediCoreX

Run the API:

cd MediCoreX.Api
dotnet restore
dotnet build
dotnet run
🔐 Configure Secrets

For local development, configure sensitive values using .NET User Secrets.

Example:

dotnet user-secrets init

Configure Gemini:

dotnet user-secrets set "Gemini:ApiKey" "YOUR_API_KEY"

Do not commit API keys, passwords, or other secrets to Git.

🖥️ Angular Frontend Setup

Move to the Angular project:

cd medicorex-ui

Install dependencies:

npm install

Run Angular:

ng serve

The frontend will normally be available at:

http://localhost:4200

The Angular application communicates with the ASP.NET Core API.

📖 Swagger

Swagger/OpenAPI is enabled for API testing and documentation.

When running the API locally, open:

http://localhost:5038/swagger

When running through Docker:

http://localhost:8080/swagger

Swagger can be used to test:

Authentication
Patient APIs
Refresh token flow
AI patient summary
Admin-protected endpoints
🛠️ Technology Stack
Backend
C#
.NET 9
ASP.NET Core Web API
Entity Framework Core 9
MySQL
LINQ
JWT
FluentValidation
AutoMapper
Swagger / OpenAPI
xUnit
Frontend
Angular
TypeScript
HTML
SCSS
Reactive Forms
Angular Router
HTTP Interceptors
AI
Google Gemini
Google.GenAI SDK
Generative AI
DevOps
Docker
Docker Compose
Git
GitHub
📚 Key Concepts Demonstrated

This project demonstrates practical understanding of:

REST API development
Layered Architecture
Dependency Injection
SOLID principles
DTO pattern
AutoMapper
Entity Framework Core
LINQ
Async/Await
JWT Authentication
Role-Based Authorization
Refresh Token Rotation
Password Hashing
Middleware
Global Exception Handling
Structured Logging
FluentValidation
Pagination
Search / Filter / Sort
Unit Testing
Docker
Environment-based configuration
GenAI API integration
Secure API key management
🎯 Project Purpose

MediCoreX was developed as an interview-focused full-stack project to demonstrate practical software engineering skills across backend API development, authentication, database management, testing, frontend integration, Docker, and AI integration.

The project focuses on implementing real-world backend concepts using a clean and maintainable architecture rather than building unnecessary business complexity.

🔗 Repository

GitHub:

https://github.com/sumitdy/MediCoreX

👨‍💻 Author
Sumit Dubey

Full Stack Developer | .NET & Angular

Focused on building secure, maintainable, and scalable web applications using ASP.NET Core, Angular, Entity Framework Core, and MySQL.

