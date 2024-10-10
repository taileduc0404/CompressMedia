using CompressMedia.Imaging;

namespace CompressMedia.Repositories.Interfaces
{
	public interface IImageFFmpegService
	{
		Task<Stream> ResizeAsync(
			Stream inputStream,
			ImageResizeArgs resizeArgs,
			CancellationToken cancellationToken = default
			);
	}
}
