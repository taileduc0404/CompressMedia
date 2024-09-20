using CompressMedia.Data;
using CompressMedia.DTOs;
using CompressMedia.Models;
using CompressMedia.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using System.Diagnostics;

namespace CompressMedia.Repositories
{
    public class BlobService : IBlobService
    {
        private readonly ApplicationDbContext _context;
        private readonly BlobStorageDbContext _storageContext;
        private readonly IGridFSBucket _gridFSBucket;

        public BlobService(ApplicationDbContext context, BlobStorageDbContext storageContext)
        {
            _context = context;
            _storageContext = storageContext;
            _gridFSBucket = new GridFSBucket(_storageContext._mongoDatabase);
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
        private static string GetOption(string videoPath, string outputPath, int width, int height, string fps, bool isFps, bool isResolution, bool isBitrate)
        {

            string resolution = $"{width}x{height}";
            string key = $"{resolution}_{fps}_{(isFps ? "true" : "false")}Fps_{(isResolution ? "true" : "false")}Resolution_{(isBitrate ? "true" : "false")}Bitrate";

            // Trường hợp chỉ nén fps
            if (isFps && !isResolution && !isBitrate && CompressOption.FpsOnlyOption.TryGetValue(key, out string? fpsCommand))
            {
                return fpsCommand.Replace("{videoPath}", videoPath).Replace("{outputPath}", outputPath);
            }

            // Trường hợp chỉ nén resolution
            if (isResolution && !isFps && !isBitrate && CompressOption.ResolutionOnlyOption.TryGetValue(key, out string? resolutionCommand))
            {
                return resolutionCommand.Replace("{videoPath}", videoPath).Replace("{outputPath}", outputPath);
            }

            // Trwufogn hợp chỉ nén bitrate
            if (isBitrate && !isFps && !isResolution && CompressOption.BitrateOnlyOption.TryGetValue(key, out string? bitrateCommand))
            {
                return bitrateCommand.Replace("{videoPath}", videoPath).Replace("{outputPath}", outputPath);
            }

            // Trường hợp nén fps và resolution
            if (!isBitrate && isFps && isResolution && CompressOption.FpsVsResolutionOption.TryGetValue(key, out string? fpsVsResolutionCommand))
            {
                return fpsVsResolutionCommand.Replace("{videoPath}", videoPath).Replace("{outputPath}", outputPath);
            }

            // Trường hợp nén fps và bitrate
            if (isBitrate && isFps && !isResolution && CompressOption.FpsVsBitrateOption.TryGetValue(key, out string? fpsVsBitrateCommand))
            {
                return fpsVsBitrateCommand.Replace("{videoPath}", videoPath).Replace("{outputPath}", outputPath);
            }

            // Trường hợp nén resolution và bitrate
            if (isBitrate && !isFps && isResolution && CompressOption.ResolutionVsBitrateOption.TryGetValue(key, out string? resolutionVsBitrateCommand))
            {
                return resolutionVsBitrateCommand.Replace("{videoPath}", videoPath).Replace("{outputPath}", outputPath);
            }

            if (width < height)
            {
                return $"-i {videoPath} -c:v libvpx-vp9 -vf pad=width=ih*16/9:height=ih:x=(ow-iw)/2:y=0 -preset ultrafast -b:v 1M -minrate 500K -maxrate 964K -bufsize 2M -crf 30 -af volume=0.5 -b:a 64K {outputPath}";
            }

            // Trường hợp nén full option
            if (CompressOption.CompressFullOption.TryGetValue(key, out string? fullCommand))
            {
                return fullCommand.Replace("{videoPath}", videoPath).Replace("{outputPath}", outputPath);
            }

            // Truonwggf hợp người dùng không chọn option nào
            return $"-i {videoPath} -c:v libvpx-vp9 -preset ultrafast -af volume=0.5 -b:a 64K {outputPath}";
        }

        /// <summary>
        /// Tạo 1 blob
        /// </summary>
        /// <param name="blobDto"></param>
        /// <returns></returns>
        public async Task<bool> CreateBlobAsync(BlobDto blobDto)
        {
            if (blobDto == null || blobDto.Data?.Length == 0)
            {
                return false;
            }

            var metadata = new BsonDocument
            {
                {"filename", blobDto.Data?.FileName },
                {"contentType", blobDto.Data?.ContentType },
                {"length",blobDto.Data?.Length},
                {"uploadDate", DateTime.Now }
            };

            using (Stream stream = blobDto.Data!.OpenReadStream())
            {
                var fileId = await _gridFSBucket.UploadFromStreamAsync(blobDto.Data.FileName, stream, new GridFSUploadOptions
                {
                    Metadata = metadata
                });

                Blob newBlob = new Blob
                {
                    BlobId = fileId.ToString(),
                    ContainerId = blobDto.ContainerId,
                    BlobName = blobDto.Data.FileName,
                    Status = "Original",
                    ContentType = blobDto.Data.ContentType,
                    Size = blobDto.Data.Length,
                    UploadDate = blobDto.UploadedDate,
                };
                await _context.blobs.AddAsync(newBlob);
                await _context.SaveChangesAsync();
            }

            return true;
        }

        /// <summary>
        /// Xóa 1 blob
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> DeleteBlobAsync(string blobId)
        {
            if (blobId is null)
            {
                return false;
            }
            var oldBlob = await _context.blobs.FirstOrDefaultAsync(b => b.BlobId == blobId);
            if (oldBlob != null)
            {
                _context.blobs.Remove(oldBlob);
                await _context.SaveChangesAsync();
            }
            var oldFileId = new ObjectId(blobId);
            var filter = Builders<GridFSFileInfo<ObjectId>>.Filter.Eq(x => x.Id, oldFileId);
            var oldFileEntry = await _gridFSBucket.FindAsync(filter);
            var oldFile = oldFileEntry.FirstOrDefault();
            if (oldFile != null)
            {
                await _gridFSBucket.DeleteAsync(oldFile.Id);
            }
            return true;
        }

        /// <summary>
        /// Download file về máy
        /// </summary>
        /// <param name="blobDto"></param>
        /// <returns></returns>
        private async Task<string> DownloadFile(BlobDto blobDto)
        {
            var filter = Builders<GridFSFileInfo<ObjectId>>
                                  .Filter.Eq(x => x.Filename, blobDto.BlobName);
            var searchResult = await _gridFSBucket.FindAsync(filter);
            var fileEntry = searchResult.FirstOrDefault();

            byte[] content = await _gridFSBucket.DownloadAsBytesAsync(fileEntry.Id);
            if (content == null)
            {
                return null!;
            }

            using (MemoryStream memoryStream = new MemoryStream(content))
            {
                string tempInputPath = Path.GetTempFileName();
                using (FileStream fileStream = new FileStream(tempInputPath, FileMode.Create, FileAccess.Write))
                {
                    memoryStream.CopyTo(fileStream);
                    return tempInputPath;
                }
            }
        }

        /// <summary>
        /// Get nội dung của blob (data của blob)
        /// </summary>
        /// <param name="blobDto"></param>
        /// <returns></returns>
        public async Task<bool> GetBlobContentAsync(BlobDto blobDto)
        {
            string result = await DownloadFile(blobDto);
            if (result is null)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Get thông tin của blob
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task GetBlobMetadataAsync()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get danh sách blob
        /// </summary>
        /// <param name="containerId"></param>
        /// <returns></returns>
        public async Task<ICollection<Blob>> GetListBlobAsync(int containerId)
        {
            return await _context.blobs.Where(b => b.ContainerId == containerId).ToListAsync();
        }

        /// <summary>
        /// Cập nhật blob
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task UpdateBlobAsync()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Xử lý nén
        /// </summary>
        /// <param name="videoPath"></param>
        /// <param name="fileNameOutput"></param>
        /// <returns></returns>
        public async Task<string> OptimizeVideo(BlobDto blobDto)
        {
            string tempFile = await DownloadFile(blobDto);
            string fileTempOuput = Path.GetTempFileName();
            string fileTempOuputFinal = Path.ChangeExtension(fileTempOuput, ".mp4");
            if (tempFile is not null)
            {
                GetVideoInfo(tempFile, out int width, out int height, out int fps, out int bitrate);

                string fpsString = $"{fps}fps";
                string arg = GetOption(tempFile, fileTempOuputFinal, width, height, fpsString, blobDto.IsFps, blobDto.IsResolution, blobDto.IsBitrateVideo);

                ExecuteCommand(arg);
                File.Delete(tempFile);
            }
            return fileTempOuputFinal;
        }

        /// <summary>
        /// Nén media
        /// </summary>
        /// <param name="mediaDto"></param>
        /// <returns></returns>
        /// <exception cref="FileNotFoundException"></exception>
        public async Task<bool> CompressMedia(BlobDto blobDto)
        {
            var oldBlob = await _context.blobs.FirstOrDefaultAsync(b => b.BlobId == blobDto.BlobId);
            oldBlob!.Status = "Compressing...";
            _context.blobs.Update(oldBlob);
            await _context.SaveChangesAsync();

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            string fileTempOutput = await OptimizeVideo(blobDto);
            stopwatch.Stop();
            TimeSpan timeSpan = stopwatch.Elapsed;
            string elapsedTime = string.Format("{0:00}:{1:00}:{2:00}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);

            byte[] fileBytes = File.ReadAllBytes(fileTempOutput);

            var metadata = new BsonDocument
            {
                {"filename", blobDto.BlobName },
                {"contentType", blobDto.ContentType },
                {"length",fileBytes.Length},
                {"uploadDate", oldBlob.UploadDate }
            };

            using (Stream stream = new MemoryStream(fileBytes))
            {
                var fileId = await _gridFSBucket.UploadFromStreamAsync(blobDto.BlobName, stream, new GridFSUploadOptions
                {
                    Metadata = metadata
                });

                Blob newBlob = new Blob
                {
                    BlobId = fileId.ToString(),
                    ContainerId = blobDto.ContainerId,
                    BlobName = blobDto.BlobName!,
                    Status = "Compressed",
                    ContentType = blobDto.ContentType!,
                    Size = fileBytes.Length,
                    CompressionTime = elapsedTime,
                    UploadDate = oldBlob.UploadDate,

                };
                await _context.blobs.AddAsync(newBlob);
                await _context.SaveChangesAsync();
            }

            File.Delete(fileTempOutput);

            //Xóa video cũ trong sql server
            if (oldBlob != null)
            {
                _context.blobs.Remove(oldBlob);
                await _context.SaveChangesAsync();
            }

            //Xóa video cũ trong mongodb
            var oldFileId = new ObjectId(blobDto.BlobId);
            var filter = Builders<GridFSFileInfo<ObjectId>>.Filter.Eq(x => x.Id, oldFileId);
            var oldFileEntry = await _gridFSBucket.FindAsync(filter);
            var oldFile = oldFileEntry.FirstOrDefault();
            if (oldFile != null)
            {
                await _gridFSBucket.DeleteAsync(oldFile.Id);
            }

            return true;
        }

        /// <summary>
        /// Đọc dữ liệu blob vào stream để hiển thị vidoe lên
        /// </summary>
        /// <param name="blobId"></param>
        /// <returns></returns>
        /// <exception cref="FileNotFoundException"></exception>
        public async Task<Stream> GetVideoStreamAsync(string blobId)
        {
            var filter = Builders<GridFSFileInfo<ObjectId>>.Filter.Eq(x => x.Id, new ObjectId(blobId));
            var searchResult = await _gridFSBucket.FindAsync(filter);
            var fileEntyr = searchResult.FirstOrDefault();
            if (fileEntyr != null)
            {
                var stream = await _gridFSBucket.OpenDownloadStreamAsync(fileEntyr.Id);
                return stream;
            }

            throw new FileNotFoundException("Video not found");
        }
    }

}