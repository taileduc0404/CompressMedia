using CompressMedia.Exceptions.BaseException;

namespace CompressMedia.Exceptions
{
    public class FFprobeException : VideoInfoException
    {
        public FFprobeException(string error) : base($"FFprob error: {error}")
        {
        }
    }
}
