namespace CompressMedia.BlobStorage
{
	public class BlobDatabaseSettings
	{
		public string ConnectionString { get; set; } = null!;
		public string DatabaseName { get; set; } = null!;
		public string BlobCollectionName { get; set; } = null!;
	}
}
