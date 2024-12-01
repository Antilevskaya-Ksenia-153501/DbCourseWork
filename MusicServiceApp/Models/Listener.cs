namespace MusicService.Models;

public partial class Listener
{
    public int ListenerId { get; set; }

    public int UserIdFk { get; set; }

    public string Firstname { get; set; } = null!;

    public string Lastname { get; set; } = null!;

    public DateOnly DateOfBirth { get; set; }

    public string Email { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public virtual ListenerSubscription? ListenerSubscription { get; set; }

    public virtual PaymentMethod? PaymentMethod { get; set; }

    public virtual ICollection<Playlist> Playlists { get; set; } = new List<Playlist>();

    public virtual User UserIdFkNavigation { get; set; } = null!;
}
