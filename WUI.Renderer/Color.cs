namespace WUI.Renderer;

public class Color
{
    public byte R0255 { get; private set; }
    public float R01 => R0255 / 255f;
    public byte G0255 { get; private set; }
    public float G01 => G0255 / 255f;
    public byte B0255 { get; private set; }
    public float B01 => B0255 / 255f;
    public byte A0255 { get; private set; }
    public float A01 => A0255 / 255f;
    
    public Color(byte r, byte g, byte b, byte a = 255)
    {
        R0255 = r;
        G0255 = g;
        B0255 = b;
        A0255 = a;
    }
    
    public static Color FromHex(string rrggbbaa)
    {
        var hex = rrggbbaa
            .Trim()
            .Replace("#", "");

        if (hex.Length != 6 && hex.Length != 8)
        {
            throw new ArgumentException("Hex string must be in format RRGGBB or RRGGBBAA");
        }

        var r16 = hex.Substring(0, 2);
        var g16 = hex.Substring(2, 2);
        var b16 = hex.Substring(4, 2);

        var r = byte.Parse(r16, System.Globalization.NumberStyles.HexNumber);
        var g = byte.Parse(g16, System.Globalization.NumberStyles.HexNumber);
        var b = byte.Parse(b16, System.Globalization.NumberStyles.HexNumber);
        var a = (byte)255;

        if (hex.Length == 8)
        {
            var a16 = hex.Substring(6, 2);

            a = byte.Parse(a16, System.Globalization.NumberStyles.HexNumber);
        }

        return new Color(r, g, b, a);
    }
}