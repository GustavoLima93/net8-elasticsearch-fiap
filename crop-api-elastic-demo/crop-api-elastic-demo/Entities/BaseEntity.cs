using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace crop_api_elastic_demo.Entities;

public abstract class BaseEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
}