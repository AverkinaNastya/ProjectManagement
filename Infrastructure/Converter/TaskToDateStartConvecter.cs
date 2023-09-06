using Microsoft.EntityFrameworkCore;
using ProjectManagement.Infrastructure.Classes;
using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace ProjectManagement.Infrastructure.Converter
{
    internal class TaskToDateStartConvecter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Task)
            {
                using (ProjectManagementContext db = new ProjectManagementContext())
                {
                    db.Tasks.Load();
                    db.Projects.Load();
                    db.Phases.Load();
                    Task task = db.Tasks.Include(e => e.MainTasks).Single(e => e.Id == ((Task)value).Id);
                    if (task.MainTasks.Count != 0)
                    {
                        return FerialDays.AddDay(task.MainTasks.Max(e => e.DateComplet), 1);
                    }
                    else
                    {
                        return db.Projects.Single(e => e.Phases.Contains(task.PhaseNavigation)).StartDate;
                    }
                }
            }
            else return new DateOnly();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
