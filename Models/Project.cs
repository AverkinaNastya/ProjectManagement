using System;
using System.Collections.Generic;

namespace ProjectManagement;

public partial class Project
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public DateOnly StartDate { get; set; }

    public string? Comment { get; set; }

    public bool Completed { get; set; }

    public DateOnly? CompletDate { get; set; }

    public virtual ICollection<Phase> Phases { get; } = new List<Phase>();

    public virtual ICollection<Employee> Employees { get; } = new List<Employee>();
}
