namespace StreamViewer.Common;

public static class UnitConverter
{
    public static double GetPixelsPerMm(double dpi) =>
        new PixelUnitFactor(dpi).Mm;

    public static double MmToPx(double mm, double dpi) =>
        mm * GetPixelsPerMm(dpi);

    public static double PxToMm(double px, double dpi) =>
        px / GetPixelsPerMm(dpi);
}
