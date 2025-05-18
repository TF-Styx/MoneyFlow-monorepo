using MoneyFlow.WPF.Enums;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MoneyFlow.WPF.Views.ValueConverts
{
    public class AccountManagementControlsVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is AuthenticationType mode && parameter is string targetMode)
            {
                return mode.ToString() == targetMode ? Visibility.Visible : Visibility.Collapsed;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
