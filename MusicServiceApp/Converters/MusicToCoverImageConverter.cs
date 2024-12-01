using System.Globalization;
using System.Windows.Data;

namespace MusicService.Converters;

public class MusicToCoverImageConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not string mp3FilePath) return null;
        var file = TagLib.File.Create(mp3FilePath);
        if (file.Tag.Pictures.Length > 0 && file.Tag.Pictures[0] != null)
            return file.Tag.Pictures[0].Data.Data;
        return null;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}