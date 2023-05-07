using MongoDB.Bson.Serialization.Attributes;

namespace WebbutvecklingLabb2.DataAccess.Models;

public class OrderItemModel
{
    [BsonElement]
    public ProductModel Product { get; set; }
    [BsonElement]
    public int Amount { get; set; }
}