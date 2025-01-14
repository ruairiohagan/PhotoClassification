using ClassificationData;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassificationData.Models;

using Microsoft.EntityFrameworkCore;
using ClassificationAPI;
using System.Windows.Threading;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Windows.Data;
using System.Globalization;
using BootstrapIcons.Wpf;
using System.Windows;

namespace PhotoFinder
{
    /// <summary>
    /// View Model class for the main form to which all bindings will be attached
    /// </summary>
    partial class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {
            DBController.CheckDBStructures(); // Make sure our DB structures are in place and correct version
            LoadImagefolders();
            GetItemSummary();
        }


        public event PropertyChangedEventHandler? PropertyChanged; 
        protected void OnPropertyChanged(string propertyName) 
        { 
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); 
        }
    }

}
