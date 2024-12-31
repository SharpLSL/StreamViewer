namespace StreamViewer.Common;

public class PixelUnitFactor
{
    public PixelUnitFactor(double dpi)
    {
        Inch = dpi;
        Mm = Inch / 25.4;
    }

    public const double Px = 1.0;

    public double Inch { get; }

    public double Mm { get; }
}


// References:
// https://github.com/FastReports/FastReport/blob/030a1e9fe091474abc4ee333acc85f02d2f399ea/FastReport.Base/Utils/Units.cs#L41
