
using Interior_Quotation_Ecommerce.MongoDB.Interfaces;
using Interior_Quotation_Ecommerce.MongoDB.Entities;
using MongoDB.Driver;

namespace Interior_Quotation_Ecommerce.MongoDB.Implements
{
    public class ConstructImagesRepository : IConstructImagesRepository
    {
        private MongoClient mongoClient = null;
        private IMongoDatabase mongoDatabase = null;
        private IMongoCollection<ConstructImages> mongoCollection = null;

        public ConstructImagesRepository(IConfiguration configuration) 
        {
            this.mongoClient = new MongoClient(configuration.GetConnectionString("MongoDBConnection"));
            this.mongoDatabase = mongoClient.GetDatabase("InteriorQuotationDB");
            this.mongoCollection = mongoDatabase.GetCollection<ConstructImages>("ConstructImages");
        }

        public void Create(ConstructImages constructImages)
        {
            mongoCollection.InsertOne(constructImages);
        }

        public void CreateMany(IEnumerable<ConstructImages> constructImagesDTO)
        {
            mongoCollection.InsertMany(constructImagesDTO);
        }

        public void Delete(object id)
        {
            try
            {
                var filter = Builders<ConstructImages>.Filter.Eq("_id", id);
                mongoCollection.DeleteOne(filter);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<ConstructImages> GetAll()
        {
            return mongoCollection.Find(i => true).ToList();
        }

        public ConstructImages GetById(string id)
        {
            return mongoCollection.Find<ConstructImages>(x => x.Id == id).FirstOrDefault();
        }

        public void Update(ConstructImages updateConstructImages)
        {
            try
            {
                mongoCollection.ReplaceOne(ci => ci.Id == updateConstructImages.Id, updateConstructImages);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
