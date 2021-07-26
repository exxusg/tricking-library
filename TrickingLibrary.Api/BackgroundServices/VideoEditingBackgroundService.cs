using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TrickingLibrary.Data;
using TrickingLibrary.Models;

namespace TrickingLibrary.Api.BackgroundServices
{
    public class VideoEditingBackgroundService : BackgroundService
    {
        private readonly ChannelReader<EditVideoMessage> _channelReader;
        private readonly ILogger<VideoEditingBackgroundService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly VideoManager _videoManager;

        public VideoEditingBackgroundService(
            Channel<EditVideoMessage> channel,
            ILogger<VideoEditingBackgroundService> logger,
            IServiceProvider serviceProvider,
            VideoManager videoManager)
        {
            _channelReader = channel.Reader;
            _logger = logger;
            _serviceProvider = serviceProvider;
            _videoManager = videoManager;
        }
        
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // we are using the channel to communicate between the background process and the main process
            while (await _channelReader.WaitToReadAsync(stoppingToken))
            {
                var message = await _channelReader.ReadAsync(stoppingToken);
                var inputPath = _videoManager.TemporarySavePath(message.Input);
                var outputConvertedName = _videoManager.GenerateConvertedFileName();
                var outputThumbnailName = _videoManager.GenerateThumbnailFileName();
                var outputConvertedPath = _videoManager.TemporarySavePath(outputConvertedName);
                var outputThumbnailPath = _videoManager.TemporarySavePath(outputThumbnailName);
                
                try
                {
                    var startInfo = new ProcessStartInfo
                    {
                        FileName = _videoManager.FFMPEGPath,
                        Arguments =
                            $"-y -i {inputPath} -an -vf scale=640x480 {outputConvertedPath} -ss 00:00:00 -vframes 1 -vf scale=640x480 {outputThumbnailPath}",
                        CreateNoWindow = true,
                        UseShellExecute = false
                    };

                    using var process = new Process {StartInfo = startInfo};
                    process.Start();
                    process.WaitForExit();

                    if (!_videoManager.TemporaryFileExists(outputConvertedName))
                    {
                        throw new Exception("FFMPEG failed to generate converted video");
                    }

                    if (!_videoManager.TemporaryFileExists(outputThumbnailName))
                    {
                        throw new Exception("FFMPEG failed to generate thumbnail");
                    }

                    using var scope = _serviceProvider.CreateScope();
                    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    var submission = context.Submissions.FirstOrDefault(x => x.Id.Equals(message.SubmissionId));

                    submission.Video = new Video
                    {
                        VideoLink = outputConvertedName,
                        ThumbLink = outputThumbnailName
                    };
                    submission.VideoProcessed = false;

                    await context.SaveChangesAsync(stoppingToken);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Video processing failed for {Input}", message.Input);
                    _videoManager.DeleteTemporaryFile(outputConvertedName);
                    _videoManager.DeleteTemporaryFile(outputConvertedName);
                }
                finally
                {
                    _videoManager.DeleteTemporaryFile(message.Input);
                }
                
            }
        }
    }
}