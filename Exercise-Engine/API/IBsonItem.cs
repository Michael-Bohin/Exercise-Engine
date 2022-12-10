using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Exercise_Engine.API
{
    public interface IBsonItem
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
    }
}
