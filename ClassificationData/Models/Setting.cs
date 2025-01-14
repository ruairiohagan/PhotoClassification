using System;
using System.Collections.Generic;

namespace ClassificationData.Models;

public partial class Setting
{
    public string Name { get; set; } = null!;

    public string Value { get; set; } = null!;

    public string Comment { get; set; } = null!;
}
