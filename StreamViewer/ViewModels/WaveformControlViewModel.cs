using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.VisualTree;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;

using SharpGraphics;

using StreamViewer.Common;

namespace StreamViewer.ViewModels;

public partial class WaveformControlViewModel : ViewModelBase
{
    public WaveformControlViewModel(
        Settings settings,
        ILogger<WaveformControlViewModel> logger)
    {
        this.settings = settings;
        this.settings.PropertyChanged += Settings_PropertyChanged;

        this.logger = logger;
    }

    [ObservableProperty]
    private WriteableBitmap? bitmap;

    private void Settings_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (scaledWidth <= 0 || scaledHeight <= 0)
        {
            return;
        }

        var createBitmaps = false;
        var recalculateDotsPer100ms = false;
        var drawBackground = false;

        switch (e.PropertyName)
        {
            case nameof(Settings.GraphicsEngine):
                createBitmaps = true;
                break;
            case nameof(Settings.IsAntialias):
                drawBackground = true;
                break;
            case nameof(Settings.DotsPerMm):
                recalculateDotsPer100ms = true;
                break;
            case nameof(Settings.Speed):
                recalculateDotsPer100ms = true;
                break;
            case nameof(Settings.Sensitivity):
                break;
            case nameof(Settings.BackColor):
                drawBackground = true;
                break;
            case nameof(Settings.TickColor):
                drawBackground = true;
                break;
            case nameof(Settings.OneSecondGridColor):
                drawBackground = true;
                break;
            case nameof(Settings.TwoHundredMillisecondGridColor):
                drawBackground = true;
                break;
            case nameof(Settings.WaveColor):
                break;
            default:
                break;
        }

        if (createBitmaps)
        {
            CreateBitmaps();

            drawBackground = true;
        }

        if (recalculateDotsPer100ms)
        {
            RecalculateDotsPer100ms();

            drawBackground = true;
        }

        if (drawBackground)
        {
            DrawBackground();
        }


    }

    [RelayCommand]
    private void Initialized(UserControl userControl)
    {
        this.userControl ??= userControl;

        SetScaleFactor(userControl);
    }

    [RelayCommand]
    private void Loaded(UserControl userControl)
    {
        this.userControl ??= userControl;

        SetScaleFactor(userControl);
    }

    [RelayCommand]
    private void SizeChanged(SizeChangedEventArgs e)
    {
        logger.SizeChanged(e.NewSize.Width, e.NewSize.Height);

        SetScaleFactor(userControl!);

        scaledWidth = Convert.ToInt32(e.NewSize.Width * scaleFactor);
        scaledHeight = Convert.ToInt32(e.NewSize.Height * scaleFactor);

        if (scaledWidth > 0 && scaledHeight > 0)
        {
            CreateBitmaps();

            DrawBackground();

            using var surface = Bitmap!.CreateGraphicsSurface(settings.GraphicsEngine);
            using var gfx = surface.CreateGraphics();

            var rect = new Rectangle(0, 0, scaledWidth, scaledHeight);
            gfx.DrawBitmap(backgroundBitmap, rect, rect);
        }
    }

    private void SetScaleFactor(UserControl userControl)
    {
        Debug.Assert(userControl != null);

        scaleFactor = userControl?.GetVisualRoot()?.RenderScaling ?? 1;

        if (settings.DotsPerMm > 0)
        {
            dotsPerMm = settings.DotsPerMm;
        }
        else
        {
            dotsPerMm = UnitConverter.GetPixelsPerMm(scaleFactor * 96.0);
        }

        RecalculateDotsPer100ms();
    }

    private void CreateBitmaps()
    {
        if (backgroundBitmap == null ||
            backgroundBitmap.Width < scaledWidth ||
            backgroundBitmap.Height < scaledHeight)
        {
            backgroundBitmap = GraphicsHelper.CreateBitmap(
                scaledWidth, scaledHeight, settings.GraphicsEngine);
            //backgroundBitmap.SetResolution(
            //    Convert.ToSingle(96 * scaleFactor),
            //    Convert.ToSingle(96 * scaleFactor));
        }

        var newBitmap = new WriteableBitmap(
             new PixelSize(scaledWidth, scaledHeight),
             new Vector(96 * scaleFactor, 96 * scaleFactor),
             PixelFormat.Bgra8888,
             AlphaFormat.Premul);
        Bitmap = newBitmap;
    }

    private void RecalculateDotsPer100ms()
    {
        var dotsPerSecond = dotsPerMm * (int)settings.Speed;
        dotsPer100ms = dotsPerSecond / 10.0;
    }

    private void DrawBackground()
    {
        using var gfx = backgroundBitmap!.CreateGraphics();

        DrawGrid(gfx);
    }

    private void DrawGrid(IGraphics gfx)
    {
        gfx.IsAntialias = settings.IsAntialias;

        gfx.Clear(settings.BackColor.ToColor());

        var count = 0;
        for (double x = 0; x < scaledWidth; count += 1)
        {
            x = count * dotsPer100ms;

            if (count % 10 == 0)
            {
                gfx.DrawLine(
                    (float)x,
                    0,
                    (float)x,
                    scaledHeight,
                    settings.OneSecondGridColor.ToColor());
            }
            else if (count % 2 == 0)
            {
                gfx.DrawLine(
                    (float)x,
                    0,
                    (float)x,
                    scaledHeight,
                    settings.TwoHundredMillisecondGridColor.ToColor());
            }

            double tickLength;
            if (count % 10 == 0)
            {
                tickLength = settings.LongTickLength;
            }
            else if (count % 5 == 0)
            {
                tickLength = settings.TickLength;
            }
            else
            {
                tickLength = settings.ShortTickLength;
            }

            gfx.DrawLine(
                (float)x,
                0,
                (float)x,
                (float)tickLength,
                settings.TickColor.ToColor());

            gfx.DrawLine(
                (float)x,
                scaledHeight,
                (float)x,
                (float)(scaledHeight - tickLength),
                settings.TickColor.ToColor());
        }
    }

    private readonly Settings settings;
    private readonly ILogger<WaveformControlViewModel> logger;

    private UserControl? userControl;
    private double scaleFactor = 1.0;
    private double dotsPerMm;
    private double dotsPer100ms;
    private int scaledWidth;
    private int scaledHeight;
    private IBitmap? backgroundBitmap;
}


// References:
// [Event triggered after Window and UserControl is fully displayed?](https://github.com/AvaloniaUI/Avalonia/discussions/15253)
