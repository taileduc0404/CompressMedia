namespace CompressMedia.Repositories.Interfaces
{
	public interface ILikeService
	{
		Task LikeBlob(string blobId, string userId);
		Task<bool> IsBlobLikedByUser(string blobId, string userId);
		Task<int> GetLikesCount(string blobId);
		Task<string> DeleteUserLike(string blobId, string userId);
	}
}
