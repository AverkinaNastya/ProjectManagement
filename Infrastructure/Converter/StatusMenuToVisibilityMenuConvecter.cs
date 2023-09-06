using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ProjectManagement.Infrastructure.Converter
{
    class StatusMenuToVisibilityMenuConvecter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool status)
            {
                return status ? Visibility.Visible : Visibility.Collapsed;
            }

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
