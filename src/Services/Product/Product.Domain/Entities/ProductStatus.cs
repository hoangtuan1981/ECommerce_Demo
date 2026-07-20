namespace Product.Domain.Entities;

public enum ProductStatus
{
    Draft = 0,        // Mới tạo, chưa bán

    Active = 1,       // Đang kinh doanh

    OutOfStock = 2,   // Hết hàng

    Inactive = 3,     // Ngừng kinh doanh tạm thời

    Discontinued = 4  // Ngừng kinh doanh vĩnh viễn
}