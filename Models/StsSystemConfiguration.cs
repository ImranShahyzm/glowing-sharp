using System;
using System.Collections.Generic;

namespace SagErpBlazor.Models;

public partial class StsSystemConfiguration
{
    public int ConfigurationId { get; set; }

    public int? CompanyId { get; set; }

    public string? SysColumnDt { get; set; }

    public string? SysLoginBackground { get; set; }

    public string? CssStyleColor { get; set; }

    public int? ClientId { get; set; }
}
