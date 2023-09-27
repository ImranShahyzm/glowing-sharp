using System;
using System.Collections.Generic;

namespace SagErpBlazor.Models;

public partial class StsGlcompany
{
    public int CompanyId { get; set; }

    public string Title { get; set; } = null!;

    public string ShortTitle { get; set; } = null!;

    public string? Email { get; set; }

    public string? Address { get; set; }

    public string? Phone { get; set; }

    public string? Website { get; set; }

    public string? CompanyImage { get; set; }

    public bool Inactive { get; set; }

    public string? SupportEmail { get; set; }

    public string? PostalCode { get; set; }

    public string? PoBoxNo { get; set; }

    public string? FaxNo { get; set; }

    public string? LoginBackground { get; set; }

    public string? CsscolorStyle { get; set; }
}
