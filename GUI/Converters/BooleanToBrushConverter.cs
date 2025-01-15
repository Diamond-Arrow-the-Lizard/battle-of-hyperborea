namespace BoH.GUI.Converters
{
    using Avalonia.Data.Converters;
    using Avalonia.Media;
    using System;
    using System.Globalization;

    /// <summary>
    /// Конвертер, преобразующий логическое значение (true/false) в объект типа <see cref="Brush"/>.
    /// Используется для изменения цвета фона в зависимости от состояния (например, доступности).
    /// </summary>
    public class BooleanToBrushConverter : IValueConverter
    {
        /// <summary>
        /// Преобразует логическое значение в соответствующий объект <see cref="Brush"/>.
        /// </summary>
        /// <param name="value">Логическое значение, которое нужно преобразовать.</param>
        /// <param name="targetType">Тип, в который нужно преобразовать значение. Обычно <see cref="Brush"/>.</param>
        /// <param name="parameter">Параметры преобразования (не используется в данном конвертере).</param>
        /// <param name="culture">Культура, используемая для преобразования.</param>
        /// <returns>Объект типа <see cref="Brush"/>, соответствующий значению <paramref name="value"/>.</returns>
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is bool isAccessible)
            {
                return isAccessible ? Brushes.LightGreen : Brushes.DarkGray;
            }
            return Brushes.Transparent;
        }

        /// <summary>
        /// Метод, не реализованный в этом конвертере, так как преобразование в обратном направлении не требуется.
        /// </summary>
        /// <param name="value">Значение, которое нужно преобразовать обратно.</param>
        /// <param name="targetType">Тип, в который нужно преобразовать значение.</param>
        /// <param name="parameter">Параметры преобразования.</param>
        /// <param name="culture">Культура, используемая для преобразования.</param>
        /// <returns>Метод не реализован, вызывает исключение.</returns>
        /// <exception cref="NotImplementedException">Метод не поддерживает преобразование назад.</exception>
        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
