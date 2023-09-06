using ProjectManagement.Models;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ProjectManagement.Infrastructure.Converter
{
    class MessageToAlignmentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Message message)
            {
                if (message.Employee == CurrentEmployee.currentEmployee.Id) return HorizontalAlignment.Right;
                else return HorizontalAlignment.Left;
            }
            else return HorizontalAlignment.Left;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
