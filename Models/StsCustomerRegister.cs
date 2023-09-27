using System;
using System.Collections.Generic;

namespace SagErpBlazor.Models;

public partial class StsCustomerRegister
{
    public int CustomerId { get; set; }

    public string? CustomerName { get; set; }

    public string? Pocname { get; set; }

    public string? Pocnumber { get; set; }

    public string? Pocemail { get; set; }

    public int? RoleId { get; set; }

    public bool? CustomerStatus { get; set; }

    public int? CompanyId { get; set; }

    public int? RegisterdBy { get; set; }

    public DateTime? RegisterDate { get; set; }

    public int LoginId { get; set; }

    public string? Companylogo { get; set; }

    public int? NoOfUsersAllowed { get; set; }

    public int LogSourceId { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public bool? ModifiedType { get; set; }
}
