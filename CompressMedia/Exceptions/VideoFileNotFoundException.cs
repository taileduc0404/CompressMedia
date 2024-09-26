using CompressMedia.Exceptions.BaseException;

namespace CompressMedia.Exceptions
{
	public class VideoFileNotFoundException : VideoInfoException
	{
		public VideoFileNotFoundException(string videoPath) : base($"The video file path {videoPath} not found.")
		{
		}
	}
}
