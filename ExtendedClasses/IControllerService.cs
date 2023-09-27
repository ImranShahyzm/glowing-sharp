using Microsoft.EntityFrameworkCore.Diagnostics;
using SagErpBlazor.DbClasses;
using SagErpBlazor.Models;
using SagErpBlazor.ModelServices;
using SagErpBlazor.Services;

namespace SagErpBlazor.ExtendedClasses
{
    public abstract class IControllerService
    {


        public static void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<StsTehnicalStaffService>();
            services.AddScoped<StsSupportStaffService>();
            services.AddScoped<StsTicketsRegisterService>();
        }

    }
}
