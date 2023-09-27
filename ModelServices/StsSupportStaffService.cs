using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SagErpBlazor.Data;
using SagErpBlazor.DbClasses;
using SagErpBlazor.Models;
using SagErpBlazor.Services;

namespace SagErpBlazor.ModelServices
{
    public class StsSupportStaffService
    {
        private readonly BlazorSupportTicketsContext _dbContext;
        private readonly ImageService _imageService;
        public StsSupportStaffService(BlazorSupportTicketsContext dbContext, ImageService imageService)
        {
            _dbContext = dbContext;
            _imageService = imageService;
        }
        public string MissingSequenceNo(int CompanyID)
        {
            using (BlazorSupportTicketsContext _NewDbContext = new BlazorSupportTicketsContext())
            {

                int? maxProjectNo = _NewDbContext.StsSupportStaffs
       .Where(x => x.CompanyId == CompanyID && x.LogSourceId == 0)
       .Select(item => (int?)item.SupportStaffId)
       .DefaultIfEmpty()
       .Max();


                int nextPrimaryKeyValue = (maxProjectNo ?? 0) + 1;

                return nextPrimaryKeyValue.ToString("D4");
            }
        }
        public async Task<List<StsSupportStaff>> GetAllSupportStaff (int CompanyID)
        {

            return await _dbContext.StsSupportStaffs.Where(x => x.CompanyId == CompanyID && x.LogSourceId == 0).ToListAsync();
        }
        public StsSupportStaff GetStaffMemberByID(int TechStaffId)
        {
            return _dbContext.StsSupportStaffs.FirstOrDefault(p => p.SupportStaffId == TechStaffId);
        }
        private Gluser CreateNewUserObject(StsSupportStaff _StsSupportStaff)
        {
            Gluser NewUser = new Gluser();
            NewUser.UserName = _StsSupportStaff.EmailAddress;
            NewUser.FirstName = _StsSupportStaff.Name;
            NewUser.PhoneNumber = _StsSupportStaff.PhoneNo;
            NewUser.CompanyId = _StsSupportStaff.CompanyId;
            NewUser.RoleId = _StsSupportStaff.RoleId;
            NewUser.EmailAddress = _StsSupportStaff.EmailAddress;
            NewUser.TimeStamp = DateTime.Now;
            NewUser.LastName = _StsSupportStaff.Name;
            NewUser.UserPassword = EncryptionHelper.DefaultEncrypt("12345", EncryptionHelper.DefaultKey);

            return NewUser;
        }
        public async Task<(CustomErrorClass, StsSupportStaff)> AddTechnicalStaff(StsSupportStaff TechStaff)
        {
            CustomErrorClass _CustomErrorClass = new CustomErrorClass();
            try
            {


                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        //Get Login ID first *******




                        TechStaff.SupportStaffId = 0;
                        TechStaff.RoleId = RoleDescriptionClass.GetRoleIDFromDescription("Support Engineer");
                        var NewUser = CreateNewUserObject(TechStaff);
                        GLUser _HelperLUser = new GLUser();
                        TechStaff.LoginId = _HelperLUser.GetLoginIDAfterSaving(NewUser);
                        _dbContext.StsSupportStaffs.Add(TechStaff);
                        await _dbContext.SaveChangesAsync();
                        transaction.Commit();

                       var ImageUrl = await _imageService.SaveImageAsync("SupportProfile", Convert.ToString(TechStaff.SupportStaffId), TechStaff.ProfileImage);


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


            return (_CustomErrorClass, TechStaff);
        }
        public async Task<(CustomErrorClass, StsSupportStaff)> UpdateTechStaffLogs(StsSupportStaff TechStaff)
        {
            CustomErrorClass _CustomErrorClass = new CustomErrorClass();
            try
            {
                using (BlazorSupportTicketsContext _NewDbContext = new BlazorSupportTicketsContext())
                {

                    StsSupportStaff LogRecord = new StsSupportStaff();
                    string JsonProject = JsonConvert.SerializeObject(TechStaff);
                    LogRecord = JsonConvert.DeserializeObject<StsSupportStaff>(JsonProject);
                    LogRecord.LogSourceId = TechStaff.SupportStaffId;
                    LogRecord.SupportStaffId = 0;

                    _NewDbContext.StsSupportStaffs.Add(LogRecord);
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
            return (_CustomErrorClass, TechStaff);
        }

        public async Task<(CustomErrorClass, StsSupportStaff)> UpdateTechnicalStaff(StsSupportStaff TechStaff)
        {
            CustomErrorClass _CustomErrorClass = new CustomErrorClass();
            using (var Transcation = _dbContext.Database.BeginTransaction())
            {
                try
                {


                
                    await UpdateTechStaffLogs(TechStaff);



                    TechStaff.ModifiedDate = System.DateTime.Now;
                    _dbContext.StsSupportStaffs.Update(TechStaff);
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
            return (_CustomErrorClass, TechStaff);
        }
        public async Task<CustomErrorClass> DeleteStaff(int TechStaffId)
        {
            CustomErrorClass _CustomErrorClass = new CustomErrorClass();
            try
            {
                var _StsSupportStaff = _dbContext.StsSupportStaffs.FirstOrDefault(p => p.SupportStaffId == TechStaffId);
                if (_StsSupportStaff != null)
                {

                    _StsSupportStaff.ModifiedDate = System.DateTime.Now;
                    _StsSupportStaff.ModifiedType = true;
                    _StsSupportStaff.LogSourceId = _StsSupportStaff.SupportStaffId;
                    _dbContext.Entry(_StsSupportStaff).State = EntityState.Modified;
                    await _dbContext.SaveChangesAsync();
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
