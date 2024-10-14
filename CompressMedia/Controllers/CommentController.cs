using CompressMedia.DTOs;
using CompressMedia.Models;
using CompressMedia.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CompressMedia.Controllers
{
	public class CommentController : Controller
	{
		private readonly ICommentService _commentService;
		public CommentController(ICommentService commentService)
		{
			_commentService = commentService;
		}

		[HttpGet]
		public async Task<IActionResult> Index(string blobId)
		{
			string userId = HttpContext.User.FindFirstValue("UserId")!;
			List<Comment> comments = await _commentService.GetAllComment(userId, blobId);

			IEnumerable<CommentDto> commentDto = comments.Select(comment => new CommentDto
			{
				CommentId = comment.CommentId,
				Content = comment.Content,
				CreatedDate = comment.CreatedDate,
				UserName = comment.User!.Username
			});

			return View(commentDto);
		}

		[HttpGet]
		public IActionResult CreateComment(string blobId)
		{
			CommentDto commentDto = new CommentDto
			{
				BlobId = blobId
			};
			return View(commentDto);
		}

		[HttpPost]
		public async Task<IActionResult> CreateComment(CommentDto commentDto)
		{
			string? userId = HttpContext.User.FindFirstValue("UserId");
			string result = await _commentService.CreateComment(userId!, commentDto);
			if (result == null)
				return View();

			return RedirectToAction("Index", new { blobId = commentDto.BlobId });
		}

		[HttpGet]
		public IActionResult ReplyComment(int commentId)
		{
			CommentDto commentDto = new CommentDto
			{
				CommentId = commentId
			};
			return View(commentDto);
		}

		[HttpPost]
		public async Task<IActionResult> ReplyComment(int commentId, CommentDto commentDto)
		{
			string? userId = HttpContext.User.FindFirstValue("UserId");
			string result = await _commentService.ReplyComment(commentId, userId!, commentDto);
			if (result == null)
				return View();

			return RedirectToAction("Index", new { blobId = commentDto.BlobId });
		}

	}
}
