using System;
using System.Collections.Generic;

namespace SagErpBlazor.Models;

public partial class StsProjectRegister
{
    public int ProjectId { get; set; }

    public int? ProjectNo { get; set; }

    public string? ProjectCode { get; set; }

    public string? ProjectName { get; set; }

    public DateTime? ProjectDeploymentDate { get; set; }

    public DateTime? ProjectSupportDate { get; set; }

    public DateTime? ProjectExpiryDate { get; set; }

    public bool? ProjectStatus { get; set; }

    public int? ProjectBillingType { get; set; }

    public int CompanyId { get; set; }

    public int LogSourceId { get; set; }

    public int? RegisteredBy { get; set; }

    public DateTime? RegisterDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public bool? ModifiedType { get; set; }
}
