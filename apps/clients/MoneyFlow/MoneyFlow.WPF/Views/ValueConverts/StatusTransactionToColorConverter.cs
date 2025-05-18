using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace MoneyFlow.WPF.Views.ValueConverts
{
    internal class StatusTransactionToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string transactionName)
            {
#pragma warning disable CS8603 // Возможно, возврат ссылки, допускающей значение NULL.
                return transactionName switch
                {
                    "Прибыль" => (Brush)new BrushConverter().ConvertFrom("#FF1FA818"),
                    "Расход" => (Brush)new BrushConverter().ConvertFrom("#FFC41D1D"),
                    _ => (Brush)new BrushConverter().ConvertFrom("#FF020101"),
                };
#pragma warning restore CS8603 // Возможно, возврат ссылки, допускающей значение NULL.
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
