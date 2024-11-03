using System;
using System.Diagnostics;
using System.Drawing;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using StreamViewer.Common;
using StreamViewer.Models;

namespace StreamViewer.ViewModels;

public partial class WaveformControlViewModel : ViewModelBase
{
    [ObservableProperty]
    private WriteableBitmap? bitmap;

    [RelayCommand]
    private void SizeChanged(SizeChangedEventArgs e)
    {
        Trace.WriteLine($"Size changed: {e.NewSize.Width}x{e.NewSize.Height}.");

        if (e.NewSize.Width > 0 && e.NewSize.Height > 0)
        {
            var newBitmap = new WriteableBitmap(
                new PixelSize(
                    Convert.ToInt32(e.NewSize.Width),
                    Convert.ToInt32(e.NewSize.Height)),
                new Vector(96, 96), // TODO:
                PixelFormat.Bgra8888,
                AlphaFormat.Premul);
            Bitmap = newBitmap;

            using var surface = Bitmap.CreateGraphicsSurface(GraphicsEngine.GdiPlus);
            using var gfx = surface.CreateGraphics();

            gfx.Clear(Color.SkyBlue);
            gfx.DrawLine(0, 0, 100, 100, Color.Black);
        }
    }
}
