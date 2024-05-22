using Interior_Quotation_Ecommerce.MongoDB.Interfaces;
using Interior_Quotation_Ecommerce.MongoDB.Entities;
using MongoDB.Driver;

namespace Interior_Quotation_Ecommerce.MongoDB.Implements
{
    public class ProductImagesRepository : IProductImagesRepository
    {
        private MongoClient mongoClient = null;
        private IMongoDatabase mongoDatabase = null;
        private IMongoCollection<ProductImages> mongoCollection = null;

        public ProductImagesRepository(IConfiguration configuration) 
        {
            mongoClient = new MongoClient(configuration.GetConnectionString("MongoDBConnection"));
            mongoDatabase = mongoClient.GetDatabase("InteriorQuotationDB");
            mongoCollection = mongoDatabase.GetCollection<ProductImages>("ProductImages");
        }

        public void Create(ProductImages productImages)
        {
            try
            {
                mongoCollection.InsertOne(productImages);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void CreateMany(IEnumerable<ProductImages> productImages)
        {
            try
            {
                mongoCollection.InsertMany(productImages);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Delete(object id)
        {
            try
            {
                mongoCollection.DeleteOne(p  => p.Id == id);
            }
            catch(Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        public List<ProductImages> GetAll()
        {
            try
            {
                return mongoCollection.Find<ProductImages>(p => true).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Update(ProductImages productImages)
        {
            try
            {
                mongoCollection.ReplaceOne(ci => ci.Id == productImages.Id, productImages);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
