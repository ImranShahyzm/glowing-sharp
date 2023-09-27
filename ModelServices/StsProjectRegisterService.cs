using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SagErpBlazor.DbClasses;
using SagErpBlazor.Models;

namespace SagErpBlazor.ModelServices
{
    public class StsProjectRegisterService
    {
        private readonly BlazorSupportTicketsContext _dbContext;

        public StsProjectRegisterService(BlazorSupportTicketsContext dbContext)
        {
            _dbContext = dbContext;
        }

      
        public async Task<List<StsProjectRegister>> GetAllProjects(int CompanyID)
        {
            return await _dbContext.StsProjectRegisters.Where(x=>x.CompanyId==CompanyID && x.LogSourceId==0).ToListAsync();
        }

        public string MissingSequenceNo(int CompanyID)
        {
            using (BlazorSupportTicketsContext _NewDbContext = new BlazorSupportTicketsContext())
            {

                var maxProjectNo = _NewDbContext.StsProjectRegisters.Where(x => x.CompanyId == CompanyID && x.LogSourceId==0).Max(item => item.ProjectNo) ?? 0; // Use null coalescing operator to handle null case

                var missingNumbers = Enumerable.Range(1, maxProjectNo + 1)
                                              .Except(_NewDbContext.StsProjectRegisters
                                                              .Where(item => item.ProjectNo != null && item.CompanyId == CompanyID && item.LogSourceId==0)
                                                              .Select(item => item.ProjectNo.Value))
                                              .FirstOrDefault();
                return missingNumbers.ToString("D4");
            }
        }
        public StsProjectRegister GetProjectById(int projectId)
        {
            return _dbContext.StsProjectRegisters.FirstOrDefault(p => p.ProjectId == projectId);
        }

        public async Task<(CustomErrorClass,StsProjectRegister)> AddProjectAsync(StsProjectRegister project)
        {
             CustomErrorClass _CustomErrorClass = new CustomErrorClass();
            try
            {
                var _MissingSequenceNo = MissingSequenceNo(project.CompanyId);
                project.ProjectNo = Convert.ToInt32(_MissingSequenceNo);
                project.ProjectCode =(_MissingSequenceNo);

                _dbContext.StsProjectRegisters.Add(project);
                await _dbContext.SaveChangesAsync();
                _CustomErrorClass.IsError = false;
                _CustomErrorClass.UserMessage= "Your Records have been Saved Successfully...";
             
            }
            catch (Exception ex)
            {
                _CustomErrorClass.IsError = true;
                _CustomErrorClass.UserMessage = "There was an Error Occured during the Saving Process....";
                _CustomErrorClass.ActualException = ex.Message;
            }
            return (_CustomErrorClass,project);
        }

        public async Task<(CustomErrorClass, StsProjectRegister)> UpdateProject(StsProjectRegister project)
        {
            CustomErrorClass _CustomErrorClass = new CustomErrorClass();
            try
            {

                await UpdateProjectLogs(project);
                project.ModifiedDate = DateTime.Now;
                project.ModifiedType = false;

                _dbContext.Entry(project).State = EntityState.Modified;
                 await  _dbContext.SaveChangesAsync();
                _CustomErrorClass.IsError = false;
                _CustomErrorClass.UserMessage = "Your Records have been Updated Successfully...";
            }
            catch (Exception ex)
            {
                _CustomErrorClass.IsError = true;
                _CustomErrorClass.UserMessage = "There was an Error Occured during the Saving Process....";
                _CustomErrorClass.ActualException = ex.Message;
            }
            return (_CustomErrorClass, project);
        }


        public async Task<(CustomErrorClass, StsProjectRegister)> UpdateProjectLogs(StsProjectRegister project)
        {
            CustomErrorClass _CustomErrorClass = new CustomErrorClass();
            try
            {
                using (BlazorSupportTicketsContext _NewDbContext = new BlazorSupportTicketsContext())
                {

                    StsProjectRegister LogProject = new StsProjectRegister();
                    string JsonProject = JsonConvert.SerializeObject(project);
                    LogProject = JsonConvert.DeserializeObject<StsProjectRegister>(JsonProject);
                    LogProject.LogSourceId = project.ProjectId;
                    LogProject.ProjectId = 0;

                    _NewDbContext.StsProjectRegisters.Add(LogProject);
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

        public async Task<CustomErrorClass> DeleteProject(int projectId)
        {
            CustomErrorClass _CustomErrorClass = new CustomErrorClass();
            try
            {
                var project = _dbContext.StsProjectRegisters.FirstOrDefault(p => p.ProjectId == projectId);
                if (project != null)
                {

                    project.ModifiedDate = System.DateTime.Now;
                    project.ModifiedType = true;
                    project.LogSourceId = project.ProjectId;
                    _dbContext.Entry(project).State = EntityState.Modified;
                    await _dbContext.SaveChangesAsync();

                    //_dbContext.StsProjectRegisters.Remove(project);
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
