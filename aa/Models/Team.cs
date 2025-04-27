using aa.Models;
using System;
using System.Collections.Generic;

namespace aa.Models;

public partial class Team
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int ProjectId { get; set; }

    public string Role { get; set; } = null!;

    public int Level { get; set; }

    public virtual Project Project { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
