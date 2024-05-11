namespace crop_api_elastic_demo.Entities;

public class CropLog
{

    public CropLog(string Id, string Name)
    {
        this.Id = Id;
        this.Name = Name;
        this.DateCrop = DateTime.Now;
    }
    
    public string Id { get; private set; }
    public string Name { get; private set; }
    public DateTime DateCrop { get; private set; }
}