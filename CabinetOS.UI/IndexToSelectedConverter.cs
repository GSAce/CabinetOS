using System;
using System.Globalization;
using System.Windows.Data;

namespace CabinetOS.UI
{
    public class IndexToSelectedConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 2)
                return false;

            if (values[0] is int itemIndex && values[1] is int selectedIndex)
                return itemIndex == selectedIndex;

            return false;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}