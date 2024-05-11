namespace console_join_video_segments_demo.Infra;

public interface IElasticSettings
{
    string ApiKey { get; set; }
    string CloudId { get; set; }
}

public class ElasticSettings : IElasticSettings
{
    public string ApiKey { get; set; }
    public string CloudId { get; set; }
}