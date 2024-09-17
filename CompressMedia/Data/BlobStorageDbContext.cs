using CompressMedia.Models;
using MongoDB.Driver;

namespace CompressMedia.Data
{
    public class BlobStorageDbContext
    {
		private readonly IMongoDatabase _mongoDatabase;
		public BlobStorageDbContext(string connectionString, string databaseName)
		{
			var client = new MongoClient(connectionString);
			_mongoDatabase = client.GetDatabase(databaseName);
		}

		public IMongoCollection<BlobData> BlobData => _mongoDatabase.GetCollection<BlobData>("BlobData");
	}
}
