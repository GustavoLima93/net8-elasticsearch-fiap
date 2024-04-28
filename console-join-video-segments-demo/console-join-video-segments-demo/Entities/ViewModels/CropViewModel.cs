namespace console_join_video_segments_demo.ViewModels;

public class CropViewModel
{
    public CropViewModel(string id, string name, string programName, int startTime, int endTime, int startSegment, int endSegment)
    {
        Id = id;
        Name = name;
        ProgramName = programName;
        StartTime = startTime;
        EndTime = endTime;
        StartSegment = startSegment;
        EndSegment = endSegment;
    }

    public string Id { get; set; }
    public string Name { get; set;}
    public string ProgramName { get; set;}
    public int StartTime { get; set;}
    public int EndTime { get; set;}
    public int StartSegment { get; set;}
    public int EndSegment { get; set;}
}