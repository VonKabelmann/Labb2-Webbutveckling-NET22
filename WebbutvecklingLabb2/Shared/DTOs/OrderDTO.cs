namespace WebbutvecklingLabb2.Shared.DTOs;

public class OrderDto
{
    public string Id { get; set; }
    public IEnumerable<OrderItemDto> OrderedProducts { get; set; }
    public DateTime OrderDate { get; set; }
    public CustomerDto Customer { get; set; }
}