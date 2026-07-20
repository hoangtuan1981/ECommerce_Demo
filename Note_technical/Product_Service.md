# Microsoft 365 copilot

Dựa trên cấu trúc Identity Service trong file của bạn và kiến trúc đã chọn (DDD + Clean Architecture + CQRS + MediatR + FluentValidation + Result Pattern + EF Core + Minimal API), tôi đề xuất xây dựng Product Service theo đúng pattern hiện tại để đảm bảo đồng nhất giữa các microservices.

1. Tạo Solution Structure cho Product Service

Cấu trúc nên tương tự Identity:

Product.API
Product.Application
Product.Domain
Product.Infrastructure

2. Product.Domain
2.1 Common
Product.Domain
│
├── Common
│   ├── Entity.cs
│   ├── AggregateRoot.cs
│   └── AuditableEntity.cs


Tái sử dụng từ Identity.

2.2 Entities
Product.Domain
│
└── Entities
    ├── Product.cs
    ├── Category.cs
    ├── ProductImage.cs
    ├── ProductVariant.cs

Product
public class Product : AggregateRoot<Guid>
{
    public string ProductCode { get; private set; }
    public string ProductName { get; private set; }
    public Guid CategoryId { get; private set; }

    public decimal Price { get; private set; }

    public string Description { get; private set; }

    public int Quantity { get; private set; }

    public bool IsDeleted { get; private set; }

    public ProductStatus Status { get; private set; }

    public ICollection<ProductVariant> Variants { get; private set; }

    public ICollection<ProductImage> Images { get; private set; }
}

ProductVariant

Tách Size/Color thành Variant.

public class ProductVariant : Entity<Guid>
{
    public Guid ProductId { get; private set; }

    public string Color { get; private set; }

    public string Size { get; private set; }

    public int Quantity { get; private set; }
}

ProductImage
public class ProductImage : Entity<Guid>
{
    public Guid ProductId { get; private set; }

    public string ImageUrl { get; private set; }

    public bool IsDefault { get; private set; }
}

Category
public class Category : AggregateRoot<Guid>
{
    public string CategoryCode { get; private set; }

    public string CategoryName { get; private set; }
}

3. Product.Application

Tương tự Identity.Application.

3.1 Common
Product.Application
│
├── Common
│   ├── Persistence
│   │   ├── IProductRepository.cs
│   │   ├── ICategoryRepository.cs
│   │   └── IUnitOfWork.cs
│   │
│   ├── Pagination
│   │   ├── PaginationRequest.cs
│   │   └── PagedResult.cs
│   │
│   └── Results
│       ├── Result.cs
│       ├── ResultT.cs
│       ├── Error.cs
│       └── ErrorType.cs


Có thể dùng chung Result Pattern hiện tại của Identity.

3.2 Features
Features
│
└── Products
    │
    ├── CreateProduct
    │   ├── CreateProductCommand.cs
    │   ├── CreateProductCommandHandler.cs
    │   ├── CreateProductCommandValidator.cs
    │   └── CreateProductResponse.cs
    │
    ├── UpdateProduct
    │
    ├── DeleteProduct
    │
    ├── GetProductById
    │
    └── GetProducts

4. CQRS Commands & Queries
Create Product
CreateProduct
├── CreateProductCommand
├── CreateProductCommandHandler
├── CreateProductCommandValidator
└── CreateProductResponse

Update Product
UpdateProduct
├── UpdateProductCommand
├── UpdateProductCommandHandler
├── UpdateProductCommandValidator
└── UpdateProductResponse

Soft Delete
DeleteProduct
├── DeleteProductCommand
├── DeleteProductCommandHandler
└── DeleteProductResponse


SQL thực tế:

product.IsDeleted = true;

Product Detail
GetProductById
├── GetProductByIdQuery
├── GetProductByIdQueryHandler
└── ProductDetailResponse

Product Listing
GetProducts
├── GetProductsQuery
├── GetProductsQueryHandler
└── ProductListResponse


Hỗ trợ:

Paging
Search
Filter
5. Product Filter Model
public class ProductSearchRequest
{
    public string? Keyword { get; set; }

    public string? ProductCode { get; set; }

    public Guid? CategoryId { get; set; }

    public string? Color { get; set; }

    public string? Size { get; set; }

    public decimal? MinPrice { get; set; }

    public decimal? MaxPrice { get; set; }

    public int PageNumber { get; set; }

    public int PageSize { get; set; }
}


Đáp ứng đầy đủ yêu cầu:

Tìm theo tên
Tìm theo mã
Lọc Category
Lọc Color
Lọc Size
Lọc Price Range
Pagination
6. Product.Infrastructure
6.1 DbContext
Product.Infrastructure
│
├── ProductDbContext.cs

public class ProductDbContext : DbContext
{
    public DbSet<Product> Products { get; set; }

    public DbSet<Category> Categories { get; set; }

    public DbSet<ProductVariant> ProductVariants { get; set; }

    public DbSet<ProductImage> ProductImages { get; set; }
}

6.2 Configurations
Configurations
│
├── ProductConfiguration.cs
├── CategoryConfiguration.cs
├── ProductVariantConfiguration.cs
└── ProductImageConfiguration.cs


Giống cách Identity cấu hình entity bằng Fluent API.

6.3 Repositories
Persistence
│
├── Repositories
│   ├── ProductRepository.cs
│   ├── CategoryRepository.cs
│   └── UnitOfWork.cs

7. Product.API
7.1 Contracts
Contracts
│
└── Products
    ├── CreateProductRequest.cs
    ├── UpdateProductRequest.cs
    └── ProductSearchRequest.cs

7.2 Endpoints
Endpoints
│
├── ProductEndpoints.cs
└── EndpointExtensions.cs


Tương tự AuthenticationEndpoints.cs, UserEndpoints.cs trong Identity.

8. Minimal API Endpoints
Public
GET    /api/products
GET    /api/products/{id}

Admin
POST   /api/products

PUT    /api/products/{id}

DELETE /api/products/{id}

9. Database Design
Product
Products
---------
Id
ProductCode
ProductName
CategoryId
Price
Description
Quantity
Status
IsDeleted
CreatedDate
UpdatedDate

ProductVariants
ProductVariants
---------------
Id
ProductId
Color
Size
Quantity

ProductImages
ProductImages
-------------
Id
ProductId
ImageUrl
IsDefault

Categories
Categories
----------
Id
CategoryCode
CategoryName

10. Triển khai theo Sprint
Sprint 1
Product Domain
EF Core
Migration
Repository
UnitOfWork
Sprint 2
Create Product
Update Product
Delete Product
Sprint 3
Get Product Detail
Get Product List
Pagination
Sprint 4
Search
Category Filter
Size Filter
Color Filter
Price Range Filter
Sprint 5
JWT Authorization
API Gateway YARP Mapping
Docker
Cấu trúc thư mục Product Service cuối cùng
Product.API
│
├── Contracts
│   └── Products
├── Endpoints
├── DependencyInjection.cs
└── Program.cs

Product.Application
│
├── Common
│   ├── Persistence
│   ├── Pagination
│   └── Results
│
├── Errors
│   └── ProductErrors.cs
│
└── Features
    └── Products
        ├── CreateProduct
        ├── UpdateProduct
        ├── DeleteProduct
        ├── GetProductById
        └── GetProducts

Product.Domain
│
├── Common
└── Entities
    ├── Product.cs
    ├── Category.cs
    ├── ProductVariant.cs
    └── ProductImage.cs

Product.Infrastructure
│
├── Authentication
├── Configurations
├── Persistence
│   ├── Extensions
│   └── Repositories
├── Migrations
├── Seed
├── ProductDbContext.cs
└── DependencyInjection.cs


Đây là cấu trúc gần như 100% tương thích với Identity Service hiện tại, giúp bạn phát triển Product Service nhanh mà vẫn giữ chuẩn Clean Architecture + DDD + CQRS cho toàn bộ hệ thống E-Commerce.