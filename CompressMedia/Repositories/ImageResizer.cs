
using CompressMedia.Imaging;
using CompressMedia.Repositories.Interfaces;

namespace CompressMedia.Repositories
{
	public class ImageResizer : IImageResizer
	{
		//private readonly IImageSharpService _imageSharpService;
		private readonly IImageFFmpegService _imageFFmpegService;

		public ImageResizer(IImageFFmpegService imageFFmpegService)
		{
			//_imageSharpService = imageSharpService;
			_imageFFmpegService = imageFFmpegService;
		}

		public async Task<ImageResizeResult<Stream>> ResizeAsync(
			Stream inputStream,
			ImageResizeArgs resizeArgs,
			string mimeType = null!,
			CancellationToken cancellationToken = default
			)
		{
			Stream result = await _imageFFmpegService.ResizeAsync(inputStream, resizeArgs, cancellationToken);
			if (result == null)
			{
				return new ImageResizeResult<Stream>(result!, ImageProcessState.Canceled);
			}
			return new ImageResizeResult<Stream>(result!, ImageProcessState.Done);
		}
	}
}
