using Amazon.Auth.AccessControlPolicy;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using WebbutvecklingLabb2.DataAccess.Interfaces;
using WebbutvecklingLabb2.DataAccess.Models;
using WebbutvecklingLabb2.Shared.DTOs;

namespace WebbutvecklingLabb2.DataAccess.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly IMongoDatabase _database;
    private readonly IMongoCollection<ProductModel> _collection;
    private readonly IUnitOfWork _unitOfWork;
    public ProductRepository(IConfiguration config, IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        var databaseName = config["DatabaseNames:MongoDb"];
        var settings = MongoClientSettings.FromConnectionString(config["ConnectionStrings:MongoDb"]);
        settings.ServerApi = new ServerApi(ServerApiVersion.V1);

        var client = new MongoClient(settings);
        var database = client.GetDatabase(databaseName);
        _collection =
            database.GetCollection<ProductModel>("Products",
                new MongoCollectionSettings() { AssignIdOnInsert = true });
    }
    public void Add(ProductDto item)
    {
        Action operation = () =>
        {
            _collection.InsertOne(ConvertToModel(item));
        };
        _unitOfWork.AddOperation(operation);
    }

    public async Task<IEnumerable<ProductDto>> GetAllAsync()
    {
        var filter = Builders<ProductModel>
            .Filter.Empty;
        var all = await _collection
            .FindAsync(filter);
        return all.ToEnumerable().Select(ConvertToDto);
    }

    public void Replace(object id, ProductDto item)
    {
        Action operation = () =>
        {
            var filter = Builders<ProductModel>
                .Filter.Eq("_id", id);
            var update = Builders<ProductModel>
                .Update
                .Set("ProductName", item.ProductName)
                .Set("ProductDescription", item.ProductDescription)
                .Set("Price", item.Price)
                .Set("Category", item.Category)
                .Set("IsAvailable", item.IsAvailable)
                .Set("ImageSource", item.ImageSource);

            _collection.FindOneAndUpdate(filter, update);
        };
        _unitOfWork.AddOperation(operation);
    }

    public void Delete(object id)
    {
        Action operation = () =>
        {
            var filter = Builders<ProductModel>
                .Filter.Eq("_id", id);
            _collection.FindOneAndDelete(filter);
        };
        _unitOfWork.AddOperation(operation);
    }

    public async Task<bool> CheckExistsByPropertyAsync(string property, object value)
    {
        var filter = Builders<ProductModel>
            .Filter.Eq(property, value);
        return await _collection.Find(filter).CountDocumentsAsync() > 0;
    }

    public async Task<ProductDto> GetByPropertyAsync(string property, object value)
    {
        var filter = Builders<ProductModel>
            .Filter.Eq(property, value);
        var results = await _collection.FindAsync(filter);
        return ConvertToDto(results.First());
    }

    public void SetIsAvailable(object id, bool isAvailable)
    {
        Action operation = () =>
        {
            var filter = Builders<ProductModel>
                .Filter.Eq("_id", id);
            var update = Builders<ProductModel>
                .Update
                .Set("IsAvailable", isAvailable);

            _collection.FindOneAndUpdate(filter, update);
        };
        _unitOfWork.AddOperation(operation);
    }

    private ProductModel ConvertToModel(ProductDto dto)
    {
        return new ProductModel
        {
            ProductDescription = dto.ProductDescription,
            ProductName = dto.ProductName,
            ImageSource = dto.ImageSource,
            Category = dto.Category,
            Price = dto.Price,
            IsAvailable = dto.IsAvailable
        };
    }

    private ProductDto ConvertToDto(ProductModel model)
    {
        return new ProductDto
        {
            ProductNumber = model.ProductNumber.ToString(),
            ProductDescription = model.ProductDescription,
            ProductName = model.ProductName,
            ImageSource = model.ImageSource,
            Category = model.Category,
            Price = model.Price,
            IsAvailable = model.IsAvailable
        };
    }
}