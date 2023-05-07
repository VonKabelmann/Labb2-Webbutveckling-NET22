namespace WebbutvecklingLabb2.DataAccess.Interfaces;

public interface IUnitOfWork
{
    IDisposable Session { get; }
    void AddOperation(Action operation);
    void CleanOperations();
    Task CommitChanges();
}