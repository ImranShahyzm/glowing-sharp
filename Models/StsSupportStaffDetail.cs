using System;
using System.Collections.Generic;

namespace SagErpBlazor.Models;

public partial class StsSupportStaffDetail
{
    public int SupportStaffDetailsId { get; set; }

    public int? SupportStaffId { get; set; }

    public int? TechStaffId { get; set; }

    public bool? IsLog { get; set; }
}
