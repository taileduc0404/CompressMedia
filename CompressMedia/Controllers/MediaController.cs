using AspNetCoreHero.ToastNotification.Abstractions;
using CompressMedia.DTOs;
using CompressMedia.Models;
using CompressMedia.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CustomAuth.Controllers
{
    public class MediaController : Controller
    {
        private readonly IMediaService _mediaService;
        private readonly INotyfService _notyfService;

        public MediaController(IMediaService imageService, INotyfService notyfService)
        {
            _mediaService = imageService;
            _notyfService = notyfService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<Media> mediaList = await _mediaService.GetAllVideo();

            if (mediaList == null)
            {
                return RedirectToAction("AccessDenied");
            }

            // Chuyển đổi danh sách Media sang MediaDto để truyền vào View
            IEnumerable<MediaDto> mediaDtoList = mediaList.Select(media => new MediaDto
            {
                //MediaType = media.MediaType,
                CreatedDate = media.CreatedDate,
                //UserId = media.UserId,
                MediaPath = media.MediaPath,
                Size = (double)(media.Size / 1048576.0)
            });

            return View(mediaDtoList);
        }


        [HttpGet]
        public IActionResult Video()
        {
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

    }
}
