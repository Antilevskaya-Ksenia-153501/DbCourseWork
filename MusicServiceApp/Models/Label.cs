namespace MusicService.Models;

public partial class Label
{
    public int LabelId { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public virtual ICollection<Artist> Artists { get; set; } = new List<Artist>();
}
