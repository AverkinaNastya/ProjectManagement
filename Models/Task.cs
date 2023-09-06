using System;
using System.Collections.Generic;

namespace ProjectManagement;

public partial class Task
{
    public int Id { get; set; }

    public int Phase { get; set; }

    public string Name { get; set; } = null!;

    public int Executor { get; set; }

    public DateOnly DateComplet { get; set; }

    public int Status { get; set; }

    public int Responsible { get; set; }

    public string? Comment { get; set; }

    public virtual Employee ExecutorNavigation { get; set; } = null!;

    public virtual ICollection<Message> Messages { get; } = new List<Message>();

    public virtual Phase PhaseNavigation { get; set; } = null!;

    public virtual Employee ResponsibleNavigation { get; set; } = null!;

    public virtual Status StatusNavigation { get; set; } = null!;

    public virtual ICollection<Task> DependentTasks { get; } = new List<Task>();
    public virtual ICollection<Task> MainTasks { get; } = new List<Task>();
}
