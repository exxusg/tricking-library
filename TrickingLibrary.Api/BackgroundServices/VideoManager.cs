using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

namespace TrickingLibrary.Api.BackgroundServices
{
    public class VideoManager
    {
        private readonly IWebHostEnvironment _environment;
        public const string TempPrefix = "temp_";
        public const string ConvertedPrefix = "converted_";
        public const string ThumbnailPrefix = "thumbnail_";        

        public string WorkingDirectory => _environment.WebRootPath;

        public VideoManager(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public bool Temporary(string fileName)
        {
            return fileName.StartsWith(TempPrefix);
        }

        public string DevVideoPath(string fileName)
        {
            return !_environment.IsDevelopment() ? null : Path.Combine(WorkingDirectory, fileName);
        }

        public async Task<string> SaveTemporaryVideo(IFormFile video)
        {
           var fileName = string.Concat(TempPrefix, DateTime.Now.Ticks, Path.GetExtension(video.FileName));
           var savePath = TemporarySavePath(fileName);
           
           await using (var fileStream = new FileStream(savePath, FileMode.Create, FileAccess.Write))
           {
               await video.CopyToAsync(fileStream);
           }

           return fileName;
        }

        public bool TemporaryFileExists(string fileName)
        {
            var path = TemporarySavePath(fileName);
            return File.Exists(path);
        }
        
        public void DeleteTemporaryVideo(string fileName)
        {
            var path = TemporarySavePath(fileName);
            File.Delete(path);
        }
        
        public string TemporarySavePath(string fileName)
        {
            return Path.Combine(WorkingDirectory, fileName);
        }

        public string GenerateConvertedFileName() => $"{ConvertedPrefix}{DateTime.Now.Ticks}.mp4";
        public string GenerateThumbnailFileName() => $"{ThumbnailPrefix}{DateTime.Now.Ticks}.jpg";

    }
}