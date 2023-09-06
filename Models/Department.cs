using System;
using System.Collections.Generic;

namespace ProjectManagement;

public partial class Department
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Post> Posts { get; } = new List<Post>();
}
