namespace crop_api_elastic_demo.Entities;

public class CropLog
{
    public CropLog(string Id, string Nome)
    {
        this.Id = Id;
        this.Nome = Nome;
        this.DataDoCrop = DateTime.Now;
    }
    public string Id { get; private set; }
    public string Nome { get; private set; }
    public DateTime DataDoCrop { get; private set; }
}