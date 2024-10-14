namespace CompressMedia.DTOs
{
    public class LikeDto
    {
        //public int Id { get; set; }
        public string? BlobId { get; set; }
        public string? UserId { get; set; }
        public DateTime LikedAt { get; set; }
        public int LikeCount { get; set; }
    }
}
