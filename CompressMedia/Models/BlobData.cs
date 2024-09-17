using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace CompressMedia.Models
{
    public class BlobData
    {
        [BsonId]
        public ObjectId Id { get; set; }

        //[BsonElement("blobId")]
        //public int BlobId { get; set; }

        [BsonElement("data")]
        public byte[]? Data { get; set; }
    }
}
