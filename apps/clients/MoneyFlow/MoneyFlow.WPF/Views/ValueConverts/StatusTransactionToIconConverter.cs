using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace MoneyFlow.WPF.Views.ValueConverts
{
    internal class StatusTransactionToIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string transactionName)
            {
                return transactionName switch
                {
                    "Прибыль" => "↓",
                    "Расход" => "↑",
                    _ => "–",
                };
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
