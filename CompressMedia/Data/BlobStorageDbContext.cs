using MongoDB.Driver;

namespace CompressMedia.Data
{
    public class BlobStorageDbContext
    {
        public IMongoDatabase _mongoDatabase;
        public BlobStorageDbContext(string connectionString, string databaseName)
        {
            var client = new MongoClient(connectionString);
            _mongoDatabase = client.GetDatabase(databaseName);
        }
    }
}
