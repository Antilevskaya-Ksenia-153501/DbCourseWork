namespace MusicService.Models;

public partial class Album
{
    public int AlbumId { get; set; }

    public int? ArtistIdFk { get; set; }

    public string Title { get; set; } = null!;

    public DateOnly ReleaseDate { get; set; }

    public string? CoverImagePath { get; set; }

    public int? SongCount { get; set; }

    public int? TotalDuration { get; set; }

    public virtual Artist? ArtistIdFkNavigation { get; set; }

    public virtual ICollection<Song> Songs { get; set; } = new List<Song>();
}
