using System;
using System.Collections.Generic;

namespace SagErpBlazor.Models;

public partial class VwTicketsStatusInfo
{
    public int TicketId { get; set; }

    public int TicketNo { get; set; }

    public DateTime RegisteredDate { get; set; }

    public int? RegisteredBy { get; set; }

    public int CustomerId { get; set; }

    public string TicketName { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string? PhoneNo { get; set; }

    public string? EmailAddress { get; set; }

    public int ProjectId { get; set; }

    public int? SupportStaffId { get; set; }

    public int TicketChannel { get; set; }

    public int TicketPriority { get; set; }

    public int TicketStatus { get; set; }

    public int TicketType { get; set; }

    public int? NoOfRevisions { get; set; }

    public int? CompanyId { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public int? ModifiedBy { get; set; }

    public int? LogSourceId { get; set; }

    public bool? ModifiedType { get; set; }

    public string? ProjectName { get; set; }

    public int TicketStatusId { get; set; }

    public string StatusName { get; set; } = null!;

    public string? TypeName { get; set; }

    public string? MainProjectName { get; set; }

    public string? TypeIcon { get; set; }

    public string StatusColor { get; set; } = null!;
}
