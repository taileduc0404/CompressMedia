using CompressMedia.DTOs;
using CompressMedia.PermissionRequirement;
using CompressMedia.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CompressMedia.Controllers
{
    public class ReportController : Controller
    {
        private readonly IReportService _reportService;
        private readonly IBlobService _blobService;

        public ReportController(IReportService reportService, IBlobService blobService)
        {
            _reportService = reportService;
            _blobService = blobService;
        }

        [HttpGet]
        [CustomPermission("ViewReport")]
        public async Task<IActionResult> Index()
        {
            var reports = await _reportService.GetAllReports();

            var reportDtos = reports
                .GroupBy(r => new { r.MediaId, r.TenantId })
                .Select(g => new ReportDto
                {
                    MediaId = g.First().MediaId,
                    MediaName = g.First().Blob!.BlobName,
                    TenantName = g.First().Tenant!.TenantName,
                    ReportCount = g.Count(),
                    FirstReportDate = g.Min(r => r.ReportDate)
                })
                .ToList();

            return View(reportDtos);
        }

        [HttpGet]
        [ActionName("ReportMedia")]
        [CustomPermission("Report")]
        public IActionResult ReportGet(string blobId)
        {
            ReportDto reportDto = new ReportDto()
            {
                MediaId = blobId,
            };
            return View(reportDto);
        }

        [HttpPost]
        [ActionName("ReportMedia")]
        [CustomPermission("Report")]
        public async Task<IActionResult> ReportPost(string blobId)
        {
            string? userId = HttpContext.User.FindFirstValue("UserId");
            string? tenantIdString = HttpContext.User.FindFirstValue("TenantId");
            Guid? _tenantId = null;

            if (!string.IsNullOrEmpty(tenantIdString) && Guid.TryParse(tenantIdString, out Guid tenantId))
            {
                _tenantId = tenantId;
            }

            string result = await _reportService.ReportMedia(blobId, userId!, _tenantId);

            if (result is null) return View();

            return RedirectToAction("Index", "Report");

        }

        [HttpGet]
        [ActionName("DeleteMedia")]
        [CustomPermission("DeleteMedia")]
        public IActionResult DeleteMediaGet(string mediaId)
        {
            ReportDto reportDto = new ReportDto()
            {
                MediaId = mediaId,
            };
            return View(reportDto);
        }

        [HttpPost]
        [ActionName("DeleteMedia")]
        [CustomPermission("DeleteMedia")]
        public async Task<IActionResult> DeleteMediaPost(string mediaId)
        {
            string result = await _blobService.DeleteBlobAsync(mediaId);
            if (result is null) return View();
            return RedirectToAction("Index", "Report");
        }

    }
}
