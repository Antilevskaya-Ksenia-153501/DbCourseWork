namespace MusicService.Models;

public partial class Genre
{
    public int GenreId { get; set; }

    public string Type { get; set; } = null!;

    public virtual ICollection<SongGenre> SongGenres { get; set; } = new List<SongGenre>();
}
