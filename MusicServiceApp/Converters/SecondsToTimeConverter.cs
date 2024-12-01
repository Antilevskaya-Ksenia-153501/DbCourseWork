using System.Globalization;
using System.Windows.Data;

namespace MusicService.Converters;

public class SecondsToTimeConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not int seconds) return null;

        var time = TimeSpan.FromSeconds(seconds);
        return $"{time.Hours:D2}:{time.Minutes:D2}:{time.Seconds:D2}";
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}