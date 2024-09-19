namespace CompressMedia.Models
{
    public class CompressOption
    {
        /// <summary>
        /// Nén full options
        /// </summary>
        /// <returns></returns>
        public static readonly Dictionary<string, string> CompressFullOption = new()
        {
				#region Full Option
				{"1920x1080_60fps_trueFps_trueResolution_trueBitrate","-i {videoPath} -c:v libvpx-vp9 -vf scale=1280:720 -aspect 16:9 -preset ultrafast -b:v 1M -minrate 500K -maxrate 964K -bufsize 2M -crf 30 -af volume=0.5 -b:a 64K {outputPath}" },
                {"1920x1080_30fps_trueFps_trueResolution_trueBitrate","-i {videoPath} -c:v libvpx-vp9 -vf scale=1280:720 -aspect 16:9 -preset ultrafast -b:v 1M -minrate 500K -maxrate 964K -bufsize 2M -crf 30 -af volume=0.5 -b:a 64K {outputPath}" },
                {"1280x720_60fps_trueFps_trueResolution_trueBitrate","-i {videoPath} -c:v libvpx-vp9 -vf scale=854:480 -aspect 16:9 -preset ultrafast -b:v 1M -minrate 500K -maxrate 964K -bufsize 2M -crf 30 -af volume=0.5 -b:a 64K {outputPath}" },
                {"1280x720_30fps_trueFps_trueResolution_trueBitrate","-i {videoPath} -c:v libvpx-vp9 -vf scale=854:480 -aspect 16:9 -preset ultrafast -b:v 1M -minrate 500K -maxrate 964K -bufsize 2M -crf 30 -af volume=0.5 -b:a 64K {outputPath}" },
                {"854x480_60fps_trueFps_trueResolution_trueBitrate","-i {videoPath} -c:v libvpx-vp9 -vf scale=640:360 -aspect 4:3 -preset ultrafast -b:v 1M -minrate 500K -maxrate 964K -bufsize 2M -crf 30 -af volume=0.5 -b:a 64K {outputPath}" },
                {"854x480_30fps_trueFps_trueResolution_trueBitrate","-i {videoPath} -c:v libvpx-vp9 -vf scale=640:360 -aspect 4:3 -preset ultrafast -b:v 1M -minrate 500K -maxrate 964K -bufsize 2M -crf 30 -af volume=0.5 -b:a 64K {outputPath}" },
                {"640x360_60fps_trueFps_trueResolution_trueBitrate","-i {videoPath} -c:v libvpx-vp9 -vf scale=426:240 -aspect 16:9 -preset ultrafast -b:v 1M -minrate 500K -maxrate 964K -bufsize 2M -crf 30 -af volume=0.5 -b:a 64K {outputPath}" },
                {"640x360_30fps_trueFps_trueResolution_trueBitrate","-i {videoPath} -c:v libvpx-vp9 -vf scale=426:240 -aspect 16:9 -preset ultrafast -b:v 1M -minrate 500K -maxrate 964K -bufsize 2M -crf 30 -af volume=0.5 -b:a 64K {outputPath}" },
				#endregion
		};

        /// <summary>
        /// Chỉ nén fps
        /// </summary>
        public static readonly Dictionary<string, string> FpsOnlyOption = new()
        { 
				#region Chỉ nén fps
				{"1920x1080_60fps_trueFps_falseResolution_falseBitrate","-i {videoPath} -c:v libvpx-vp9 -r 30 -preset ultrafast -af volume=0.5 -b:a 64K {outputPath}" },
                {"1920x1080_30fps_trueFps_falseResolution_falseBitrate","-i {videoPath} -c:v libvpx-vp9 -r 24 -preset ultrafast -af volume=0.5 -b:a 64K {outputPath}" },
                {"1280x720_60fps_trueFps_falseResolution_falseBitrate","-i {videoPath} -c:v libvpx-vp9 -r 30 -preset ultrafast -af volume=0.5 -b:a 64K {outputPath}" },
                {"1280x720_30fps_trueFps_falseResolution_falseBitrate","-i {videoPath} -c:v libvpx-vp9 -r 24 -preset ultrafast -af volume=0.5 -b:a 64K {outputPath}" },
                {"854x480_60fps_trueFps_falseResolution_falseBitrate","-i {videoPath} -c:v libvpx-vp9 -r 30 -preset ultrafast -af volume=0.5 -b:a 64K {outputPath}" },
                {"854x480_30fps_trueFps_falseResolution_falseBitrate","-i {videoPath} -c:v libvpx-vp9 -r 24 -preset ultrafast -af volume=0.5 -b:a 64K {outputPath}" },
                {"640x360_60fps_trueFps_falseResolution_falseBitrate","-i {videoPath} -c:v libvpx-vp9 -r 30 -preset ultrafast -af volume=0.5 -b:a 64K {outputPath}" },
                {"640x360_30fps_trueFps_falseResolution_falseBitrate","-i {videoPath} -c:v libvpx-vp9 -r 24 -preset ultrafast -af volume=0.5 -b:a 64K {outputPath}" },
				#endregion
		};

        /// <summary>
        /// CHỉ nén độ phân giải
        /// </summary>
        public static readonly Dictionary<string, string> ResolutionOnlyOption = new() { 
				#region Chỉ nén resolution
				{"1920x1080_60fps_falseFps_trueResolution_falseBitrate","-i {videoPath} -c:v libvpx-vp9 -vf scale=1280:720 -aspect 16:9 -preset ultrafast -af volume=0.5 -b:a 64K {outputPath}" },
                {"1920x1080_30fps_falseFps_trueResolution_falseBitrate","-i {videoPath} -c:v libvpx-vp9 -vf scale=1280:720 -aspect 16:9 -preset ultrafast -af volume=0.5 -b:a 64K {outputPath}" },
                {"1280x720_60fps_falseFps_trueResolution_falseBitrate","-i {videoPath} -c:v libvpx-vp9 -vf scale=854:480 -aspect 16:9 -preset ultrafast -af volume=0.5 -b:a 64K {outputPath}" },
                {"1280x720_30fps_falseFps_trueResolution_falseBitrate","-i {videoPath} -c:v libvpx-vp9 -vf scale=854:480 -aspect 16:9 -preset ultrafast -af volume=0.5 -b:a 64K {outputPath}" },
                {"854x480_60fps_falseFps_trueResolution_falseBitrate","-i {videoPath} -c:v libvpx-vp9 -vf scale=640:360 -aspect 4:3 -preset ultrafast -af volume=0.5 -b:a 64K {outputPath}" },
                {"854x480_30fps_falseFps_trueResolution_falseBitrate","-i {videoPath} -c:v libvpx-vp9 -vf scale=640:360 -aspect 4:3 -preset ultrafast -af volume=0.5 -b:a 64K {outputPath}" },
                {"640x360_60fps_falseFps_trueResolution_falseBitrate","-i {videoPath} -c:v libvpx-vp9 -vf scale=426:240 -aspect 4:3 -preset ultrafast -af volume=0.5 -b:a 64K {outputPath}" },
                {"640x360_30fps_falseFps_trueResolution_falseBitrate","-i {videoPath} -c:v libvpx-vp9 -vf scale=426:240 -aspect 4:3 -preset ultrafast -af volume=0.5 -b:a 64K {outputPath}" },
				#endregion
		};

        /// <summary>
        /// Chỉ nensn bitrate video
        /// </summary>
        public static readonly Dictionary<string, string> BitrateOnlyOption = new()
        {
				#region Chỉ nén bitrate video
				{"1920x1080_60fps_falseFps_falseResolution_trueBitrate","-i {videoPath} -c:v libvpx-vp9 -preset ultrafast -b:v 1M -minrate 500K -maxrate 964K -bufsize 2M -crf 30 -af volume=0.5 -b:a 64K {outputPath}" },
                {"1920x1080_30fps_falseFps_falseResolution_trueBitrate","-i {videoPath} -c:v libvpx-vp9 -preset ultrafast -b:v 1M -minrate 500K -maxrate 964K -bufsize 2M -crf 30 -af volume=0.5 -b:a 64K {outputPath}" },
                {"1280x720_60fps_falseFps_falseResolution_trueBitrate","-i {videoPath} -c:v libvpx-vp9 -preset ultrafast -b:v 1M -minrate 500K -maxrate 964K -bufsize 2M -crf 30 -af volume=0.5 -b:a 64K {outputPath}" },
                {"1280x720_30fps_falseFps_falseResolution_trueBitrate","-i {videoPath} -c:v libvpx-vp9 -preset ultrafast -b:v 1M -minrate 500K -maxrate 964K -bufsize 2M -crf 30 -af volume=0.5 -b:a 64K {outputPath}" },
                {"854x480_60fps_falseFps_falseResolution_trueBitrate","-i {videoPath} -c:v libvpx-vp9 -preset ultrafast -b:v 1M -minrate 500K -maxrate 964K -bufsize 2M -crf 30 -af volume=0.5 -b:a 64K {outputPath}" },
                {"854x480_30fps_falseFps_falseResolution_trueBitrate","-i {videoPath} -c:v libvpx-vp9 -preset ultrafast -b:v 1M -minrate 500K -maxrate 964K -bufsize 2M -crf 30 -af volume=0.5 -b:a 64K {outputPath}" },
                {"640x360_60fps_falseFps_falseResolution_trueBitrate","-i {videoPath} -c:v libvpx-vp9 -preset ultrafast -b:v 1M -minrate 500K -maxrate 964K -bufsize 2M -crf 30 -af volume=0.5 -b:a 64K {outputPath}" },
                {"640x360_30fps_falseFps_falseResolution_trueBitrate","-i {videoPath} -c:v libvpx-vp9 -preset ultrafast -b:v 1M -minrate 500K -maxrate 964K -bufsize 2M -crf 30 -af volume=0.5 -b:a 64K {outputPath}" },
				#endregion
		};

        /// <summary>
        /// Nén kết hợp fps và độ phân giải
        /// </summary>
        public static readonly Dictionary<string, string> FpsVsResolutionOption = new()
        {
			#region Chỉ nén fps
				{"1920x1080_60fps_trueFps_trueResolution_falseBitrate","-i {videoPath} -c:v libvpx-vp9 -r 30 -vf scale=1280:720 -aspect 16:9 -preset ultrafast -af volume=0.5 -b:a 64K {outputPath}" },
                {"1920x1080_30fps_trueFps_trueResolution_falseBitrate","-i {videoPath} -c:v libvpx-vp9 -r 24 -vf scale=1280:720 -aspect 16:9 -preset ultrafast -af volume=0.5 -b:a 64K {outputPath}" },
                {"1280x720_60fps_trueFps_trueResolution_falseBitrate","-i {videoPath} -c:v libvpx-vp9 -r 30 -vf scale=854:480 -aspect 16:9 -preset ultrafast -af volume=0.5 -b:a 64K {outputPath}" },
                {"1280x720_30fps_trueFps_trueResolution_falseBitrate","-i {videoPath} -c:v libvpx-vp9 -r 24 -vf scale=854:480 -aspect 16:9 -preset ultrafast -af volume=0.5 -b:a 64K {outputPath}" },
                {"854x480_60fps_trueFps_trueResolution_falseBitrate","-i {videoPath} -c:v libvpx-vp9 -r 30 -vf scale=640:360 -aspect 4:3 -preset ultrafast -af volume=0.5 -b:a 64K {outputPath}" },
                {"854x480_30fps_trueFps_trueResolution_falseBitrate","-i {videoPath} -c:v libvpx-vp9 -r 24 -vf scale=640:360 -aspect 4:3 -preset ultrafast -af volume=0.5 -b:a 64K {outputPath}" },
                {"640x360_60fps_trueFps_trueResolution_falseBitrate","-i {videoPath} -c:v libvpx-vp9 -r 30 -vf scale=426:240 -aspect 4:3 -preset ultrafast -af volume=0.5 -b:a 64K {outputPath}" },
                {"640x360_30fps_trueFps_trueResolution_falseBitrate","-i {videoPath} -c:v libvpx-vp9 -r 24 -vf scale=426:240 -aspect 4:3 -preset ultrafast -af volume=0.5 -b:a 64K {outputPath}" },
				#endregion
		};

        /// <summary>
        /// Nén kết hợp fps và bitrate video
        /// </summary>
        public static readonly Dictionary<string, string> FpsVsBitrateOption = new()
        {
				#region Chỉ nén fps
				{ "1920x1080_60fps_trueFps_falseResolution_trueBitrate","-i {videoPath} -c:v libvpx-vp9 -r 30 -preset ultrafast -b:v 1M -minrate 500K -maxrate 964K -bufsize 2M -crf 30 -af volume=0.5 -b:a 64K {outputPath}" },
                { "1920x1080_30fps_trueFps_falseResolution_trueBitrate","-i {videoPath} -c:v libvpx-vp9 -r 24 -preset ultrafast -b:v 1M -minrate 500K -maxrate 964K -bufsize 2M -crf 30 -af volume=0.5 -b:a 64K {outputPath}" },
                { "1280x720_60fps_trueFps_falseResolution_trueBitrate","-i {videoPath} -c:v libvpx-vp9 -r 30 -preset ultrafast -b:v 1M -minrate 500K -maxrate 964K -bufsize 2M -crf 30 -af volume=0.5 -b:a 64K {outputPath}" },
                { "1280x720_30fps_trueFps_falseResolution_trueBitrate","-i {videoPath} -c:v libvpx-vp9 -r 24 -preset ultrafast -b:v 1M -minrate 500K -maxrate 964K -bufsize 2M -crf 30 -af volume=0.5 -b:a 64K {outputPath}" },
                { "854x480_60fps_trueFps_falseResolution_trueBitrate","-i {videoPath} -c:v libvpx-vp9 -r 30 -preset ultrafast -b:v 1M -minrate 500K -maxrate 964K -bufsize 2M -crf 30 -af volume=0.5 -b:a 64K {outputPath}" },
                { "854x480_30fps_trueFps_falseResolution_trueBitrate","-i {videoPath} -c:v libvpx-vp9 -r 24 -preset ultrafast -b:v 1M -minrate 500K -maxrate 964K -bufsize 2M -crf 30 -af volume=0.5 -b:a 64K {outputPath}" },
                { "640x360_60fps_trueFps_falseResolution_trueBitrate","-i {videoPath} -c:v libvpx-vp9 -r 30 -preset ultrafast -b:v 1M -minrate 500K -maxrate 964K -bufsize 2M -crf 30 -af volume=0.5 -b:a 64K {outputPath}" },
                { "640x360_30fps_trueFps_falseResolution_trueBitrate","-i {videoPath} -c:v libvpx-vp9 -r 24 -preset ultrafast -b:v 1M -minrate 500K -maxrate 964K -bufsize 2M -crf 30 -af volume=0.5 -b:a 64K {outputPath}" },
				#endregion
		};

        /// <summary>
        /// Nén kết hợp độ phân giải và bitrate video
        /// </summary>
        public static readonly Dictionary<string, string> ResolutionVsBitrateOption = new()
        {
			#region Chỉ nén resolution
				{"1920x1080_60fps_falseFps_trueResolution_trueBitrate","-i {videoPath} -c:v libvpx-vp9 -vf scale=1280:720 -aspect 16:9 -preset ultrafast -b:v 1M -minrate 500K -maxrate 964K -bufsize 2M -crf 30 -af volume=0.5 -b:a 64K {outputPath}" },
                {"1920x1080_30fps_falseFps_trueResolution_trueBitrate","-i {videoPath} -c:v libvpx-vp9 -vf scale=1280:720 -aspect 16:9 -preset ultrafast -b:v 1M -minrate 500K -maxrate 964K -bufsize 2M -crf 30 -af volume=0.5 -b:a 64K {outputPath}" },
                {"1280x720_60fps_falseFps_trueResolution_trueBitrate","-i {videoPath} -c:v libvpx-vp9 -vf scale=854:480 -aspect 16:9 -preset ultrafast -b:v 1M -minrate 500K -maxrate 964K -bufsize 2M -crf 30 -af volume=0.5 -b:a 64K {outputPath}" },
                {"1280x720_30fps_falseFps_trueResolution_trueBitrate","-i {videoPath} -c:v libvpx-vp9 -vf scale=854:480 -aspect 16:9 -preset ultrafast -b:v 1M -minrate 500K -maxrate 964K -bufsize 2M -crf 30 -af volume=0.5 -b:a 64K {outputPath}" },
                {"854x480_60fps_falseFps_trueResolution_trueBitrate","-i {videoPath} -c:v libvpx-vp9 -vf scale=640:360 -aspect 4:3 -preset ultrafast -b:v 1M -minrate 500K -maxrate 964K -bufsize 2M -crf 30 -af volume=0.5 -b:a 64K {outputPath}" },
                {"854x480_30fps_falseFps_trueResolution_trueBitrate","-i {videoPath} -c:v libvpx-vp9 -vf scale=640:360 -aspect 4:3 -preset ultrafast -b:v 1M -minrate 500K -maxrate 964K -bufsize 2M -crf 30 -af volume=0.5 -b:a 64K {outputPath}" },
                {"640x360_60fps_falseFps_trueResolution_trueBitrate","-i {videoPath} -c:v libvpx-vp9 -vf scale=426:240 -aspect 4:3 -preset ultrafast -b:v 1M -minrate 500K -maxrate 964K -bufsize 2M -crf 30 -af volume=0.5 -b:a 64K {outputPath}" },
                {"640x360_30fps_falseFps_trueResolution_trueBitrate","-i {videoPath} -c:v libvpx-vp9 -vf scale=426:240 -aspect 4:3 -preset ultrafast -b:v 1M -minrate 500K -maxrate 964K -bufsize 2M -crf 30 -af volume=0.5 -b:a 64K {outputPath}" },
				#endregion
		};
    }
}
