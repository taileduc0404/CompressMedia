using AspNetCoreHero.ToastNotification.Abstractions;
using CompressMedia.DTOs;
using CompressMedia.Models;
using CompressMedia.Repositories;
using CompressMedia.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CustomAuth.Controllers
{
	public class MediaController : Controller
	{
		private readonly IMediaService _mediaService;
		private readonly IUserService _userService;
		private readonly INotyfService _notyfService;

		public MediaController(IMediaService imageService, INotyfService notyfService, IUserService userService)
		{
			_mediaService = imageService;
			_notyfService = notyfService;
			_userService = userService;
		}

		[HttpGet]
		public async Task<IActionResult> Index()
		{
			IEnumerable<User> users = await _userService.GetAllUser();
			if (users == null)
			{
				return RedirectToAction("AccessDenied");
			}
			IEnumerable<Media> mediaList = await _mediaService.GetAllVideo();

			if (mediaList == null)
			{
				return RedirectToAction("AccessDenied");
			}

			// Chuyển đổi danh sách Media sang MediaDto để truyền vào View
			string splitString = @"D:\BÀI TẬP\ASP.NET\CompressMedia\CompressMedia\wwwroot\Medias\Videos\";
			IEnumerable<MediaDto> mediaDtoList = mediaList.Select(media => new MediaDto
			{
				MediaType = media.MediaType,
				CreatedDate = media.CreatedDate,
				//UserId = media.UserId,
				Status = media.Status,
				MediaPath = media.MediaPath!.Replace(splitString, ""),
				Size = Math.Round((double)(media.Size / 1048576.0), 2)
			});

			return View(mediaDtoList);
		}

		public IActionResult AccessDenied()
		{
			return View();
		}

		[HttpGet]
		public async Task<IActionResult> UploadVideo()
		{
			IEnumerable<User> users = await _userService.GetAllUser();
			if (users == null)
			{
				return RedirectToAction("AccessDenied");
			}
			return View();
		}

		[HttpGet]
		public async Task<IActionResult> UploadAndCompressVideo()
		{
			IEnumerable<User> users = await _userService.GetAllUser();
			if (users == null)
			{
				return RedirectToAction("AccessDenied");
			}
			return View();
		}

		[HttpPost]
		[RequestFormLimits(MultipartBodyLengthLimit = 536870912)]
		public IActionResult UploadVideo(MediaDto mediaDto)
		{
			bool result = _mediaService.UploadMedia(mediaDto);
			if (!result)
			{
				_notyfService.Success("Upload image failed.");
				return RedirectToAction("Index");
			}

			_notyfService.Success("Upload image successfully.");
			return RedirectToAction("Index");
		}

		[HttpPost]
		[RequestFormLimits(MultipartBodyLengthLimit = 536870912)]
		public IActionResult UploadAndCompressVideo(MediaDto mediaDto)
		{
			bool result = _mediaService.UploadAndCompressMedia(mediaDto);
			if (!result)
			{
				_notyfService.Success("Upload image failed.");
				return RedirectToAction("Index");
			}

			_notyfService.Success("Upload image successfully.");
			return RedirectToAction("Index");
		}

	}
}
