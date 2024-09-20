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
                return RedirectToAction("Index", new { containerId = blobDto.ContainerId });
            }
            return RedirectToAction("Index", new { containerId = blobDto.ContainerId });
        }

        [HttpGet]
        public async Task<IActionResult> Download(string blobName)
        {
            IEnumerable<User> users = await _userService.GetAllUser();
            if (users == null)
            {
                return RedirectToAction("AccessDenied");
            }

            BlobDto blobDto = new BlobDto
            {
                BlobName = blobName,
            };
            return View(blobDto);
        }

        [HttpPost]
        public async Task<IActionResult> Download(BlobDto blobDto)
        {
            bool result = await _blobService.GetBlobContentAsync(blobDto);
            if (result == false)
            {
                _notyfService.Error("Download failed.");
                return RedirectToAction("Index", new { containerId = blobDto.ContainerId });
            }

            _notyfService.Success("Download successfully");
            return RedirectToAction("Index", new { containerId = blobDto.ContainerId });
        }

        [HttpGet]
        public async Task<IActionResult> Compress(string blobId, string blobName, string contentType, int containerId)
        {
            IEnumerable<User> users = await _userService.GetAllUser();
            if (users == null)
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
            if (blobDto != null)
            {
                bool result = await _blobService.CompressMedia(blobDto);
                if (!result)
                {
                    _notyfService.Error("Compress failed.");
                    return RedirectToAction("Index", new { containerId = blobDto.ContainerId });
                }
            }
            else
            {
                _notyfService.Warning("Model is null.");
                return RedirectToAction("Index", new { containerId = blobDto!.ContainerId });
            }

            _notyfService.Success("Compress successfully");
            return RedirectToAction("Index", new { containerId = blobDto.ContainerId });

        }

        [HttpGet]
        [ActionName("DeleteBlob")]
        public async Task<IActionResult> DeleteBlobGet(string blobId, int containerId)
        {
            IEnumerable<User> users = await _userService.GetAllUser();
            if (users == null)
            {
                return RedirectToAction("AccessDenied");
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
            if (blobId != null)
            {
                bool result = await _blobService.DeleteBlobAsync(blobId);
                if (!result)
                {
                    _notyfService.Error("Compress failed.");
                    return RedirectToAction("Index", new { containerId = containerId });
                }
            }
            else
            {
                _notyfService.Warning("Model is null.");
                return RedirectToAction("Index", new { containerId = containerId });
            }

            _notyfService.Success("Compress successfully");
            return RedirectToAction("Index", new { containerId = containerId });

        }

        [HttpGet]
        public async Task<IActionResult> GetVideo(string blobId)
        {
            try
            {
                var stream = await _blobService.GetVideoStreamAsync(blobId);
                return File(stream, "video/mp4"); 
            }
            catch (FileNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
