namespace crop_api_elastic_demo.Infra;

public interface IRabbitSettings
{
    string ConnectionString { get; set; }
}

public class RabbitSettings : IRabbitSettings
{
    public string ConnectionString { get; set; }
}