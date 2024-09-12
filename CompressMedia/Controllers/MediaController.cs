using AspNetCoreHero.ToastNotification.Abstractions;
using CompressMedia.Data;
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
        private readonly ApplicationDbContext _context;

        public MediaController(IMediaService imageService, INotyfService notyfService, IUserService userService, ApplicationDbContext context)
        {
            _mediaService = imageService;
            _notyfService = notyfService;
            _userService = userService;
            _context = context;
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
            //string splitString = @"D:\BÀI TẬP\ASP.NET\CompressMedia\CompressMedia\wwwroot\Medias\Videos\";
            IEnumerable<MediaDto> mediaDtoList = mediaList.Select(media => new MediaDto
            {
                MediaType = media.MediaType,
                CreatedDate = media.CreatedDate,
                MediaId = media.MediaId,
                Status = media.Status,
                //MediaPath = media.MediaPath!.Replace(splitString, ""),
                MediaPath = media.MediaPath!.Split('&')[1],
                Size = Math.Round((double)(media.Size / 1048576.0), 1)
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
        [ActionName("CompressVideo")]
        public async Task<IActionResult> CompressVideoGet(int mediaId)
        {
            IEnumerable<User> users = await _userService.GetAllUser();
            if (users == null)
            {
                return RedirectToAction("AccessDenied");
            }

            Media media = await _mediaService.GetMediaById(mediaId);

            if (media == null)
            {
                _notyfService.Error("Video not found");
                return RedirectToAction("Index");
            }

            var mediaDto = new MediaDto
            {
                MediaId = media.MediaId,
                MediaPath = media.MediaPath
                // Thêm các thuộc tính khác nếu cần
            };
            return View(mediaDto);
        }

        [HttpPost]
        [RequestFormLimits(MultipartBodyLengthLimit = 536870912)]
        public IActionResult UploadVideo(MediaDto mediaDto)
        {
            string result = _mediaService.UploadMedia(mediaDto);
            if (result == "videoNameExist")
            {
                _notyfService.Warning("Your Video's Name Exist.");
                return RedirectToAction("Index");
            }
            else if (result == null)
            {
                _notyfService.Error("Upload video failed.");
                return RedirectToAction("Index");
            }
            _notyfService.Success("Upload video successfully.");
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ActionName("CompressVideo")]
        [RequestFormLimits(MultipartBodyLengthLimit = 536870912)]
        public async Task<IActionResult> CompressVideoPost(int mediaId)
        {
            Media media = _context.medias.FirstOrDefault(m => m.MediaId == mediaId)!;

            if (media == null)
            {
                _notyfService.Error("Video not found");
                return RedirectToAction("Index");
            }

            media.Status = "Compressing";
            _context.Update(media);
            await _context.SaveChangesAsync();  

            string splitString = @"D:\BÀI TẬP\ASP.NET\CompressMedia\CompressMedia\wwwroot\Medias\Videos\";
            _notyfService.Success("Video is being compressed. You can continue working while we process the video.");
            bool result = _mediaService.CompressMedia(media.MediaPath!.Replace(splitString, ""));
            if (!result)
            {
                _notyfService.Success("Upload video failed.");
                return RedirectToAction("Index");
            }

            _notyfService.Success("Upload video successfully.");
            return RedirectToAction("Index");
        }
    }
}
