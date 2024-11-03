using System;
using System.Globalization;

using Avalonia.Data;
using Avalonia.Data.Converters;

using SharpCommon;

namespace StreamViewer.Converters;

public class EnumDescriptionConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is Enum enumValue)
        {
            var description = enumValue.GetDescription();
            return description ?? enumValue.ToString();
        }

        return BindingNotification.UnsetValue;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotImplementedException();
}
