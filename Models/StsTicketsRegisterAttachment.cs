using System;
using System.Collections.Generic;

namespace SagErpBlazor.Models;

public partial class StsTicketsRegisterAttachment
{
    public int TicketAttacmentId { get; set; }

    public int? TicketId { get; set; }

    public string? DocAttachment { get; set; }

    public string? AttachmentType { get; set; }

    public string? AttachDescription { get; set; }

    public string? AttachmentUrl { get; set; }

    public string? AttachmentName { get; set; }

    public bool? IsLog { get; set; }
}
