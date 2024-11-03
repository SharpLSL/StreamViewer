using System.IO;

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
    private Speed speed = Speed.ThreeCmPerSecond;

    [ObservableProperty]
    [property: JsonProperty]
    private Sensitivity sensitivity = Sensitivity.TwentyMicrovoltPerMm;

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
