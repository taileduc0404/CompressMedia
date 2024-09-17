using AspNetCoreHero.ToastNotification.Abstractions;
using CompressMedia.DTOs;
using CompressMedia.Models;
using CompressMedia.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CompressMedia.Controllers
{
	public class BlobController : Controller
	{
		private readonly IBlobService _blobService;
		private readonly IUserService _userService;
		private readonly INotyfService _notyfService;

		public BlobController(IBlobService blobService, IUserService userService, INotyfService notyfService)
		{
			_blobService = blobService;
			_userService = userService;
			_notyfService = notyfService;
		}

		/// <summary>
		/// Lỗi truy cập
		/// </summary>
		/// <returns></returns>
		public IActionResult AccessDenied()
		{
			return View();
		}

		[HttpGet]
		public async Task<IActionResult> Index(int containerId)
		{
			IEnumerable<User> users = await _userService.GetAllUser();
			if (users == null)
			{
				return RedirectToAction("AccessDenied");
			}

			IEnumerable<Blob> blobList = await _blobService.GetListBlobAsync(containerId);

			if (blobList == null)
			{
				return RedirectToAction("AccessDenied");
			}

			IEnumerable<BlobDto> blobDto = blobList.Select(blob => new BlobDto
			{
				BlobName = blob.BlobName,
			});

			return View(blobDto);
		}

		/// <summary>
		/// Chuyển hướng sang trang upload video
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public async Task<IActionResult> CreateBlob(int containerId)
		{
			IEnumerable<User> users = await _userService.GetAllUser();
			if (users == null)
			{
				return RedirectToAction("AccessDenied");
			}

			BlobDto blobDto = new BlobDto
			{
				ContainerId = containerId,
			};
			return View(blobDto);
		}

		/// <summary>
		/// Thực hiện việc upload video
		/// </summary>
		/// <param name="mediaDto"></param>
		/// <returns></returns>
		[HttpPost]
		[RequestFormLimits(MultipartBodyLengthLimit = 536870912)]
		public async Task<IActionResult> CreateBlob(BlobDto blobDto)
		{
			bool result = await _blobService.CreateBlobAsync(blobDto);
			if (result == false)
			{
				_notyfService.Warning("Upload Video Failed.");
				return RedirectToAction("Index");
			}
			else if (result == true)
			{
				_notyfService.Success("Upload video successfully.");
				return RedirectToAction("Index");
			}
			return RedirectToAction("Index", "Blob", new { blobDto.ContainerId});
		}

	}
}
