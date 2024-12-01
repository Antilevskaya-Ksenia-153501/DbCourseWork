namespace MusicService.Models;

public partial class PaymentMethod
{
    public int PaymentMethodId { get; set; }

    public int ListenerIdFk { get; set; }

    public string CardNumber { get; set; } = null!;

    public DateOnly ExpiryDate { get; set; }

    public string Cvv { get; set; } = null!;

    public virtual Listener ListenerIdFkNavigation { get; set; } = null!;
}
