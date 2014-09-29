using System;
using Xamarin.Forms;
using System.Globalization;

namespace NogginAgenda.Xaml.Converters
{

    public class UriConverter: IValueConverter   
    {
          
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var uriString = value as String;
            if (string.IsNullOrEmpty (uriString)) return null;

            return new Uri (uriString);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("Not implemented url -> string conversion"); 
        }
    }
}