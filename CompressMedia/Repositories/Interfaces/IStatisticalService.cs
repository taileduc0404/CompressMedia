using CompressMedia.Models;

namespace CompressMedia.Repositories.Interfaces
{
	public interface IStatisticalService
	{
		Task<List<Blob>> Get10MediaWithTheMostLikes(Guid? tenantId);
		Task<List<Blob>> Get10MediaWithTheMostComments(Guid? tenantId);

	}
}
