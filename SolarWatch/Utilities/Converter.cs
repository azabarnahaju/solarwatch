using System.Globalization;

namespace SolarWatch.Utilities;

public static class Converter
{
    public static string ConvertDoubleFormat(double num)
    {
        NumberFormatInfo nfi = new NumberFormatInfo();
        nfi.NumberDecimalSeparator = ".";
        return num.ToString(nfi);
    }

    public static DateTime UtcToDateTime(string dateAsString)
    {
        DateTimeOffset dateTimeOffset = DateTimeOffset.Parse(dateAsString);
        return dateTimeOffset.DateTime;
    }
    
}