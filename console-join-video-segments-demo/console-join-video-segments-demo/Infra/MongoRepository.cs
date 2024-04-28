using console_join_video_segments_demo.Entities;
using MongoDB.Driver;

namespace console_join_video_segments_demo.Infra;

public interface IMongoRepository<T>
{
    List<T> Get();
    T Get(string id);
    T Create(T crop);
    void Update(string id, T crop);
    void Remove(String id);
}
    
public class MongoRepository<T> : IMongoRepository<T> where T: BaseEntity
{
    private readonly IMongoCollection<T> _model;

    public MongoRepository(IDatabaseSettings settings)
    {
        var client = new MongoClient(settings.ConnectionString);
        var database = client.GetDatabase(settings.DatabaseName);

        _model = database.GetCollection<T>(typeof(T).Name.ToLower());
    }

    public List<T> Get() => _model.Find<T>((active => true)).ToList();

    public T Get(string id) => _model.Find<T>(crop => crop.Id == id).FirstOrDefault();

    public T Create(T crop)
    {
        _model.InsertOne(crop);
        return crop;
    }

    public void Update(string id, T crop) => _model.ReplaceOne(crop => crop.Id == id, crop);

    public void Remove(string id) => _model.DeleteOne( crop => crop.Id == id);
}