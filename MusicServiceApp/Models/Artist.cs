namespace MusicService.Models;

public partial class Artist
{
    public int ArtistId { get; set; }

    public int UserIdFk { get; set; }

    public int? LabelIdFk { get; set; }

    public string Name { get; set; } = null!;

    public string? Biography { get; set; }

    public string ImagePath { get; set; } = null!;

    public virtual ICollection<Album> Albums { get; set; } = new List<Album>();

    public virtual Label? LabelIdFkNavigation { get; set; }

    public virtual ICollection<Song> Songs { get; set; } = new List<Song>();

    public virtual User UserIdFkNavigation { get; set; } = null!;
}
