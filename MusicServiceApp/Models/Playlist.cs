namespace MusicService.Models;

public partial class Playlist
{
    public int PlaylistId { get; set; }

    public int ListenerIdFk { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public string? CoverImagePath { get; set; }

    public int? SongCount { get; set; }

    public int? TotalDuration { get; set; }

    public virtual Listener ListenerIdFkNavigation { get; set; } = null!;

    public virtual ICollection<PlaylistSong> PlaylistSongs { get; set; } = new List<PlaylistSong>();
}
