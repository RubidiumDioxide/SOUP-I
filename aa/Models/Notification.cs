using aa.Models;
using System;
using System.Collections.Generic;

namespace aa.Models;

public partial class Notification
{
    public int Id { get; set; }

    public int ProjectId { get; set; }

    public int SenderId { get; set; }

    public int ReceiverId { get; set; }

    public string Role { get; set; } = null!;

    public string Type { get; set; } = null!;

    public virtual Project Project { get; set; } = null!;

    public virtual User Receiver { get; set; } = null!;

    public virtual User Sender { get; set; } = null!;
}
