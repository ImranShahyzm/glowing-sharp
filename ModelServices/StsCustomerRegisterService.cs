using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SagErpBlazor.Data;
using SagErpBlazor.DbClasses;
using SagErpBlazor.ExtendedClasses;
using SagErpBlazor.Models;
using SagErpBlazor.Services;

namespace SagErpBlazor.ModelServices
{
    public class StsCustomerRegisterService
    {
        private readonly BlazorSupportTicketsContext _dbContext;
        private readonly ImageService _imageService;
        public StsCustomerRegisterService(BlazorSupportTicketsContext dbContext, ImageService imageService)
        {
            _dbContext = dbContext;
            _imageService = imageService;
        }
        public async Task<List<StsCustomerRegister>> GetAllCustomers(int CompanyID)
        {

            return await _dbContext.StsCustomerRegisters.Where(x => x.CompanyId == CompanyID && x.LogSourceId == 0).ToListAsync();
        }
        public async Task<List<StsCustomerRegisterDetail>> GetAllCustomersDetail(int CompanyID)
        {

            return await _dbContext.StsCustomerRegisterDetails.ToListAsync();
        }

        public string MissingSequenceNo(int CompanyID)
        {
            using (BlazorSupportTicketsContext _NewDbContext = new BlazorSupportTicketsContext())
            {

                int? maxProjectNo = _NewDbContext.StsCustomerRegisters
       .Where(x => x.CompanyId == CompanyID && x.LogSourceId == 0)
       .Select(item => (int?)item.CustomerId)
       .DefaultIfEmpty() 
       .Max();

              
                int nextPrimaryKeyValue = (maxProjectNo ?? 0) + 1;

                return nextPrimaryKeyValue.ToString("D4");
            }
        }
        public StsCustomerRegister GetCustomerByID(int CustomerId)
        {
            return _dbContext.StsCustomerRegisters.FirstOrDefault(p => p.CustomerId == CustomerId);
        }

        private Gluser CreateNewUserObject(StsCustomerRegister _stsCustomerRegister)
        {
            Gluser NewUser = new Gluser();
            NewUser.UserName = _stsCustomerRegister.Pocemail;
            NewUser.FirstName = _stsCustomerRegister.Pocname;
            NewUser.PhoneNumber = _stsCustomerRegister.Pocnumber;
            NewUser.CompanyId = _stsCustomerRegister.CompanyId;
            NewUser.RoleId = _stsCustomerRegister.RoleId;
            NewUser.EmailAddress = _stsCustomerRegister.Pocemail;
            NewUser.TimeStamp = DateTime.Now;
            NewUser.LastName= _stsCustomerRegister.Pocname;
            NewUser.UserPassword = EncryptionHelper.DefaultEncrypt("12345", EncryptionHelper.DefaultKey);

            return NewUser;
        }
        public async Task<(CustomErrorClass, StsCustomerRegister)> AddCustomersAsync(StsCustomerRegister Customer, List<StsCustomerRegisterDetail> DetailData)
        {
            CustomErrorClass _CustomErrorClass = new CustomErrorClass();
            try
            {
               
               
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        //Get Login ID first *******
                     



                        Customer.CustomerId = 0;
                        Customer.RoleId = RoleDescriptionClass.GetRoleIDFromDescription("Client");


                        var NewUser = CreateNewUserObject(Customer);
                        GLUser _HelperLUser = new GLUser();
                       

                        Customer.LoginId = _HelperLUser.GetLoginIDAfterSaving(NewUser); 

                        _dbContext.StsCustomerRegisters.Add(Customer);
                        await _dbContext.SaveChangesAsync();

                        int CustomerId = Customer.CustomerId;
                        DetailData.ForEach(DetailData => DetailData.CustomerId = CustomerId);

                         _dbContext.StsCustomerRegisterDetails.AddRange(DetailData);
                        await _dbContext.SaveChangesAsync();
                        transaction.Commit();

                        var ImageUrl = await _imageService.SaveImageAsync("CompanyLogos", Convert.ToString(Customer.CustomerId), Customer.Companylogo);


                        _CustomErrorClass.IsError = false;
                        _CustomErrorClass.UserMessage = "Your Records have been Saved Successfully...";
                    }
                    catch (Exception ex)
                    {

                        transaction.Rollback();
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                _CustomErrorClass.IsError = true;
                _CustomErrorClass.UserMessage = "There was an Error Occured during the Saving Process....";
                _CustomErrorClass.ActualException = ex.Message;
            }

            
            return (_CustomErrorClass, Customer);
        }

        public async Task<(CustomErrorClass, StsCustomerRegister)> UpdateCustomer(StsCustomerRegister project ,List<StsCustomerRegisterDetail> DetailData)
        {
            CustomErrorClass _CustomErrorClass = new CustomErrorClass();
            using (var Transcation = _dbContext.Database.BeginTransaction())
            {
                try
                {


                    var recordsToDelete = _dbContext.StsCustomerRegisterDetails.Where(d => d.CustomerId == project.CustomerId).ToList();
                    _dbContext.StsCustomerRegisterDetails.RemoveRange(recordsToDelete);
                    _dbContext.SaveChanges();

                    await UpdateCustomerLogs(project);


                    DetailData.ForEach(DetailData => DetailData.CustomerId = project.CustomerId);
                    DetailData.ForEach(DetailData => DetailData.CustomerDetailId = 0);

                    _dbContext.StsCustomerRegisterDetails.AddRange(DetailData);
                    await _dbContext.SaveChangesAsync();
                    project.ModifiedDate=System.DateTime.Now;
                    _dbContext.StsCustomerRegisters.Update(project);
                    await _dbContext.SaveChangesAsync();

                    Transcation.Commit();
                    _CustomErrorClass.IsError = false;
                    _CustomErrorClass.UserMessage = "Your Records have been Updated Successfully...";
                }
                catch (Exception ex)
                {
                    Transcation.Rollback();
                    _CustomErrorClass.IsError = true;
                    _CustomErrorClass.UserMessage = "There was an Error Occured during the Saving Process....";
                    _CustomErrorClass.ActualException = ex.Message;
                }
               
            }
            return (_CustomErrorClass, project);
        }


        public async Task<(CustomErrorClass, StsCustomerRegister)> UpdateCustomerLogs(StsCustomerRegister project)
        {
            CustomErrorClass _CustomErrorClass = new CustomErrorClass();
            try
            {
                using (BlazorSupportTicketsContext _NewDbContext = new BlazorSupportTicketsContext())
                {

                    StsCustomerRegister LogProject = new StsCustomerRegister();
                    string JsonProject = JsonConvert.SerializeObject(project);
                    LogProject = JsonConvert.DeserializeObject<StsCustomerRegister>(JsonProject);
                    LogProject.LogSourceId = project.CustomerId;
                    LogProject.CustomerId = 0;

                    _NewDbContext.StsCustomerRegisters.Add(LogProject);
                    await _NewDbContext.SaveChangesAsync();
                    _CustomErrorClass.IsError = false;
                    _CustomErrorClass.UserMessage = "Your Records have been Updated Successfully...";
                }
            }
            catch (Exception ex)
            {
                _CustomErrorClass.IsError = true;
                _CustomErrorClass.UserMessage = "There was an Error Occured during the Saving Process....";
                _CustomErrorClass.ActualException = ex.Message;
            }
            return (_CustomErrorClass, project);
        }

        public async Task<CustomErrorClass> DeleteProject(int CustomerId)
        {
            CustomErrorClass _CustomErrorClass = new CustomErrorClass();
            try
            {
                var project = _dbContext.StsCustomerRegisters.FirstOrDefault(p => p.CustomerId == CustomerId);
                if (project != null)
                {

                    project.ModifiedDate = System.DateTime.Now;
                    project.ModifiedType = true;
                    project.LogSourceId = project.CustomerId;
                    _dbContext.Entry(project).State = EntityState.Modified;
                    await _dbContext.SaveChangesAsync();

                    //_dbContext.StsCustomerRegisters.Remove(project);
                    //_dbContext.SaveChanges();
                    _CustomErrorClass.IsError = false;
                    _CustomErrorClass.UserMessage = "Your Selected Records are Deleted Successfully....";
                }

            }
            catch (Exception ex)
            {
                _CustomErrorClass.IsError = true;
                _CustomErrorClass.UserMessage = "There was an Error Occured during the Delete Process....";
                _CustomErrorClass.ActualException = ex.Message;
            }
            return (_CustomErrorClass);
        }

    }
}
