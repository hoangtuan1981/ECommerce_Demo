# Architect use rules:
    DDD
    Clean Architecture
    CQRS
    MediatR
    FluentValidation
    Result Pattern
    Repository (if needed)
    Unit Of Work (if needed)
    JWT + Refresh Token
    Docker
    YARP
    Logging
    Exception Middleware
    Minimal API or RESTful API
    API Versioning:
        /api/v1/products
        /api/v2/products
    OpenAPI (Swagger)
    Endpoint doesn't contain Business Logic.
    Aggregate Root in Microservices
    BFF service use YARP (Aggregation)
# Frontend (ReactJS):

Vite + React 18 + TypeScript.
State: Zustand (or Redux Toolkit).
API: Axios + interceptors (auto refresh token).
UI: shadcn/ui + Tailwind + TanStack Query.
Auth: Context + Protected Routes.
Routing: React Router v6.4+ (file-based hoặc config).

Cross-cutting:

Docker + docker-compose (multi-service).
JWT + Refresh Token Rotation (secure).
Result Pattern, FluentValidation, Global Exception.
OpenAPI (Swagger).


/ecommerce-microservices
├── src/
│   ├── Gateway/ApiGateway          # YARP
│   ├── Services/
│   │   ├── Identity/
│   │   ├── Product/
│   │   ├── Cart/
│   │   ├── Order/
│   │   └── ... (each service follow Clean Arch)
│   ├── Shared/                     # Common (DTOs, Events, Exceptions)
│   └── frontend/                   # React App
├── docker-compose.yml
├── .env (if needed)
└── README.md


# Architecture
    1. Depend:

    Presentation (Minimal API)
            │
            ▼
    Application
        Commands
        Queries
        Handlers
        Validators
        DTO
        Interfaces
        Behaviors
            │
            ▼
    Infrastructure
        Repository
        DbContext
        Jwt
        PasswordHasher
        RefreshToken
            │
            ▼
    Domain

    2. What does Clean Architecture require?

        Dependency luôn phải hướng vào trong.

            Presentation
                    │
            Infrastructure
                    │
            Application
                    │
            Domain

        Application doesn't know:
            EF Core
            Dapper
            MongoDB
            SQL Server
            Redis

        Application only know:
            Interface
            Business Rules

# ECommerce_Demo

    1. ApiGateway --> BFF
        use Yarp.ReverseProxy:  

    2. Identity.Application
        Seperate by CQRS

    React

        |

        BFF

        |

    -------------------------

    Catalog API

    Order API

    Identity API

    Payment API

    3. BFF sẽ Aggregation
        GET /home
        ↓
        Catalog
        ↓
        Promotion
        ↓
        Recommendation
        ↓
        Return
        Một request thay vì: React --> 4 APIs

# cài package hỗ trợ API Versioning chính thức của .NET:

Package: Asp.Versioning.Http (cho Minimal API)

Package: Asp.Versioning.Mvc (cho Controller)

# Don't read .md files in Note_technical folder

# Swagger
    package:
        dotnet add package Swashbuckle.AspNetCore
    product api swagger:
        https://localhost:7002/swagger/index.html
# Health check
    dotnet add package Microsoft.AspNetCore.Diagnostics.HealthChecks
