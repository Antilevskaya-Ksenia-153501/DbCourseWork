namespace MusicService.Models;

public partial class User
{
    public int UserId { get; set; }

    public int RoleIdFk { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public virtual Artist? Artist { get; set; }

    public virtual Listener? Listener { get; set; }

    public virtual Role RoleIdFkNavigation { get; set; } = null!;
}
