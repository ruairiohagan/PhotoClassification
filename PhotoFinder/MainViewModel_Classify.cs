using ClassificationAPI;
using ClassificationData;
using ClassificationData.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static ClassificationAPI.Classify;

namespace PhotoFinder
{
    /// <summary>
    /// View Model class for the main form to which all bindings will be attached - Classify tab
    /// </summary>
    partial class MainViewModel : INotifyPropertyChanged
    {
        private string _ClassificationStatus = "";
        public string ClassificationStatus
        {
            get { return _ClassificationStatus; }
            set
            {
                _ClassificationStatus = value;
                OnPropertyChanged(nameof(ClassificationStatus));
            }
        }
        private bool _BusyClearing = false;
        public bool BusyClearing
        {
            get { return _BusyClearing; }
            set
            {
                _BusyClearing = value;
                OnPropertyChanged(nameof(BusyClearing));
            }
        }
        private bool _BusyClassifying = false;
        public bool BusyClassifying
        {
            get { return _BusyClassifying; }
            set
            {
                _BusyClassifying = value;
                OnPropertyChanged(nameof(BusyClassifying));
            }
        }
        private bool _OK2Continue = true;
        public bool OK2Continue
        {
            get { return _OK2Continue; }
            set
            {
                if (!value)
                {
                    ClassificationStatus = "Cancelling...";
                }
                _OK2Continue = value;
            }
        }
        public async void StartClassification()
        {
            using (var context = new PhotoClassificationContext())
            {
                ClassificationStatus = "";
                LastProcessedImage = "";
                LastDescription = "";

                OK2Continue = true;
                BusyClassifying = true;
                try
                {
                    var imFolders = context?.ImageFolders?.ToList();
                    if (imFolders != null)
                    {
                        foreach (var imFolder in imFolders)
                        {
                            if (!OK2Continue) return;

                            string recursiveText = imFolder.Recursive > 0 ? "(with subfolders)" : "";
                            ClassificationStatus = $"Processing folder {imFolder.FolderPath}{recursiveText}";

                            await Classify.ClassifyFolderImages(imFolder.FolderPath, Settings.APILocation, Settings.ImageModel, Settings.ClassificationQuestion, imFolder.Recursive != 0, false, Settings.TimeoutInMS, ClassificationProgress);
                        }

                        ClassificationStatus = "Classification complete.";
                        GetItemSummary();
                    }
                }
                finally
                {
                    BusyClassifying = false;
                }
            }
        }
        private double _ClassProgress = 0;
        public double ClassProgress
        {
            get { return _ClassProgress; }
            set
            {
                _ClassProgress = value * 100;
                OnPropertyChanged(nameof(ClassProgress));
            }
        }
        private string _LastDescription = "";
        public string LastDescription
        {
            get { return _LastDescription; }
            set
            {
                _LastDescription = value;
                OnPropertyChanged(nameof(LastDescription));
            }
        }
        private string _LastProcessedImage = "";
        public string LastProcessedImage
        {
            get { return _LastProcessedImage; }
            set
            {
                _LastProcessedImage = value;
                OnPropertyChanged(nameof(LastProcessedImage));
            }
        }
        /// <summary>
        /// Callback for progress update for classification
        /// </summary>
        private bool ClassificationProgress(ClassifyProgress progress)
        {
            if (progress.Status == null)
            {
                ClassProgress = progress.Progress;
                ClassificationStatus = $"{progress.Description}: {progress.ImagePath}";
            }
            else
            {
                //Image has been completed
                LastProcessedImage = progress.ImagePath;
                LastDescription = progress.Description;

                if (progress.Status != System.Net.HttpStatusCode.OK)
                {
                    // Error has occurred, don't update the stats, just ask if user wants to continue
                    return (MessageBox.Show($"The following error occurred during classification, click \"OK\" to continue processing:\n\n{progress.Description}", "Classification Error!", MessageBoxButton.OKCancel, MessageBoxImage.Error, MessageBoxResult.Yes)
                        == MessageBoxResult.OK);
                }

                ImageCount++;
                ImageClassDurationMS += (long)progress.ElapsedMS;
            }

            return OK2Continue;
        }
        private double _ClearProgress = 0;
        public double ClearProgress
        {
            get { return _ClearProgress; }
            set
            {
                _ClearProgress = value * 100;
                OnPropertyChanged(nameof(ClearProgress));
            }
        }
        private bool ClearingProgress(decimal progress)
        {
            ClearProgress = (double)progress;

            return OK2Continue;
        }
        /// <summary>
        /// Clear all image classifications
        /// </summary>
        public async void StartClearing()
        {
            using (var context = new PhotoClassificationContext())
            {
                ClassificationStatus = "";
                LastProcessedImage = "";
                LastDescription = "";

                OK2Continue = true;
                BusyClearing = true;
                try
                {
                    ClassificationStatus = "Clearing image classifications";
                    await Images.ClearAllClassificaitons(ClearingProgress);
                    GetItemSummary();
                }
                finally
                {
                    ClassificationStatus = "Classification clearing completed";
                    BusyClearing = false;
                }
            }
        }

    }
}
