
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Interior_Quotation_Ecommerce.MongoDB.Entities
{
    public class ConstructImages
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
        public long ConstructId { get; set; }
        public byte[]? Image { get; set; }
    }
}