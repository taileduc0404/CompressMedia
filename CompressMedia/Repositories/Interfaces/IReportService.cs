using CompressMedia.Models;

namespace CompressMedia.Repositories.Interfaces
{
	public interface IReportService
	{
		Task<List<Report>> GetAllReports();
		Task<string> ReportMedia(string blobId, string userId, Guid? tenantId);
	}
}
