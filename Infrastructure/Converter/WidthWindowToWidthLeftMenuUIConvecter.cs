using System;
using System.Globalization;
using System.Windows.Data;

namespace ProjectManagement.Infrastructure.Converter
{
    class WidthWindowToWidthLeftMenuUIConvecter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double height)
            {
                if (parameter.ToString() == "FontSize")
                {
                    if (height <= 450) return 18;
                    else return (height / 25) > 26 ? 26 : (height / 25);
                }
                if (parameter.ToString() == "WidthIcon")
                {
                    if (height <= 450) return 26;
                    else return (height / 17.3) > 36 ? 36 : (height / 17.3);
                }
                if (parameter.ToString() == "WidthMainIcon")
                {
                    if (height <= 450) return 32;
                    else return (height / 14) > 42 ? 42 : (height / 14);
                }

                return 0;
            }
            else
                return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
