﻿using AspNetCoreHero.ToastNotification.Abstractions;
using CompressMedia.Data;
using CompressMedia.DTOs;
using CompressMedia.Models;
using CompressMedia.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CompressMedia.Controllers
{
	public class BlobController : Controller
	{
		private readonly IBlobService _blobService;
		private readonly IUserService _userService;
		private readonly INotyfService _notyfService;
		private readonly ApplicationDbContext _context;
		private readonly ICompressService _compressService;

		public BlobController(IBlobService blobService, IUserService userService, INotyfService notyfService, ApplicationDbContext context, ICompressService compressService)
		{
			_blobService = blobService;
			_userService = userService;
			_notyfService = notyfService;
			_context = context;
			_compressService = compressService;
		}

		public IActionResult AccessDenied()
		{
			return View();
		}

		[HttpGet]
		public async Task<IActionResult> Index(int containerId)
		{
			IEnumerable<User> users = await _userService.GetAllUser();
			if (users == null || !users.Any())
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
				BlobId = blob.BlobId,
				BlobName = blob.BlobName,
				ContentType = blob.ContentType,
				Size = Math.Round(blob.Size / 1048576.0, 1),
				CompressionTime = blob.CompressionTime,
				Status = blob.Status!,
				ContainerId = containerId,
			});

			return View(blobDto);
		}

		[HttpGet]
		public async Task<IActionResult> CreateBlob(int containerId)
		{
			IEnumerable<User> users = await _userService.GetAllUser();
			if (users == null || !users.Any())
			{
				return RedirectToAction("AccessDenied");
			}

			BlobDto blobDto = new BlobDto
			{
				ContainerId = containerId,
			};
			return View(blobDto);
		}

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
				return RedirectToAction("Index", new { containerId = blobDto.ContainerId });
			}
			return RedirectToAction("Index", new { containerId = blobDto.ContainerId });
		}

		[HttpGet]
		public async Task<IActionResult> Compress(string blobId, string blobName, string contentType, int containerId)
		{
			IEnumerable<User> users = await _userService.GetAllUser();
			if (users == null || !users.Any())
			{
				return RedirectToAction("AccessDenied");
			}

			BlobDto blobDto = new BlobDto
			{
				BlobId = blobId,
				BlobName = blobName,
				ContentType = contentType,
				ContainerId = containerId
			};
			return View(blobDto);
		}

		[HttpPost]
		public async Task<IActionResult> Compress(BlobDto blobDto)
		{
			try
			{
				string result = await _compressService.CompressMedia(blobDto);
				switch (result)
				{
					case "notfound":
						_notyfService.Error("Video not found.");
						break;
					case "cannotGetInfo":
						_notyfService.Error("Cannot get video's info.");
						break;
					case "compressed":
						_notyfService.Error("This video has been compressed.");
						break;

				}

				_notyfService.Success("Compress successfully.");
				return RedirectToAction("Index", new { containerId = blobDto.ContainerId });
			}
			catch (Exception)
			{
				_notyfService.Error("Cannot Compress");
				return RedirectToAction("Index", new { containerId = blobDto.ContainerId });
			}
		}

		[HttpGet]
		[ActionName("DeleteBlob")]
		public async Task<IActionResult> DeleteBlobGet(string blobId, int containerId)
		{
			IEnumerable<User> users = await _userService.GetAllUser();
			if (users == null || !users.Any())
			{
				return RedirectToAction("AccessDenied");
			}

			Blob? blob = await _context.blobs.SingleOrDefaultAsync(x => x.BlobId == blobId);

			if (blob is null)
			{
				_notyfService.Error("Blob not found.");
				return RedirectToAction("Index", new { containerId = containerId });
			}

			BlobDto blobDto = new BlobDto
			{
				BlobId = blobId,
				ContainerId = containerId
			};
			return View(blobDto);
		}

		[HttpPost]
		[ActionName("DeleteBlob")]
		public async Task<IActionResult> DeleteBlobPost(string blobId, int containerId)
		{
			string result = await _blobService.DeleteBlobAsync(blobId);
			switch (result)
			{
				case "null":
					_notyfService.Error("Blob notfound");
					return RedirectToAction("Index", new { containerId = containerId });
				case "notfound":
					_notyfService.Error("Blob notfound");
					return RedirectToAction("Index", new { containerId = containerId });
				default:
					_notyfService.Success("Delete blob successfully");
					return RedirectToAction("Index", new { containerId = containerId });
			}

		}

		[HttpGet]
		public async Task<IActionResult> ViewBlob(string blobId)
		{
			try
			{
				Blob? blob = await _context.blobs.SingleOrDefaultAsync(x => x.BlobId == blobId);
				if (blob is not null)
				{
					var stream = await _blobService.GetBlobStreamAsync(blobId);
					if (blob!.BlobName!.EndsWith(".jpg"))
					{
						return File(stream, "image/jpg");
					}
					if (blob!.BlobName.EndsWith(".png"))
					{
						return File(stream, "image/png");
					}
					if (blob!.BlobName.EndsWith(".webp"))
					{
						return File(stream, "image/webp");
					}
					return File(stream, "video/mp4");
				}
				_notyfService.Error("Image not found");
				return RedirectToAction("Index", new { containerId = blob!.ContainerId });
			}
			catch (FileNotFoundException)
			{
				return NotFound();
			}
		}

		[HttpGet]
		public async Task<IActionResult> Resize(string blobId, string blobName, string contentType, int containerId)
		{
			IEnumerable<User> users = await _userService.GetAllUser();
			if (users == null || !users.Any())
			{
				return RedirectToAction("AccessDenied");
			}

			BlobDto blobDto = new BlobDto
			{
				BlobId = blobId,
				BlobName = blobName,
				ContentType = contentType,
				ContainerId = containerId
			};
			return View(blobDto);
		}

		[HttpPost]
		public async Task<IActionResult> Resize(BlobDto blobDto)
		{
			try
			{
				string result = await _compressService.ImageResizer(blobDto);
				switch (result)
				{
					case "notfound":
						_notyfService.Error("Image not found.");
						break;
					case "cannotGetInfo":
						_notyfService.Error("Cannot get image's info.");
						break;
					case "compressed":
						_notyfService.Error("This image has been compressed.");
						break;
				}
				_notyfService.Success("Compress successfully.");
				return RedirectToAction("Index", new { containerId = blobDto.ContainerId });
			}
			catch (Exception)
			{
				_notyfService.Error("Cannot Compress");
				return RedirectToAction("Index", new { containerId = blobDto.ContainerId });
			}
		}
	}
}
