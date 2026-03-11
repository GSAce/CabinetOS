using System;
using System.Globalization;
using System.Windows.Data;

namespace CabinetOS.UI
{
    public class GameCountToLabelConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 1 || values[0] == null)
                return "0 Games";

            if (values[0] is int count)
                return count == 1 ? "1 Game" : $"{count} Games";

            return "0 Games";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}