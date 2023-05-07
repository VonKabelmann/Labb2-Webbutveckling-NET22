using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebbutvecklingLabb2.DataAccess.Models;

public class OrderModel
{
    [BsonId]
    public ObjectId Id { get; set; }
    [BsonElement]
    public IEnumerable<OrderItemModel> OrderedProducts { get; set; }
    [BsonElement]
    public DateTime OrderDate { get; set; }
    [BsonElement]
    public CustomerModel Customer { get; set; }
}