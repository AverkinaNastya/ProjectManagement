using System;
using System.Collections.Generic;

namespace ProjectManagement;

public partial class Message
{
    public int Id { get; set; }

    public int Employee { get; set; }

    public int Task { get; set; }

    public string Text { get; set; } = null!;

    public virtual Employee EmployeeNavigation { get; set; } = null!;

    public virtual Task TaskNavigation { get; set; } = null!;
}
