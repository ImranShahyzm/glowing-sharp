using Blazorise;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SagErpBlazor.Data;
using SagErpBlazor.DbClasses;
using SagErpBlazor.ExtendedClasses;
using SagErpBlazor.Models;
using SagErpBlazor.Services;
using SagErpBlazor.StaticEnums;
using System.ComponentModel.Design;
using System.Net;

namespace SagErpBlazor.ModelServices
{
    public class StsTicketsRegisterService
    {
        private readonly BlazorSupportTicketsContext _dbContext;
        private readonly ImageService _imageService;
        private IMailInterface _MailInterface;
        private IUserService _UserService;
        private readonly MailSettings _mailConfig;
        public StsTicketsRegisterService(BlazorSupportTicketsContext dbContext, ImageService imageService, IMailInterface mailInterface,IUserService userService, MailSettings mailConfig)
        {
            _dbContext = dbContext;
            _imageService = imageService;
            _MailInterface = mailInterface;
            _UserService = userService;
            _mailConfig = mailConfig;
        }

        public async Task<(StsTicketsRegister,List<StsTicketsRegisterAttachment>)> GetTicketsMasterDetailByID(int TicketID)
        {
            StsTicketsRegister _StsTicketsRegister = new StsTicketsRegister();
            List<StsTicketsRegisterAttachment> _StsTicketsRegisterAttachments = new List<StsTicketsRegisterAttachment>();

            using (BlazorSupportTicketsContext _NewDbContext = new BlazorSupportTicketsContext())
            {
                try
                {
                     _StsTicketsRegister = await _NewDbContext.StsTicketsRegisters.Where(x => x.TicketId == TicketID).FirstOrDefaultAsync();
                     _StsTicketsRegisterAttachments = await _NewDbContext.StsTicketsRegisterAttachments.Where(x => x.TicketId == TicketID).ToListAsync();


                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return (_StsTicketsRegister, _StsTicketsRegisterAttachments);
            
        }
        private string GetTicketsMessageBody(StsTicketsRegister _StsTicketsRegister)
        {
            string strBody = "";
            if (!string.IsNullOrEmpty(_StsTicketsRegister.EmailAddress))
            {
                 strBody = @"
<table border=""0"" width=""100%"" cellpadding=""0"" bgcolor=""#ededed"" style=""padding:20px;background-color:#ededed"" summary=""o_mail_notification"">
    <tbody>
        <tr>
            <td align=""center"" style=""min-width:590px"">
                <table width=""590"" border=""0"" cellpadding=""0"" bgcolor=""#875A7B"" style=""min-width:590px;background-color:#f099b3;padding:20px"">
                    <tbody>
                        <tr>
                            <td valign=""middle"">
                                <span style=""font-size:20px;color:#70061e;font-weight:bold;text-align:left"">
                                    Corbis Soft Support Ticket Created # "+ _StsTicketsRegister.TicketNo + @"
                                </span>
                            </td>
                            <td valign=""middle"" align=""right"">
                                <img src="+_mailConfig.TicketLogo+@" style=""padding:0px;margin:0px;height:auto;width:80px"" alt=""Corbis Soft"" class=""CToWUd"" data-bit=""iit"">
                            </td>
                        </tr>
                    </tbody>
                </table>
            </td>
        </tr>

        <tr>
            <td align=""center"" style=""min-width:590px"">
                <font color=""#888888"">
                </font>
                <font color=""#888888"">
                </font>
                <table width=""590"" border=""0"" cellpadding=""0"" bgcolor=""#ffffff"" style=""min-width:590px;background-color:rgb(255,255,255);padding:20px"">
                    <tbody>
                        <tr>
                            <td valign=""top"" style=""font-family:Arial,Helvetica,sans-serif;color:#555;font-size:14px"">
                                <br>
                                <p>
                                    Dear Customer,
                                </p>
                                <br>
                                <p>
                                    We hope this message finds you well. We would like to inform you that a support ticket has been created for the issue you reported. Your issue has been assigned Ticket Number "+_StsTicketsRegister.TicketNo+@".
                                </p>
                                <br>
                                <span style=""font-size:18px;color:#875a7b;font-weight:bold;text-align:left"">Ticket Information!</span><br>
                                <p><span style=""font-size:12px;color:#875a7b;font-weight:bold;text-align:left"">Subject line:</span> "+_StsTicketsRegister.TicketName+@".</p>
                                <p><span style=""font-size:12px;color:#875a7b;font-weight:bold;text-align:left"">Project Name:</span> "+_StsTicketsRegister.ProjectName+@".</p>
                                <p><span style=""font-size:12px;color:#875a7b;font-weight:bold;text-align:left"">Description :</span> "+_StsTicketsRegister.Description+@" .</p>
                                <center>
                                    <br>
                                    <p><a style=""background-color:#1abc9c;padding:20px;text-decoration:none;color:#fff;border-radius:5px;font-size:16px"" href="" target=""_blank"">Click to Check the Status</a></p>
                                    <br><br><br>
                                </center>
                            </td>
                        </tr>
                    </tbody>
                </table>
                <font color=""#888888"">
                </font>
            </td>
        </tr>
        <tr>
        </tr>
    </tbody>
</table>";

                

            }
            return strBody;
        }

        public string MissingSequenceNo(int CompanyID)
        {
            using (BlazorSupportTicketsContext _NewDbContext = new BlazorSupportTicketsContext())
            {
                try
                {
                    var QueryResult = _NewDbContext.StsTicketsRegisters.Where(x => x.CompanyId == CompanyID && x.LogSourceId == 0);
                    var maxProjectNo = 0;
                    if (QueryResult.Any())
                    {
                        maxProjectNo = QueryResult.Max(item => item.TicketNo);
                    }
                   
                        var missingNumbers = Enumerable.Range(388881, maxProjectNo + 1)
                                                      .Except(_NewDbContext.StsTicketsRegisters
                                                                      .Where(item => item.TicketNo != 0 && item.CompanyId == CompanyID && item.LogSourceId == 0)
                                                                      .Select(item => item.TicketNo))
                                                      .FirstOrDefault();

                        return missingNumbers.ToString("D4");
                    
                }catch(Exception e)
                {
                    return e.Message;
                }
            }
        }
        public async Task<List<StsCustomerRegister>> GettAllCustomersList(int CompanyID,string UserRole = "",int UserID=0)
        {
            int RoleID = 0;
            if (!string.IsNullOrEmpty(UserRole))
            {
                RoleID = RoleDescriptionClass.GetRoleIDFromDescription(UserRole);

            }
            using (BlazorSupportTicketsContext _NewDbContext = new BlazorSupportTicketsContext())
            {
                if (RoleID == 4)
                {
                    return await _NewDbContext.StsCustomerRegisters.Where(x => x.CompanyId == CompanyID && x.LogSourceId == 0 && x.LoginId==UserID).ToListAsync();
                }
                else
                {
                    return await _NewDbContext.StsCustomerRegisters.Where(x => x.CompanyId == CompanyID && x.LogSourceId == 0).ToListAsync();
                }

            }

        }


        public async Task<List<StsTicketsType>> GetTicketsType()
        {
            
            using (BlazorSupportTicketsContext _NewDbContext = new BlazorSupportTicketsContext())
            {
              
                    return await _NewDbContext.StsTicketsTypes.ToListAsync();
            }

        }
        public async Task<List<StsProjectRegister>> GetAllProjects(int CompanyID)
        {
            return await _dbContext.StsProjectRegisters.Where(x => x.CompanyId == CompanyID && x.LogSourceId == 0).ToListAsync();
        }
        public async Task<List<StsCustomerRegisterDetail>> GetAllAssignedProjects (int CompanyID)
        {
            using (var NewdbContext = new BlazorSupportTicketsContext()) 
            {
               
                var query = from customerDetail in NewdbContext.StsCustomerRegisterDetails
                            join projectRegister in NewdbContext.StsProjectRegisters
                            on customerDetail.ProjectId equals projectRegister.ProjectId
                            where projectRegister.CompanyId == CompanyID
                            select new StsCustomerRegisterDetail
                            {
                                ProjectId=projectRegister.ProjectId,
                                ProjectDescription=customerDetail.ProjectDescription,
                                CustomerId=customerDetail.CustomerId
                            };
                List<StsCustomerRegisterDetail> resultList = await query.ToListAsync();

                return resultList;
            }
        }

        public async Task<(CustomErrorClass, StsTicketsRegister)> AddNewTicketsAsync(StsTicketsRegister ticketsRegister, List<StsTicketsRegisterAttachment> DetailData)
        {
            CustomErrorClass _CustomErrorClass = new CustomErrorClass();
            try
            {
                using (BlazorSupportTicketsContext _NewDbContext = new BlazorSupportTicketsContext())
                {
                    using (var transaction = _NewDbContext.Database.BeginTransaction())
                    {
                        try
                        {
                            //Get Primary ID first *******




                            ticketsRegister.TicketId = 0;
                            ticketsRegister.EmailAddress = await _UserService.GetUserEmailAddress(ticketsRegister.RegisteredBy??0);
                            //ticketsRegister.TicketNo = Convert.ToInt32(MissingSequenceNo(ticketsRegister.CompanyId??0));
                            _NewDbContext.StsTicketsRegisters.Add(ticketsRegister);
                            await _NewDbContext.SaveChangesAsync();
                            ticketsRegister.TicketNo = ticketsRegister.TicketId;
                            int TicketID = ticketsRegister.TicketId;
                            DetailData.ForEach(DetailData => DetailData.TicketId = TicketID);

                            _NewDbContext.StsTicketsRegisterAttachments.AddRange(DetailData);
                            await _NewDbContext.SaveChangesAsync();
                            transaction.Commit();

                        

                            _CustomErrorClass.IsError = false;
                            _CustomErrorClass.UserMessage = "Your Records have been Saved Successfully...";


                            if (!_CustomErrorClass.IsError)
                            {
                                if (!string.IsNullOrEmpty(ticketsRegister.EmailAddress))
                                {
                                    string MessageBody = GetTicketsMessageBody(ticketsRegister);

                                    _CustomErrorClass.UserMessage = await _MailInterface.SendEmailAsync(ticketsRegister.EmailAddress, "Support Ticket # " + Convert.ToString(ticketsRegister.TicketNo), MessageBody);
                                }
                            }
                        }
                        catch (Exception ex)
                        {

                            transaction.Rollback();
                            throw;
                        }
                    }

                }

                
            }
            catch (Exception ex)
            {
                _CustomErrorClass.IsError = true;
                _CustomErrorClass.UserMessage = "There was an Error Occured during the Saving Process....";
                _CustomErrorClass.ActualException = ex.Message;
                if(ex.InnerException!=null)
                {
                    _CustomErrorClass.InnerException = Convert.ToString(ex.InnerException.Message);
                }
            }


            return (_CustomErrorClass, ticketsRegister);
        }


        public async Task <CustomErrorClass> UpdateTicketsLogs(int TicketID)
        {
            CustomErrorClass _customErrorClass = new CustomErrorClass();
            using (BlazorSupportTicketsContext _UpdateDbContext=new BlazorSupportTicketsContext())
            {
                using (var transaction = _UpdateDbContext.Database.BeginTransaction())
                {
                    try
                    {
                        StsTicketsRegister stsTicketsRegister = new StsTicketsRegister();
                        List<StsTicketsRegisterAttachment> stsTicketsRegisterAttachments = new List<StsTicketsRegisterAttachment>();
                        (stsTicketsRegister, stsTicketsRegisterAttachments) = await GetTicketsMasterDetailByID(TicketID);
                        string JsonObject = JsonConvert.SerializeObject(stsTicketsRegister);
                        var LogRecords = JsonConvert.DeserializeObject<StsTicketsRegister>(JsonObject);
                        string JsonObjectDetail = JsonConvert.SerializeObject(stsTicketsRegisterAttachments);
                        var TicketDetailRecords = JsonConvert.DeserializeObject<List<StsTicketsRegisterAttachment>>(JsonObjectDetail);

                        LogRecords.LogSourceId = TicketID;
                        LogRecords.TicketId = 0;

                        _UpdateDbContext.StsTicketsRegisters.Add(LogRecords);
                        await _UpdateDbContext.SaveChangesAsync();
                        LogRecords.TicketNo = LogRecords.TicketId;
                        int TicketIDLog = LogRecords.TicketId;
                        TicketDetailRecords.ForEach(DetailData => {
                            DetailData.TicketId = TicketIDLog;
                            DetailData.IsLog = true;
                            DetailData.TicketAttacmentId = 0;
                        });
                        _UpdateDbContext.StsTicketsRegisterAttachments.AddRange(TicketDetailRecords);
                        await _UpdateDbContext.SaveChangesAsync();
                        transaction.Commit();
                        _customErrorClass.IsError = false;
                        
                    }
                    catch (Exception e)
                    {
                        _customErrorClass.IsError = true;
                        _customErrorClass.ActualException = e.Message;
                        _customErrorClass.InnerException = e.InnerException.Message;
                        _customErrorClass.UserMessage = "Records Not Added in Logs";
                       
                    }
                }


            }
            return _customErrorClass;

        }
        public async Task<(CustomErrorClass, StsTicketsRegister)> UpdateTicketsAsync(StsTicketsRegister ticketsRegister, List<StsTicketsRegisterAttachment> DetailData)
        {
            CustomErrorClass _CustomErrorClass = new CustomErrorClass();
            _CustomErrorClass = await UpdateTicketsLogs(ticketsRegister.TicketId);
            if (!_CustomErrorClass.IsError)
            {
                try
                {

                    using (BlazorSupportTicketsContext _NewDbContext = new BlazorSupportTicketsContext())
                    {
                        using (var Transcation = _NewDbContext.Database.BeginTransaction())
                        {
                            try
                            {
                                //Get Primary ID first *******

                                var recordsToDelete = _NewDbContext.StsTicketsRegisterAttachments.Where(d => d.TicketId == ticketsRegister.TicketId).ToList();
                                _NewDbContext.StsTicketsRegisterAttachments.RemoveRange(recordsToDelete);
                                _NewDbContext.SaveChanges();


                                DetailData.ForEach(DetailData => {
                                    DetailData.TicketId = ticketsRegister.TicketId;
                                    DetailData.TicketAttacmentId = 0;
                                    DetailData.IsLog = false;
                                });

                                _NewDbContext.StsTicketsRegisterAttachments.AddRange(DetailData);
                                await _NewDbContext.SaveChangesAsync();
                                ticketsRegister.ModifiedDate = System.DateTime.Now;

                                _NewDbContext.StsTicketsRegisters.Update(ticketsRegister);
                                await _NewDbContext.SaveChangesAsync();

                                Transcation.Commit();
                                _CustomErrorClass.IsError = false;
                                _CustomErrorClass.UserMessage = "Your Records have been Updated Successfully...";


                            


                                if (!_CustomErrorClass.IsError)
                                {
                                    if (!string.IsNullOrEmpty(ticketsRegister.EmailAddress))
                                    {
                                        string MessageBody = GetTicketsMessageBody(ticketsRegister);

                                        _CustomErrorClass.UserMessage = await _MailInterface.SendEmailAsync(ticketsRegister.EmailAddress, "Updated Ticket # " + Convert.ToString(ticketsRegister.TicketNo), MessageBody);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {

                                Transcation.Rollback();
                                throw;
                            }
                        }

                    }


                }
                catch (Exception ex)
                {
                    _CustomErrorClass.IsError = true;
                    _CustomErrorClass.UserMessage = "There was an Error Occured during the Saving Process....";
                    _CustomErrorClass.ActualException = ex.Message;
                    if (ex.InnerException != null)
                    {
                        _CustomErrorClass.InnerException = Convert.ToString(ex.InnerException.Message);
                    }
                }
            }


            return (_CustomErrorClass, ticketsRegister);
        }

        public async Task<CustomErrorClass> DeleteTicketsAsync(StsTicketsRegister ticketsRegister, List<StsTicketsRegisterAttachment> DetailData)
        {
            CustomErrorClass _CustomErrorClass = new CustomErrorClass();
           
            if (!_CustomErrorClass.IsError)
            {
                try
                {

                    using (BlazorSupportTicketsContext _NewDbContext = new BlazorSupportTicketsContext())
                    {
                        using (var Transcation = _NewDbContext.Database.BeginTransaction())
                        {
                            try
                            {
                                //Get Primary ID first *******

                                var recordsToDelete = _NewDbContext.StsTicketsRegisterAttachments.Where(d => d.TicketId == ticketsRegister.TicketId).ToList();
                                _NewDbContext.StsTicketsRegisterAttachments.RemoveRange(recordsToDelete);
                                _NewDbContext.SaveChanges();


                                DetailData.ForEach(DetailData => {
                                    DetailData.TicketId = ticketsRegister.TicketId;
                                    DetailData.TicketAttacmentId = 0;
                                    DetailData.IsLog = true;
                                });

                                _NewDbContext.StsTicketsRegisterAttachments.AddRange(DetailData);
                                await _NewDbContext.SaveChangesAsync();
                                ticketsRegister.ModifiedDate = System.DateTime.Now;
                                ticketsRegister.LogSourceId = ticketsRegister.TicketId;

                                _NewDbContext.StsTicketsRegisters.Update(ticketsRegister);
                                await _NewDbContext.SaveChangesAsync();

                                Transcation.Commit();
                                _CustomErrorClass.IsError = false;
                                _CustomErrorClass.UserMessage = "Your Records have been Deleted Successfully...";
                            }
                            catch (Exception ex)
                            {

                                Transcation.Rollback();
                                throw;
                            }
                        }

                    }


                }
                catch (Exception ex)
                {
                    _CustomErrorClass.IsError = true;
                    _CustomErrorClass.UserMessage = "There was an Error Occured during the Delete Process....";
                    _CustomErrorClass.ActualException = ex.Message;
                    if (ex.InnerException != null)
                    {
                        _CustomErrorClass.InnerException = Convert.ToString(ex.InnerException.Message);
                    }
                }
            }


            return _CustomErrorClass;
        }

        public async Task<List<VwTicketsStatusInfo>> GetAllTickets(int CompanyID,int CustomerID)
        {
            using (BlazorSupportTicketsContext _NewDbContext = new BlazorSupportTicketsContext())
            {

                if (CustomerID > 0)
                {
                    return await _NewDbContext.VwTicketsStatusInfos.Where(x => x.CompanyId == CompanyID && x.LogSourceId == 0 && x.CustomerId == CustomerID).ToListAsync();
                }
                else
                {
                    return await _NewDbContext.VwTicketsStatusInfos.Where(x => x.CompanyId == CompanyID && x.LogSourceId == 0).ToListAsync();
                }
            }
        }

        public async Task<List<VwTicketsStatusInfo>> GetAllTicketsWithStatus(int CompanyID, int CustomerID,int StatusID)
        {
            using (BlazorSupportTicketsContext _NewDbContext = new BlazorSupportTicketsContext())
            {

                if (CustomerID > 0)
                {
                    return await _NewDbContext.VwTicketsStatusInfos.Where(x => x.CompanyId == CompanyID && x.LogSourceId == 0 && x.CustomerId == CustomerID && x.TicketStatusId==StatusID).ToListAsync();
                }
                else
                {
                    return await _NewDbContext.VwTicketsStatusInfos.Where(x => x.CompanyId == CompanyID && x.LogSourceId == 0 && x.TicketStatusId == StatusID).ToListAsync();
                }
            }
        }

        public async Task<List<VwTicketsStatusInfo>> GetAllTicketsWithClosedStatus(int CompanyID, int CustomerID, int StatusID)
        {
            using (BlazorSupportTicketsContext _NewDbContext = new BlazorSupportTicketsContext())
            {

                List<int> TicketStatusIds = new List<int>
                                            {
                                                (int)TicketStatus.Closed,
                                                (int)TicketStatus.VerifiedOrClosed
                                            };
                if (CustomerID > 0)
                {
                    return await _NewDbContext.VwTicketsStatusInfos.Where(x => x.CompanyId == CompanyID && x.LogSourceId == 0 && x.CustomerId == CustomerID && TicketStatusIds.Contains(x.TicketStatusId)).ToListAsync();
                }
                else
                {
                    return await _NewDbContext.VwTicketsStatusInfos.Where(x => x.CompanyId == CompanyID && x.LogSourceId == 0 && TicketStatusIds.Contains(x.TicketStatusId)).ToListAsync();
                }
            }
        }

        public async Task<List<VwTicketsStatusInfo>> GetAllTicketsWithPendingStatus(int CompanyID, int CustomerID, int StatusID)
        {
            using (BlazorSupportTicketsContext _NewDbContext = new BlazorSupportTicketsContext())
            {

                List<int> TicketStatusIds = new List<int>
                                            {
                                                (int)TicketStatus.Assigned,
                                                (int)TicketStatus.OnHold,
                                                (int)TicketStatus.WaitingForCustomer,
                                                (int)TicketStatus.Open,
                                                (int)TicketStatus.ReOpen
                                            };
                if (CustomerID > 0)
                {
                    return await _NewDbContext.VwTicketsStatusInfos.Where(x => x.CompanyId == CompanyID && x.LogSourceId == 0 && x.CustomerId == CustomerID && TicketStatusIds.Contains(x.TicketStatusId)).ToListAsync();
                }
                else
                {
                    return await _NewDbContext.VwTicketsStatusInfos.Where(x => x.CompanyId == CompanyID && x.LogSourceId == 0 && TicketStatusIds.Contains(x.TicketStatusId)).ToListAsync();
                }
            }
        }
        public async Task<List<VwTicketsStatusInfo>> GetAllTicketsWithActiveStatus(int CompanyID, int CustomerID, int StatusID)
        {
            using (BlazorSupportTicketsContext _NewDbContext = new BlazorSupportTicketsContext())
            {

                List<int> ActiveTicketStatusIds = new List<int>
                                            {
                                                (int)TicketStatus.Deployed,
                                                (int)TicketStatus.Resolved,
                                                (int)TicketStatus.InProgress,
                                                (int)TicketStatus.UnderQA
                                            };
                if (CustomerID > 0)
                {
                    return await _NewDbContext.VwTicketsStatusInfos.Where(x => x.CompanyId == CompanyID && x.LogSourceId == 0 && x.CustomerId == CustomerID && ActiveTicketStatusIds.Contains(x.TicketStatusId)).ToListAsync();
                }
                else
                {
                    return await _NewDbContext.VwTicketsStatusInfos.Where(x => x.CompanyId == CompanyID && x.LogSourceId == 0 && ActiveTicketStatusIds.Contains(x.TicketStatusId)).ToListAsync();
                }
            }
        }


    }
}
