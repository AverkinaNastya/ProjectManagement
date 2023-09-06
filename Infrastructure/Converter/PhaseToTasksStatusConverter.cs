using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace ProjectManagement.Infrastructure.Converter
{
    class PhaseToTasksStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int statusId;
            if (value is List<Task> tasks && Int32.TryParse(parameter.ToString(), out statusId))
            {
                using (ProjectManagementContext db = new ())
                {
                    return statusId switch
                    {
                        1 => tasks.Where(e => e.Status == 1).OrderBy(e => e.DateComplet).ToList(),
                        2 => tasks.Where(e => e.Status == 2).OrderBy(e => e.DateComplet).ToList(),
                        3 => tasks.Where(e => e.Status == 3).OrderBy(e => e.DateComplet).ToList(),
                        _ => null!,
                    };
                }
            }
            else
            {
                return null!;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
