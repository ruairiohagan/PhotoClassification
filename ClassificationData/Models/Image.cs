using System;
using System.Collections.Generic;

namespace ClassificationData.Models;

public partial class Image
{
    public string ImagePath { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string ImageDate { get; set; } = null!;

    public string DescriptionDate { get; set; } = null!;

    public string ModelUsed { get; set; } = null!;

    public int ClassificationMs { get; set; }
}
