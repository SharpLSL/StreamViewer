using System;

using Avalonia.Markup.Xaml;

namespace StreamViewer.Common;

public class EnumBindingSourceExtension : MarkupExtension
{
    public EnumBindingSourceExtension()
    {
    }

    public EnumBindingSourceExtension(Type enumType) => EnumType = enumType;

    public Type? EnumType
    {
        get => enumType;

        set
        {
            if (value != enumType)
            {
                if (value != null)
                {
                    var enumType = Nullable.GetUnderlyingType(value) ?? value;
                    if (!enumType!.IsEnum)
                    {
                        throw new ArgumentException("Type must be for an Enum.");
                    }
                }

                enumType = value;
            }
        }
    }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        if (enumType == null)
        {
            throw new InvalidOperationException("The EnumType must be specified.");
        }

        var actualEnumType = Nullable.GetUnderlyingType(enumType) ?? enumType;
        var enumValues = Enum.GetValues(actualEnumType!);

        if (actualEnumType == enumType)
        {
            return enumValues;
        }

        var tempArray = Array.CreateInstance(actualEnumType, enumValues.Length + 1);
        enumValues.CopyTo(tempArray, 1);
        return tempArray;
    }

    private Type? enumType;
}


// References:
// https://github.com/brianlagunas/BindingEnumsInWpf
// https://brianlagunas.com/a-better-way-to-data-bind-enums-in-wpf/
// https://brianlagunas.com/localize-enum-descriptions-in-wpf/
// [Enum collection binding with CompiledBinding.](https://github.com/AvaloniaUI/Avalonia/discussions/17017)
