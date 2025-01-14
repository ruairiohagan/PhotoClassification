using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassificationData
{
    /// <summary>
    /// Extender class for the EF Image class to handle date to string conversions
    /// </summary>
    public static class ImageEx 
    {

        public static void SetImageDate(this Models.Image image, DateTime value)
        {
            image.ImageDate = value.ToString("yyyy-MM-dd HH:mm:ss");
        }
        public static DateTime GetImageDate(this Models.Image image)
        {
            return DateTime.ParseExact(image.ImageDate, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
        }
        public static void SetDescriptionDate(this Models.Image image, DateTime value)
        {
            image.DescriptionDate = value.ToString("yyyy-MM-dd HH:mm:ss");
        }
        public static DateTime GetDescriptionDate(this Models.Image image)
        {
            return DateTime.ParseExact(image.DescriptionDate, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
        }
    }
}
