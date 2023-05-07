using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebbutvecklingLabb2.DataAccess.Models;

public class ProductModel
{
    [BsonId]
    public ObjectId ProductNumber { get; set; }
    [BsonElement]
    public string ProductName { get; set; }
    [BsonElement]
    public string ProductDescription { get; set; }
    [BsonElement]
    public string ImageSource { get; set; }
    [BsonElement]
    public decimal Price { get; set; }
    [BsonElement]
    public string Category { get; set; }
    [BsonElement]
    public bool IsAvailable { get; set; }
}