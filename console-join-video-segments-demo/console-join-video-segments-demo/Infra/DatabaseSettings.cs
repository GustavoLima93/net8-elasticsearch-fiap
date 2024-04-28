namespace console_join_video_segments_demo.Infra;

public class DatabaseSettings : IDatabaseSettings
{
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }
}

public interface IDatabaseSettings
{
    string ConnectionString { get; set; }
    string DatabaseName { get; set; }
}