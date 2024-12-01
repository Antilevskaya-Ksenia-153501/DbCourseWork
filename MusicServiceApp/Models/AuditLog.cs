namespace MusicService.Models;

public partial class AuditLog
{
    public int LogId { get; set; }

    public string TableName { get; set; } = null!;

    public string Operation { get; set; } = null!;

    public int? RecordId { get; set; }

    public DateTime? Timestamp { get; set; }
}
