using Product.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Product.Application.Features.Products.ActivateProduct;

public sealed record ActivateProductResponse(
    Guid Id,
    ProductStatus Status
);