namespace MusicService.Models;

public partial class PlaylistSong
{
    public int PlaylistSong1 { get; set; }

    public int? SongIdFk { get; set; }

    public int? PlaylistIdFk { get; set; }

    public virtual Playlist? PlaylistIdFkNavigation { get; set; }

    public virtual Song? SongIdFkNavigation { get; set; }
}
