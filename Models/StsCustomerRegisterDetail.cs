using System;
using System.Collections.Generic;

namespace SagErpBlazor.Models;

public partial class StsCustomerRegisterDetail
{
    public int CustomerDetailId { get; set; }

    public int? CustomerId { get; set; }

    public int? ProjectId { get; set; }

    public string? ProjectDescription { get; set; }
}
