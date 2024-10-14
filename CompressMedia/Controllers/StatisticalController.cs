using CompressMedia.DTOs;
using CompressMedia.Models;
using CompressMedia.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CompressMedia.Controllers
{
    public class StatisticalController : Controller
    {
        private readonly IStatisticalService _statisticalService;
        private readonly ILikeService _likeService;

        public StatisticalController(IStatisticalService statisticalService, ILikeService likeService)
        {
            _statisticalService = statisticalService;
            _likeService = likeService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            string? tenantIdString = HttpContext.User.FindFirstValue("TenantId");
            Guid? _tenantId = null;

            if (!string.IsNullOrEmpty(tenantIdString) && Guid.TryParse(tenantIdString, out Guid tenantId))
            {
                _tenantId = tenantId;
            }

            List<Blob> blobLikes = await _statisticalService.Get10MediaWithTheMostLikes(_tenantId);
            List<Blob> blobComments = await _statisticalService.Get10MediaWithTheMostComments(_tenantId);

            var blobIdsForLikes = blobLikes.Select(blob => blob.BlobId!.ToString()).ToList();
            var blobIdsForComments = blobComments.Select(blob => blob.BlobId!.ToString()).ToList();

            var likeCounts = await _likeService.GetLikesCountForBlobsAsync(blobIdsForLikes);

            List<BlobDto> blobLikesDtos = new List<BlobDto>();
            foreach (var blobLike in blobLikes)
            {
                var blobDto = new BlobDto
                {
                    BlobId = blobLike.BlobId,
                    BlobName = Path.GetFileNameWithoutExtension(blobLike.BlobName),
                    ContentType = blobLike.ContentType!.StartsWith("video/") ? "Video" : "Image",
                    Size = Math.Round(blobLike.Size / 1048576.0, 1),
                    CompressionTime = blobLike.CompressionTime,
                    Status = blobLike.Status!,
                    UploadedDate = blobLike.UploadDate,
                    Author = blobLike.User!.FirstName + " " + blobLike.User.LastName,
                    LikeCount = likeCounts.TryGetValue(blobLike.BlobId!.ToString(), out var count) ? count : 0
                };
                blobLikesDtos.Add(blobDto);
            }

            var commentCounts = await _likeService.GetLikesCountForBlobsAsync(blobIdsForComments);

            List<BlobDto> blobCommentsDtos = new List<BlobDto>();
            foreach (var blobComment in blobComments)
            {
                var blobDto = new BlobDto
                {
                    BlobId = blobComment.BlobId,
                    BlobName = Path.GetFileNameWithoutExtension(blobComment.BlobName),
                    ContentType = blobComment.ContentType!.StartsWith("video/") ? "Video" : "Image",
                    Size = Math.Round(blobComment.Size / 1048576.0, 1),
                    CompressionTime = blobComment.CompressionTime,
                    Status = blobComment.Status!,
                    UploadedDate = blobComment.UploadDate,
                    Author = blobComment.User!.FirstName + " " + blobComment.User.LastName,
                    LikeCount = commentCounts.TryGetValue(blobComment.BlobId!.ToString(), out var count) ? count : 0
                };
                blobCommentsDtos.Add(blobDto);
            }

            ViewBag.BlobLikes = blobLikesDtos;
            ViewBag.BlobComments = blobCommentsDtos;

            return View(blobLikesDtos);
        }

    }
}
