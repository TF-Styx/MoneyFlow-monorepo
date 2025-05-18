using System.Globalization;
using System.Windows.Data;

namespace MoneyFlow.WPF.Views.ValueConverts
{
    internal class DecimalToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Проверяем, что значение является decimal
            if (value is decimal decimalValue)
            {
                // Создаем объект NumberFormatInfo для настройки форматирования
                NumberFormatInfo nfi = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();

                // Устанавливаем разделитель групп разрядов (тысяч) - пробел
                nfi.NumberGroupSeparator = " ";
                // Устанавливаем десятичный разделитель - запятая
                nfi.NumberDecimalSeparator = ",";
                // Устанавливаем количество знаков после запятой
                nfi.NumberDecimalDigits = 2;

                try
                {
                    // Форматируем число с использованием наших настроек
                    // "N" - стандартный числовой формат, который использует NumberFormatInfo
                    return decimalValue.ToString("N", nfi);
                }
                catch (FormatException)
                {
                    // В случае ошибки форматирования, вернуть исходное значение или пустую строку
                    return value.ToString();
                }
            }

            // Если значение не decimal или null, вернуть пустую строку или null
            return string.Empty; // или return null; или return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
