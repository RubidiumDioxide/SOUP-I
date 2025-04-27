using aa.Models;
using System;
using System.Collections.Generic;

namespace aa.Models;

public partial class Project
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public int Creator { get; set; }

    public bool IsComplete { get; set; }

    public DateTime DateBegan { get; set; }

    public DateTime? DateFinished { get; set; }

    public DateTime? DateDeadline { get; set; }

    public bool IsPrivate { get; set; }

    public virtual ICollection<Action> Actions { get; set; } = new List<Action>();

    public virtual User CreatorNavigation { get; set; } = null!;

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual Repository? Repository { get; set; }

    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();

    public virtual ICollection<Team> Teams { get; set; } = new List<Team>();
}
