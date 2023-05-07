using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebbutvecklingLabb2.DataAccess.Models;

public class CustomerModel
{
    [BsonId]
    public ObjectId Id { get; set; }
    [BsonElement]
    public string FirstName { get; set; }
    [BsonElement]
    public string LastName { get; set; }
    [BsonElement]
    public string Country { get; set; }
    [BsonElement]
    public string City { get; set; }
    [BsonElement]
    public string PostalCode { get; set; }
    [BsonElement]
    public string Address { get; set; }
    [BsonElement]
    public string PhoneNumber { get; set; }
    [BsonElement("E-Mail")]
    public string EMail { get; set; }

}