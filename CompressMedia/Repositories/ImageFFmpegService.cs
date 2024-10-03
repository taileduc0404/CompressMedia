using CompressMedia.Imaging;
using CompressMedia.Repositories.Interfaces;

namespace CompressMedia.Repositories
{
	public class ImageFFmpegService : IImageFFmpegService
	{
		private readonly IMediaService _mediaService;

		public ImageFFmpegService(IMediaService mediaService)
		{
			_mediaService = mediaService;
		}

		public async Task<Stream> ResizeAsync(Stream inputStream, ImageResizeArgs resizeArgs, CancellationToken cancellationToken = default)
		{
			string tempPath = Path.GetTempFileName();
			using (Stream image = new FileStream(tempPath, FileMode.Open, FileAccess.ReadWrite))
			{
				await inputStream.CopyToAsync(image);


				string tempReturnPath = Path.ChangeExtension(Path.GetTempFileName(), ".jpg");
				string arg = $"-i {tempPath} -vf scale={resizeArgs.Width}:{resizeArgs.Height} {tempReturnPath}";
				if (arg is null)
				{
					return null!;
				}
				_mediaService.ExecuteCommand(arg);

				MemoryStream result = new MemoryStream();
				using (FileStream fileStream = new FileStream(tempReturnPath, FileMode.Open, FileAccess.Read))
				{
					await fileStream.CopyToAsync(result);
				}
				result.Position = 0;

				_mediaService.DeleteTemporaryFiles();
				return result;
			}
		}
	}
}
