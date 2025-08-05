using EngagerMark4.ApplicationCore.Entities;
using EngagerMark4.ApplicationCore.Entities.Application;
using EngagerMark4.ApplicationCore.Entities.Configurations;
using EngagerMark4.ApplicationCore.Entities.Users;
using EngagerMark4.ApplicationCore.Dummy.Entities.Users;
using EngagerMark4.ApplicationCore.Dummy.Entities.MeetingServices;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using EngagerMark4.Common;
using EngagerMark4.ApplicationCore.Account.Entities;
using EngagerMark4.ApplicationCore.Customer.Entities;
using EngagerMark4.ApplicationCore.Job.Entities;
using EngagerMark4.Common.Utilities;
using EngagerMark4.ApplicationCore.SOP.Entities.WorkOrders;
using EngagerMark4.ApplicationCore.Inventory.Entities;
using EngagerMark4.ApplicationCore.SOP.Entities.Jobs;
using EngagerMark4.ApplicationCore.SOP.Entities.CreditNotes;
using EngagerMark4.ApplicationCore.SOP.Entities.SalesInvoices;
using EngagerMark4.ApplicationCore.Equipment.Entities.Equipments;
using EngagerMark4.ApplicationCore.Equipment.Entities.Conveyors;

namespace EngagerMark4.Infrasturcture.EFContext
{
    [DbConfigurationType("EngagerMark4.ApplicationCore.Caches.EFConfiguration, EngagerMark4.ApplicationCore")]
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            this.Database.CommandTimeout = 180;
            Database.SetInitializer(new ApplicationDbContextInitializer());
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<Audit> Audits
        {
            get;
            set;
        }

        public DbSet<Company> Companies
        {
            get;
            set;
        }

        public DbSet<BlobFile> BlobFiles
        {
            get;
            set;
        }

        public DbSet<BlobFileContent> BlobFileContents
        {
            get;
            set;
        }

        #region Configurations

        public DbSet<Vehicle> Vehicles { get; set; }

        public DbSet<Location> Locations { get; set; }

        public DbSet<ConfigurationGroup> ConfigurationGroups { get; set; }

        public DbSet<CommonConfiguration> CommonConfigurations { get; set; }

        public DbSet<Hotel> Hotels { get; set; }

        //PCR2021
        public DbSet<Letterhead> Letterheads { get; set; }

        #endregion

        #region Applications

        public DbSet<Module> Modules { get; set; }

        public DbSet<Function> Functions { get; set; }

        public DbSet<SystemSetting> SystemSettings { get; set; }

        public DbSet<PermissionDetails> PermissionDetails { get; set; }

        public DbSet<ModulePermission> ModulePermissions { get; set; }

        public DbSet<SMTP> SMTPs { get; set; }

        public DbSet<Notification> Notifications { get; set; }

        #endregion

        #region Users

        public DbSet<Role> EngagerRoles { get; set; }
        
        public DbSet<User> EngagerUsers { get; set; }

        public DbSet<MobileUser> EngagerMobileUsers { get; set; }

        public DbSet<UserRole> EngagerUserRole { get; set; }

        public DbSet<RolePermission> RolePermissions { get; set; }

        public DbSet<RolePermissionDetails> RolePermissionDetails
        {
            get;
            set;
        }

        #endregion

        #region Dummy Users

        public DbSet<MeetingServiceDetails> MeetingServiceDetails { get; set; }

        public DbSet<MeetingService> MeetingServices { get; set; }

        public DbSet<UserCustomer> UserCustomers { get; set; }

        public DbSet<UserVehicle> UserVehicles { get; set; }

        #endregion

        #region Account

        public DbSet<GeneralLedger> GeneralLedgers { get; set; }

        public DbSet<GST> GSTs { get; set; }

        #endregion

        #region Customer

        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerLocation> CustomerLocations { get; set; }
        public DbSet<CustomerVessel> CustomerVessels { get; set; }

        #endregion

        #region Job

        public DbSet<CheckList> CheckLists { get; set; }
        public DbSet<ChecklistTemplate> ChecklistTemplates { get; set; }
        public DbSet<CheckListTemplateDetail> ChecklistTemplateDetails { get; set; }
        #endregion

        #region SOP

        #region Work Order
        public DbSet<WorkOrderSerialNo> WorkOrderSerialNoes { get; set; }
        public DbSet<WorkOrder> WorkOrders { get; set; }
        public DbSet<WorkOrderAirportMeetingService> WorkOrderMeetingServices { get; set; }
        public DbSet<WorkOrderLocation> WorkOrderLocations { get; set; }
        public DbSet<WorkOrderPassenger> WorkOrderPassengers { get; set; }

        //Added - 20191003 - Aung Ye Kaung
        public DbSet<WorkOrderHistory> WorkOrderHistories { get; set; }

        public DbSet<ServiceJob> ServiceJobs { get; set; }
        public DbSet<ServiceJobSerialNo> ServiceJobSerialNoes { get; set; }
        public DbSet<SalesInvoice> SalesInvoices { get; set; }
        public DbSet<SalesInvoiceDetails> SalesInvoiceDetails { get; set; }
        public DbSet<SalesInvoiceSerialNo> SalesInvoiceSerialNoes { get; set; }
        public DbSet<SalesInvoiceReportSerialNo> SalesInvoiceReportSerialNoes { get; set; }
        public DbSet<SalesInvoiceSummary> SalesInvoiceSummaries { get; set; }
        public DbSet<SalesInvoiceSummaryDetails> SalesInvoiceSummaryDetails { get; set; }

        //PCR2021
        public DbSet<ServiceJobChecklistItem> ServiceJobChecklistItems { get; set; }

        #endregion

        #region Credit Note
        public DbSet<CreditNote> CreditNotes { get; set; }
        public DbSet<CreditNoteDetails> CreditNoteDetails { get; set; }
        public DbSet<CreditNoteSerialNo> CreditNoteSerialNo { get; set; }
        public DbSet<CreditNoteReportSerialNo> CreditNoteReportSerialNoes { get; set; }
        #endregion

        #endregion

        #region Inventory
        public DbSet<Price> Prices { get; set; }
        public DbSet<PriceLocation> PriceLocations { get; set; }
        public DbSet<PriceSerialNo> PriceSerialNos { get; set; }
        #endregion

        #region Equipment
        public DbSet<Conveyor> Conveyors { get; set; }
        public DbSet<Equipment> Equipments { get; set; }
        #endregion

        #region CreditNoteSummary
        public DbSet<CreditNoteSummary> CreditNoteSummaries { get; set; }
        public DbSet<CreditNoteSummaryDetails> CreditNoteSummaryDetails { get; set; }
        #endregion

        public string userId;

        private string GetCurrentUserId()
        {
            try
            {
                if (!string.IsNullOrEmpty(GlobalVariable.mobile_userId) && HttpContext.Current.Request.IsAuthenticated == false)
                    return GlobalVariable.mobile_userId;

                if (!string.IsNullOrEmpty(this.userId))
                    return this.userId;

                string userName = HttpContext.Current.User.Identity.Name;
                string userId = HttpContext.Current.User.Identity.GetUserId();
                return userId;
            }
            catch (Exception ex)
            {
                return userId;
            }
        }

        //OBSOLETE - 2019/10/18 - Aung Ye Kaung
        //private string GetCurrentUserId()
        //{
        //    try
        //    {
        //        if (!string.IsNullOrEmpty(this.userId))
        //            return this.userId;
        //        string userName = HttpContext.Current.User.Identity.Name;
        //        string userId = HttpContext.Current.User.Identity.GetUserId();
        //        return userId;
        //    }
        //    catch (Exception ex)
        //    {
        //        return userId;
        //    }
        //}

        public string userName = string.Empty;

        private string GetUserName()
        {
            if (!string.IsNullOrEmpty(userName))
                return userName;
            try
            {
                if(!String.IsNullOrEmpty(GlobalVariable.mobile_userName) && HttpContext.Current.Request.IsAuthenticated == false)
                {
                    return GlobalVariable.mobile_userName;
                }
                else
                {
                    return GlobalVariable.USER_NAMES[GetCurrentUserId()];
                }
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        public override int SaveChanges()
        {
            string userId = GetCurrentUserId();

            var selectedEntityList = ChangeTracker.Entries()
                                     .Where(x => (x.Entity is BasicEntity || x.Entity is User || x.Entity is BaseEntity) && (x.State == EntityState.Added || x.State == EntityState.Modified));
            foreach (var entity in selectedEntityList)
            {
                if (entity.Entity is BasicEntity)
                {
                    if (entity.State == EntityState.Added)
                    {
                        ((BasicEntity)entity.Entity).ParentCompanyId = GlobalVariable.COMPANY_ID;
                        ((BasicEntity)entity.Entity).CreatedBy = userId;
                        ((BasicEntity)entity.Entity).CreatedByName = GetUserName();
                        ((BasicEntity)entity.Entity).Created = TimeUtil.GetLocalTime();
                    }
                    else if (entity.State == EntityState.Modified)
                    {
                        var baseEntity = (BasicEntity)entity.Entity;
                        this.Entry(baseEntity).Property(x => x.ParentCompanyId).IsModified = false;
                        this.Entry(baseEntity).Property(x => x.CreatedBy).IsModified = false;
                        this.Entry(baseEntity).Property(x => x.CreatedByName).IsModified = false;
                        baseEntity.ModifiedBy = userId;
                        baseEntity.Modified = TimeUtil.GetLocalTime();
                    }
                }
                else if (entity.Entity is BaseEntity)
                {
                    if (entity.State == EntityState.Added)
                    {
                        ((BaseEntity)entity.Entity).ParentCompanyId = GlobalVariable.COMPANY_ID;
                        ((BaseEntity)entity.Entity).CreatedBy = userId;
                        ((BasicEntity)entity.Entity).Created = TimeUtil.GetLocalTime();
                    }
                    else if (entity.State == EntityState.Modified)
                    {
                        var baseEntity = (BaseEntity)entity.Entity;
                        baseEntity.ModifiedBy = userId;
                        this.Entry(baseEntity).Property(x => x.ParentCompanyId).IsModified = false;
                        this.Entry(baseEntity).Property(x => x.CreatedBy).IsModified = false;
                        this.Entry(baseEntity).Property(x => x.CreatedByName).IsModified = false;
                        baseEntity.Modified = TimeUtil.GetLocalTime();
                    }
                }
                else if (entity.Entity is User)
                {
                    if (entity.State == EntityState.Added)
                    {
                        ((User)entity.Entity).ParentCompanyId = GlobalVariable.COMPANY_ID;
                    }
                    else if (entity.State == EntityState.Modified)
                    {
                        
                        var userEntity = (User)entity.Entity;
                        this.Entry(userEntity).Property(x => x.ParentCompanyId).IsModified = false;
                    }
                }
            }
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync()
        {
            string userId = GetCurrentUserId();
            var selectedEntityList = ChangeTracker.Entries()
                                     .Where(x => (x.Entity is BasicEntity || x.Entity is User || x.Entity is BaseEntity) && (x.State == EntityState.Added || x.State == EntityState.Modified));
            foreach (var entity in selectedEntityList)
            {
                if (entity.Entity is BasicEntity)
                {
                    if (entity.State == EntityState.Added)
                    {
                        ((BasicEntity)entity.Entity).ParentCompanyId = GlobalVariable.COMPANY_ID;
                        ((BasicEntity)entity.Entity).CreatedBy = userId;
                        ((BasicEntity)entity.Entity).Created = TimeUtil.GetLocalTime();
                    }
                    else if (entity.State == EntityState.Modified)
                    {
                        var baseEntity = (BasicEntity)entity.Entity;
                        baseEntity.ModifiedBy = userId;
                        baseEntity.Modified = TimeUtil.GetLocalTime();
                        this.Entry(baseEntity).Property(x => x.ParentCompanyId).IsModified = false;
                        this.Entry(baseEntity).Property(x => x.CreatedBy).IsModified = false;
                        this.Entry(baseEntity).Property(x => x.CreatedByName).IsModified = false;
                        this.Entry(baseEntity).Property(x => x.Created).IsModified = false;
                    }
                }
                else if (entity.Entity is BaseEntity)
                {
                    if (entity.State == EntityState.Added)
                    {
                        ((BaseEntity)entity.Entity).ParentCompanyId = GlobalVariable.COMPANY_ID;
                        ((BaseEntity)entity.Entity).CreatedBy = userId;
                        ((BaseEntity)entity.Entity).Created = TimeUtil.GetLocalTime();
                    }
                    else if (entity.State == EntityState.Modified)
                    {
                        var baseEntity = (BaseEntity)entity.Entity;
                        baseEntity.ModifiedBy = userId;
                        baseEntity.Modified = TimeUtil.GetLocalTime();
                        this.Entry(baseEntity).Property(x => x.ParentCompanyId).IsModified = false;
                        this.Entry(baseEntity).Property(x => x.CreatedBy).IsModified = false;
                        this.Entry(baseEntity).Property(x => x.CreatedByName).IsModified = false;
                        this.Entry(baseEntity).Property(x => x.Created).IsModified = false;
                    }
                }
                else if (entity.Entity is User)
                {
                    if (entity.State == EntityState.Added)
                    {
                        ((User)entity.Entity).ParentCompanyId = GlobalVariable.COMPANY_ID;
                    }
                    else if (entity.State == EntityState.Modified)
                    {
                        var userEntity = (User)entity.Entity;
                        this.Entry(userEntity).Property(x => x.ParentCompanyId).IsModified = false;
                    }
                }
            }
            return base.SaveChangesAsync();
        }
    }
}
