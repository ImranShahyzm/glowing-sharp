using SagErpBlazor.Data;

namespace SagErpBlazor.Services
{
    public class UserWithToken:GLUser
    {
        public UserWithToken(GLUser user)
        {
            this.Userid = user.Userid;
            this.Username  = user.Username;
            this.FirstName = user.FirstName;
            this.MiddleName = user.MiddleName;
            this.LastName = user.LastName;
            this.CompanyID = user.CompanyID;
            this.Role = user.Role;
            this._CustomerLogin = user._CustomerLogin;
            this._StsSupportStaff = user._StsSupportStaff;
            this._StsTechnicalStaff = user._StsTechnicalStaff;
        }

    }
}