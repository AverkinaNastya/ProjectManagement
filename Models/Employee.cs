using System;
using System.Collections.Generic;

namespace ProjectManagement;

public partial class Employee
{
    public int Id { get; set; }

    public string? Surname { get; set; } = null!;

    public string? Name { get; set; } = null!;

    public string? Patronymic { get; set; }

    public DateOnly? DateBirth { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public bool Blocked { get; set; }

    public bool? New { get; set; }

    public int? Post { get; set; }

    public bool Admin { get; set; }

    public virtual Post PostNavigation { get; set; } = null!;

    public virtual ICollection<Message> Messages { get; } = new List<Message>();

    public virtual ICollection<Notification> Notifications { get; } = new List<Notification>();

    public virtual ICollection<Task> TaskExecutorNavigations { get; } = new List<Task>();

    public virtual ICollection<Task> TaskResponsibleNavigations { get; } = new List<Task>();

    public virtual ICollection<Project> Projects { get; } = new List<Project>();
}
