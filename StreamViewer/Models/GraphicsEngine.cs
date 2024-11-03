using System.ComponentModel;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using SharpCommon;

namespace StreamViewer.Models;

[JsonConverter(typeof(StringEnumConverter))]
[TypeConverter(typeof(EnumDescriptionTypeConverter))]
public enum GraphicsEngine
{
#if WINDOWS
    [Description("GDI+")]
    GdiPlus = 0,
#endif

    [Description("Skia")]
    SkiaSharp = 1,
}
