using System.IO;

using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;

using StreamViewer.Common;
using StreamViewer.Models;

namespace StreamViewer.ViewModels;

[JsonObject(MemberSerialization.OptIn)]
public partial class Settings : ObservableObject
{
    [ObservableProperty]
    [property: JsonProperty]
#if WINDOWS
    private GraphicsEngine graphicsEngine = GraphicsEngine.GdiPlus;
#else
    private GraphicsEngine graphicsEngine = GraphicsEngine.SkiaSharp;
#endif

    [ObservableProperty]
    [property: JsonProperty]
    private bool isAntialias;

    [ObservableProperty]
    [property: JsonProperty]
    private double dotsPerMm;

    [ObservableProperty]
    [property: JsonProperty]
    private Speed speed = Speed.ThreeCmPerSecond;

    [ObservableProperty]
    [property: JsonProperty]
    private Sensitivity sensitivity = Sensitivity.TwentyMicrovoltPerMm;

    [ObservableProperty]
    [property: JsonProperty]
    private Color backColor = Colors.WhiteSmoke;

    [ObservableProperty]
    [property: JsonProperty]
    private Color tickColor = Colors.Black;

    [ObservableProperty]
    [property: JsonProperty]
    private Color oneSecondGridColor = Colors.LightGray;

    [ObservableProperty]
    [property: JsonProperty]
    private Color twoHundredMillisecondGridColor = Colors.LightGray;

    [ObservableProperty]
    [property: JsonProperty]
    private Color waveColor = Colors.Black;

    [ObservableProperty]
    [property: JsonProperty]
    private double longTickLength = 8;

    [ObservableProperty]
    [property: JsonProperty]
    private double tickLength = 5;

    [ObservableProperty]
    [property: JsonProperty]
    private double shortTickLength = 3;

    public static Settings Load(string filePath)
    {
        var settings = JsonHelper.DeserializeFromFile<Settings>(filePath);
        settings ??= new Settings();
        settings.filePath = filePath;

        return settings;
    }

    public void Store(string? filePath = null)
    {
        filePath ??= this.filePath;

        if (string.IsNullOrEmpty(filePath))
        {
            throw new IOException($"Invalid file path: {filePath}.");
        }

        JsonHelper.SerializeToFile(this, filePath);
    }

    private string? filePath;
}
