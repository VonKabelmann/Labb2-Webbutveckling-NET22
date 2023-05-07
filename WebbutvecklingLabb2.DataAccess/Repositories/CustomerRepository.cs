using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using WebbutvecklingLabb2.DataAccess.Interfaces;
using WebbutvecklingLabb2.DataAccess.Models;
using WebbutvecklingLabb2.Shared.DTOs;

namespace WebbutvecklingLabb2.DataAccess.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly IMongoDatabase _database;
    private readonly IMongoCollection<CustomerModel> _collection;
    private readonly IUnitOfWork _unitOfWork;

    public CustomerRepository(IConfiguration config, IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;

        var databaseName = config["DatabaseNames:MongoDb"];
        var settings = MongoClientSettings.FromConnectionString(config["ConnectionStrings:MongoDb"]);
        settings.ServerApi = new ServerApi(ServerApiVersion.V1);

        var client = new MongoClient(settings);
        _database = client.GetDatabase(databaseName);
        _collection =
            _database.GetCollection<CustomerModel>("Customers",
                new MongoCollectionSettings() { AssignIdOnInsert = true });
    }
    public void Add(CustomerDto item)
    {
        Action operation = () =>
        {
            _collection.InsertOne(ConvertToModel(item));
        };
        _unitOfWork.AddOperation(operation);
    }

    public async Task<IEnumerable<CustomerDto>> GetAllAsync()
    {
        var filter = Builders<CustomerModel>
            .Filter.Empty;
        var all = await _collection
            .FindAsync(filter);
        return all.ToEnumerable().Select(ConvertToDto);
    }

    public void Replace(object id, CustomerDto item)
    {
        Action operation = () =>
        {
            var filter = Builders<CustomerModel>
                .Filter.Eq("_id", id);
            var update = Builders<CustomerModel>
                .Update
                .Set("FirstName", item.FirstName)
                .Set("LastName", item.LastName)
                .Set("Country", item.Country)
                .Set("City", item.City)
                .Set("PostalCode", item.PostalCode)
                .Set("Address", item.Address)
                .Set("PhoneNumber", item.PhoneNumber);

            _collection.FindOneAndUpdate(filter, update);
        };
        _unitOfWork.AddOperation(operation);

    }

    public void Delete(object id)
    {
        Action operation = () =>
        {
            var filter = Builders<CustomerModel>
                .Filter.Eq("_id", id);
            _collection.FindOneAndDelete(filter);
        };
        _unitOfWork.AddOperation(operation);
    }

    public async Task<bool> CheckExistsByPropertyAsync(string property, object value)
    {
        var filter = Builders<CustomerModel>
            .Filter.Eq(property, value);
        return await _collection.Find(filter)
            .CountDocumentsAsync() > 0;
    }

    public async Task<CustomerDto> GetByEmailAsync(object email)
    {
        var filter = Builders<CustomerModel>
            .Filter.Eq("E-Mail", email);
        var results = await _collection.FindAsync(filter);

        return ConvertToDto(results.First());
    }

    private CustomerModel ConvertToModel(CustomerDto dto)
    {
        return new CustomerModel
        {
            Address = dto.Address,
            PhoneNumber = dto.PhoneNumber,
            City = dto.City,
            PostalCode = dto.PostalCode,
            Country = dto.Country, 
            EMail = dto.EMail,
            FirstName = dto.FirstName,
            LastName = dto.LastName
        };
    }

    private CustomerDto ConvertToDto(CustomerModel model)
    {
        return new CustomerDto
        {
            Id = model.Id.ToString(),
            Address = model.Address,
            PhoneNumber = model.PhoneNumber,
            City = model.City,
            PostalCode = model.PostalCode,
            Country = model.Country,
            EMail = model.EMail,
            FirstName = model.FirstName,
            LastName = model.LastName
        };
    }
}