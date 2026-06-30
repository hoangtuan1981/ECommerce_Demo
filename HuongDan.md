# Hướng dẫn xây dựng E-Commerce Microservices Solution - chatGPT

## Step 1: Tạo Solution
```bash
mkdir ecommerce
cd ecommerce

dotnet new sln -n ECommerce
mkdir src
cd src
```

## Step 2: Tạo folder structure
```bash
mkdir BuildingBlocks
mkdir Gateway
mkdir BFF
mkdir Services
mkdir Frontend
```

## Step 3: Tạo Identity Service (Clean Architecture)
```bash
cd Services
mkdir Identity
cd Identity

dotnet new webapi -n Identity.API
dotnet new classlib -n Identity.Application
dotnet new classlib -n Identity.Domain
dotnet new classlib -n Identity.Infrastructure
```

## Step 4: Add vào Solution
```bash
cd ../../../

dotnet sln add src/Services/Identity/Identity.API
dotnet sln add src/Services/Identity/Identity.Application
dotnet sln add src/Services/Identity/Identity.Domain
dotnet sln add src/Services/Identity/Identity.Infrastructure
```

## Step 5: Setup dependencies
```bash
cd src/Services/Identity

# Application -> Domain
dotnet add Identity.Application reference Identity.Domain

# Infrastructure -> Application + Domain
dotnet add Identity.Infrastructure reference Identity.Application
dotnet add Identity.Infrastructure reference Identity.Domain

# API -> Application + Infrastructure
dotnet add Identity.API reference Identity.Application
dotnet add Identity.API reference Identity.Infrastructure
```

## Step 6: Tạo API Gateway
```bash
cd ../../Gateway

dotnet new webapi -n ApiGateway
dotnet sln add src/Gateway/ApiGateway

cd ApiGateway
dotnet add package Yarp.ReverseProxy
```

### Program.cs
```csharp
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

app.MapReverseProxy();
```

### appsettings.json
```json
{
  "ReverseProxy": {
    "Routes": {
      "identityRoute": {
        "ClusterId": "identityCluster",
        "Match": {
          "Path": "/identity/{**catch-all}"
        }
      }
    },
    "Clusters": {
      "identityCluster": {
        "Destinations": {
          "d1": {
            "Address": "http://localhost:5001/"
          }
        }
      }
    }
  }
}
```

## Step 7: Tạo BFF
```bash
cd ../../BFF

dotnet new webapi -n WebBff
dotnet sln add src/BFF/WebBff
```

## Step 8: BuildingBlocks
```bash
cd ../BuildingBlocks

dotnet new classlib -n SharedKernel
dotnet sln add src/BuildingBlocks/SharedKernel
```

## Step 9: Frontend React
```bash
cd ../Frontend
npx create-react-app ReactApp
```

## Step 10: Docker Compose
```yaml
version: '3.8'

services:
  identity.api:
    build: ./src/Services/Identity/Identity.API
    ports:
      - "5001:80"

  apigateway:
    build: ./src/Gateway/ApiGateway
    ports:
      - "5000:80"

  webbff:
    build: ./src/BFF/WebBff
    ports:
      - "5002:80"

  frontend:
    build: ./src/Frontend/ReactApp
    ports:
      - "3000:3000"
```

## Step 11: Flow hệ thống
```
React → BFF → API Gateway → Microservices
```

## Step 12: Mở rộng services
- Catalog
- Basket
- Order
- Payment

## Step 13: Infra
- Redis
- RabbitMQ
- SQL Server/PostgreSQL

## Step 14: Patterns
- CQRS + MediatR
- Event-driven
- Saga

## Step 15: Best Practices
- Mỗi service 1 DB riêng
- Không share DB
- Giao tiếp qua API/Event
- Gateway không chứa business logic
