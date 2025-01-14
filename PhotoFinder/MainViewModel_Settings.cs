using ClassificationData.Models;
using ClassificationData;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassificationAPI;

namespace PhotoFinder
{
    /// <summary>
    /// View Model class for the main form to which all bindings will be attached - Settings tab
    /// </summary>
    partial class MainViewModel : INotifyPropertyChanged
    {
        public Settings Settings { get; set; } = new Settings();

        private void ProcessModelsReturned(ModelsEventArgs e)
        {
            Models = new ObservableCollection<string>(e.models);
        }
        private ObservableCollection<string>? models;
        public ObservableCollection<string>? Models
        {
            get { return models; }
            set
            {
                models = value;
                OnPropertyChanged(nameof(Models));
            }
        }
        public void LoadImagefolders()
        {
            using (var context = new PhotoClassificationContext())
            {
                var imFolders = context?.ImageFolders?.ToList();
                if (imFolders != null)
                {
                    if (imFolders.Count == 0)
                    {
                        imFolders.Add(new ImageFolder() { FolderPath = "", Recursive = 0 });
                    }
                    ImageFolders = new ObservableCollection<ImageFolder>(imFolders);
                }
            }
        }

        private ObservableCollection<ImageFolder>? imageFolders;
        public ObservableCollection<ImageFolder>? ImageFolders
        {
            get
            {
                return imageFolders;
            }
            set
            {
                imageFolders = value;
                OnPropertyChanged(nameof(ImageFolders));
            }
        }

        public void LoadModels()
        {
            string apiLoc = Settings.APILocation;
            Task.Factory.StartNew(async () =>
            {
                ModelsEventArgs e = await LMStudio.GetModelList(apiLoc);
                ProcessModelsReturned(e);
            });
        }

    }
}
