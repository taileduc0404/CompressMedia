using CompressMedia.Exceptions.BaseException;

namespace CompressMedia.Exceptions
{
	public class UnexpectedOutputFormatException : VideoInfoException
	{
		public UnexpectedOutputFormatException() : base("Unexpected FFprobe output format.") { }
	}
}
