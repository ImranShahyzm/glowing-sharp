using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SagErpBlazor.Models;

public partial class BlazorSupportTicketsContext : DbContext
{
    public BlazorSupportTicketsContext()
    {
    }

    public BlazorSupportTicketsContext(DbContextOptions<BlazorSupportTicketsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Gluser> Glusers { get; set; }

    public virtual DbSet<StsCustomerRegister> StsCustomerRegisters { get; set; }

    public virtual DbSet<StsCustomerRegisterDetail> StsCustomerRegisterDetails { get; set; }

    public virtual DbSet<StsGlcompany> StsGlcompanies { get; set; }

    public virtual DbSet<StsProjectRegister> StsProjectRegisters { get; set; }

    public virtual DbSet<StsRefreshToken> StsRefreshTokens { get; set; }

    public virtual DbSet<StsRole> StsRoles { get; set; }

    public virtual DbSet<StsSupportStaff> StsSupportStaffs { get; set; }

    public virtual DbSet<StsSupportStaffDetail> StsSupportStaffDetails { get; set; }

    public virtual DbSet<StsSystemConfiguration> StsSystemConfigurations { get; set; }

    public virtual DbSet<StsTechnicalStaff> StsTechnicalStaffs { get; set; }

    public virtual DbSet<StsTicketChannel> StsTicketChannels { get; set; }

    public virtual DbSet<StsTicketPrority> StsTicketProrities { get; set; }

    public virtual DbSet<StsTicketStatus> StsTicketStatuses { get; set; }

    public virtual DbSet<StsTicketsAssigning> StsTicketsAssignings { get; set; }

    public virtual DbSet<StsTicketsRegister> StsTicketsRegisters { get; set; }

    public virtual DbSet<StsTicketsRegisterAttachment> StsTicketsRegisterAttachments { get; set; }

    public virtual DbSet<StsTicketsType> StsTicketsTypes { get; set; }

    public virtual DbSet<StsTicketsWorkList> StsTicketsWorkLists { get; set; }

    public virtual DbSet<VwTicketsStatusInfo> VwTicketsStatusInfos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();
        optionsBuilder.UseSqlServer(configuration.GetConnectionString("ConnectionStringName"));
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Gluser>(entity =>
        {
            entity.HasKey(e => e.Userid);

            entity.ToTable("GLUser");

            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");
            entity.Property(e => e.EmailAddress).HasMaxLength(250);
            entity.Property(e => e.FirstName).HasMaxLength(250);
            entity.Property(e => e.LastName).HasMaxLength(250);
            entity.Property(e => e.PhoneNumber).HasMaxLength(150);
            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.TimeStamp).HasColumnType("datetime");
            entity.Property(e => e.UserName)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<StsCustomerRegister>(entity =>
        {
            entity.HasKey(e => e.CustomerId);

            entity.ToTable("STS_CustomerRegister");

            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");
            entity.Property(e => e.CustomerName).HasMaxLength(500);
            entity.Property(e => e.LogSourceId).HasColumnName("LogSourceID");
            entity.Property(e => e.LoginId).HasColumnName("LoginID");
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.Pocemail)
                .HasMaxLength(500)
                .HasColumnName("POCEmail");
            entity.Property(e => e.Pocname)
                .HasMaxLength(500)
                .HasColumnName("POCName");
            entity.Property(e => e.Pocnumber)
                .HasMaxLength(500)
                .HasColumnName("POCNumber");
            entity.Property(e => e.RegisterDate).HasColumnType("datetime");
            entity.Property(e => e.RoleId).HasColumnName("RoleID");
        });

        modelBuilder.Entity<StsCustomerRegisterDetail>(entity =>
        {
            entity.HasKey(e => e.CustomerDetailId);

            entity.ToTable("STS_CustomerRegisterDetail");

            entity.Property(e => e.CustomerDetailId).HasColumnName("CustomerDetailID");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.ProjectDescription).HasMaxLength(500);
            entity.Property(e => e.ProjectId).HasColumnName("ProjectID");
        });

        modelBuilder.Entity<StsGlcompany>(entity =>
        {
            entity.HasKey(e => e.CompanyId).HasName("PK_GLCompany");

            entity.ToTable("STS_GLCompany");

            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");
            entity.Property(e => e.Address).HasMaxLength(500);
            entity.Property(e => e.CompanyImage).HasMaxLength(100);
            entity.Property(e => e.CsscolorStyle)
                .HasMaxLength(100)
                .HasColumnName("CSSColorStyle");
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.FaxNo).HasMaxLength(250);
            entity.Property(e => e.Phone).HasMaxLength(50);
            entity.Property(e => e.PoBoxNo).HasMaxLength(250);
            entity.Property(e => e.PostalCode).HasMaxLength(250);
            entity.Property(e => e.ShortTitle)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.SupportEmail).HasMaxLength(50);
            entity.Property(e => e.Title).HasMaxLength(500);
            entity.Property(e => e.Website)
                .HasMaxLength(100)
                .HasColumnName("website");
        });

        modelBuilder.Entity<StsProjectRegister>(entity =>
        {
            entity.HasKey(e => e.ProjectId);

            entity.ToTable("STS_ProjectRegister");

            entity.Property(e => e.ProjectId).HasColumnName("ProjectID");
            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");
            entity.Property(e => e.LogSourceId).HasColumnName("LogSourceID");
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.ProjectCode).HasMaxLength(50);
            entity.Property(e => e.ProjectDeploymentDate).HasColumnType("datetime");
            entity.Property(e => e.ProjectExpiryDate).HasColumnType("datetime");
            entity.Property(e => e.ProjectName).HasMaxLength(750);
            entity.Property(e => e.ProjectSupportDate).HasColumnType("datetime");
            entity.Property(e => e.RegisterDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<StsRefreshToken>(entity =>
        {
            entity.HasKey(e => e.TokenId);

            entity.ToTable("STS_RefreshToken");

            entity.Property(e => e.TokenId).HasColumnName("Token_id");
            entity.Property(e => e.ExpiryDate)
                .HasColumnType("datetime")
                .HasColumnName("Expiry_date");
            entity.Property(e => e.Token)
                .HasMaxLength(200)
                .HasColumnName("token");
            entity.Property(e => e.UserId).HasColumnName("user_id");
        });

        modelBuilder.Entity<StsRole>(entity =>
        {
            entity.HasKey(e => e.RoleId);

            entity.ToTable("STS_Roles");

            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.RoleDescription).HasMaxLength(250);
        });

        modelBuilder.Entity<StsSupportStaff>(entity =>
        {
            entity.HasKey(e => e.SupportStaffId);

            entity.ToTable("STS_SupportStaff");

            entity.Property(e => e.SupportStaffId).HasColumnName("SupportStaffID");
            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");
            entity.Property(e => e.EmailAddress).HasMaxLength(500);
            entity.Property(e => e.LogSourceId).HasColumnName("LogSourceID");
            entity.Property(e => e.LoginId).HasColumnName("LoginID");
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(500);
            entity.Property(e => e.PhoneNo).HasMaxLength(500);
            entity.Property(e => e.RegisterDate).HasColumnType("datetime");
            entity.Property(e => e.RoleId).HasColumnName("RoleID");
        });

        modelBuilder.Entity<StsSupportStaffDetail>(entity =>
        {
            entity.HasKey(e => e.SupportStaffDetailsId);

            entity.ToTable("STS_SupportStaffDetail");

            entity.Property(e => e.SupportStaffDetailsId).HasColumnName("SupportStaffDetailsID");
            entity.Property(e => e.SupportStaffId).HasColumnName("SupportStaffID");
            entity.Property(e => e.TechStaffId).HasColumnName("TechStaffID");
        });

        modelBuilder.Entity<StsSystemConfiguration>(entity =>
        {
            entity.HasKey(e => e.ConfigurationId);

            entity.ToTable("STS_SystemConfiguration");

            entity.Property(e => e.ConfigurationId).HasColumnName("ConfigurationID");
            entity.Property(e => e.ClientId).HasColumnName("ClientID");
            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");
        });

        modelBuilder.Entity<StsTechnicalStaff>(entity =>
        {
            entity.HasKey(e => e.TechStaffId);

            entity.ToTable("STS_TechnicalStaff");

            entity.Property(e => e.TechStaffId).HasColumnName("TechStaffID");
            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");
            entity.Property(e => e.EmailAddress).HasMaxLength(500);
            entity.Property(e => e.LogSourceId).HasColumnName("LogSourceID");
            entity.Property(e => e.LoginId).HasColumnName("LoginID");
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.PhoneNo).HasMaxLength(500);
            entity.Property(e => e.RegisterDate).HasColumnType("datetime");
            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.StaffName).HasMaxLength(500);
        });

        modelBuilder.Entity<StsTicketChannel>(entity =>
        {
            entity.HasKey(e => e.ChannelId);

            entity.ToTable("STS_TicketChannels");

            entity.Property(e => e.ChannelId)
                .ValueGeneratedNever()
                .HasColumnName("ChannelID");
            entity.Property(e => e.ChannelName).HasMaxLength(500);
        });

        modelBuilder.Entity<StsTicketPrority>(entity =>
        {
            entity.HasKey(e => e.ProrityId).HasName("PK_STS_TickePrority");

            entity.ToTable("STS_TicketPrority");

            entity.Property(e => e.ProrityId)
                .ValueGeneratedNever()
                .HasColumnName("ProrityID");
            entity.Property(e => e.ProrityName).HasMaxLength(500);
        });

        modelBuilder.Entity<StsTicketStatus>(entity =>
        {
            entity.HasKey(e => e.TicketStatusId);

            entity.ToTable("STS_TicketStatus");

            entity.Property(e => e.TicketStatusId).HasColumnName("TicketStatusID");
            entity.Property(e => e.StatusColor).HasMaxLength(250);
            entity.Property(e => e.StatusName).HasMaxLength(250);
        });

        modelBuilder.Entity<StsTicketsAssigning>(entity =>
        {
            entity.HasKey(e => e.AssignId);

            entity.ToTable("STS_TicketsAssigning");

            entity.Property(e => e.AssignId)
                .ValueGeneratedNever()
                .HasColumnName("AssignID");
            entity.Property(e => e.ApporvalTimeStamp).HasColumnType("datetime");
            entity.Property(e => e.ApprovalDate).HasColumnType("datetime");
            entity.Property(e => e.AssignDate).HasColumnType("datetime");
            entity.Property(e => e.AssignTimeStamp).HasColumnType("datetime");
            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");
            entity.Property(e => e.LogSourceId).HasColumnName("LogSourceID");
            entity.Property(e => e.SupportStaffId).HasColumnName("SupportStaffID");
            entity.Property(e => e.TechStaffId).HasColumnName("TechStaffID");
        });

        modelBuilder.Entity<StsTicketsRegister>(entity =>
        {
            entity.HasKey(e => e.TicketId).HasName("PK_Ticket");

            entity.ToTable("STS_TicketsRegister");

            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.EmailAddress).HasMaxLength(500);
            entity.Property(e => e.LogSourceId)
                .HasDefaultValueSql("((0))")
                .HasColumnName("LogSourceID");
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.PhoneNo).HasMaxLength(500);
            entity.Property(e => e.ProjectId).HasColumnName("ProjectID");
            entity.Property(e => e.RegisteredDate).HasColumnType("datetime");
            entity.Property(e => e.SupportStaffId).HasColumnName("SupportStaffID");
            entity.Property(e => e.TicketName).HasMaxLength(500);
        });

        modelBuilder.Entity<StsTicketsRegisterAttachment>(entity =>
        {
            entity.HasKey(e => e.TicketAttacmentId);

            entity.ToTable("Sts_TicketsRegisterAttachments");

            entity.Property(e => e.TicketAttacmentId).HasColumnName("TicketAttacmentID");
            entity.Property(e => e.IsLog).HasColumnName("isLog");
        });

        modelBuilder.Entity<StsTicketsType>(entity =>
        {
            entity.HasKey(e => e.TypeId);

            entity.ToTable("STS_TicketsType");

            entity.Property(e => e.TypeId)
                .ValueGeneratedNever()
                .HasColumnName("TypeID");
            entity.Property(e => e.TypeIcon).HasMaxLength(250);
            entity.Property(e => e.TypeName).HasMaxLength(500);
        });

        modelBuilder.Entity<StsTicketsWorkList>(entity =>
        {
            entity.HasKey(e => e.WorkListId);

            entity.ToTable("STS_TicketsWorkList");

            entity.Property(e => e.WorkListId).HasColumnName("WorkListID");
            entity.Property(e => e.AddedDate).HasColumnType("datetime");
            entity.Property(e => e.AssignId).HasColumnName("AssignID");
            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");
            entity.Property(e => e.PreviousStatusId).HasColumnName("PreviousStatusID");
            entity.Property(e => e.TechStaffId).HasColumnName("TechStaffID");
            entity.Property(e => e.TicketStatusId).HasColumnName("TicketStatusID");
        });

        modelBuilder.Entity<VwTicketsStatusInfo>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_TicketsStatusInfo");

            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.EmailAddress).HasMaxLength(500);
            entity.Property(e => e.LogSourceId).HasColumnName("LogSourceID");
            entity.Property(e => e.MainProjectName).HasMaxLength(750);
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.PhoneNo).HasMaxLength(500);
            entity.Property(e => e.ProjectId).HasColumnName("ProjectID");
            entity.Property(e => e.RegisteredDate).HasColumnType("datetime");
            entity.Property(e => e.StatusColor).HasMaxLength(250);
            entity.Property(e => e.StatusName).HasMaxLength(250);
            entity.Property(e => e.SupportStaffId).HasColumnName("SupportStaffID");
            entity.Property(e => e.TicketName).HasMaxLength(500);
            entity.Property(e => e.TicketStatusId).HasColumnName("TicketStatusID");
            entity.Property(e => e.TypeIcon).HasMaxLength(250);
            entity.Property(e => e.TypeName).HasMaxLength(500);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
