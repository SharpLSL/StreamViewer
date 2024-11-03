using System;

using Avalonia.Media.Imaging;

using SharpGraphics.Common.Avalonia;
#if WINDOWS
using SharpGraphics.GdiPlus.Avalonia;
#endif
using SharpGraphics.SkiaSharp.Avalonia;

using StreamViewer.Models;

namespace StreamViewer.Common;

public static class WriteableBitmapHelper
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
}
