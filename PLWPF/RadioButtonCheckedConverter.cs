using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace PLWPF
{
    public class RadioButtonYesNoConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value.Equals(Enum.Parse(typeof(BE.Enums.Yes_No), parameter.ToString()));
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            return value.Equals(true) ? Enum.Parse(typeof(BE.Enums.Yes_No), parameter.ToString()) : Binding.DoNothing;
        }
    }

    public class RadioButtonEnum1Converter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value.Equals(Enum.Parse(typeof(BE.Enums.enum_1), parameter.ToString()));
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            return value.Equals(true) ? Enum.Parse(typeof(BE.Enums.enum_1), parameter.ToString()) : Binding.DoNothing;
        }
    }
}

