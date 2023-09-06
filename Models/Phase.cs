using System;
using System.Collections.Generic;

namespace ProjectManagement;

public partial class Phase
{
    public int Id { get; set; }

    public int Project { get; set; }

    public string Name { get; set; } = null!;

    public virtual Project ProjectNavigation { get; set; } = null!;

    public virtual ICollection<Task> Tasks { get; } = new List<Task>();
}
