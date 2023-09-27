using SagErpBlazor.Data;
using System.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Options;
using SagErpBlazor.Models;
using Microsoft.EntityFrameworkCore;

namespace SagErpBlazor.Services
{
    public class CompanyService
    {
        public HttpClient _httpClient { get; }
        public JWTSettings _JWTSettings { get; }
        public DataBaseServices _DataBaseServices { get; set; }
        private readonly BlazorSupportTicketsContext _dbContext;


        public CompanyService(HttpClient httpClient, DataBaseServices dataBaseServices, IOptions<JWTSettings> JWTSettings, BlazorSupportTicketsContext dbContext)
        {
            httpClient.DefaultRequestHeaders.Add("User-Agent", "BlazorServer");
            _httpClient = httpClient;
            _DataBaseServices = dataBaseServices;
            _JWTSettings = JWTSettings.Value;
            _dbContext = dbContext;
        }
      
        public async Task<StsGlcompany> GetCompanyData(int CompanyID)
        {
            var CompanyObje = _dbContext.StsGlcompanies.Find(CompanyID);
            if (CompanyObje == null)
            {
                return null;
            }
            else
            {
                return CompanyObje;
            }
        }
    }
}
