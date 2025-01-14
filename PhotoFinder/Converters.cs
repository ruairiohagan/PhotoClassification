using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;
using System.Windows.Media.Imaging;
using System.IO;

namespace PhotoFinder
{
    public class FolderRecursiveIntToEnabledConverter : IValueConverter
    {

        /// <summary>
        /// Returns the correct visibility for the recursive icon for the folders listbox Recursive setting
        /// </summary>
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int iRecursive = (int)value;

            return iRecursive > 0 ? Visibility.Visible : Visibility.Collapsed;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class FolderNotRecursiveIntToEnabledConverter : IValueConverter
    {

        /// <summary>
        /// Returns the correct visibility for the not recursive icon for the folders listbox Recursive setting
        /// </summary>
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int iRecursive = (int)value;

            return iRecursive == 0 ? Visibility.Visible : Visibility.Collapsed;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class BoolToVisibilityConverter : IValueConverter
    {

        /// <summary>
        /// Returns visible if busy
        /// </summary>
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool busy = (bool)value;

            return busy ? Visibility.Visible : Visibility.Collapsed;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class AntiBoolToVisibilityConverter : IValueConverter
    {

        /// <summary>
        /// Returns visible if not busy
        /// </summary>
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool busy = (bool)value;

            return busy ? Visibility.Collapsed : Visibility.Visible;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class PathToFileNameConverter : IValueConverter
    {

        /// <summary>
        /// Returns the file name from a path
        /// </summary>
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string? path = value as string;
            if (string.IsNullOrWhiteSpace(path)) return string.Empty;

            return Path.GetFileName(path);
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class MStoSecondsConverter : IValueConverter
    {

        /// <summary>
        /// Converts milliseconds to seconds
        /// </summary>
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int milliSecs = (int)value;

            return (decimal)milliSecs / 1000m;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class MillisecondsToTimeDescConverter : IValueConverter
    {

        /// <summary>
        /// Returns a human readable time duration from a count of milliseconds
        /// </summary>
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            long milliSecs = (long)value;
            long seconds = milliSecs / 1000;

            return GetApproxTimeDesc(seconds);
        }
        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        public static string GetApproxTimeDesc(long seconds)
        {
            string durationDesc = "";
            switch (seconds)
            {
                case > 60 * 60 * 24 * 2: // two days
                    durationDesc = $"over {seconds / (60 * 60 * 24)} days";
                    break;
                case > 60 * 60 * 2: // two hours
                    durationDesc = $"over {seconds / (60 * 60)} hours";
                    break;
                case > 60 * 2: // two minutes
                    durationDesc = $"over {seconds / 60} minutes";
                    break;
                default:
                    durationDesc = $"{seconds} seconds";
                    break;
            }
            return durationDesc;
        }
    }

    public class InvertedBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool) return !(bool)value;
            return false;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool) return !(bool)value;
            return false;
        }
    }
    public class ObjectToVisibilityConverter : IValueConverter
    {

        /// <summary>
        /// Returns visible if the object is not null
        /// </summary>
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value != null) ? Visibility.Visible : Visibility.Collapsed;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
