using aa.Models;
using System;
using System.Collections.Generic;

namespace aa.Models;

public partial class Task
{
    public int Id { get; set; }

    public int ProjectId { get; set; }

    public int CreatorId { get; set; }

    public int AssigneeId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public bool IsComplete { get; set; }

    public virtual ICollection<Action> Actions { get; set; } = new List<Action>();

    public virtual User Assignee { get; set; } = null!;

    public virtual User Creator { get; set; } = null!;

    public virtual Project Project { get; set; } = null!;
}
