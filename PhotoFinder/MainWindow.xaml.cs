using ClassificationAPI;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ClassificationData;
using Microsoft.Win32;
using ClassificationData.Models;
using System.Security.Cryptography;

namespace PhotoFinder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainViewModel mainViewModel;
        public MainWindow()
        {
            InitializeComponent();
            mainViewModel = new MainViewModel();
            DataContext = mainViewModel;
        }

        private void cmdDelImageFolder_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (listImageFolders.SelectedIndex == -1)
                {
                    MessageBox.Show("Select a folder to delete", "Settings", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                ImageFolder? selectedfolder = listImageFolders.SelectedItem as ImageFolder;
                if (selectedfolder != null)
                {
                    using (PhotoClassificationContext context = new PhotoClassificationContext())
                    {
                        ImageFolder? delFolder = context?.ImageFolders?.Where(f => f.FolderPath == selectedfolder.FolderPath).FirstOrDefault();
                        if (delFolder != null)
                        {
                            context?.ImageFolders?.Remove(delFolder);
                            context?.SaveChanges();
                        }
                    }
                    mainViewModel.LoadImagefolders();
                }
            }
            catch (Exception ex)
            {
                ErrorHandler(ex);
            }
        }
        private void cmdAddImageFolder_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFolderDialog ofd = new OpenFolderDialog();
                ofd.Multiselect = false;
                if (ofd?.ShowDialog() ?? false)
                {
                    using (PhotoClassificationContext context = new PhotoClassificationContext())
                    {
                        if (context?.ImageFolders?.Where(i => i.FolderPath == ofd.FolderName)?.Any() ?? false)
                        {
                            MessageBox.Show($"Folder {ofd.FolderName} is already in list.", "Settings", MessageBoxButton.OK, MessageBoxImage.Information);
                            return;
                        }                        
                        context?.ImageFolders?.Add(new ImageFolder() { FolderPath = ofd.FolderName, Recursive = 1 });
                        context?.SaveChanges();
                    }
                    mainViewModel.LoadImagefolders();
                }

            }
            catch (Exception ex)
            {
                ErrorHandler(ex);
            }
        }

        public static void ErrorHandler(Exception ex)
        {
            MessageBox.Show(ex.ToString(), "Error!", MessageBoxButton.OK, MessageBoxImage.Hand);
        }

        private void cmdRecursive_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button? clickedButton = sender as Button;
                if (clickedButton != null)
                {
                    ImageFolder? clickedImageFolder = clickedButton.DataContext as ImageFolder;
                    if (clickedImageFolder != null)
                    {
                        using (PhotoClassificationContext context = new PhotoClassificationContext())
                        {
                            ImageFolder? dbFolder = context?.ImageFolders?.Where(f => f.FolderPath == clickedImageFolder.FolderPath)?.FirstOrDefault();
                            if (dbFolder != null)
                            {
                                dbFolder.Recursive = clickedImageFolder.Recursive == 0 ? 1 : 0;
                                context?.SaveChanges();
                            }
                        }
                        mainViewModel.LoadImagefolders();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHandler(ex);
            }
        }

        private void cmdClassify_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!mainViewModel.BusyClassifying)
                {
                    mainViewModel.StartClassification();
                }
                else
                {
                    mainViewModel.OK2Continue = false;
                }
            }
            catch (Exception ex)
            {
                ErrorHandler(ex);
            }
        }

        private void textLMStudioLocation_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (e.Changes.Where(c => c.AddedLength == textLMStudioLocation.Text.Length).Any())
                {
                    // This is the first setting of the value when form is loaded, so go get the models
                    mainViewModel.LoadModels();
                }
            }
            catch (Exception ex)
            {
                ErrorHandler(ex);
            }
        }

        private void textLMStudioLocation_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                mainViewModel.LoadModels();
            }
            catch (Exception ex)
            {
                ErrorHandler(ex);
            }
        }

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                mainViewModel.SearchImages();
            }
            catch (Exception ex)
            {
                ErrorHandler(ex);
            }
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                {
                    mainViewModel.SearchImages();
                }
            }
            catch (Exception ex)
            {
                ErrorHandler(ex);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                txtSearch.Focus();
            }
            catch (Exception ex)
            {
                ErrorHandler(ex);
            }
        }

        private void cmdClearClass_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!mainViewModel.BusyClassifying)
                {
                    if (mainViewModel.ImageCount > 30)
                    {
                        if (MessageBox.Show($"Are you sure you wish to clear all the classifications? {mainViewModel.ImageCount} images have been classified which took {MillisecondsToTimeDescConverter.GetApproxTimeDesc((long)mainViewModel.ImageClassDurationMS / 1000)}. Are you *sure* you want to continue?",
                            "WOAH THERE!", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning, MessageBoxResult.No) == MessageBoxResult.Yes)
                        {
                            mainViewModel.StartClearing();
                        }
                    }
                    else
                    {
                        mainViewModel.StartClearing();
                    }
                }
                else
                {
                    mainViewModel.OK2Continue = false;
                }
            }
            catch (Exception ex)
            {
                ErrorHandler(ex);
            }
        }

        private void cmdOpenImage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                mainViewModel.OpenImage();
            }
            catch (Exception ex)
            {
                ErrorHandler(ex);
            }
        }

        private void cmdOpenInExplorer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                mainViewModel.OpenImageInExplorer();
            }
            catch (Exception ex)
            {
                ErrorHandler(ex);
            }
        }
    }
}