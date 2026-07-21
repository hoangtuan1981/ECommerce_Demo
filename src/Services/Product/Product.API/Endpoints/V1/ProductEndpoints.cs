using MediatR;
using Product.API.Contracts.Products;
using Product.Application.Features.Products.ActivateProduct;
using Product.Application.Features.Products.CreateProduct;
using Product.Application.Features.Products.DeleteProduct;
using Product.Application.Features.Products.InactivateProduct;
using Product.Application.Features.Products.UpdateProduct;

namespace Product.API.Endpoints.V1;

public static class ProductEndpoints
{
    public static IEndpointRouteBuilder MapAuthenticationEndpoints(
        this IEndpointRouteBuilder app)
    {
        var v1 = app.MapGroup("/api/v1")
                    .WithTags("product");

        v1.MapPost("/", CreateProduct);

        v1.MapPut("/{id:guid}", UpdateProduct);

        v1.MapDelete("/{id:guid}", DeleteProduct);

        v1.MapPost(
            "/{id:guid}/activate",
            ActivateProduct);

        v1.MapPost(
            "/{id:guid}/inactivate",
            InactivateProduct);

        return app;
    }

    private static async Task<IResult> CreateProduct(
        CreateProductRequest request,
        ISender sender)
    {
        var command = new CreateProductCommand(
            request.Name,
            request.Description,
            request.Price,
            request.StockQuantity,
            request.CategoryId
            );

        var result = await sender.Send(command);

        if (!result.IsSuccess)
        {
            return Results.BadRequest(result.Error);
        }

        return Results.Created(
            $"/api/v1/products/{result.Value.Id}",
            result.Value);
    }

    private static async Task<IResult> UpdateProduct(
        Guid id,
        UpdateProductRequest request,
        ISender sender)
    {
        var command = new UpdateProductCommand(
            id,
            request.Name,
            request.Description,
            request.Price,
            request.StockQuantity,
            request.CategoryId);

        var result = await sender.Send(command);

        if (!result.IsSuccess)
        {
            return Results.BadRequest(result.Error);
        }

        return Results.Ok(result.Value);
    }

    private static async Task<IResult> DeleteProduct(
        Guid id,
        ISender sender)
    {
        var command = new DeleteProductCommand(id);

        var result = await sender.Send(command);

        if (!result.IsSuccess)
        {
            return Results.BadRequest(result.Error);
        }

        return Results.Ok(result.Value);
    }

    private static async Task<IResult> ActivateProduct(
        Guid id,
        ISender sender)
    {
        var result = await sender.Send(
            new ActivateProductCommand(id));

        // TODO
        //return result.IsSuccess
        //    ? Results.Ok(result.Value)
        //    : result.ToProblemDetails();
        return Results.Ok(result.Value);
    }

    private static async Task<IResult> InactivateProduct(
        Guid id,
        ISender sender)
    {
        var result = await sender.Send(
            new InactivateProductCommand(id));

        // TODO
        //return result.IsSuccess
        //    ? Results.Ok(result.Value)
        //    : result.ToProblemDetails();
        return Results.Ok(result.Value);
    }
}
