namespace MusicService.Models;

public partial class SongGenre
{
    public int SongGenreId { get; set; }

    public int? SongIdFk { get; set; }

    public int? GenreIdFk { get; set; }

    public virtual Genre? GenreIdFkNavigation { get; set; }

    public virtual Song? SongIdFkNavigation { get; set; }
}
