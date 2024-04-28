using System.Diagnostics;
using System.Text;
using console_join_video_segments_demo.Infra;
using console_join_video_segments_demo.ViewModels;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace console_join_video_segments_demo.Services;

public class JoinService : IHostedService
{
    private readonly IRabbitClient _client;

    public JoinService(IRabbitClient client)
    {
        _client = client;
    }
    
    private void ProcessMessage(string message)
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

            ExecuteFFmpegCommand(ffmpegCommand);
        }
    }
    
    private static string CalculateToTime(int totalSegments, int cutEndSeconds)
    {
        int totalDurationInSeconds = totalSegments * 10;
        return TimeSpan.FromSeconds(totalDurationInSeconds - cutEndSeconds).ToString(@"hh\:mm\:ss");
    }

    private static void ExecuteFFmpegCommand(string command)
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
        string error = process.StandardError.ReadToEnd();
        process.WaitForExit();

        
        if (process.ExitCode != 0) 
        {
            Console.WriteLine("Error: " + error);
        }
        else
        {
            Console.WriteLine("FFmpeg output: " + output);
            if (!string.IsNullOrEmpty(error))
            {
                Console.WriteLine("FFmpeg logs: " + error); 
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