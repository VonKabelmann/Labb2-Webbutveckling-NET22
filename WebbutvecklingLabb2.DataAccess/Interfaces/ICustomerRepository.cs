using WebbutvecklingLabb2.Shared.DTOs;

namespace WebbutvecklingLabb2.DataAccess.Interfaces;

public interface ICustomerRepository
{
    void Add(CustomerDto item);
    Task<IEnumerable<CustomerDto>> GetAllAsync();
    void Replace(object id, CustomerDto item);
    void Delete(object id);
    Task<bool> CheckExistsByPropertyAsync(string property, object value);
    Task<CustomerDto> GetByEmailAsync(object email);
}