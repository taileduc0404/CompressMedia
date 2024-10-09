using CompressMedia.Imaging;

namespace CompressMedia.Repositories.Interfaces
{
    public interface IImageResizer
    {
        Task<ImageResizeResult<Stream>> ResizeAsync(
            Stream inputStream,
            ImageResizeArgs resizeArgs,
            string mimeType = null!,
            CancellationToken cancellationToken = default
        );
    }
}
