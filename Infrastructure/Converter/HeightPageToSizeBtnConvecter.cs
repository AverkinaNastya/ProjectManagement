using System;
using System.Globalization;
using System.Windows.Data;

namespace ProjectManagement.Infrastructure.Converter
{
    class HeightPageToSizeBtnConvecter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double height = (double)value;
            if (height < 450) return 16;
            return (height / 28) > 20 ? 20 : height / 28; 
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
