using System;
using System.Drawing;

using Avalonia.Media.Imaging;

using SharpGraphics;
using SharpGraphics.Common.Avalonia;
#if WINDOWS
using SharpGraphics.GdiPlus;
using SharpGraphics.GdiPlus.Avalonia;
#endif
using SharpGraphics.SkiaSharp;
using SharpGraphics.SkiaSharp.Avalonia;

using StreamViewer.Models;

namespace StreamViewer.Common;

public static class GraphicsHelper
{
    public static WriteableBitmapGraphicsSurface CreateGraphicsSurface(
        this WriteableBitmap writeableBitmap, GraphicsEngine graphicsEngine) => graphicsEngine switch
        {
#if WINDOWS
            GraphicsEngine.GdiPlus => new WriteableBitmapGdipGraphicsSurface(writeableBitmap),
#endif
            GraphicsEngine.SkiaSharp => new WriteableBitmapSkiaGraphicsSurface(writeableBitmap),
            _ => throw new ArgumentException($"Unsupported graphics engine {graphicsEngine}.")
        };

    public static IBitmap CreateBitmap(int width, int height, GraphicsEngine graphicsEngine) => graphicsEngine switch
    {
#if WINDOWS
        GraphicsEngine.GdiPlus => new GdipBitmap(width, height),
#endif
        GraphicsEngine.SkiaSharp => new SkiaBitmap(width, height),
        _ => throw new ArgumentException($"Unsupported graphics engine {graphicsEngine}.")
    };

    public static Color ToColor(this Avalonia.Media.Color color) =>
        Color.FromArgb(color.A, color.R, color.G, color.B);
}
