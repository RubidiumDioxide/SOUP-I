using System;
using System.Collections.Generic;

namespace aa.Models;

public partial class Repository
{
    public int Id { get; set; }

    public string GithubName { get; set; } = null!;

    public string GithubCreator { get; set; } = null!;

    public virtual Project IdNavigation { get; set; } = null!;
}
