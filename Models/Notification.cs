using System;
using System.Collections.Generic;

namespace ProjectManagement;

public partial class Notification
{
    public int Id { get; set; }

    public int Employee { get; set; }

    public string Text { get; set; } = null!;

    public DateOnly Date { get; set; }

    public bool? New { get; set; }

    public virtual Employee EmployeeNavigation { get; set; } = null!;
}
