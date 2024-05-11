using System.Diagnostics;
using System.Text;
using console_join_video_segments_demo.Entities;
using console_join_video_segments_demo.Infra;
using console_join_video_segments_demo.ViewModels;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace console_join_video_segments_demo.Services;

public class JoinService : IHostedService
{
    private readonly IRabbitClient _client;
    private readonly IElasticClient<CropLog> _elasticClient;

    public JoinService(IRabbitClient client, IElasticClient<CropLog> elasticClient)
    {
        _client = client;
        _elasticClient = elasticClient;
    }
    
    private async void ProcessMessage(string message)
    {
        Console.WriteLine($"Processing message: {message}");
        CropViewModel? videoDetails = JsonConvert.DeserializeObject<CropViewModel>(message);
        
        StringBuilder ffmpegFileList = new StringBuilder();
        
        Console.WriteLine($"videoDetails: {videoDetails?.Name}");
        
        string directoryPath = "~/Downloads/videos";
        string resolvedPath = Environment.ExpandEnvironmentVariables(directoryPath.Replace("~", Environment.GetEnvironmentVariable("HOME")));
        Console.WriteLine($"resolvedPath: {resolvedPath}");
        
        if (videoDetails != null)
        {
            for (int i = videoDetails.StartSegment; i <= videoDetails.EndSegment; i++)
            {
                ffmpegFileList.Append($"file '{resolvedPath}/output{i}.ts'\n");
            }

            string concatFilePath = $"{resolvedPath}/ffmpeg_concat_list.txt";
            File.WriteAllText(concatFilePath, ffmpegFileList.ToString());
            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");

            string outputFilePath = $"{resolvedPath}/concat-{timestamp}.mp4";
            string ffmpegCommand =
                $"ffmpeg -f concat -safe 0 -i {concatFilePath}  {outputFilePath}";
            
            //-c copy -ss 00:00:{videoDetails.StartTime} -to {CalculateToTime(videoDetails.EndSegment - videoDetails.StartSegment, videoDetails.EndTime)}

            await ExecuteFFmpegCommand(ffmpegCommand, videoDetails);
        }
    }
    
    private static string CalculateToTime(int totalSegments, int cutEndSeconds)
    {
        int totalDurationInSeconds = totalSegments * 10;
        return TimeSpan.FromSeconds(totalDurationInSeconds - cutEndSeconds).ToString(@"hh\:mm\:ss");
    }

    private  async Task ExecuteFFmpegCommand(string command, CropViewModel? cropViewModel)
    {
        Process process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "/bin/bash",  
                Arguments = $"-c \"{command}\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            }
        };

        process.Start();
        string output = process.StandardOutput.ReadToEnd();
        string end = process.StandardError.ReadToEnd();
        process.WaitForExit();

        
        if (process.ExitCode != 0) 
        {
            Console.WriteLine("Error: " + end);
            var crop = new CropLog(cropViewModel.Id, cropViewModel.Name, end, false);
           await _elasticClient.Create(crop, "crop-join");
        }
        else
        {
            if (!string.IsNullOrEmpty(end))
            {
                var cropError = new CropLog(cropViewModel.Id, cropViewModel.Name, end, true);
                await _elasticClient.Create(cropError, "crop-join");
            }
        }
    }
    

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _client.MessageReceived += ProcessMessage;

        _client.ConsumeMessage("create_crop");
        
        return Task.CompletedTask;
    }
    
    

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
    
}