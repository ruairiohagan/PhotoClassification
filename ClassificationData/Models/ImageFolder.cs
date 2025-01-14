using System;
using System.Collections.Generic;

namespace ClassificationData.Models;

public partial class ImageFolder
{
    public string FolderPath { get; set; } = null!;

    public int Recursive { get; set; }
}
