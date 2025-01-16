namespace BoH.GUI.Converters
{
    using Avalonia.Data.Converters;
    using System;
    using System.Globalization;
    using BoH.Interfaces;

    /// <summary>
    /// Конвертер, преобразующий объект в строковое представление для отображения.
    /// Например, преобразует юнита или препятствие в соответствующую строку для визуализации в UI.
    /// </summary>
    public class ContentToDisplayConverter : IValueConverter
    {
        /// <summary>
        /// Преобразует объект в строковое представление для отображения в UI.
        /// </summary>
        /// <param name="value">Объект, который нужно преобразовать (например, юнит или препятствие).</param>
        /// <param name="targetType">Тип, в который нужно преобразовать значение. Обычно это строка.</param>
        /// <param name="parameter">Параметры преобразования (не используется в данном конвертере).</param>
        /// <param name="culture">Культура, используемая для преобразования.</param>
        /// <returns>Строковое представление объекта или пустая строка, если объект не задан.</returns>
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is IUnit unit)
            {
                return unit.Icon.ToString(); // Возвращает имя типа юнита
            }

            if (value is IObstacle obstacle)
            {
                return obstacle.Icon.ToString(); // Возвращает символ препятствия
            }

            return string.Empty; // Пустая строка, если объект отсутствует
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
