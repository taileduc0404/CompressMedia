namespace CompressMedia.Repositories.Interfaces
{
	public interface ILikeService
	{
		Task LikeBlob(string blobId, string userId);
		Task<bool> IsBlobLikedByUser(string blobId, string userId);
		Task<Dictionary<string, int>> GetLikesCountForBlobsAsync(IEnumerable<string> blobIds);
		Task<int> GetLikesCount(string blobId);
		Task<string> DeleteUserLike(string blobId, string userId);
	}
}
