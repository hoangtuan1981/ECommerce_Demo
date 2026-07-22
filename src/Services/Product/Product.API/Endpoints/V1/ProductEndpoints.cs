using MediatR;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Product.API.Contracts.Products;
using Product.Application.Features.Products.ActivateProduct;
using Product.Application.Features.Products.CreateProduct;
using Product.Application.Features.Products.DeleteProduct;
using Product.Application.Features.Products.InactivateProduct;
using Product.Application.Features.Products.UpdateProduct;
using static FluentValidation.Validators.PredicateValidator;

namespace Product.API.Endpoints.V1;

public static class ProductEndpoints
{
    const string product_group_v1 = "/api/v1";
    public static IEndpointRouteBuilder MapProductEndpoints(
        this IEndpointRouteBuilder app)
    {
        var v1 = app.MapGroup(product_group_v1)
                    .WithTags("Products");

        v1.MapPost("/", CreateProduct);

        v1.MapPut("/{id:guid}", UpdateProduct);

        v1.MapDelete("/{id:guid}", DeleteProduct);

        v1.MapPost(
            "/{id:guid}/activate",
            ActivateProduct);

        v1.MapPost(
            "/{id:guid}/inactivate",
            InactivateProduct);

        v1.MapHealthChecks("/health/live",
                new HealthCheckOptions
                {
                    Predicate = _ => false
                });

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
            $"{product_group_v1}/products/{result.Value.Id}",
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
