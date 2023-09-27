using SagErpBlazor.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SagErpBlazor.Services
{
    public interface IUserService
    {
        public Task<GLUser> LoginAsync(GLUser user);
        public Task<GLUser> GetUserByAccessTokenAsync(string accessToken);
        public Task<string> GetUserEmailAddress(int UserID);
    }
}
