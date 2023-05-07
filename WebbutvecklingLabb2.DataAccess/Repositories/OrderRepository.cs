using Amazon.Runtime.Internal.Transform;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using WebbutvecklingLabb2.DataAccess.Interfaces;
using WebbutvecklingLabb2.DataAccess.Models;
using WebbutvecklingLabb2.Shared.DTOs;

namespace WebbutvecklingLabb2.DataAccess.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly IMongoDatabase _database;
    private readonly IMongoCollection<OrderModel> _collection;
    private readonly IUnitOfWork _unitOfWork;

    public OrderRepository(IConfiguration config, IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        var databaseName = config["DatabaseNames:MongoDb"];
        var settings = MongoClientSettings.FromConnectionString(config["ConnectionStrings:MongoDb"]);
        settings.ServerApi = new ServerApi(ServerApiVersion.V1);

        var client = new MongoClient(settings);
        _database = client.GetDatabase(databaseName);
        _collection =
            _database.GetCollection<OrderModel>("Orders",
                new MongoCollectionSettings() { AssignIdOnInsert = true });
    }
    public void Add(OrderDto item)
    {
        Action operation = () =>
        {
            _collection.InsertOne(ConvertToModel(item));
        };
        _unitOfWork.AddOperation(operation);
    }

    public async Task<IEnumerable<OrderDto>> GetAllAsync()
    {
        var filter = Builders<OrderModel>
            .Filter.Empty;
        var all = await _collection
            .FindAsync(filter);
        return all.ToEnumerable().Select(ConvertToDto);
    }

    public async Task<bool> CheckExistsByPropertyAsync(string property, object value)
    {
        var filter = Builders<OrderModel>
            .Filter.Eq(property, value);
        return await _collection.Find(filter)
            .CountDocumentsAsync() > 0;
    }

    public async Task<OrderDto> GetByIdAsync(object id)
    {
        var filter = Builders<OrderModel>
            .Filter.Eq("_id", id);
        var results = await _collection.FindAsync(filter);

        return ConvertToDto(results.First());
    }

    public async Task<IEnumerable<OrderDto>> GetByCustomerEmail(string email)
    {
        var filter = Builders<OrderModel>
            .Filter.Eq(order => order.Customer.EMail, email);
        var results = await _collection.FindAsync(filter);

        return results.ToEnumerable().Select(ConvertToDto);
    }

    private OrderModel ConvertToModel(OrderDto dto)
    {
        var productsList = dto.OrderedProducts.Select(orderItem => new OrderItemModel()
            {
                Amount = orderItem.Amount,
                Product = new ProductModel()
                {
                    ProductNumber = new ObjectId(orderItem.Product.ProductNumber),
                    ProductName = orderItem.Product.ProductName,
                    ProductDescription = orderItem.Product.ProductDescription,
                    Category = orderItem.Product.Category,
                    Price = orderItem.Product.Price,
                    ImageSource = orderItem.Product.ImageSource
                }
            });

        return new OrderModel
        {
            Customer = new CustomerModel
            {
                Id = new ObjectId(dto.Customer.Id),
                Address = dto.Customer.Address,
                PhoneNumber = dto.Customer.PhoneNumber,
                City = dto.Customer.City,
                PostalCode = dto.Customer.PostalCode,
                Country = dto.Customer.Country,
                EMail = dto.Customer.EMail,
                FirstName = dto.Customer.FirstName,
                LastName = dto.Customer.LastName
            },
            OrderedProducts = productsList,
            OrderDate = dto.OrderDate
        };
    }

    private OrderDto ConvertToDto(OrderModel model)
    {
        var productList = model.OrderedProducts.Select(orderItem => new OrderItemDto
        {
            Amount = orderItem.Amount,
            Product = new ProductDto
            {
                ProductNumber = orderItem.Product.ProductNumber.ToString(),
                ProductName = orderItem.Product.ProductName,
                ProductDescription = orderItem.Product.ProductDescription,
                Category = orderItem.Product.Category,
                Price = orderItem.Product.Price,
                IsAvailable = orderItem.Product.IsAvailable,
                ImageSource = orderItem.Product.ImageSource
            }
        });
        return new OrderDto
        {
            Customer = new CustomerDto
            {
                Id = model.Customer.Id.ToString(),
                FirstName = model.Customer.FirstName,
                LastName = model.Customer.LastName,
                Address = model.Customer.Address,
                City = model.Customer.City,
                Country = model.Customer.Country,
                EMail = model.Customer.EMail,
                PhoneNumber = model.Customer.PhoneNumber,
                PostalCode = model.Customer.PostalCode
            },
            OrderedProducts = productList,
            OrderDate = model.OrderDate,
            Id = model.Id.ToString()
        };
    }
}