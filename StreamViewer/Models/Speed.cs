using System.ComponentModel;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SharpCommon;

namespace StreamViewer.Models;

[JsonConverter(typeof(StringEnumConverter))]
[TypeConverter(typeof(EnumDescriptionTypeConverter))]
public enum Speed
{
    [Description("5 mm/s")]
    FiveMmPerSecond = 5,

    [Description("1 cm/s")]
    OneCmPerSecond = 10,

    [Description("1.5 cm/s")]
    OnePointFiveCmPerSecond = 15,

    [Description("2 cm/s")]
    TwoCmPerSecond = 20,

    [Description("3 cm/s")]
    ThreeCmPerSecond = 30,

    [Description("4.5 cm/s")]
    FourPointFiveCmPerSecond = 45,

    [Description("6 cm/s")]
    SixCmPerSecond = 60,

    [Description("9 cm/s")]
    NineCmPerSecond = 90,

    [Description("12 cm/s")]
    TwelveCmPerSecond = 120,

    [Description("15 cm/s")]
    FifteenCmPerSecond = 150,

    [Description("18 cm/s")]
    EighteenCmPerSecond = 180,
}
