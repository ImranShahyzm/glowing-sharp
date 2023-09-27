using System;
using System.Collections.Generic;

namespace SagErpBlazor.Models;

public partial class StsRefreshToken
{
    public int TokenId { get; set; }

    public int UserId { get; set; }

    public string Token { get; set; } = null!;

    public DateTime ExpiryDate { get; set; }
}
