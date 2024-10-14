using CompressMedia.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CompressMedia.Controllers
{
	public class LikeController : Controller
	{
		private readonly ILikeService _likeService;

		public LikeController(ILikeService likeService)
		{
			_likeService = likeService;
		}

		[HttpPost]
		public async Task<IActionResult> LikeBlob(string blobId)
		{
			string? userId = HttpContext.User.FindFirstValue("UserId");

			bool isLike = await _likeService.IsBlobLikedByUser(blobId, userId!);

			if (isLike)
			{
				await _likeService.DeleteUserLike(blobId, userId!);
			}
			else
			{
				await _likeService.LikeBlob(blobId, userId!);
			}

			int likeCount = await _likeService.GetLikesCount(blobId);
			return Json(new { success = true, likeCount });
		}
	}
}
