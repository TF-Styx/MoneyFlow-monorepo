using System.Globalization;
using System.Windows.Data;

namespace MoneyFlow.WPF.Views.ValueConverts
{
    public class DateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime date)
            {
                string formattedDate = date.ToString("dddd, d MMMM yyyy г.", new CultureInfo("ru-RU")); /*"dddd, d MMMM yyyy г. HH:mm"*/
                return char.ToUpper(formattedDate[0]) + formattedDate.Substring(1);
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
