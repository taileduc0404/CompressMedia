using CompressMedia.Data;
using CompressMedia.Models;
using CompressMedia.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CompressMedia.Repositories
{
	public class ReportService : IReportService
	{
		private readonly ApplicationDbContext _context;

		public ReportService(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<List<Report>> GetAllReports()
		{
			List<Report>? reports = await _context.Report
				.Include(x => x.Tenant)
				.Include(x => x!.Blob)
				.ToListAsync();
			return reports.Any() ? reports : new List<Report>();
		}

		public async Task<string> ReportMedia(string blobId, string userId, Guid? tenantId)
		{
			if (string.IsNullOrWhiteSpace(blobId))
			{
				return null!;
			}

			await _context.Report.AddAsync(new Report
			{
				MediaId = blobId,
				ReportDate = DateTime.Now,
				UserId = userId,
				TenantId = tenantId,
			});
			await _context.SaveChangesAsync();
			return "ok";
		}
	}
}
