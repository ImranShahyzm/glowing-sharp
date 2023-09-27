using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SagErpBlazor.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using SagErpBlazor.DbClasses;
using System.Security.Cryptography;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using SagErpBlazor.Models;
using Microsoft.EntityFrameworkCore;

namespace SagErpBlazor.Services
{
    public class UserService : IUserService
    {
        public HttpClient _httpClient { get; }
        public JWTSettings _JWTSettings { get; }
        public DataBaseServices _DataBaseServices { get; set; }


        public UserService(HttpClient httpClient, DataBaseServices dataBaseServices, IOptions<JWTSettings> JWTSettings)
        {
            httpClient.DefaultRequestHeaders.Add("User-Agent", "BlazorServer");
            _httpClient = httpClient;
            _DataBaseServices = dataBaseServices;
            _JWTSettings = JWTSettings.Value;
        }

        public async Task<string> GetUserEmailAddress(int UserID)
        {
            using (BlazorSupportTicketsContext _NewDbContext = new BlazorSupportTicketsContext())
            {
                var EmailAddress = await _NewDbContext.Glusers.Where(x=>x.Userid==UserID).Select(x=>x.EmailAddress).FirstOrDefaultAsync();

                return EmailAddress;
            }
        }
        private GLUser GetUserObjectBasedOnRoles(int RoleID,DataRow row)
        {
            GLUser _ActiveUser = new GLUser();

            switch (RoleID)
            {
                case 1:
                       _ActiveUser = GenerateGLUserForAdmin(row);
                        break;    
                case 2:
                    _ActiveUser = GenerateGLUserForSupport(row);
                    break;
                case 3:
                    _ActiveUser = GenerateGLUserForTechnical(row);
                    break;
                case 4:
                    _ActiveUser = GenerateGLUserForCustomer(row);
                    break;
                default:
                    break;
            }
            return _ActiveUser;
        }

        private GLUser GenerateGLUserForAdmin( DataRow row)
        {
            // Create and return the GLUser object for RoleID 1
            GLUser user = new GLUser
            {
                // Mapping properties from the DataTable columns
                Userid = Convert.ToInt32((row["Userid"]).ToString()),
                Username = row["Username"].ToString(),
                UserPassword = row["UserPassword"].ToString(),
                FirstName = row["FirstName"].ToString(),
                LastName = row["LastName"].ToString(),
                CompanyID = Convert.ToInt32((row["CompanyID"].ToString())),
                Role = new Role
                {
                    RoleId = Convert.ToInt32((row["RoleID"]).ToString()),
                    RoleDescription = Convert.ToString(row["RoleDescription"])
                },
                _StsSupportStaff = new StsSupportStaff
                {
                    SupportStaffId = 0,
                    Name = ""
                },
                _StsTechnicalStaff = new StsTechnicalStaff
                {
                    TechStaffId = 0,
                    StaffName = ""
                },
                _CustomerLogin = new StsCustomerRegister
                {
                    CustomerId = 0,
                    CustomerName = ""
                },
            };

            return user;
        }

        private GLUser GenerateGLUserForCustomer(DataRow row)
        {
            // Create and return the GLUser object for RoleID 1
            GLUser user = new GLUser
            {
                // Mapping properties from the DataTable columns
                Userid = Convert.ToInt32((row["Userid"]).ToString()),
                Username = row["Username"].ToString(),
                UserPassword = row["UserPassword"].ToString(),
                FirstName = row["FirstName"].ToString(),
                LastName = row["LastName"].ToString(),
                CompanyID = Convert.ToInt32((row["CompanyID"].ToString())),
                Role = new Role
                {
                    RoleId = Convert.ToInt32((row["RoleID"]).ToString()),
                    RoleDescription = Convert.ToString(row["RoleDescription"])
                },
                _CustomerLogin = new StsCustomerRegister
                {
                    CustomerId = Convert.ToInt32((row["CustomerID"])),
                    CustomerName = Convert.ToString(row["CustomerName"])
                },
                _StsSupportStaff = new StsSupportStaff
                {
                    SupportStaffId = 0,
                    Name = ""
                },
                _StsTechnicalStaff = new StsTechnicalStaff
                {
                    TechStaffId = 0,
                    StaffName = ""
                },
            };

            return user;
        }
        private GLUser GenerateGLUserForTechnical(DataRow row)
        {
            // Create and return the GLUser object for RoleID 1
            GLUser user = new GLUser
            {
                // Mapping properties from the DataTable columns
                Userid = Convert.ToInt32((row["Userid"]).ToString()),
                Username = row["Username"].ToString(),
                UserPassword = row["UserPassword"].ToString(),
                FirstName = row["FirstName"].ToString(),
                LastName = row["LastName"].ToString(),
                CompanyID = Convert.ToInt32((row["CompanyID"].ToString())),
                Role = new Role
                {
                    RoleId = Convert.ToInt32((row["RoleID"]).ToString()),
                    RoleDescription = Convert.ToString(row["RoleDescription"])
                },

                _StsTechnicalStaff = new StsTechnicalStaff
                {
                    TechStaffId = Convert.ToInt32((row["TechStaffID"])),
                    StaffName = Convert.ToString(row["StaffName"])
                },
             
                _CustomerLogin = new StsCustomerRegister
                {
                    CustomerId = 0,
                    CustomerName = ""
                },
                _StsSupportStaff = new StsSupportStaff
                {
                    SupportStaffId = 0,
                    Name = ""
                }

            };

            return user;
        }
        private GLUser GenerateGLUserForSupport(DataRow row)
        {
            // Create and return the GLUser object for RoleID 1
            GLUser user = new GLUser
            {
                // Mapping properties from the DataTable columns
                Userid = Convert.ToInt32((row["Userid"]).ToString()),
                Username = row["Username"].ToString(),
                UserPassword = row["UserPassword"].ToString(),
                FirstName = row["FirstName"].ToString(),
                LastName = row["LastName"].ToString(),
                CompanyID = Convert.ToInt32((row["CompanyID"].ToString())),
                Role = new Role
                {
                    RoleId = Convert.ToInt32((row["RoleID"]).ToString()),
                    RoleDescription = Convert.ToString(row["RoleDescription"])
                },
                _StsTechnicalStaff = new StsTechnicalStaff
                {
                    TechStaffId =0,
                    StaffName = ""
                },
                _CustomerLogin = new StsCustomerRegister
                {
                    CustomerId = 0,
                    CustomerName =""
                },
                _StsSupportStaff = new StsSupportStaff
                {
                    SupportStaffId = Convert.ToInt32((row["SupportStaffID"])),
                    Name = Convert.ToString(row["SupportStaffName"])
                }

            };

            return user;
        }
        public GLUser GetUserData(int Userid)
        {



            GLUser _ActiveUser = null;

            _ActiveUser = ActiveUserSingleton.Instance;

            if (_ActiveUser == null)
            {


                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(@"select gluser.*,dbo.STS_Roles.*,dbo.STS_CustomerRegister.CustomerID,
dbo.STS_CustomerRegister.CustomerName,
dbo.STS_TechnicalStaff.TechStaffID,
dbo.STS_TechnicalStaff.StaffName,
STS_SupportStaff.SupportStaffID,
STS_SupportStaff.Name AS SupportStaffName

 from gluser LEFT JOIN STS_Roles ON STS_Roles.RoleID = GLUser.RoleID 

LEFT JOIN dbo.STS_CustomerRegister ON STS_CustomerRegister.LoginID = GLUser.Userid AND ISNULL(dbo.STS_CustomerRegister.LogSourceID,0)=0
LEFT JOIN dbo.STS_TechnicalStaff ON STS_TechnicalStaff.LoginID = gluser.Userid AND ISNULL(dbo.STS_TechnicalStaff.LogSourceID,0)=0
LEFT JOIN dbo.STS_SupportStaff ON STS_SupportStaff.LoginID = gluser.Userid AND ISNULL(dbo.STS_SupportStaff.LogSourceID,0)=0 
 where  GLUser.userid='" + Userid + "'", _DataBaseServices.GetSqlConnection());
                da.Fill(dt);

                if (dt.Rows.Count == 0)
                {
                    return _ActiveUser;
                }
                else
                {




                    DataRow row = dt.Rows[0];
                    int RoleID = Convert.ToInt32((row["RoleID"]).ToString());
                    _ActiveUser = GetUserObjectBasedOnRoles(RoleID, row);
                    ActiveUserSingleton.Initialize(_ActiveUser);

                   
                }
            }
            return _ActiveUser;
        }
            public async Task<GLUser> LoginAsync(GLUser user)
        {
            GLUser _ActiveUser = new GLUser();
            user.UserPassword = EncryptionHelper.DefaultEncrypt(user.UserPassword,EncryptionHelper.DefaultKey);
            string serializedUser = JsonConvert.SerializeObject(user);

            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(@"select gluser.*,dbo.STS_Roles.*,dbo.STS_CustomerRegister.CustomerID,
dbo.STS_CustomerRegister.CustomerName,
dbo.STS_TechnicalStaff.TechStaffID,
dbo.STS_TechnicalStaff.StaffName,
STS_SupportStaff.SupportStaffID,
STS_SupportStaff.Name AS SupportStaffName

 from gluser LEFT JOIN STS_Roles ON STS_Roles.RoleID = GLUser.RoleID 

LEFT JOIN dbo.STS_CustomerRegister ON STS_CustomerRegister.LoginID = GLUser.Userid AND ISNULL(dbo.STS_CustomerRegister.LogSourceID,0)=0
LEFT JOIN dbo.STS_TechnicalStaff ON STS_TechnicalStaff.LoginID = gluser.Userid AND ISNULL(dbo.STS_TechnicalStaff.LogSourceID,0)=0
LEFT JOIN dbo.STS_SupportStaff ON STS_SupportStaff.LoginID = gluser.Userid AND ISNULL(dbo.STS_SupportStaff.LogSourceID,0)=0 
 where GLUser.UserPassword='" + user.UserPassword + "' and GLUser.UserName= '" + user.Username + "'", _DataBaseServices.GetSqlConnection());
            da.Fill(dt);

            if (dt.Rows.Count == 0)
            {
                return await Task.FromResult(_ActiveUser);
            }
            else
            {


                UserWithToken userWithToken = null;

                DataRow row = dt.Rows[0];
                int RoleID = Convert.ToInt32((row["RoleID"]).ToString());
                _ActiveUser = GetUserObjectBasedOnRoles(RoleID, row);
                {
                    RefreshToken refreshToken = GenerateRefreshToken();
                    userWithToken = new UserWithToken(_ActiveUser);
                    userWithToken.RefreshToken = refreshToken.Token;
                }

             
                userWithToken.AccessToken = GenerateAccessToken(_ActiveUser.Userid);

                return await Task.FromResult(userWithToken);
            }


          

          

        }
        private string GenerateAccessToken(int userId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_JWTSettings.SecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, Convert.ToString(userId))
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        private RefreshToken GenerateRefreshToken()
        {
            RefreshToken refreshToken = new RefreshToken();

            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                refreshToken.Token = Convert.ToBase64String(randomNumber);
            }
            refreshToken.ExpiryDate = DateTime.UtcNow.AddMonths(3);

            return refreshToken;
        }



        private async Task<GLUser> GetUserFromAccessToken(string accessToken)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_JWTSettings.SecretKey);

                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };

                SecurityToken securityToken;
                var principle = tokenHandler.ValidateToken(accessToken, tokenValidationParameters, out securityToken);

                JwtSecurityToken jwtSecurityToken = securityToken as JwtSecurityToken;

                if (jwtSecurityToken != null && jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    var userId = principle.FindFirst(ClaimTypes.Name)?.Value;
                    return  GetUserData(Convert.ToInt32(userId));
                 
                }
            }
            catch (Exception)
            {
                return new GLUser();
            }

            return new GLUser();
        }


        public async Task<GLUser> GetUserByAccessTokenAsync(string accessToken)
        {
            string serializedRefreshRequest = JsonConvert.SerializeObject(accessToken);

           GLUser returnedUser= await GetUserFromAccessToken(accessToken);

            return await Task.FromResult(returnedUser);
        }
    }
}
