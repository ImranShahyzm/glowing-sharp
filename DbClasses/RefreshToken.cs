using SagErpBlazor.Data;

namespace SagErpBlazor.DbClasses
{
    public partial class RefreshToken
    {
            public int TokenId { get; set; }
            public int UserId { get; set; }
            public string Token { get; set; }
            public DateTime ExpiryDate { get; set; }
            public virtual GLUser User { get; set; }
        
    }
}
