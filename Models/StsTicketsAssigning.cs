using System;
using System.Collections.Generic;

namespace SagErpBlazor.Models;

public partial class StsTicketsAssigning
{
    public int AssignId { get; set; }

    public int? TicketId { get; set; }

    public int? TechStaffId { get; set; }

    public int? SupportStaffId { get; set; }

    public DateTime? AssignDate { get; set; }

    public int? AssignApproval { get; set; }

    public DateTime? ApprovalDate { get; set; }

    public DateTime? AssignTimeStamp { get; set; }

    public DateTime? ApporvalTimeStamp { get; set; }

    public bool? AddedToWorkList { get; set; }

    public int? CompanyId { get; set; }

    public int? Userid { get; set; }

    public int LogSourceId { get; set; }
}
