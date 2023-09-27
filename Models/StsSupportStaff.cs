using System;
using System.Collections.Generic;

namespace SagErpBlazor.Models;

public partial class StsSupportStaff
{
    public int SupportStaffId { get; set; }

    public string? Name { get; set; }

    public string? PhoneNo { get; set; }

    public string? EmailAddress { get; set; }

    public int RoleId { get; set; }

    public int? RegisterdBy { get; set; }

    public DateTime? RegisterDate { get; set; }

    public int? CompanyId { get; set; }

    public int? LoginId { get; set; }

    public bool ActiveStatus { get; set; }

    public int LogSourceId { get; set; }

    public bool? ModifiedType { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public int? ModifiedBy { get; set; }

    public string? ProfileImage { get; set; }
}
