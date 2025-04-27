using aa.Models;
using System;
using System.Collections.Generic;

namespace aa.Models;

public partial class Action
{
    public int Id { get; set; }

    public int ProjectId { get; set; }

    public int ActorId { get; set; }

    public int TaskId { get; set; }

    public string Description { get; set; } = null!;

    public string? Commit { get; set; }

    public DateTime Date { get; set; }

    public virtual User Actor { get; set; } = null!;

    public virtual Project Project { get; set; } = null!;

    public virtual Task Task { get; set; } = null!;
}
