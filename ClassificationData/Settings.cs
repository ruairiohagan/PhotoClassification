using ClassificationData.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassificationData
{
    /// <summary>
    /// DB operations relating to settings table including static properties to easily retireve and set current setting values
    /// </summary>
    public class Settings : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private static string GetValue(string settingName)
        {
            using (var context = new PhotoClassificationContext())
            {
                return context?.Settings?.Where(s => s.Name == settingName).FirstOrDefault()?.Value ?? "";
            }
        }
        private void SetValue(string settingName, string value)
        {
            using (var context = new PhotoClassificationContext())
            {
                Setting? setting = context?.Settings?.Where(s => s.Name == settingName).FirstOrDefault();
                if (setting != null) 
                {
                    setting.Value = value;
                    context?.SaveChanges();
                }
            }
        }
        public string APILocation
        {
            get
            {
                return GetValue("APILocation");
            }
            set 
            {
                SetValue("APILocation", value);
                OnPropertyChanged(nameof(APILocation));
            }
        }
        public string ImageModel
        {
            get
            {
                return GetValue("ImageModel");
            }
            set
            {
                SetValue("ImageModel", value);
                OnPropertyChanged(nameof(ImageModel));
            }
        }
        public string ClassificationQuestion
        {
            get
            {
                return GetValue("ClassificationQuestion");
            }
            set
            {
                SetValue("ClassificationQuestion", value);
                OnPropertyChanged(nameof(ClassificationQuestion));
            }
        }
        public int TimeoutInMS
        {
            get
            {
                int timeoutMS = 0;
                if (!int.TryParse(GetValue("TimeoutInMS"), out timeoutMS))
                {
                    timeoutMS = 30000;
                }
                return timeoutMS;
            }
            set
            {
                SetValue("TimeoutInMS", value.ToString());
                OnPropertyChanged(nameof(TimeoutInMS));
            }
        }
    }
}
