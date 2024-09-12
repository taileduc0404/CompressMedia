using CompressMedia.Data;
using CompressMedia.DTOs;
using CompressMedia.Models;
using CompressMedia.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json;
using System;
using System.Diagnostics;

namespace CompressMedia.Repositories
{
    public class MediaService : IMediaService
    {
        private readonly ApplicationDbContext _context;
        private readonly IAuthService _authService;
        private readonly IFileProvider _fileProvider;
        public MediaService(ApplicationDbContext context, IAuthService authService, IFileProvider fileProvider)
        {
            _context = context;
            _authService = authService;
            _fileProvider = fileProvider;
        }

        /// <summary>
        /// Kiểm tra xem tên của Video có tồn tại không
        /// </summary>
        /// <param name="videoName"></param>
        /// <returns></returns>
        private bool CheckVideoNameExist(string videoName)
        {
            string splitString = @"D:\BÀI TẬP\ASP.NET\CompressMedia\CompressMedia\wwwroot\Medias\Videos\";
            Media result = _context.medias.FirstOrDefault(m => m.MediaPath!.Replace(splitString, "") == videoName)!;
            if (result is not null)
            {
                return false;
            }
            return true;

        }

        /// <summary>
        /// Tìm media bằng Id
        /// </summary>
        /// <param name="mediaId"></param>
        /// <returns></returns>
        public async Task<Media> GetMediaById(int mediaId)
        {
            Media media = await _context.medias.SingleOrDefaultAsync(m => m.MediaId == mediaId);
            if (media == null)
            {
                return null!;
            }

            return media;
        }

        /// <summary>
        /// Lấy ra danh sách video của user đagn đăngg nhập
        /// </summary>
        /// <returns></returns>
        public async Task<ICollection<Media>> GetAllVideo()
        {
            string cookie = _authService.GetLoginInfoFromCookie();
            string cookieDecode = _authService.DecodeFromBase64(cookie);
            LoginDto userInfo = JsonConvert.DeserializeObject<LoginDto>(cookieDecode);
            User? user = await _context.users.FirstOrDefaultAsync(u => u.Username == userInfo.Username);
            return await _context.medias.Where(m => m.MediaType!.StartsWith("video") && m.UserId == user!.UserId).ToListAsync();
        }

        /// <summary>
        /// Thực thi command
        /// </summary>
        /// <param name="arguments"></param>
        private void ExecuteCommand(string arguments)
        {

            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "ffmpeg",
                Arguments = arguments,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                RedirectStandardInput = true
            };

            using (Process process = new Process())
            {
                process.StartInfo = psi;

                // Show ra output hoặc error
                process.OutputDataReceived += (sender, args) => Console.WriteLine(args.Data);
                process.ErrorDataReceived += (sender, args) => Console.WriteLine(args.Data);

                // Chạy process
                process.Start();

                // Đọc output hoặc error
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();

                // Chờ cho kết thúc
                process.WaitForExit();
            }
        }

        /// <summary>
        /// Lấy thông tin video
        /// </summary>
        /// <param name="videoPath"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="fps"></param>
        /// <param name="bitrate"></param>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="Exception"></exception>
        private void GetVideoInfo(string videoPath, out int width, out int height, out int fps, out int bitrate)
        {
            if (!File.Exists(videoPath))
            {
                throw new FileNotFoundException($"The file {videoPath} does not exist");
            }

            string argument = $"-v error -select_streams v:0 -show_entries stream=width,height,r_frame_rate,bit_rate -of csv=p=0 {videoPath}";
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "ffprobe",
                Arguments = argument,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            using (Process process = new Process())
            {
                process.StartInfo = psi;
                process.Start();

                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();
                process.WaitForExit();

                if (!string.IsNullOrEmpty(error))
                {
                    throw new Exception($"FFprobe Error: {error}");
                }

                if (string.IsNullOrEmpty(output))
                {
                    throw new Exception("No output from ffprobe.");
                }

                string[] lines = output.Split(',');
                if (lines.Length >= 4)
                {
                    width = int.Parse(lines[0]);
                    height = int.Parse(lines[1]);
                    fps = ParseFps(lines[2]);
                    bitrate = int.Parse(lines[3]) / 1024; // Đổi byte sang kb
                }
                else
                {
                    throw new Exception("Unexpected ffprobe output format.");
                }
            }
        }

        /// <summary>
        /// Định dạng lại fps
        /// </summary>
        /// <param name="fpsString"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private int ParseFps(string fpsString)
        {
            var parts = fpsString.Split('/');
            if (parts.Length == 2 && int.TryParse(parts[0], out int numerator) && int.TryParse(parts[1], out int denominator))
            {
                return numerator / denominator;
            }
            throw new Exception("Invalid FPS format.");
        }

        /// <summary>
        /// Chọn option: nén video.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="videoPath"></param>
        /// <param name="fileNameOutput"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>  
        public string GetOption(string videoPath, string videoOutput, int width, int height, int fps, int bitrate)
        {
            string root = "/Medias/Videos/";
            if (width == 1920 && height == 1080 && fps == 60 && bitrate >= 4500 && bitrate <= 9000)         // Áp dụng cho video 1920x1080 60fps, bitrate 4500-9000
            {
                return $"-i {videoPath} -c:v libvpx-vp9 -vf scale=1280:720 -aspect 16:9 -preset ultrafast -b:v 1M -minrate 500K -maxrate 964K -bufsize 2M -crf 30 -af volume=0.5 -b:a 64K {"wwwroot" + root + videoOutput}";
            }
            else if (width == 1920 && height == 1080 && fps == 30 && bitrate >= 3000 && bitrate <= 6000)    // Áp dụng cho video 1920x1080 60fps, bitrate 4500-9000
            {
                return $"-i {videoPath} -c:v libvpx-vp9 -vf scale=1280:720 -aspect 16:9 -preset ultrafast -b:v 1M -minrate 500K -maxrate 964K -bufsize 2M -crf 30 -af volume=0.5 -b:a 64K {"wwwroot" + root + videoOutput}";
            }
            else if (width == 1280 && height == 720 && fps == 60 && bitrate >= 2250 && bitrate <= 6000)     // Áp dụng cho video 1280x720 60fps, bitrate 2250-6000
            {
                return $"-i {videoPath} -c:v libvpx-vp9 -vf scale=854:480 -aspect 16:9 -preset ultrafast -b:v 1M -minrate 500K -maxrate 964K -bufsize 2M -crf 30 -af volume=0.5 -b:a 64K {"wwwroot" + root + videoOutput}";
            }
            else if (width == 1280 && height == 720 && fps == 30 && bitrate >= 1500 && bitrate <= 4000)     // Áp dụng cho video 1280x720 30fps, bitrate 1500-4000
            {
                return $"-i {videoPath} -c:v libvpx-vp9 -vf scale=854:480 -aspect 16:9 -preset ultrafast -b:v 1M -minrate 500K -maxrate 964K -bufsize 2M -crf 30 -af volume=0.5 -b:a 64K {"wwwroot" + root + videoOutput}";
            }
            else if (width == 854 && height == 480 && fps == 60 && bitrate >= 1200 && bitrate <= 2500)     // Áp dụng cho vid0eo 854:480 60fps, bitrate 1200-2500
            {
                return $"-i {videoPath} -c:v libvpx-vp9 -vf scale=640:360 -aspect 4:3 -preset ultrafast -b:v 1M -minrate 500K -maxrate 964K -bufsize 2M -crf 30 -af volume=0.5 -b:a 64K {"wwwroot" + root + videoOutput}";
            }
            else if (width == 854 && height == 480 && fps == 30 && bitrate >= 700 && bitrate <= 1500)     // Áp dụng cho video 854:480 30fps, bitrate 700-1500
            {
                return $"-i {videoPath} -c:v libvpx-vp9 -vf scale=640:360 -aspect 4:3 -preset ultrafast -b:v 1M -minrate 500K -maxrate 964K -bufsize 2M -crf 30 -af volume=0.5 -b:a 64K {"wwwroot" + root + videoOutput}";
            }
            else if (width == 640 && height == 360 && fps == 60 && bitrate >= 800 && bitrate <= 1800)     // Áp dụng cho video 640:360 60fps, bitrate 800-1800
            {
                return $"-i {videoPath} -c:v libvpx-vp9 -vf scale=426:240 -aspect 16:9 -preset ultrafast -b:v 1M -minrate 500K -maxrate 964K -bufsize 2M -crf 30 -af volume=0.5 -b:a 64K {"wwwroot" + root + videoOutput}";
            }
            else if (width == 640 && height == 360 && fps == 30 && bitrate >= 500 && bitrate <= 1200)     // Áp dụng cho video 640:360 30fps, bitrate 500-1200
            {
                return $"-i {videoPath} -c:v libvpx-vp9 -vf scale=426:240 -aspect 16:9 -preset ultrafast -b:v 1M -minrate 500K -maxrate 964K -bufsize 2M -crf 30 -af volume=0.5 -b:a 64K {"wwwroot" + root + videoOutput}";
            }
            else if (width < height)
            {
                return $"-i {videoPath} -c:v libvpx-vp9 -vf pad=width=ih*16/9:height=ih:x=(ow-iw)/2:y=0 -preset ultrafast -b:v 1M -minrate 500K -maxrate 964K -bufsize 2M -crf 30 -af volume=0.5 -b:a 64K {"wwwroot" + root + videoOutput}";
            }
            else
            {
                return $"-i {videoPath} -c:v libvpx-vp9 -preset ultrafast -b:v 1M -minrate 500K -maxrate 964K -bufsize 2M -crf 30 -af volume=0.5 -b:a 64K  {"wwwroot" + root + videoOutput}";
            }
        }

        /// <summary>
        /// Nén video
        /// </summary>
        /// <param name="videoPath"></param>
        /// <param name="fileNameOutput"></param>
        /// <returns></returns>
        public string OptimizeVideo(string videoPath, string fileNameOutput)
        {
            if (videoPath is not null)
            {
                GetVideoInfo(videoPath, out int width, out int height, out int fps, out int bitrate);
                string arg = GetOption(videoPath, fileNameOutput, width, height, fps, bitrate);
                ExecuteCommand(arg);
            }
            return fileNameOutput;
        }

        /// <summary>
        /// Upload và nén media
        /// </summary>
        /// <param name="mediaDto"></param>
        /// <returns></returns>
        /// <exception cref="FileNotFoundException"></exception>
        public bool CompressMedia(string fileNameInput)
        {
            // Kiểm tra người dùng có đăng nhập chưa
            string cookie = _authService.GetLoginInfoFromCookie();
            string cookieDecode = _authService.DecodeFromBase64(cookie);
            LoginDto userInfo = JsonConvert.DeserializeObject<LoginDto>(cookieDecode);
            User? user = _context.users.FirstOrDefault(u => u.Username == userInfo.Username);



            string root = "/Medias/Videos/";
            // fileNameInput: 7a87788f-f8a7-45b1-83b4-b2e2eb9bc30b&test.mp4

            string rootPathFileNameInput = root + fileNameInput;
            IFileInfo fileInputInfo = _fileProvider.GetFileInfo(rootPathFileNameInput);
            string? rootPathFileInputInfo = fileInputInfo.PhysicalPath;
            Media deleteMedia = _context.medias.FirstOrDefault(m => m.MediaPath == rootPathFileInputInfo)!;

            deleteMedia.Status = "Compressing";

            string fileNameOutput = Guid.NewGuid() + "&" + fileNameInput!.Split('&')[1];
            string result = OptimizeVideo("wwwroot" + rootPathFileNameInput, fileNameOutput);



            string srcOptimized = root + result;
            IFileInfo fileInfoOptimized = _fileProvider.GetFileInfo(srcOptimized);
            string? rootPathOptimized = fileInfoOptimized.PhysicalPath!;
            if (rootPathOptimized != null)
            {
                // Xóa ảnh gốc sau khi optimize
                File.Delete("wwwroot" + rootPathFileNameInput);

                _context.medias.Remove(deleteMedia);

                if (!File.Exists(rootPathOptimized))
                {
                    throw new FileNotFoundException("File optimized does not exist.", rootPathOptimized);
                }

                // Lấy độ dài file của ảnh đã optimize
                FileInfo fileInfo = new FileInfo(rootPathOptimized);
                long fileLength = fileInfo.Length;

                // Xác định loại media
                string mediaType = fileInfo.Extension.ToLower() switch
                {
                    ".mp4" => "video/mp4",
                    ".jpg" => "image/jpg",
                    ".jpeg" => "image/jpeg",
                    ".png" => "image/png",
                    ".gif" => "image/gif",
                    _ => "unknown"
                };

                Media media = new Media()
                {
                    MediaPath = rootPathOptimized,
                    CreatedDate = DateTime.Now,
                    Size = fileLength,
                    Status = "Compressed",
                    MediaType = mediaType,
                    UserId = user!.UserId
                };

                _context.medias.Add(media);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Upload media
        /// </summary>
        /// <param name="mediaDto"></param>
        /// <returns></returns>
        public string UploadMedia(MediaDto mediaDto)
        {
            // Kiểm tra người dùng có đăng nhập chưa
            string cookie = _authService.GetLoginInfoFromCookie();
            string cookieDecode = _authService.DecodeFromBase64(cookie);
            LoginDto userInfo = JsonConvert.DeserializeObject<LoginDto>(cookieDecode);
            User? user = _context.users.FirstOrDefault(u => u.Username == userInfo.Username);

            // Kiểm tra xem người dùng có upload file media lên chưa
            if (mediaDto.Media != null)
            {
                string root = "/Medias/Videos/";
                string imageName = $"{Guid.NewGuid() + "&" + mediaDto.Media.FileName}";
                //string imageName = $"{mediaDto.Media.FileName}";

                if (!Directory.Exists("wwwroot" + root))
                {
                    Directory.CreateDirectory("wwwroot" + root);
                }

                string src = root + imageName;
                IFileInfo imgInfo = _fileProvider.GetFileInfo(src);
                string? rootPath = imgInfo.PhysicalPath!;

                if (string.IsNullOrEmpty(rootPath)) return null!;
                using (var fileStream = new FileStream(rootPath, FileMode.Create))
                {
                    mediaDto.Media.CopyTo(fileStream);
                }

                if (string.IsNullOrEmpty(rootPath)) return null!;

                if (rootPath != null)
                {
                    // Lấy độ dài file của ảnh đã optimize
                    FileInfo fileInfo = new FileInfo(rootPath);
                    long fileLength = fileInfo.Length;

                    // Xác định loại media
                    string mediaType = fileInfo.Extension.ToLower() switch
                    {
                        ".mp4" => "video/mp4",
                        ".jpg" => "image/jpg",
                        ".jpeg" => "image/jpeg",
                        ".png" => "image/png",
                        ".gif" => "image/gif",
                        _ => "unknown"
                    };

                    Media media = new Media()
                    {
                        MediaPath = rootPath,
                        CreatedDate = DateTime.Now,
                        Size = fileLength,
                        Status = "Original",
                        MediaType = mediaType,
                        UserId = user!.UserId
                    };

                    _context.medias.Add(media);
                    _context.SaveChanges();
                    return "wwwroot" + src;
                }
                return null!;
            }
            return null!;
        }

       
    }
}