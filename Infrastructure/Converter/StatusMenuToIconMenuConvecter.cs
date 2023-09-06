using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ProjectManagement.Infrastructure.Converter
{
    class StatusMenuToIconMenuConvecter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool status)
            {
                return status ? "Solid_GripLinesVertical" : "Solid_GripLines";
            }

            return "Solid_GripLines";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
