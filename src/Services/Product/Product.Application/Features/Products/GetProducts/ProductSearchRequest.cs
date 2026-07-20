namespace Product.Application.Features.Products.GetProducts;

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