using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;
using WebbutvecklingLabb2.DataAccess.Interfaces;

namespace WebbutvecklingLabb2.DataAccess;

public class UnitOfWork : IUnitOfWork
{
    private IClientSessionHandle session { get; }
    public IDisposable Session => session;
    private List<Action> _operations { get; set; }
    public UnitOfWork(IConfiguration config)
    {
        var settings = MongoClientSettings.FromConnectionString(config["ConnectionStrings:MongoDb"]);
        settings.ServerApi = new ServerApi(ServerApiVersion.V1);
        var mongoClient = new MongoClient(settings);
        session = mongoClient.StartSession();

        _operations = new List<Action>();
    }
    public void AddOperation(Action operation)
    {
        _operations.Add(operation);
    }

    public void CleanOperations()
    {
        _operations.Clear();
    }

    public async Task CommitChanges()
    {
        session.StartTransaction();

        _operations.ForEach(o =>
        {
            o.Invoke();
        });

        await session.CommitTransactionAsync();

        CleanOperations();
    }
}