using AutoMapper;
using crop_api_elastic_demo.Entities;
using crop_api_elastic_demo.Infra;
using crop_api_elastic_demo.ViewModels;
using Elastic.Clients.Elasticsearch;
using Newtonsoft.Json;



namespace crop_api_elastic_demo.Services;

public class CropService
{
    private readonly IMapper _mapper;

    private readonly IMongoRepository<Crop> _crop;

    private readonly IRabbitClient _client;
    
    private readonly IElasticClient<CropLog> _elasticClient;

    public CropService(IMapper mapper, IMongoRepository<Crop> crop, IRabbitClient rabbitClient, IElasticClient<CropLog> elasticClient)
    {
        _mapper = mapper;
        _crop = crop;
        _client = rabbitClient;
        _elasticClient = elasticClient;
    }

    public async Task<IReadOnlyCollection<CropLog>> GetLogs(int page, int size, IndexName index)
    {
        var documents = await this._elasticClient.Get(page, size, index);
        return documents;
    }
    
    public List<CropViewModel> Get() => _mapper.Map<List<CropViewModel>>(_crop.Get().ToList());
    
    public CropViewModel Get(string id) => _mapper.Map<CropViewModel>(_crop.Get(id));

    public async Task<CropViewModel> Create(CropViewModel crop)
    {
        var entity = new Crop(crop.Name, crop.ProgramName, crop.StartTime, crop.EndTime, crop.StartSegment, crop.EndSegment);
        _crop.Create(entity);
        var message = JsonConvert.SerializeObject(entity);
        _client.SendMessage("create_crop", message);
        var cropLog = new CropLog(entity.Id, entity.Name);
        var log = await _elasticClient.Create(cropLog, "crop-log");
        return Get(entity.Id);
    }

    public void Update(string id, CropViewModel crop)
    {
        _crop.Update(id, _mapper.Map<Crop>(crop));
    }

    public void Remove(string id) => _crop.Remove(id);

   
}