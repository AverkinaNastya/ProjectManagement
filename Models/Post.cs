using System;
using System.Collections.Generic;

namespace ProjectManagement;

public partial class Post
{
    public int Id { get; set; }

    public int Department { get; set; }

    public string Name { get; set; } = null!;

    public bool? UserGroup { get; set; }

    public virtual Department DepartmentNavigation { get; set; } = null!;

    public virtual ICollection<Employee> Employees { get; } = new List<Employee>();
}
