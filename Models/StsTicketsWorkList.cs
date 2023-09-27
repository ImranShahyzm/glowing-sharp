using System;
using System.Collections.Generic;

namespace SagErpBlazor.Models;

public partial class StsTicketsWorkList
{
    public int WorkListId { get; set; }

    public int? AssignId { get; set; }

    public int? TicketId { get; set; }

    public int? ActionNo { get; set; }

    public int? TicketStatusId { get; set; }

    public DateTime? AddedDate { get; set; }

    public int? PreviousStatusId { get; set; }

    public int? TechStaffId { get; set; }

    public bool IsLog { get; set; }

    public string? Summary { get; set; }

    public int? Userid { get; set; }

    public int? CompanyId { get; set; }
}
