using System.ComponentModel;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using SharpCommon;

namespace StreamViewer.Models;

[JsonConverter(typeof(StringEnumConverter))]
[TypeConverter(typeof(EnumDescriptionTypeConverter))]
public enum Sensitivity
{
    [Description("0 µV/mm")]
    Off = 0,

    [Description("1 µV/mm")]
    OneMicrovoltPerMm = 1,

    [Description("2 µV/mm")]
    TwoMicrovoltPerMm = 2,

    [Description("3 µV/mm")]
    ThreeMicrovoltPerMm = 3,

    [Description("4 µV/mm")]
    FourMicrovoltPerMm = 4,

    [Description("5 µV/mm")]
    FiveMicrovoltPerMm = 5,

    [Description("6 µV/mm")]
    SixMicrovoltPerMm = 6,

    [Description("7 µV/mm")]
    SevenMicrovoltPerMm = 7,

    [Description("8 µV/mm")]
    EightMicrovoltPerMm = 8,

    [Description("9 µV/mm")]
    NineMicrovoltPerMm = 9,

    [Description("10 µV/mm")]
    TenMicrovoltPerMm = 10,

    [Description("12 µV/mm")]
    TwelveMicrovoltPerMm = 12,

    [Description("15 µV/mm")]
    FifteenMicrovoltPerMm = 15,

    [Description("20 µV/mm")]
    TwentyMicrovoltPerMm = 20,

    [Description("30 µV/mm")]
    ThirtyMicrovoltPerMm = 30,

    [Description("40 µV/mm")]
    FortyMicrovoltPerMm = 40,

    [Description("50 µV/mm")]
    FiftyMicrovoltPerMm = 50,

    [Description("60 µV/mm")]
    SixtyMicrovoltPerMm = 60,

    [Description("80 µV/mm")]
    EightyMicrovoltPerMm = 80,

    [Description("100 µV/mm")]
    OneHundredMicrovoltPerMm = 100,

    [Description("120 µV/mm")]
    OneHundredTwentyMicrovoltPerMm = 120,

    [Description("150 µV/mm")]
    OneHundredFiftyMicrovoltPerMm = 150,

    [Description("200 µV/mm")]
    TwoHundredMicrovoltPerMm = 200,

    [Description("300 µV/mm")]
    ThreeHundredMicrovoltPerMm = 300,

    [Description("400 µV/mm")]
    FourHundredMicrovoltPerMm = 400,

    [Description("500 µV/mm")]
    FiveHundredMicrovoltPerMm = 500,

    [Description("600 µV/mm")]
    SixHundredMicrovoltPerMm = 600,

    [Description("800 µV/mm")]
    EightHundredMicrovoltPerMm = 800,

    [Description("1 mV/mm")]
    OneMillivoltPerMm = 1000,

    [Description("2 mV/mm")]
    TwoMillivoltPerMm = 2000,

    [Description("5 mV/mm")]
    FiveMillivoltPerMm = 5000,

    [Description("10 mV/mm")]
    TenMillivoltPerMm = 10000,

    [Description("20 mV/mm")]
    TwentyMillivoltPerMm = 20000,

    [Description("50 mV/mm")]
    FiftyMillivoltPerMm = 50000,

    [Description("100 mV/mm")]
    OneHundredMillivoltPerMm = 100000,

    [Description("200 mV/mm")]
    TwoHundredMillivoltPerMm = 200000,

    [Description("500 mV/mm")]
    FiveHundredMillivoltPerMm = 500000,

    [Description("1 V/mm")]
    OneThousandMillivoltPerMm = 1000000,

    [Description("2 V/mm")]
    TwoThousandMillivoltPerMm = 2000000,

    [Description("5 V/mm")]
    FiveThousandMillivoltPerMm = 5000000
}
