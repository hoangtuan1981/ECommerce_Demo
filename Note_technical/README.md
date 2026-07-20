# Architect dùng các nguyên tắc:
    DDD
    Clean Architecture
    CQRS
    MediatR
    FluentValidation
    Result Pattern
    Repository (chỉ khi thật sự cần)
    Unit Of Work (nếu cần)
    Minimal API
    JWT + Refresh Token
    Docker
    YARP
    Logging
    Exception Middleware
    API Versioning (nếu bạn muốn)
    OpenAPI
    Seed dữ liệu
    Không để Business Logic trong Endpoint

# Frontend (ReactJS):

Vite + React 18 + TypeScript.
State: Zustand (hoặc Redux Toolkit).
API: Axios + interceptors (auto refresh token).
UI: shadcn/ui + Tailwind + TanStack Query.
Auth: Context + Protected Routes.
Routing: React Router v6.4+ (file-based hoặc config).

Công nghệ Cross-cutting:

Docker + docker-compose (multi-service).
JWT + Refresh Token Rotation (secure).
Result Pattern, FluentValidation, Global Exception.
OpenAPI (Swagger).


/ecommerce-microservices
├── src/
│   ├── Gateway/ApiGateway          # YARP
│   ├── Services/
│   │   ├── Identity/               # (dựa repo gốc)
│   │   ├── Product/
│   │   ├── Cart/
│   │   ├── Order/
│   │   └── ... (mỗi service theo Clean Arch)
│   ├── Shared/                     # Common (DTOs, Events, Exceptions)
│   └── frontend/                   # React App
├── docker-compose.yml
├── .env
└── README.md


# Kiến trúc
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

    2. Clean Architecture yêu cầu gì?

        Dependency luôn phải hướng vào trong.

            Presentation
                    │
            Infrastructure
                    │
            Application
                    │
            Domain

        Application không được biết:
            EF Core
            Dapper
            MongoDB
            SQL Server
            Redis

        Application chỉ biết:

            Interface
            Business Rules

# ECommerce_Demo

    1. ApiGateway
        use Yarp.ReverseProxy:  

    2. Identity.Application
        Chia theo CQRS
        

# Build lại
dotnet clean
dotnet restore
dotnet build

# docker
docker compose down
docker compose build --no-cache
docker compose up


# Dockerfile
1. Build rieng le
    F:\Investigate\eCommerce\src> 

    1. ApiGateway
    cd src/Gateway/ApiGateway
    docker build -t ecommerce-apigateway .
    
     docker build -t ecommerce-apigateway .

    2. 
    docker build -t ecommerce-frontend .

    docker build -f Identity.API/Dockerfile -t identity-api .

# Frontend chỉ gọi Gateway
    http://localhost:5175/api/identity/auth/login
    http://localhost:5175/api/product/products
    http://localhost:5175/api/cart/items
    http://localhost:5175/api/order/orders


# EshopOnContainer BFF vs Gateway
1. chatGPT
    1. structure
                    Browser
                    │
                    │
                React / Blazor
                    │
                ┌────────────┐
                │     BFF    │
                └────────────┘
                        │
                ┌──────────────┐
                │ API Gateway  │
                └──────────────┘
            ┌────────┬─────────┬──────────┐
            │        │         │          │
    Catalog Identity Basket Ordering ...
    2. Giải pháp BFF
    Tạo backend riêng cho từng frontend:
        Web
        |
        Web BFF
        |
        Microservices


        Mobile
        |
        Mobile BFF
        |
        Microservices