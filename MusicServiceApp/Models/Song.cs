namespace MusicService.Models;

public partial class Song
{
    public int SongId { get; set; }

    public int? ArtistIdFk { get; set; }

    public int? AlbumIdFk { get; set; }

    public string Title { get; set; } = null!;

    public string FilePath { get; set; } = null!;

    public int? Duration { get; set; }

    public virtual Album? AlbumIdFkNavigation { get; set; }

    public virtual Artist? ArtistIdFkNavigation { get; set; }

    public virtual ICollection<PlaylistSong> PlaylistSongs { get; set; } = new List<PlaylistSong>();

    public virtual ICollection<SongGenre> SongGenres { get; set; } = new List<SongGenre>();
}
