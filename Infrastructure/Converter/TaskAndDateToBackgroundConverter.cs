using Microsoft.EntityFrameworkCore;
using ProjectManagement.Infrastructure.Classes;
using System;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;

namespace ProjectManagement.Infrastructure.Converter
{
    class TaskAndDateToBackgroundConverter : IMultiValueConverter
    {
        public object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value[1] is Task task1 && value[0] is DataGridCell cell && value[2] is DateOnly date)
            {
                date = date.AddDays(cell.TabIndex - 6);
                using (ProjectManagementContext db = new ())
                {
                    db.Phases.Load();
                    db.Projects.Load();
                    db.Tasks.Load();

                    Task task = db.Tasks.Include(e => e.MainTasks).Single(e => e.Id == (task1.Id));
                    DateOnly DateStart = (task.MainTasks.Count == 0) ? 
                        db.Projects.Where(e => e.Phases.Contains(task.PhaseNavigation)).Single().StartDate : 
                        FerialDays.AddDay(task.MainTasks.Max(e => e.DateComplet), 1);
                    if (date >= DateStart && date <= task.DateComplet) 
                        return (new System.Windows.Media.BrushConverter()).ConvertFromString("#895164")!;
                    else return System.Windows.Media.Brushes.Transparent;
                }
            }
            return System.Windows.Media.Brushes.Transparent;
        }

        public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
