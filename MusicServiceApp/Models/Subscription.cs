namespace MusicService.Models;

public partial class Subscription
{
    public int SubscriptionId { get; set; }

    public string Name { get; set; } = null!;

    public decimal Price { get; set; }

    public string Features { get; set; } = null!;

    public virtual ICollection<ListenerSubscription> ListenerSubscriptions { get; set; } = new List<ListenerSubscription>();
}
