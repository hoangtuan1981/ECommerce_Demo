# Clean Architecture + DDD
    Identity.API
            |
            |
    Identity.Application
            |
            |
    Identity.Domain
            |
            |
    Identity.Infrastructure

1. Identity.Domain
    chứa Business Logic.
    Identity.Domain
    │
    ├── Entities
    │      User.cs
    │
    ├── ValueObjects
    │
    ├── Enums
    │
    ├── Events
    │
    └── Repositories
        IUserRepository.cs

    Identity.Infrastructure
    │
    ├── Persistence
    │      IdentityDbContext.cs
    │
    ├── Configurations
    │      UserConfiguration.cs
    │
    ├── Repositories
    │      UserRepository.cs
    │
    ├── Migrations
    │
    └── DependencyInjection.cs


# Đây là mô hình RBAC (Role-Based Access Control)
    Users
    Roles
    Permissions
    RolePermissions
    UserRoles
    RefreshTokens
                User
                 │
        1        │        N
                 │
            UserRole
                 │
        N        │        1
                 │
                Role
                 │
        1        │        N
                 │
          RolePermission
                 │
        N        │        1
                 │
            Permission


        User
        │
        │1
        │
        N
        RefreshToken

# Sau này sẽ có

    Customer
    Seller
    Admin
    Warehouse
    Support
    Finance
    SuperAdmin

    Một người có thể đồng thời là

    Admin
    Warehouse

    hoặc

    Support
    Finance



Một cải tiến nữa mình khuyến nghị

Đối với hệ thống Production, không nên lưu Token dạng plain text trong database.

Thay vì:

RefreshToken
----------------
Token

nên lưu:

RefreshToken
----------------
TokenHash

Luồng hoạt động:

Sinh refresh token ngẫu nhiên (ví dụ 64 byte).
Trả token gốc cho client.
Hash token bằng SHA-256 hoặc HMACSHA256.
Chỉ lưu TokenHash vào database.
Khi client gửi refresh token, hash lại rồi so sánh với TokenHash.

Điều này giúp nếu database bị lộ, kẻ tấn công không thể sử dụng trực tiếp các refresh token để đăng nhập. Đây là cách triển khai phổ biến trong các hệ thống Identity hiện đại và mình khuyến nghị áp dụng ngay từ đầu cho dự án của chúng ta.



Application
    │
    ├── Commands
    ├── Queries
    ├── Validators
    └── Repository Interfaces
            │
            ▼
Infrastructure
    ├── EF Core Repositories
    └── IdentityDbContext (SaveChangesAsync)


Vậy tại sao còn tạo IUnitOfWork?

Không phải vì EF Core thiếu.

Mà vì Clean Architecture.

Application Layer không nên biết EF Core.

Nếu Command Handler viết:
await _dbContext.SaveChangesAsync();

thì Application đang phụ thuộc Infrastructure.

Thay vào đó:

await _unitOfWork.SaveChangesAsync();

Application chỉ biết abstraction.


Identity.Application
│
└── Interfaces
    ├── IJwtTokenGenerator
    ├── IPasswordHasher
    └── IRefreshTokenGenerator

Identity.Infrastructure
│
└── Authentication
    ├── JwtTokenGenerator
    ├── PasswordHasher
    └── RefreshTokenGenerator

# Note 

Tuy nhiên, để tránh tạo quá nhiều repository "CRUD thuần", mình khuyến nghị:

Repository chỉ chứa các truy vấn/phương thức phục vụ Aggregate Root (User, Role).
Không tạo repository riêng cho các entity phụ như UserRole, RolePermission. Chúng nên được quản lý thông qua User và Role.
RefreshToken có thể có repository riêng vì thường có các truy vấn độc lập (tra cứu theo token, thu hồi token hết hạn...).

Với RBAC của chúng ta, mình sẽ chỉ tạo 4 repository:

IUserRepository
IRoleRepository
IPermissionRepository (chỉ nếu có màn hình quản lý Permission)
IRefreshTokenRepository

Không tạo IUserRoleRepository và IRolePermissionRepository. Điều này bám sát DDD hơn, giảm số lượng repository và giữ cho Aggregate Root chịu trách nhiệm quản lý các entity liên quan. Đây cũng là hướng mình đề xuất áp dụng xuyên suốt cho toàn bộ các microservice trong dự án.

JWT sẽ chứa gì?

Khi User đăng nhập:

User

↓

Roles

↓

Permissions

↓

JWT Claims

Ví dụ:

{
  "sub": "7c3b...",
  "email": "admin@shop.com",
  "roles": [
    "Admin",
    "Warehouse"
  ],
  "permissions": [
    "PRODUCT_CREATE",
    "PRODUCT_UPDATE",
    "ORDER_CREATE",
    "ORDER_CANCEL",
    "USER_MANAGEMENT"
  ]
}



# Bước 9. Tạo Migration

Từ thư mục solution: Add & Update migrations:

dotnet ef migrations add AddUserNameColumn --project src/Services/Identity/Identity.Infrastructure --startup-project src/Services/Identity/Identity.API

dotnet ef database update --project src/Services/Identity/Identity.Infrastructure --startup-project src/Services/Identity/Identity.API