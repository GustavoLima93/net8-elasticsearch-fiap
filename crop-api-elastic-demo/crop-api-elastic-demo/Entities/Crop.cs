using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace crop_api_elastic_demo.Entities;

public class Crop : BaseEntity
{
    public Crop(string name, string programName, int startTime, int endTime, int startSegment, int endSegment)
    {
        Name = name;
        ProgramName = programName;
        StartTime = startTime;
        EndTime = endTime;
        StartSegment = startSegment;
        EndSegment = endSegment;
    }
    
    [BsonElement("name")]
    public string Name { get; private set; }
    [BsonElement("programName")]
    public string ProgramName { get; private set; }
    [BsonElement("startTime")]
    public int StartTime { get; private set; }
    [BsonElement("endTime")]
    public int EndTime { get; private set; }
    [BsonElement("startSegment")]
    public int StartSegment { get; private set; }
    [BsonElement("endSegment")]
    public int EndSegment { get; private set; }
}