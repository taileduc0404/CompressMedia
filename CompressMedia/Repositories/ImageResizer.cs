
using CompressMedia.Imaging;
using CompressMedia.Repositories.Interfaces;

namespace CompressMedia.Repositories
{
    /// <summary>
    /// Tại đây người dùng có thể tùy chọn thư viện chỉnh sửa ảnh thông qua service:
    /// IImageSharpService
    /// IImageFFmpegService
    /// </summary>
    public class ImageResizer : IImageResizer
    {
        private readonly IImageSharpService _imageSharpService;
        //private readonly IImageFFmpegService _imageFFmpegService;

        public ImageResizer(/*IImageFFmpegService imageFFmpegService, */IImageSharpService imageSharpService)
        {
            //_imageFFmpegService = imageFFmpegService;
            _imageSharpService = imageSharpService;
        }

        public async Task<ImageResizeResult<Stream>> ResizeAsync(
            Stream inputStream,
            ImageResizeArgs resizeArgs,
            string mimeType = null!,
            CancellationToken cancellationToken = default
            )
        {
            Stream result = await _imageSharpService.ResizeAsync(inputStream, resizeArgs, cancellationToken);
            if (result == null)
            {
                return new ImageResizeResult<Stream>(result!, ImageProcessState.Canceled);
            }
            return new ImageResizeResult<Stream>(result!, ImageProcessState.Done);
        }
    }
}
