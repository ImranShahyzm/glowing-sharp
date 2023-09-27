using System;
using System.Collections.Generic;

namespace SagErpBlazor.Models;

public partial class Gluser
{
    public int Userid { get; set; }

    public string? UserName { get; set; }

    public string? UserPassword { get; set; }

    public DateTime? TimeStamp { get; set; }

    public bool Active { get; set; }

    public int? CompanyId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? PhoneNumber { get; set; }

    public string? EmailAddress { get; set; }

    public string? ProfileImage { get; set; }

    public int? RoleId { get; set; }
}
