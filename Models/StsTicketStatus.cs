using System;
using System.Collections.Generic;

namespace SagErpBlazor.Models;

public partial class StsTicketStatus
{
    public int TicketStatusId { get; set; }

    public string? StatusName { get; set; }

    public string? StatusColor { get; set; }
}
