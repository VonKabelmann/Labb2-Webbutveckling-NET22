using WebbutvecklingLabb2.DataAccess.Models;
using WebbutvecklingLabb2.Shared.DTOs;

namespace WebbutvecklingLabb2.DataAccess.Interfaces;

public interface IProductRepository
{
    void Add(ProductDto item);
    Task<IEnumerable<ProductDto>> GetAllAsync();
    void Replace(object id, ProductDto item);
    void Delete(object id);
    Task<bool> CheckExistsByPropertyAsync(string property, object value);
    Task<ProductDto> GetByPropertyAsync(string property, object value);
    void SetIsAvailable(object id, bool isAvailable);
}