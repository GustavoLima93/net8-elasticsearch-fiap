namespace console_join_video_segments_demo.Entities;

public class CropLog
{
    public CropLog(string Id, string Nome, string ffmpeg, bool join)
    {
        this.Id = Id;
        this.Nome = Nome;
        this.Ffmpeg = ffmpeg;
        this.Join = join;
        this.DataDoJoin = DateTime.Now;
    }
    public string Id { get; private set; }
    public string Nome { get; private set; }
    
    public string Ffmpeg { get; private set; }
    public bool Join { get; private set; }
    public DateTime DataDoJoin { get; private set; }
}