using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using WebbutvecklingLabb2.DataAccess.Interfaces;
using WebbutvecklingLabb2.Shared.DTOs;

namespace WebbutvecklingLabb2.Server.Extensions;

public static class WebApplicationEndpointExtensions
{
    public static WebApplication MapCustomerEndpoints(this WebApplication app)
    {
        app.MapGet("/customers", GetAllCustomersHandler);
        app.MapGet("/customers/{email}", GetCustomerByEmailHandler);
        app.MapPost("/customers", AddCustomerHandler);
        app.MapPut("/customers/{id}", UpdateCustomerHandler);
        return app;
    }

    public static WebApplication MapProductEndpoints(this WebApplication app)
    {
        app.MapGet("/products", GetAllProductsHandler);
        app.MapPost("/products", AddProductHandler);
        app.MapPut("/products/{id}", UpdateProductHandler);
        app.MapPatch("/products/isAvailable/{id}", ProductIsAvailableHandler);
        app.MapGet("/products/name/{name}", GetProductByNameHandler);
        app.MapGet("/products/id/{id}", GetProductByIdHandler);
        app.MapDelete("/products/{id}", DeleteProductHandler);
        return app;
    }

    public static WebApplication MapOrderEndpoints(this WebApplication app)
    {
        app.MapGet("/orders", GetAllOrdersHandler);
        app.MapPost("/orders", AddOrderHandler);
        app.MapGet("/orders/{email}", GetOrdersByEmailHandler);
        return app;
    }

    #region Customer Endpoints

    public static async Task<IResult> GetAllCustomersHandler(ICustomerRepository repo)
    {
        return Results.Ok(await repo.GetAllAsync());
    }
    public static async Task<IResult> GetCustomerByEmailHandler(ICustomerRepository repo, string email)
    {
        if (!await repo.CheckExistsByPropertyAsync("E-Mail", email)) return Results.NotFound();

        return Results.Ok(await repo.GetByEmailAsync(email));
    }

    public static async Task<IResult> AddCustomerHandler(ICustomerRepository repo, IUnitOfWork unitOfWork, CustomerDto customer)
    {
        if (await repo.CheckExistsByPropertyAsync("E-Mail", customer.EMail)) return Results.NotFound();

        repo.Add(customer);
        await unitOfWork.CommitChanges();
        return Results.Ok();
    }

    public static async Task<IResult> UpdateCustomerHandler(ICustomerRepository repo, IUnitOfWork unitOfWork, ObjectId id, CustomerDto customer)
    {
        if (!await repo.CheckExistsByPropertyAsync("_id", id)) return Results.NotFound();

        repo.Replace(id, customer);
        await unitOfWork.CommitChanges();
        return Results.Ok();
    }

    #endregion

    #region Product Endpoints

    public static async Task<IResult> GetAllProductsHandler(IProductRepository repo)
    {
        return Results.Ok(await repo.GetAllAsync());
    }

    public static async Task<IResult> AddProductHandler(IProductRepository repo, IUnitOfWork unitOfWork, ProductDto product)
    {
        if (await repo.CheckExistsByPropertyAsync("ProductName", product.ProductName)) return Results.NotFound();

        repo.Add(product);
        await unitOfWork.CommitChanges();
        return Results.Ok();
    }

    public static async Task<IResult> UpdateProductHandler(IProductRepository repo, IUnitOfWork unitOfWork, ObjectId id, ProductDto product)
    {
        if (!await repo.CheckExistsByPropertyAsync("_id", id)) return Results.NotFound();

        repo.Replace(id, product);
        await unitOfWork.CommitChanges();
        return Results.Ok();
    }

    public static async Task<IResult> ProductIsAvailableHandler(IProductRepository repo, IUnitOfWork unitOfWork, ObjectId id, [FromBody] bool isAvailable)
    {
        if (!await repo.CheckExistsByPropertyAsync("_id", id)) return Results.NotFound();

        repo.SetIsAvailable(id, isAvailable);
        await unitOfWork.CommitChanges();
        return Results.Ok();
    }
    private static async Task<IResult> GetProductByNameHandler(IProductRepository repo, string name)
    {
        if (!await repo.CheckExistsByPropertyAsync("ProductName", name)) return Results.NotFound();

        return Results.Ok(await repo.GetByPropertyAsync("ProductName", name));
    }

    private static async Task<IResult> GetProductByIdHandler(IProductRepository repo, ObjectId id)
    {
        if (!await repo.CheckExistsByPropertyAsync("_id", id)) return Results.NotFound();

        return Results.Ok(await repo.GetByPropertyAsync("_id", id));
    }

    private static async Task<IResult> DeleteProductHandler(IProductRepository repo, IUnitOfWork unitOfWork, ObjectId id)
    {
        if (!await repo.CheckExistsByPropertyAsync("_id", id)) return Results.NotFound();

        repo.Delete(id);
        await unitOfWork.CommitChanges();
        return Results.Ok();
    }

    #endregion

    #region Order Endpoints

    public static async Task<IResult> GetAllOrdersHandler(IOrderRepository repo)
    {
        return Results.Ok(await repo.GetAllAsync());
    }

    public static async Task<IResult> AddOrderHandler(IOrderRepository repo, IUnitOfWork unitOfWork, OrderDto order)
    {
        repo.Add(order);
        await unitOfWork.CommitChanges();
        return Results.Ok();
    }

    public static async Task<IResult> GetOrdersByEmailHandler(IOrderRepository repo, string email)
    {
        return Results.Ok(await repo.GetByCustomerEmail(email));
    }

    #endregion

}