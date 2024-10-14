using CompressMedia.Imaging;

namespace CompressMedia.Repositories.Interfaces
{
    public interface IImageSharpService
    {
        Task<Stream> ResizeAsync(
            Stream inputStream,
            ImageResizeArgs resizeArgs,
            CancellationToken cancellationToken = default
            );
    }
}
