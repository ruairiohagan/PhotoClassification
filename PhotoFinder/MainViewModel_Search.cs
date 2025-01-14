using ClassificationData;
using ClassificationData.Models;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;

namespace PhotoFinder
{
    /// <summary>
    /// View Model class for the main form to which all bindings will be attached - Search tab
    /// </summary>
    partial class MainViewModel : INotifyPropertyChanged
    {
        private Image? _SelectedImage;
        public Image? SelectedImage
        {
            get { return _SelectedImage; }
            set
            {
                _SelectedImage = value;
                OnPropertyChanged(nameof(SelectedImage));
            }
        }

        private string _SearchText = string.Empty;
        public string SearchText
        {
            get { return _SearchText; }
            set
            {
                _SearchText = value;
                OnPropertyChanged(nameof(SearchText));
            }
        }
        private ClassificationData.Models.Image[] _SearchResults = [];
        public ClassificationData.Models.Image[] SearchResults
        {
            get { return _SearchResults; }
            set
            {
                _SearchResults = value;
                OnPropertyChanged(nameof(SearchResults));
            }
        }
        private int _ImageCount = 0;
        public int ImageCount
        {
            get { return _ImageCount; }
            set
            {
                _ImageCount = value;
                OnPropertyChanged(nameof(ImageCount));
            }
        }
        private long _ImageClassDurationMS = 0;
        public long ImageClassDurationMS
        {
            get { return _ImageClassDurationMS; }
            set
            {
                _ImageClassDurationMS = value;
                OnPropertyChanged(nameof(ImageClassDurationMS));
            }
        }

        /// <summary>
        /// Performs image search
        /// </summary>
        public void SearchImages()
        {
            if (SearchText == string.Empty)
            {
                return;
            }
            string[] searchTerms = SearchText.Split(" ");
            Task.Factory.StartNew(async () =>
            {
                SearchResults = await Images.SearchImages(searchTerms);
            });
        }

        /// <summary>
        /// Gets and sets the total images and time taken to classify
        /// </summary>
        public void GetItemSummary()
        {
            Task.Factory.StartNew(async () =>
            {
                ImagesSummary imageSummary = await Images.GetTotalImagesAndTime();
                ImageCount = imageSummary.Count;
                ImageClassDurationMS = imageSummary.SumDuration;
            });
        }

        /// <summary>
        /// Opens the selected image in the default applicaiton for that file type
        /// </summary>
        public void OpenImage()
        {
            if (SelectedImage == null) return;
            ProcessStartInfo psi = new ProcessStartInfo(SelectedImage.ImagePath)
            {
                UseShellExecute = true
            };
            Process.Start(psi);
        }
        /// <summary>
        /// Opens the folder the image is in in file explorer
        /// </summary>
        public void OpenImageInExplorer()
        {
            if (SelectedImage == null) return;

            ProcessStartInfo psi = new ProcessStartInfo(Path.GetDirectoryName(SelectedImage.ImagePath) ?? "")
            {
                UseShellExecute = true,
            };
            Process.Start(psi);
        }
    }
}
