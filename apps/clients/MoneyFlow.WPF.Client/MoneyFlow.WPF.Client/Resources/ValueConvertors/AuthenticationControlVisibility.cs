using MoneyFlow.WPF.Client.Enums;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MoneyFlow.WPF.Client.Resources.ValueConvertors
{
    public class AuthenticationControlVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Controls controls && parameter is Controls targetMode)
                return controls == targetMode ? Visibility.Visible : Visibility.Collapsed;
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
