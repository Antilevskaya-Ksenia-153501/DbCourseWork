namespace MusicService.Models;

public partial class ListenerSubscription
{
    public int ListenerSubscriptionId { get; set; }

    public int SubscriptionIdFk { get; set; }

    public int ListenerIdFk { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public virtual Listener ListenerIdFkNavigation { get; set; } = null!;

    public virtual Subscription SubscriptionIdFkNavigation { get; set; } = null!;
}
