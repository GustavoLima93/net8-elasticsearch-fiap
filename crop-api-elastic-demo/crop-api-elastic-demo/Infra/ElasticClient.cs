using Elastic.Clients.Elasticsearch;
using Elastic.Transport;

namespace crop_api_elastic_demo.Infra;

public interface IElasticCLient<T>
{
    Task<IReadOnlyCollection<T>> Get(int page, int size, IndexName index);
    Task<bool> Create(T log, IndexName index);
}


public class ElasticClient<T> : IElasticCLient<T>
{
    private readonly ElasticsearchClient _client;

    public ElasticClient(IElasticSettings settings)
    {
        this._client = new ElasticsearchClient(settings.CloudId, new ApiKey(settings.ApiKey));
    }
    
    public async Task<IReadOnlyCollection<T>> Get(int page, int size, IndexName index)
    {
        var response = await this._client.SearchAsync<T>(
            s =>
                s
                    .Index(index)
                    .From(page)
                    .Size(size)
        );

        return response.Documents;
    }

    public async Task<bool> Create(T log, IndexName index)
    {
        var respose = await this._client.IndexAsync<T>(log, index);

        if (respose.IsValidResponse)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
