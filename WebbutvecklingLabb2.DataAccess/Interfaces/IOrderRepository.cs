using WebbutvecklingLabb2.DataAccess.Models;
using WebbutvecklingLabb2.Shared.DTOs;

namespace WebbutvecklingLabb2.DataAccess.Interfaces;

public interface IOrderRepository
{
    void Add(OrderDto item);
    Task<IEnumerable<OrderDto>> GetAllAsync();
    Task<bool> CheckExistsByPropertyAsync(string property, object value);
    Task<OrderDto> GetByIdAsync(object id);
    Task<IEnumerable<OrderDto>> GetByCustomerEmail(string email);
}