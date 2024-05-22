

using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Interior_Quotation_Ecommerce.MongoDB.Entities
{
    public class ProductImages
    {

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
        public long ProductId { get; set; }
        public byte[]? Image { get; set; }

    }
}