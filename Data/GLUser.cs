using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq.Expressions;
using SagErpBlazor.Models;

namespace SagErpBlazor.Data
{
    public partial class GLUser
    {
       
            public int Userid { get; set; }
            public string Username { get; set; }
            public string UserPassword { get; set; }
            public string Source { get; set; }
            public string FirstName { get; set; }
            public string MiddleName { get; set; }
            public string LastName { get; set; }
            public int CompanyID { get; set; }
            public DateTime? HireDate { get; set; }

            public Role Role { get; set; }
            public string ConfirmPassword { get; set; }
            public string AccessToken { get; set; } = string.Empty;
            public string RefreshToken { get; set; } = string.Empty;
            public StsCustomerRegister _CustomerLogin { get; set; }
            public StsTechnicalStaff _StsTechnicalStaff { get; set; }
            public StsSupportStaff _StsSupportStaff { get; set; }

        public int GetLoginIDAfterSaving(Gluser Object) {

            using (var _DbContext = new BlazorSupportTicketsContext())
            {
                using (var transaction = _DbContext.Database.BeginTransaction())
                {
                    try
                    {

                        _DbContext.Glusers.Add(Object);
                        _DbContext.SaveChanges();
                        transaction.Commit();
                        return Object.Userid;
                    }
                    catch (Exception ex)
                    {

                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }


    }
    
    
}
