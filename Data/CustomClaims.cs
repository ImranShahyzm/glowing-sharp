using System.Security.Claims;

namespace SagErpBlazor.Data
{
    public class CustomClaims
    {
        public IEnumerable<Claim> Claims { get; set; }
    }
}
