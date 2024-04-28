namespace crop_api_elastic_demo.Infra;

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