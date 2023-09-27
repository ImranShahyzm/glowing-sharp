using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SagErpBlazor.Data;
using SagErpBlazor.DbClasses;
using SagErpBlazor.Models;
using SagErpBlazor.Services;

namespace SagErpBlazor.ModelServices
{
    public class StsTehnicalStaffService
    {
        private readonly BlazorSupportTicketsContext _dbContext;
        private readonly ImageService _imageService;
        public StsTehnicalStaffService(BlazorSupportTicketsContext dbContext, ImageService imageService)
        {
            _dbContext = dbContext;
            _imageService = imageService;
        }
        public string MissingSequenceNo(int CompanyID)
        {
            using (BlazorSupportTicketsContext _NewDbContext = new BlazorSupportTicketsContext())
            {

                int? maxProjectNo = _NewDbContext.StsTechnicalStaffs
       .Where(x => x.CompanyId == CompanyID && x.LogSourceId == 0)
       .Select(item => (int?)item.TechStaffId)
       .DefaultIfEmpty()
       .Max();


                int nextPrimaryKeyValue = (maxProjectNo ?? 0) + 1;

                return nextPrimaryKeyValue.ToString("D4");
            }
        }
        public async Task<List<StsTechnicalStaff>> GetAllTechnicalStaff (int CompanyID)
        {
            using (BlazorSupportTicketsContext _NewDbContext = new BlazorSupportTicketsContext())
            {
                return await _NewDbContext.StsTechnicalStaffs.Where(x => x.CompanyId == CompanyID && x.LogSourceId == 0).ToListAsync();
              
            }
           
        }
        public StsTechnicalStaff GetStaffMemberByID(int TechStaffId)
        {
            using (BlazorSupportTicketsContext _NewDbContext = new BlazorSupportTicketsContext())
            {
                return _NewDbContext.StsTechnicalStaffs.FirstOrDefault(p => p.TechStaffId == TechStaffId);
            }
        }
        private Gluser CreateNewUserObject(StsTechnicalStaff _StsTechnicalStaff)
        {
            Gluser NewUser = new Gluser();
            NewUser.UserName = _StsTechnicalStaff.EmailAddress;
            NewUser.FirstName = _StsTechnicalStaff.StaffName;
            NewUser.PhoneNumber = _StsTechnicalStaff.PhoneNo;
            NewUser.CompanyId = _StsTechnicalStaff.CompanyId;
            NewUser.RoleId = _StsTechnicalStaff.RoleId;
            NewUser.EmailAddress = _StsTechnicalStaff.EmailAddress;
            NewUser.TimeStamp = DateTime.Now;
            NewUser.LastName = _StsTechnicalStaff.StaffName;
            NewUser.UserPassword = EncryptionHelper.DefaultEncrypt("12345", EncryptionHelper.DefaultKey);

            return NewUser;
        }
        public async Task<(CustomErrorClass, StsTechnicalStaff)> AddTechnicalStaff(StsTechnicalStaff TechStaff)
        {
            CustomErrorClass _CustomErrorClass = new CustomErrorClass();
            try
            {


                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        //Get Login ID first *******




                        TechStaff.TechStaffId = 0;
                        TechStaff.RoleId = RoleDescriptionClass.GetRoleIDFromDescription("Technical Engineer");


                        var NewUser = CreateNewUserObject(TechStaff);
                        GLUser _HelperLUser = new GLUser();


                        TechStaff.LoginId = _HelperLUser.GetLoginIDAfterSaving(NewUser);

                        _dbContext.StsTechnicalStaffs.Add(TechStaff);
                        await _dbContext.SaveChangesAsync();
                        transaction.Commit();

                        var ImageUrl = await _imageService.SaveImageAsync("StaffProfile", Convert.ToString(TechStaff.TechStaffId), TechStaff.ProfileImage);


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
        public async Task<(CustomErrorClass, StsTechnicalStaff)> UpdateTechStaffLogs(StsTechnicalStaff TechStaff)
        {
            CustomErrorClass _CustomErrorClass = new CustomErrorClass();
            try
            {
                using (BlazorSupportTicketsContext _NewDbContext = new BlazorSupportTicketsContext())
                {

                    StsTechnicalStaff LogRecord = new StsTechnicalStaff();
                    string JsonProject = JsonConvert.SerializeObject(TechStaff);
                    LogRecord = JsonConvert.DeserializeObject<StsTechnicalStaff>(JsonProject);
                    LogRecord.LogSourceId = TechStaff.TechStaffId;
                    LogRecord.TechStaffId = 0;

                    _NewDbContext.StsTechnicalStaffs.Add(LogRecord);
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

        public async Task<(CustomErrorClass, StsTechnicalStaff)> UpdateTechnicalStaff(StsTechnicalStaff TechStaff)
        {
            CustomErrorClass _CustomErrorClass = new CustomErrorClass();
            using (var Transcation = _dbContext.Database.BeginTransaction())
            {
                try
                {


                
                    await UpdateTechStaffLogs(TechStaff);



                    TechStaff.ModifiedDate = System.DateTime.Now;
                    _dbContext.StsTechnicalStaffs.Update(TechStaff);
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
                var _StsTechnicalStaff = _dbContext.StsTechnicalStaffs.FirstOrDefault(p => p.TechStaffId == TechStaffId);
                if (_StsTechnicalStaff != null)
                {

                    _StsTechnicalStaff.ModifiedDate = System.DateTime.Now;
                    _StsTechnicalStaff.ModifiedType = true;
                    _StsTechnicalStaff.LogSourceId = _StsTechnicalStaff.TechStaffId;
                    _dbContext.Entry(_StsTechnicalStaff).State = EntityState.Modified;
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
