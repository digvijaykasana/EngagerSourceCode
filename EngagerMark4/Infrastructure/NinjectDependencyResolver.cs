using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ninject;
using Ninject.Web.Common;
using System.Web.Mvc;

using EngagerMark4.ApplicationCore.IRepository.Configurations;
using EngagerMark4.Infrasturcture.Repository.Configurations;
using EngagerMark4.ApplicationCore.IService.Configurations;
using EngagerMark4.Service.ApplicationCore.Configurations;
using EngagerMark4.Infrasturcture.EFContext;
using EngagerMark4.ApplicationCore.IRepository.Application;
using EngagerMark4.Infrasturcture.Repository.Application;
using EngagerMark4.ApplicationCore.IService.Application;
using EngagerMark4.Service.ApplicationCore.Application;
using EngagerMark4.ApplicationCore.IRepository.Users;

using EngagerMark4.Infrasturcture.Repository.Users;
using EngagerMark4.ApplicationCore.IService.Users;
using EngagerMark4.Service.ApplicationCore.Users;

using EngagerMark4.ApplicationCore.Account.IRepository;
using EngagerMark4.ApplicationCore.Account.IService;
using EngagerMark4.Infrastructure.Account.Repository;
using EngagerMark4.Service.Account.ApplicationCore;

using EngagerMark4.ApplicationCore.Customer.IRepository;
using EngagerMark4.ApplicationCore.Customer.IService;
using EngagerMark4.Infrastructure.Customer.Repository;
using EngagerMark4.Service.Customer.ApplicationCore;

using EngagerMark4.ApplicationCore.Job.IRepository;
using EngagerMark4.ApplicationCore.Job.IService;
using EngagerMark4.Infrastructure.Job.Repository;
using EngagerMark4.Service.Job.ApplicationCore;

using EngagerMark4.ApplicationCore.Dummy.IRepository;
using EngagerMark4.ApplicationCore.Dummy.IService;
using EngagerMark4.Infrastructure.Dummy.Repository;
using EngagerMark4.Service.Dummy.ApplicationCore.MeetingServices;
using EngagerMark4.ApplicationCore.Dummy.IRepository.MeetingServices;
using EngagerMark4.Infrastructure.Dummy.Repository.MeetingServices;
using EngagerMark4.ApplicationCore.Dummy.IService.MeetingServices;

using EngagerMark4.ApplicationCore.SOP.IRepository.WorkOrders;
using EngagerMark4.Infrastructure.SOP.Repository;
using EngagerMark4.ApplicationCore.SOP.IService.WorkOrders;
using EngagerMark4.Service.SOP.WorkOrders;

using EngagerMark4.ApplicationCore.Inventory.IRepository.Price;
using EngagerMark4.Infrastructure.Inventory.Repository;
using EngagerMark4.ApplicationCore.Inventory.IService.Price;

using EngagerMark4.ApplicationCore.IService;
using EngagerMark4.Service.ApplicationCore;
using EngagerMark4.ApplicationCore.IRepository;
using EngagerMark4.Infrasturcture.Repository;
using EngagerMark4.ApplicationCore.SOP.IRepository.CreditNotes;
using EngagerMark4.Infrastructure.SOP.Repository.CreditNotes;
using EngagerMark4.ApplicationCore.SOP.IService.CreditNotes;
using EngagerMark4.Service.SOP.CreditNotes;

using EngagerMark4.ApplicationCore.SOP.IRepository.Jobs;
using EngagerMark4.Infrastructure.SOP.Repository.Jobs;
using EngagerMark4.ApplicationCore.SOP.IService.Jobs;
using EngagerMark4.Service.SOP.Jobs;
using EngagerMark4.Service.Inventory.Price;
using EngagerMark4.ApplicationCore.SOP.IRepository.SalesInvoices;
using EngagerMark4.Infrastructure.SOP.Repository.SalesInvoices;
using EngagerMark4.ApplicationCore.SOP.IService.SalesInvoices;
using EngagerMark4.Service.SOP.SalesInvoices;
using EngagerMark4.ApplicationCore.SOP.IRepository.SerivceJobs;
using EngagerMark4.Infrasturcture.MobilePushNotifications.FCM;

using EngagerMark4.ApplicationCore.Equipment.IRepository.Conveyors;
using EngagerMark4.Infrastructure.Equipment.Repository.Conveyors;
using EngagerMark4.ApplicationCore.Equipment.IService.Conveyors;
using EngagerMark4.Service.Equipment.Conveyors;

using EngagerMark4.ApplicationCore.Equipment.IRepository.Equipments;
using EngagerMark4.Infrastructure.Equipment.Repository.Equipments;
using EngagerMark4.ApplicationCore.Equipment.IService.Equipments;
using EngagerMark4.Service.Equipment.Equipments;
using EngagerMark4.ApplicationCore.SOP.IRepository.WorkOrder;
using EngagerMark4.Infrastructure.SOP.Repository.WorkOrders;

namespace EngagerMark4.Infrastructure
{
    /// <summary>
    /// Ninject Dependency injection resolver
    /// Created by: Kyaw Min Htut
    /// Created date: 05-08-2015
    /// </summary>
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel kernel;

        public NinjectDependencyResolver(IKernel kernelParam)
        {
            kernel = kernelParam;
            AddBindings();
        }

        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }

        /// <summary>
        /// Bind interface with class implementation
        /// Created by: Kyaw Min Htut
        /// Created date: 05-08-2015
        /// </summary>
        private void AddBindings()
        {
            #region Application DbContext
            kernel.Bind<ApplicationDbContext>().To<ApplicationDbContext>().InRequestScope();
            #endregion

            #region Repository
            kernel.Bind<IDriverVariableSalarySummaryRepository>().To<DriverVariableSalarySummaryRepository>().InRequestScope();
            kernel.Bind<IServiceJobChecklistItemRepository>().To<ServiceJobChecklistItemRepository>().InRequestScope();
            kernel.Bind<IWorkOrderHistoryRepository>().To<WorkOrderHistoryRepository>().InRequestScope();
            kernel.Bind<IMonthlyGeneralInvoiceReportRepository>().To<MonthlyGeneralInvoiceReportRepository>().InRequestScope();
            kernel.Bind<IMonthlyCreditNoteReportRepository>().To<MonthlyCreditNoteReportRepository>().InRequestScope();
            kernel.Bind<IMonthlyInvoiceReportRepository>().To<MonthlyInvoiceReportRepository>().InRequestScope();
            kernel.Bind<IConveyorRepository>().To<ConveyorRepository>().InRequestScope();
            kernel.Bind<IMobileUserRepository>().To<MobileUserRepository>().InRequestScope();
            kernel.Bind<IInvoiceSummaryReportRepository>().To<InvoiceSummaryReportRepository>().InRequestScope();
            kernel.Bind<ICreditNoteDetailsRepository>().To<CreditNoteDetailsRepository>().InRequestScope();
            kernel.Bind<ICreditNoteRepository>().To<CreditNoteRepository>().InRequestScope();
            kernel.Bind<IPriceLocationRepository>().To<PriceLocationRepository>().InRequestScope();
            kernel.Bind<IPriceRepository>().To<PriceRepository>().InRequestScope();
            kernel.Bind<IMeetingServiceDetailsRepository>().To<MeetingServiceDetailsRepository>().InRequestScope();
            kernel.Bind<IMeetingServiceRepository>().To<Dummy.Repository.MeetingServices.MeetingServiceRepository>().InRequestScope();
            kernel.Bind<ICheckListRepository>().To<CheckListRepository>().InRequestScope();
            kernel.Bind<ICustomerRepository>().To<CustomerRepository>().InRequestScope();
            kernel.Bind<IGSTRepository>().To<GSTRepository>().InRequestScope();
            kernel.Bind<IVehicleRepository>().To<VehicleRepository>().InRequestScope();
            kernel.Bind<ILocationRepository>().To<LocationRepository>().InRequestScope();
            kernel.Bind<IGeneralLedgerRepository>().To<GeneralLedgerRepository>().InRequestScope();
            kernel.Bind<IConfigurationGroupRepository>().To<ConfigurationGroupRepository>().InRequestScope();
            kernel.Bind<ICommonConfigurationRepository>().To<CommonConfigurationRepository>().InRequestScope();
            kernel.Bind<IModuleRepository>().To<ModuleRepository>().InRequestScope();
            kernel.Bind<IFunctionRepository>().To<FunctionRepository>().InRequestScope();
            kernel.Bind<IUserRoleRepository>().To<UserRoleRepository>().InRequestScope();
            kernel.Bind<IRoleRepository>().To<RoleRepository>().InRequestScope();
            kernel.Bind<IModulePermissionRepository>().To<ModulePermissionRepository>().InRequestScope();
            kernel.Bind<IUserRepository>().To<UserRepository>().InRequestScope();
            kernel.Bind<ICompanyRepository>().To<CompanyRepository>().InRequestScope();
            kernel.Bind<IBlobFileRepository>().To<BlobFileRepository>().InRequestScope();
            kernel.Bind<IRolePermissionRepository>().To<RolePermissionRepository>().InRequestScope();
            kernel.Bind<ISMTPRepository>().To<SMTPRepository>().InRequestScope();
            kernel.Bind<ISystemSettingRepository>().To<SystemSettingRepository>().InRequestScope();
            kernel.Bind<IChecklistTemplateRepository>().To<ChecklistTemplateRepository>().InRequestScope();
            kernel.Bind<IUserCustomerRepository>().To<UserCustomerRepository>().InRequestScope();
            kernel.Bind<IUserVehicleRepository>().To<UserVehicleRepository>().InRequestScope();
            kernel.Bind<IWorkOrderRepository>().To<WorkOrderRepository>().InRequestScope();
            kernel.Bind<IServiceJobRepository>().To<ServiceJobRepository>().InRequestScope();
            kernel.Bind<ISalesInvoiceRepository>().To<SalesInvoiceRepository>().InRequestScope();
            kernel.Bind<IVesselRepository>().To<VesselRepository>().InRequestScope();
            kernel.Bind<IDriverDailyReportRepository>().To<DriverDailyReportRepository>().InRequestScope();
            kernel.Bind<IDailySummaryJobByCompanyRepository>().To<DailySummaryJobByCompanyRepository>().InRequestScope();
            kernel.Bind<IDailySummaryJobByDriverRepository>().To<DailySummaryJobByDriverRepository>().InRequestScope();
            kernel.Bind<INotificationRepository>().To<NotificationRepository>().InRequestScope();
            kernel.Bind<ISalesInvoiceSummaryRepository>().To<SalesInvoiceSummaryRepository>().InRequestScope();
            kernel.Bind<IHotelRepository>().To<HotelRepository>().InRequestScope();
            kernel.Bind<FCMSender>().To<FCMSender>().InRequestScope();
            kernel.Bind<IAuditRepository>().To<AuditRepository>().InRequestScope();
            kernel.Bind<ICreditNoteSummaryRepository>().To<CreditNoteSummaryRepository>().InRequestScope();

            //PCR2021
            kernel.Bind<ILetterheadRepository>().To<LetterheadRepository>().InRequestScope();
            kernel.Bind<IWorkOrderPassengerRepository>().To<WorkOrderPassengerRepository>().InRequestScope();
            #endregion

            #region Service
            kernel.Bind<IConveyorService>().To<ConveyorService>().InRequestScope();
            kernel.Bind<IMobileUserService>().To<MobileUserService>().InRequestScope();
            kernel.Bind<ICreditNoteDetailsService>().To<CreditNoteDetailsService>().InRequestScope();
            kernel.Bind<ICreditNoteService>().To<CreditNoteService>().InRequestScope();
            kernel.Bind<IPriceLocationService>().To<PriceLocationService>().InRequestScope();
            kernel.Bind<IPriceService>().To<PriceService>().InRequestScope();
            kernel.Bind<IMeetingServiceDetailsService>().To<MeetingServiceDetailsService>().InRequestScope();
            kernel.Bind<ApplicationCore.Dummy.IService.MeetingServices.IMeetingServiceService>().To<MeetingServiceService>().InRequestScope();
            kernel.Bind<ICheckListService>().To<CheckListService>().InRequestScope();
            kernel.Bind<ICustomerService>().To<CustomerService>().InRequestScope();
            kernel.Bind<IGSTService>().To<GSTService>().InRequestScope();
            kernel.Bind<IVehicleService>().To<VehicleService>().InRequestScope();
            kernel.Bind<ILocationService>().To<LocationService>().InRequestScope();
            kernel.Bind<IGeneralLedgerService>().To<GeneralLedgerService>().InRequestScope();
            kernel.Bind<IConfigurationGroupService>().To<ConfigurationGroupService>().InRequestScope();
            kernel.Bind<ICommonConfigurationService>().To<CommonConfigurationService>().InRequestScope();
            kernel.Bind<IModuleService>().To<ModuleService>().InRequestScope();
            kernel.Bind<IFunctionService>().To<FunctionService>().InRequestScope();
            kernel.Bind<IUserRoleService>().To<UserRoleService>().InRequestScope();
            kernel.Bind<IRoleService>().To<RoleService>().InRequestScope();
            kernel.Bind<IUserService>().To<UserService>().InRequestScope();
            kernel.Bind<IRolePermissionService>().To<RolePermissionService>().InRequestScope();
            kernel.Bind<IModulePermissionService>().To<ModulePermissionService>().InRequestScope();
            kernel.Bind<ICompanyService>().To<CompanyService>().InRequestScope();
            kernel.Bind<IBlobFileService>().To<BlobFileService>().InRequestScope();
            kernel.Bind<ISMTPService>().To<SMTPService>().InRequestScope();
            kernel.Bind<IMailingService>().To<MailingService>().InRequestScope();
            kernel.Bind<ISystemSettingService>().To<SystemSettingService>().InRequestScope();
            kernel.Bind<IChecklistTemplateService>().To<ChecklistTemplateService>().InRequestScope();
            kernel.Bind<IUserVehicleService>().To<UserVehicleService>().InRequestScope();
            kernel.Bind<IUserCustomerService>().To<UserCustomerService>().InRequestScope();
            kernel.Bind<IWorkOrderService>().To<WorkOrderService>().InRequestScope();
            kernel.Bind<IServiceJobService>().To<ServiceJobService>().InRequestScope();
            kernel.Bind<ISalesInvoiceService>().To<SalesInvoiceService>().InRequestScope();
            kernel.Bind<IVesselService>().To<VesselService>().InRequestScope();
            kernel.Bind<ISalesInvoiceSummaryService>().To<SalesInvoiceSummaryService>().InRequestScope();
            kernel.Bind<IHotelService>().To<HotelService>().InRequestScope();
            kernel.Bind<ICreditNoteSummaryService>().To<CreditNoteSummaryService>(); 

            //PCR2021
            kernel.Bind<ILetterheadService>().To<LetterheadService>().InRequestScope();
            kernel.Bind<IWorkOrderPassengerService>().To<WorkOrderPassengerService>().InRequestScope();
            #endregion
        }
    }
}