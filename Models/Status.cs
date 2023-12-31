﻿using System;
using System.Collections.Generic;

namespace ProjectManagement;

public partial class Status
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Task> Tasks { get; } = new List<Task>();
}
