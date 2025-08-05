using EngagerMark4.ApplicationCore.Cris;
using EngagerMark4.ApplicationCore.Entities;
using EngagerMark4.ApplicationCore.IRepository;
using EngagerMark4.Infrasturcture.EFContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using EngagerMark4.Common;
using EngagerMark4.Common.Utilities;
using EngagerMark4.ApplicationCore.Entities.Configurations;
using EngagerMark4.ApplicationCore.Entities.Application;
using EngagerMark4.ApplicationCore.Entities.Users;
using System.Web;
using EngagerMark4.Common.Configs;
using EngagerMark4.ApplicationCore.Job.Entities;

namespace EngagerMark4.Infrasturcture.Repository
{
    public class CompanyRepository : GenericRepository<ApplicationDbContext, CompanyCri, Company>, ICompanyRepository
    {
        public async override Task<IEnumerable<Company>> GetByCri(CompanyCri cri)
        {
            if (HttpContext.Current.User.Identity.Name.Equals(SecurityConfig.SUPER_ADMIN))
            {
                return context.Companies.Include(x => x.Addresses).AsNoTracking().OrderBy(x => x.Name);
            }
            else
            {
                return await base.GetByCri(cri);
            }
        }

        public CompanyRepository(ApplicationDbContext aContext) : base(aContext)
        {
        }

        public override Task<Company> GetById(object id)
        {
            if (HttpContext.Current.User.Identity.Name.Equals(SecurityConfig.SUPER_ADMIN))
                return dbSet.Include(x => x.Addresses).SingleOrDefaultAsync(x => x.Id == (Int64)id);
            else
                return dbSet.Include(x => x.Addresses).SingleOrDefaultAsync(x => x.Id == (Int64)id);
        }

        public override void ExecuteWithGraph(Company model)
        {
            bool isNewRecord = model.Id == 0;
            base.ExecuteWithGraph(model);
            this.SaveChanges();
            
            GlobalVariable.COMPANY_ID = model.Id;
            if (isNewRecord)
            {
                model.ParentCompanyId = model.Id;
                #region ConfigurationGroup

                ConfigurationGroup rank = new ConfigurationGroup
                {
                    Created = TimeUtil.GetLocalTime(),
                    Code = "Rank",
                    Name = "Rank",
                };
                context.ConfigurationGroups.Add(rank);

                ConfigurationGroup boardType = new ConfigurationGroup
                {
                    Created = TimeUtil.GetLocalTime(),
                    Code = "BoardType",
                    Name = "Board Type",
                };
                context.ConfigurationGroups.Add(boardType);

                ConfigurationGroup orderStatus = new ConfigurationGroup
                {
                    Created = TimeUtil.GetLocalTime(),
                    Code = "OrderStatus",
                    Name = "Order Status",
                };
                context.ConfigurationGroups.Add(orderStatus);

                ConfigurationGroup customDetention = new ConfigurationGroup
                {
                    Created = TimeUtil.GetLocalTime(),
                    Code = "CustomDetention",
                    Name = "Custom Detention",
                };
                context.ConfigurationGroups.Add(customDetention);

                ConfigurationGroup userStatus = new ConfigurationGroup
                {
                    Created = TimeUtil.GetLocalTime(),
                    Code = "UserStatus",
                    Name = "User Status",
                };
                context.ConfigurationGroups.Add(userStatus);

                ConfigurationGroup roleStatus = new ConfigurationGroup
                {
                    Created = TimeUtil.GetLocalTime(),
                    Code = "RoleStatus",
                    Name = "Role Status",
                };
                context.ConfigurationGroups.Add(roleStatus);

                ConfigurationGroup discountType = new ConfigurationGroup
                {
                    Created = TimeUtil.GetLocalTime(),
                    Code = "DiscountType",
                    Name = "Discount Type",
                };
                context.ConfigurationGroups.Add(discountType);

                ConfigurationGroup orderLocationType = new ConfigurationGroup
                {
                    Created = TimeUtil.GetLocalTime(),
                    Code = "OrderLocationType",
                    Name = "Order Location Type",
                    ParentCompanyId = 1
                };
                context.ConfigurationGroups.Add(orderLocationType);

                ConfigurationGroup dutyType = new ConfigurationGroup
                {
                    Created = TimeUtil.GetLocalTime(),
                    Code = "DutyType",
                    Name = "Duty Type",
                    ParentCompanyId = 1
                };
                context.ConfigurationGroups.Add(dutyType);

                ConfigurationGroup task = new ConfigurationGroup
                {
                    Created = TimeUtil.GetLocalTime(),
                    Code = "Task",
                    Name = "Task",
                    ParentCompanyId = 1
                };
                context.ConfigurationGroups.Add(task);

                ConfigurationGroup paymentSchedule = new ConfigurationGroup
                {
                    Created = TimeUtil.GetLocalTime(),
                    Code = "PaymentSchedule",
                    Name = "Payment Schedule",
                };
                context.ConfigurationGroups.Add(paymentSchedule);

                ConfigurationGroup workType = new ConfigurationGroup
                {
                    Created = TimeUtil.GetLocalTime(),
                    Code = "WorkType",
                    Name = "Work Type",
                };
                context.ConfigurationGroups.Add(workType);

                ConfigurationGroup timeSlot = new ConfigurationGroup
                {
                    Created = TimeUtil.GetLocalTime(),
                    Code = "TimeSlot",
                    Name = "Time Slot",
                };
                context.ConfigurationGroups.Add(timeSlot);

                ConfigurationGroup vessel = new ConfigurationGroup
                {
                    Created = TimeUtil.GetLocalTime(),
                    Code = "VesselId",
                    Name = "Vessel",
                };
                context.ConfigurationGroups.Add(vessel);

                #endregion

                #region Common Configuration

                CommonConfiguration userStatus_Active = new CommonConfiguration
                {
                    Created = TimeUtil.GetLocalTime(),
                    ConfigurationGroup = userStatus,
                    Code = "Active",
                    Name = "Active",
                    SerialNo = 1
                };
                context.CommonConfigurations.Add(userStatus_Active);

                CommonConfiguration userStatus_InActive = new CommonConfiguration
                {
                    Created = TimeUtil.GetLocalTime(),
                    ConfigurationGroup = userStatus,
                    Code = "InActive",
                    Name = "InActive",
                    SerialNo = 2
                };
                context.CommonConfigurations.Add(userStatus_InActive);

                CommonConfiguration roleStatus_Active = new CommonConfiguration
                {
                    Created = TimeUtil.GetLocalTime(),
                    ConfigurationGroup = roleStatus,
                    Code = "Active",
                    Name = "Active",
                    SerialNo = 1,
                };
                context.CommonConfigurations.Add(roleStatus_Active);

                CommonConfiguration roleStatus_InActive = new CommonConfiguration
                {
                    Created = TimeUtil.GetLocalTime(),
                    ConfigurationGroup = roleStatus,
                    Code = "InActive",
                    Name = "InActive",
                    SerialNo = 2
                };
                context.CommonConfigurations.Add(roleStatus_InActive);

                CommonConfiguration discountType_Percent = new CommonConfiguration
                {
                    Created = TimeUtil.GetLocalTime(),
                    ConfigurationGroup = discountType,
                    Code = "Percent",
                    Name = "Percent",
                    SerialNo = 1
                };
                context.CommonConfigurations.Add(discountType_Percent);

                CommonConfiguration discountType_Amt = new CommonConfiguration
                {
                    Created = TimeUtil.GetLocalTime(),
                    ConfigurationGroup = discountType,
                    Code = "Amount",
                    Name = "Amount",
                    SerialNo = 2
                };
                context.CommonConfigurations.Add(discountType_Amt);

                CommonConfiguration orderLocationType_pickup = new CommonConfiguration
                {
                    Created = TimeUtil.GetLocalTime(),
                    ConfigurationGroup = orderLocationType,
                    Code = "PickUp",
                    Name = "Pick Up",
                    SerialNo = 1,
                };
                context.CommonConfigurations.Add(orderLocationType_pickup);

                CommonConfiguration orderLocationType_dropoff = new CommonConfiguration
                {
                    Created = TimeUtil.GetLocalTime(),
                    ConfigurationGroup = orderLocationType,
                    Code = "DropOff",
                    Name = "Drop Off",
                    SerialNo = 2,
                };
                context.CommonConfigurations.Add(orderLocationType_dropoff);

                CommonConfiguration orderLocationType_additionalStop = new CommonConfiguration
                {
                    Created = TimeUtil.GetLocalTime(),
                    ConfigurationGroup = orderLocationType,
                    Code = "AdditionalStop",
                    Name = "Additional Stop",
                    SerialNo = 3
                };
                context.CommonConfigurations.Add(orderLocationType_additionalStop);

                CommonConfiguration orderStatus_ordered = new CommonConfiguration
                {
                    Created = TimeUtil.GetLocalTime(),
                    ConfigurationGroup = orderStatus,
                    Code = "Ordered",
                    Name = "Ordered",
                    SerialNo = 1,
                };
                context.CommonConfigurations.Add(orderStatus_ordered);

                CommonConfiguration orderStatus_pending = new CommonConfiguration
                {
                    Created = TimeUtil.GetLocalTime(),
                    ConfigurationGroup = orderStatus,
                    Code = "Pending",
                    Name = "Pending",
                    SerialNo = 2,
                };
                context.CommonConfigurations.Add(orderStatus_pending);

                CommonConfiguration orderStatus_assigned = new CommonConfiguration
                {
                    Created = TimeUtil.GetLocalTime(),
                    ConfigurationGroup = orderStatus,
                    Code = "Assigned",
                    Name = "Assigned",
                    SerialNo = 3
                };
                context.CommonConfigurations.Add(orderStatus_assigned);

                CommonConfiguration orderStatus_scheduled = new CommonConfiguration
                {
                    Created = TimeUtil.GetLocalTime(),
                    ConfigurationGroup = orderStatus,
                    Code = "Scheduled",
                    Name = "Scheduled",
                    SerialNo = 4
                };
                context.CommonConfigurations.Add(orderStatus_scheduled);

                CommonConfiguration orderStatus_inprogress = new CommonConfiguration
                {
                    Created = TimeUtil.GetLocalTime(),
                    ConfigurationGroup = orderStatus,
                    Code = "InProgress",
                    Name = "In-Progress",
                    SerialNo = 5
                };
                context.CommonConfigurations.Add(orderStatus_inprogress);

                CommonConfiguration orderStatus_pendingsign = new CommonConfiguration
                {
                    Created = TimeUtil.GetLocalTime(),
                    ConfigurationGroup = orderStatus,
                    Code = "PendingSign",
                    Name = "Pending-Sign",
                    SerialNo = 6
                };
                context.CommonConfigurations.Add(orderStatus_pendingsign);

                CommonConfiguration orderStatus_submitted = new CommonConfiguration
                {
                    Created = TimeUtil.GetLocalTime(),
                    ConfigurationGroup = orderStatus,
                    Code = "Submitted",
                    Name = "Submitted",
                    SerialNo = 7,
                };
                context.CommonConfigurations.Add(orderStatus_submitted);

                CommonConfiguration orderStatus_verified = new CommonConfiguration
                {
                    Created = TimeUtil.GetLocalTime(),
                    ConfigurationGroup = orderStatus,
                    Code = "Verified",
                    Name = "Verified",
                    SerialNo = 8,
                };
                context.CommonConfigurations.Add(orderStatus_verified);

                CommonConfiguration orderStatus_withAccounts = new CommonConfiguration
                {
                    Created = TimeUtil.GetLocalTime(),
                    ConfigurationGroup = orderStatus,
                    Code = "WithAccounts",
                    Name = "With-Accounts",
                    SerialNo = 9
                };
                context.CommonConfigurations.Add(orderStatus_withAccounts);

                CommonConfiguration orderStatus_billed = new CommonConfiguration
                {
                    Created = TimeUtil.GetLocalTime(),
                    ConfigurationGroup = orderStatus,
                    Code = "Billed",
                    Name = "Billed",
                    SerialNo = 10
                };
                context.CommonConfigurations.Add(orderStatus_billed);
                #endregion

                #region Module

                #region Application Module
                Module application = new Module
                {
                    Created = TimeUtil.GetLocalTime(),
                    Code = "Application",
                    Name = "Application",
                    SerialNo = 1,
                };
                context.Modules.Add(application);

                Function company = new Function
                {
                    Controller = "CompanyController",
                    Name = "Company",
                    SerialNo = -1,
                };
                context.Functions.Add(company);

                ModulePermission applicationCompany = new ModulePermission
                {
                    Module = application,
                    Permission = company,
                };
                context.ModulePermissions.Add(applicationCompany);

                Function home = new Function
                {
                    Controller = "HomeController",
                    Name = "Home",
                    SerialNo = 0,
                };
                context.Functions.Add(home);

                ModulePermission applicationHome = new ModulePermission
                {
                    Module = application,
                    Permission = home,
                };
                context.ModulePermissions.Add(applicationHome);

                Function module = new Function
                {
                    Created = TimeUtil.GetLocalTime(),
                    Controller = "ModuleController",
                    Name = "Module",
                    SerialNo = 1
                };
                context.Functions.Add(module);
                ModulePermission applicationModule = new ModulePermission
                {
                    Created = TimeUtil.GetLocalTime(),
                    Module = application,
                    Permission = module
                };
                context.ModulePermissions.Add(applicationModule);

                Function function = new Function
                {
                    Controller = "FunctionController",
                    Name = "Function",
                    SerialNo = 2,
                };
                context.Functions.Add(function);
                ModulePermission applicationFunction = new ModulePermission
                {
                    Module = application,
                    Permission = function
                };
                context.ModulePermissions.Add(applicationFunction);

                Function smtp = new Function
                {
                    Controller = "SMTPController",
                    Name = "SMTP",
                    SerialNo = 3
                };
                context.Functions.Add(smtp);
                ModulePermission applicationSMTP = new ModulePermission
                {
                    Module = application,
                    Permission = smtp
                };
                context.ModulePermissions.Add(applicationSMTP);
                Function systemSetting = new Function
                {
                    Controller = "SystemSettingController",
                    Name = "System Setting",
                    SerialNo = 4
                };
                context.Functions.Add(systemSetting);
                ModulePermission applicationSystemSetting = new ModulePermission
                {
                    Module = application,
                    Permission = systemSetting
                };
                context.ModulePermissions.Add(applicationSystemSetting);
                Function fileUpload = new Function
                {
                    Controller = "FileUploadController",
                    Name = "File Upload",
                    SerialNo = 5,
                    ShowOnMenu = false
                };
                context.Functions.Add(fileUpload);
                ModulePermission applicationFileUpload = new ModulePermission
                {
                    Module = application,
                    Permission = fileUpload
                };
                context.ModulePermissions.Add(applicationFileUpload);
                #endregion

                #region Common Module
                Module common = new Module
                {
                    Code = "Common",
                    Name = "Common",
                    SerialNo = 1,
                };
                context.Modules.Add(common);
                Function configurationGroup = new Function
                {
                    Controller = "ConfigurationGroupController",
                    Name = "Configuration Group",
                    SerialNo = 1
                };
                context.Functions.Add(configurationGroup);
                Function commonConfiguration = new Function
                {
                    Controller = "CommonConfigurationController",
                    Name = "Common Configuration",
                    SerialNo = 2
                };
                context.Functions.Add(commonConfiguration);
                Function location = new Function
                {
                    Controller = "LocationController",
                    Name = "Location",
                    SerialNo = 3
                };
                context.Functions.Add(location);
                ModulePermission commonConfigurationGroup = new ModulePermission
                {
                    Module = common,
                    Permission = configurationGroup,
                };
                context.ModulePermissions.Add(commonConfigurationGroup);
                ModulePermission commonCommonConfiguration = new ModulePermission
                {
                    Module = common,
                    Permission = commonConfiguration
                };
                context.ModulePermissions.Add(commonCommonConfiguration);
                ModulePermission commonLocation = new ModulePermission
                {
                    Module = common,
                    Permission = location
                };
                context.ModulePermissions.Add(commonLocation);
                Function importLocationController = new Function
                {
                    Controller = "ImportLocationController",
                    Name = "Import Location",
                    Type = FunctionType.Import
                };
                context.Functions.Add(importLocationController);
                ModulePermission commonImportLocation = new ModulePermission
                {
                    Module = common,
                    Permission = importLocationController
                };
                context.ModulePermissions.Add(commonImportLocation);
                Function hotelController = new Function
                {
                    Controller = "HotelController",
                    Name ="Hotel",
                    Type = FunctionType.Setup
                };
                ModulePermission commonHotel = new ModulePermission
                {
                    Module = common,
                    Permission = hotelController
                };
                context.ModulePermissions.Add(commonHotel);
                Function importHotelController = new Function
                {
                    Controller = "ImportHotelController",
                    Name = "Import Hotel",
                    Type = FunctionType.Import
                };
                ModulePermission commonImportHotel = new ModulePermission
                {
                    Module = common,
                    Permission = importHotelController
                };
                context.ModulePermissions.Add(commonImportHotel);
                #endregion

                #region Membership Modules
                Module user = new Module
                {
                    Created = TimeUtil.GetLocalTime(),
                    Code = "Membership",
                    Name = "Membership",
                    SerialNo = 3
                };
                context.Modules.Add(user);
                Function role = new Function
                {
                    Controller = "RoleController",
                    Name = "Role",
                    SerialNo = 1
                };
                Function userFunction = new Function
                {
                    Controller = "UserController",
                    Name = "User",
                    SerialNo = 2
                };
                Function accessControl = new Function
                {
                    Controller = "AccessControlController",
                    Name = "Access Control",
                    SerialNo = 3
                };
                context.Functions.Add(accessControl);
                ModulePermission userRole = new ModulePermission
                {
                    Module = user,
                    Permission = role
                };
                context.ModulePermissions.Add(userRole);
                ModulePermission userUserFunction = new ModulePermission
                {
                    Module = user,
                    Permission = userFunction
                };
                context.ModulePermissions.Add(userUserFunction);
                ModulePermission userAccessControl = new ModulePermission
                {
                    Module = user,
                    Permission = accessControl
                };
                context.ModulePermissions.Add(userAccessControl);
                Function userVehicle = new Function
                {
                    Controller = "UserVehicleController",
                    Name = "User Vehicle",
                    SerialNo = 4,
                    ShowOnMenu = false
                };
                context.Functions.Add(userVehicle);
                ModulePermission userUserVehicle = new ModulePermission
                {
                    Module = user,
                    Permission = userVehicle
                };
                context.ModulePermissions.Add(userUserVehicle);
                Function userCustomer = new Function
                {
                    Controller = "UserCustomerController",
                    Name = "User Customer",
                    SerialNo = 5,
                    ShowOnMenu = false
                };
                context.Functions.Add(userCustomer);
                ModulePermission userUserCustomer = new ModulePermission
                {
                    Module = user,
                    Permission = userCustomer
                };
                context.ModulePermissions.Add(userUserCustomer);
                Function isAgent = new Function
                {
                    Controller = "IsAgentController",
                    Name = "Is Agent",
                    SerialNo = 6,
                    ShowOnMenu = false
                };
                context.Functions.Add(isAgent);
                ModulePermission userIsAgent = new ModulePermission
                {
                    Module = user,
                    Permission = isAgent
                };
                context.ModulePermissions.Add(userIsAgent);
                Function isAgentMgr = new Function
                {
                    Controller = "IsAgentMgrController",
                    Name = "Is Agent Manager",
                    SerialNo = 7,
                    ShowOnMenu = false
                };
                context.Functions.Add(isAgentMgr);
                ModulePermission userIsAgentMgr = new ModulePermission
                {
                    Module = user,
                    Permission = isAgentMgr,
                };
                context.ModulePermissions.Add(userIsAgentMgr);
                Function isDriver = new Function
                {
                    Controller = "IsDriverController",
                    Name = "Is Driver",
                    SerialNo = 8,
                    ShowOnMenu = false
                };
                context.Functions.Add(isDriver);
                ModulePermission userIsDriver = new ModulePermission
                {
                    Module = user,
                    Permission = isDriver
                };
                context.ModulePermissions.Add(userIsDriver);
                Function agentImport = new Function
                {
                    Controller = "AgentImportController",
                    Name = "Import Agent",
                    SerialNo = 9,
                    ShowOnMenu = true,
                    Type = FunctionType.Import
                };
                context.Functions.Add(agentImport);
                ModulePermission membershipAgentImport = new ModulePermission
                {
                    Module = user,
                    Permission = agentImport
                };
                context.ModulePermissions.Add(membershipAgentImport);
                #endregion

                #region Asset Module
                Module asset = new Module
                {
                    Code = "Asset",
                    Name = "Asset",
                    SerialNo = 4
                };
                context.Modules.Add(asset);
                Function vehicle = new Function
                {
                    Controller = "VehicleController",
                    Name = "Vehicle",
                    SerialNo = 1
                };
                context.Functions.Add(vehicle);
                ModulePermission assetVehicle = new ModulePermission
                {
                    Module = asset,
                    Permission = vehicle
                };
                context.ModulePermissions.Add(assetVehicle);
                Function vesselFun = new Function
                {
                    Controller = "VesselController",
                    Name = "Vessel",
                    SerialNo = 2
                };
                context.Functions.Add(vesselFun);
                ModulePermission assetVessel = new ModulePermission
                {
                    Module = asset,
                    Permission = vesselFun
                };
                context.ModulePermissions.Add(assetVessel);
                #endregion

                #region Currency
                Module currency = new Module
                {
                    Created = TimeUtil.GetLocalTime(),
                    Code = "Currency",
                    Name = "Currency",
                    SerialNo = 5
                };
                context.Modules.Add(currency);
                #endregion

                #region Operation
                Module operation = new Module
                {
                    Code = "Job",
                    Name = "Job",
                    SerialNo = 6
                };
                context.Modules.Add(operation);
                Function checkList = new Function
                {
                    Controller = "CheckListController",
                    Name = "Check list",
                    SerialNo = 1
                };
                context.Functions.Add(checkList);
                ModulePermission operationCheckList = new ModulePermission
                {
                    Module = operation,
                    Permission = checkList
                };
                context.ModulePermissions.Add(operationCheckList);
                Function checkListTemplate = new Function
                {
                    Controller = "ChecklistTemplateController",
                    Name = "Checklist Tempate",
                    SerialNo = 2
                };
                context.Functions.Add(checkListTemplate);
                ModulePermission operationCheckListTemplate = new ModulePermission
                {
                    Module = operation,
                    Permission = checkListTemplate
                };
                context.ModulePermissions.Add(operationCheckListTemplate);
                #endregion

                #region SOP
                Module sop = new Module
                {
                    Code = "SOP",
                    Name = "SOP",
                    SerialNo = 7
                };
                context.Modules.Add(sop);

                //Customer
                Function customer = new Function
                {
                    Controller = "CustomerController",
                    Name = "Customer",
                    SerialNo = 1
                };
                context.Functions.Add(customer);
                ModulePermission sop_customer = new ModulePermission
                {
                    Module = sop,
                    Permission = customer
                };
                context.ModulePermissions.Add(sop_customer);

                //CustomerFileUpload
                Function customerFileUpload = new Function
                {
                    Controller = "CustomerFileUploadController",
                    Name = "Customer Attachment",
                    SerialNo = 2,
                    ShowOnMenu = false
                };
                context.Functions.Add(customerFileUpload);
                ModulePermission sop_customerFileUpload = new ModulePermission
                {
                    Module = sop,
                    Permission = customerFileUpload,
                };
                context.ModulePermissions.Add(sop_customerFileUpload);

                //customerLocation
                Function customerLocation = new Function
                {
                    Controller = "CustomerLocationController",
                    Name = "Customer Location",
                    SerialNo = 3,
                    ShowOnMenu = false
                };
                context.Functions.Add(customerLocation);
                ModulePermission sop_customerLocation = new ModulePermission
                {
                    Module = sop,
                    Permission = customerLocation,
                };
                context.ModulePermissions.Add(sop_customerLocation);

                // Customer Vessel
                Function customerVessel = new Function
                {
                    Controller = "CustomerVesselController",
                    Name = "Customer Vessel",
                    SerialNo = 4,
                    ShowOnMenu = false
                };
                context.Functions.Add(customerVessel);
                ModulePermission sop_customerVessel = new ModulePermission
                {
                    Module = sop,
                    Permission = customerVessel
                };
                context.ModulePermissions.Add(sop_customerVessel);

                //meetingService
                Function meetingService = new Function
                {
                    Controller = "MeetingServiceController",
                    Name = "Meeting Service",
                    SerialNo = 4,
                    ShowOnMenu = true
                };
                ModulePermission sop_meetingService = new ModulePermission
                {
                    Module = sop,
                    Permission = meetingService
                };

                //workOrder
                Function workOrder = new Function
                {
                    Controller = "WorkOrderController",
                    Name = "Work Order",
                    SerialNo = 5,
                    ShowOnMenu = true, 
                    Type = FunctionType.Transaction
                };
                context.Functions.Add(workOrder);
                ModulePermission sop_workorder = new ModulePermission
                {
                    Module = sop,
                    Permission = workOrder,
                };
                context.ModulePermissions.Add(sop_workorder);

                //workOrderLocation
                Function workOrderLocation = new Function
                {
                    Controller = "WorkOrderLocationController",
                    Name = "Work Order Location",
                    SerialNo = 6,
                    ShowOnMenu = false,
                    Type = FunctionType.Transaction
                };
                context.Functions.Add(workOrderLocation);
                ModulePermission sop_workOrderLocation = new ModulePermission
                {
                    Module = sop,
                    Permission = workOrderLocation
                };
                context.ModulePermissions.Add(sop_workOrderLocation);

                //workOrderMeetingService
                Function workOrderMeetingService = new Function
                {
                    Controller = "WorkOrderMeetingServiceController",
                    Name = "Work Order Meeting Service",
                    SerialNo = 7,
                    ShowOnMenu = false,
                    Type = FunctionType.Transaction
                };
                context.Functions.Add(workOrderMeetingService);
                ModulePermission sop_workOrderMeetingService = new ModulePermission
                {
                    Module = sop,
                    Permission = workOrderMeetingService
                };
                context.ModulePermissions.Add(sop_workOrderMeetingService);

                //workOrderPassenger
                Function workOrderPassenger = new Function
                {
                    Controller = "WorkOrderPassengerController",
                    Name = "Work Order Passenger",
                    SerialNo = 8,
                    ShowOnMenu = false,
                    Type = FunctionType.Transaction
                };
                context.Functions.Add(workOrderPassenger);
                ModulePermission sop_workOrderPassenger = new ModulePermission
                {
                    Module = sop,
                    Permission = workOrderPassenger
                };
                context.ModulePermissions.Add(sop_workOrderPassenger);

                //workOrderFileUpload
                Function workOrderFileUpload = new Function
                {
                    Controller = "WorkOrderFileUploadController",
                    Name = "Attachment",
                    SerialNo = 9,
                    ShowOnMenu = false,
                    Type = FunctionType.Transaction
                };
                context.Functions.Add(workOrderFileUpload);
                ModulePermission sop_workOrderFileUpload = new ModulePermission
                {
                    Module = sop,
                    Permission = workOrderFileUpload
                };
                context.ModulePermissions.Add(sop_workOrderFileUpload);

                //MoveToBilled
                Function moveToBilled = new Function
                {
                    Controller = "MoveToBilledController",
                    Name = "Move To Bill",
                    SerialNo = 10,
                    ShowOnMenu = false,
                    Type = FunctionType.Transaction
                };
                context.Functions.Add(moveToBilled);
                ModulePermission sop_moveToBilled = new ModulePermission
                {
                    Module = sop,
                    Permission = moveToBilled
                };
                context.ModulePermissions.Add(sop_moveToBilled);

                //billedOrder
                Function billedOrders = new Function
                {
                    Controller = "BilledOrdersController",
                    Name = "Billed Orders",
                    SerialNo = 10,
                    ShowOnMenu = true,
                    Type = FunctionType.Transaction
                };
                context.Functions.Add(billedOrders);
                ModulePermission sop_billedorders = new ModulePermission
                {
                    Module = sop,
                    Permission = billedOrders,
                };
                context.ModulePermissions.Add(sop_billedorders);

                //workOrderLocation
                Function billedOrderLocation = new Function
                {
                    Controller = "BilledOrderLocationController",
                    Name = " Billed Order Location",
                    SerialNo = 11,
                    ShowOnMenu = false,
                    Type = FunctionType.Transaction
                };
                context.Functions.Add(billedOrderLocation);
                ModulePermission sop_billedOrderLocation = new ModulePermission
                {
                    Module = sop,
                    Permission = billedOrderLocation
                };
                context.ModulePermissions.Add(sop_billedOrderLocation);

                //billedOrderMeetingService
                Function billedOrderMeetingService = new Function
                {
                    Controller = "BilledOrderMeetingServiceController",
                    Name = "Billed Order Meeting Service",
                    SerialNo = 12,
                    ShowOnMenu = false,
                    Type = FunctionType.Transaction
                };
                context.Functions.Add(billedOrderMeetingService);
                ModulePermission sop_billedOrderMeetingService = new ModulePermission
                {
                    Module = sop,
                    Permission = billedOrderMeetingService
                };
                context.ModulePermissions.Add(sop_billedOrderMeetingService);

                //billedOrderPassenger
                Function billedOrderPassenger = new Function
                {
                    Controller = "BilledOrderPassengerController",
                    Name = "Billed Order Passenger",
                    SerialNo = 13,
                    ShowOnMenu = false,
                    Type = FunctionType.Transaction
                };
                context.Functions.Add(billedOrderPassenger);
                ModulePermission sop_billedOrderPassenger = new ModulePermission
                {
                    Module = sop,
                    Permission = billedOrderPassenger
                };
                context.ModulePermissions.Add(sop_billedOrderPassenger);

                //billedOrderFileUpload
                Function billedOrderFileUpload = new Function
                {
                    Controller = "BilledOrderFileUploadController",
                    Name = "Attachment",
                    SerialNo = 9,
                    ShowOnMenu = false,
                    Type = FunctionType.Transaction
                };
                context.Functions.Add(billedOrderFileUpload);
                ModulePermission sop_billedOrderFileUpload = new ModulePermission
                {
                    Module = sop,
                    Permission = workOrderFileUpload
                };
                context.ModulePermissions.Add(sop_billedOrderFileUpload);

                //CreditNote
                Function creditNote = new Function
                {
                    Controller = "CreditNoteController",
                    Name = "Credit Note",
                    SerialNo = 10,
                    ShowOnMenu = true,
                    Type = FunctionType.Transaction
                };
                context.Functions.Add(creditNote);
                ModulePermission sop_creditNote = new ModulePermission
                {
                    Module = sop,
                    Permission = creditNote
                };
                context.ModulePermissions.Add(sop_creditNote);

                Function salesInvoice = new Function
                {
                    Controller = "SalesInvoiceController",
                    Name = "Invoicing Details",
                    SerialNo = 10,
                    ShowOnMenu = true,
                    Type = FunctionType.Transaction
                };
                context.Functions.Add(salesInvoice);
                ModulePermission sop_salesInvoice = new ModulePermission
                {
                    Module = sop,
                    Permission = salesInvoice
                };
                context.ModulePermissions.Add(sop_salesInvoice);

                Function invoiceSummary = new Function
                {
                    Controller = "InvoiceSummaryController",
                    Name = "Sales Invoices",
                    SerialNo = 10,
                    ShowOnMenu = true,
                    Type = FunctionType.Transaction
                };
                context.Functions.Add(invoiceSummary);
                ModulePermission sop_invoiceSummary = new ModulePermission
                {
                    Module = sop,
                    Permission = invoiceSummary
                };
                context.ModulePermissions.Add(sop_invoiceSummary);

                Function exportInvoice = new Function
                {
                    Controller = "InvoiceExcelController",
                    Name = "Export to EZ Invoice",
                    ShowOnMenu = false,
                    SerialNo = 100
                };
                context.Functions.Add(exportInvoice);
                ModulePermission export_Invoice = new ModulePermission
                {
                    Module = sop,
                    Permission = exportInvoice
                };
                context.ModulePermissions.Add(export_Invoice);
                Function exportCN = new Function
                {
                    Controller = "CreditNoteExcelController",
                    Name = "Export to EZ Credit Note",
                    ShowOnMenu = false,
                    SerialNo = 200
                };
                context.Functions.Add(exportCN);
                ModulePermission export_ExportCN = new ModulePermission
                {
                    Module = sop,
                    Permission = exportCN
                };
                context.ModulePermissions.Add(export_ExportCN);
                Function customerImport = new Function
                {
                    Controller = "CustomerImportController",
                    Name = "Customer Import",
                    Type = FunctionType.Import
                };
                context.Functions.Add(customerImport);
                ModulePermission sop_customerImport = new ModulePermission
                {
                    Module = sop,
                    Permission = customerImport
                };
                context.ModulePermissions.Add(sop_customerImport);

                #endregion

                #region POP
                Module pop = new Module
                {
                    Code = "POP",
                    Name = "POP",
                    SerialNo = 8
                };
                context.Modules.Add(pop);
                #endregion

                #region Account Module
                Module account = new Module
                {
                    Code = "Accounting",
                    Name = "Accounting",
                    SerialNo = 9
                };
                context.Modules.Add(account);
                Function gl = new Function
                {
                    Controller = "GLCodeController",
                    Name = "General Ledger Code",
                    SerialNo = 1
                };
                context.Functions.Add(gl);
                ModulePermission accountGL = new ModulePermission
                {
                    Module = account,
                    Permission = gl,
                };
                context.ModulePermissions.Add(accountGL);
                Function gst = new Function
                {
                    Controller = "GSTController",
                    Name = "GST",
                    SerialNo= 2
                };
                context.Functions.Add(gst);
                ModulePermission accountGST = new ModulePermission
                {
                    Module = account,
                    Permission = gst,
                };
                context.ModulePermissions.Add(accountGST);
                #endregion

                #region Report Module
                Module report = new Module
                {
                    Code = "Job Report",
                    Name = "Job Report",
                    SerialNo = 10
                };
                context.Modules.Add(report);
                Function driverReport = new Function
                {
                    Controller = "DriverDailyReportController",
                    Name = "Driver Daily Report",
                    SerialNo = 1,
                    Type = FunctionType.Report
                };
                context.Functions.Add(driverReport);
                ModulePermission reportDriverReport = new ModulePermission
                {
                    Module = report,
                    Permission = driverReport
                };
                context.ModulePermissions.Add(reportDriverReport);
                Function dailySummaryJobByCompany = new Function
                {
                    Controller = "DailySummaryJobByCompanyController",
                    Name = "Daily Summary Job by Company",
                    SerialNo = 2,
                    Type = FunctionType.Report
                };
                context.Functions.Add(dailySummaryJobByCompany);
                ModulePermission reportDailySummaryJobByCompany = new ModulePermission
                {
                    Module = report,
                    Permission = dailySummaryJobByCompany,
                };
                context.ModulePermissions.Add(reportDailySummaryJobByCompany);
                Function dailySummaryJobByDriver = new Function
                {
                    Controller = "DailySummaryJobByDriverController",
                    Name = "Daily Summary Job by Driver",
                    SerialNo = 3,
                    Type = FunctionType.Report
                };
                context.Functions.Add(dailySummaryJobByDriver);
                ModulePermission reportDailySummaryJobbyDriver = new ModulePermission
                {
                    Module = report,
                    Permission = dailySummaryJobByDriver,
                };
                context.ModulePermissions.Add(reportDailySummaryJobbyDriver);
                Function invoiceSummaryReport = new Function
                {
                    Controller = "InvoiceSummaryReportController",
                    Name = "Invoice Summary Report",
                    SerialNo = 4,
                    Type = FunctionType.Report
                };
                context.Functions.Add(invoiceSummaryReport);
                ModulePermission reportInvoiceSummary = new ModulePermission
                {
                    Module = report,
                    Permission = invoiceSummaryReport,
                };
                context.ModulePermissions.Add(reportInvoiceSummary);
                #endregion

                #region Inventory Module
                Module inventory = new Module
                {
                    Code = "Inventory",
                    Name = "Inventory",
                    SerialNo = 11
                };
                context.Modules.Add(inventory);

                //PriceList
                Function price = new Function
                {
                    Controller = "PriceController",
                    Name = "Price List",
                    SerialNo = 1
                };
                context.Functions.Add(price);
                ModulePermission inventoryPrice = new ModulePermission
                {
                    Module = inventory,
                    Permission = price,
                };
                context.ModulePermissions.Add(inventoryPrice);

                Function priceImport = new Function
                {
                    Controller = "PriceImportController",
                    Name = "Import Price",
                    SerialNo = 0,
                    Type = FunctionType.Import
                };
                context.Functions.Add(priceImport);
                ModulePermission inventoryPriceImport = new ModulePermission
                {
                    Module = inventory,
                    Permission = priceImport
                };
                context.ModulePermissions.Add(inventoryPriceImport);
                #endregion

                #region PDF Module
                Module pdf = new Module
                {
                    Code = "PDF",
                    Name = "PDF",
                    SerialNo = 12
                };
                context.Modules.Add(pdf);
                Function serviceJobPdf = new Function
                {
                    Controller = "ServiceJobPDFController",
                    Name = "Transfer Voucher",
                    SerialNo = 1,
                    ShowOnMenu = false
                };
                context.Functions.Add(serviceJobPdf);
                ModulePermission pdf_ServiceJobPdf = new ModulePermission
                {
                    Module = pdf,
                    Permission = serviceJobPdf
                };
                context.ModulePermissions.Add(pdf_ServiceJobPdf);
                Function invoicePdf = new Function
                {
                    Controller = "InvoicePDFController",
                    Name = "Invoice",
                    SerialNo = 2,
                    ShowOnMenu = false
                };
                context.Functions.Add(invoicePdf);
                ModulePermission pdf_InvoicePdf = new ModulePermission
                {
                    Module = pdf,
                    Permission = invoicePdf
                };

                Function creditNotePDF = new Function
                {
                    Controller = "CreditNotePDFController",
                    Name = "Credit Note",
                    SerialNo = 3,
                    ShowOnMenu = false
                };
                context.Functions.Add(creditNotePDF);
                ModulePermission pdf_CreditNotePdf = new ModulePermission
                {
                    Module = pdf,
                    Permission = creditNotePDF
                };
                context.ModulePermissions.Add(pdf_CreditNotePdf);
                #endregion

                #region export Module
                Module export = new Module
                {
                    Code = "Export",
                    Name = "Export"
                };
                context.Modules.Add(export);

                #endregion

                #region Agent Module

                Module agents = new Module
                {
                    Created = TimeUtil.GetLocalTime(),
                    Code = "Agents",
                    Name = "Agent / Customer",
                    SerialNo = 3
                };
                context.Modules.Add(agents);

                Function agentSeeOwnOrders = new Function
                {
                    Controller = "AgentSeeOwnOrdersController",
                    Name = "Display only Orders of the Agent",
                    SerialNo = 1,
                    ShowOnMenu = false
                };
                context.Functions.Add(agentSeeOwnOrders);
                ModulePermission membershipAgentSeeOwnOrders = new ModulePermission
                {
                    Module = agents,
                    Permission = agentSeeOwnOrders
                };
                context.ModulePermissions.Add(membershipAgentSeeOwnOrders);

                Function agentSeeAllOrders = new Function
                {
                    Controller = "AgentSeeCompanyOrdersController",
                    Name = "Display all Orders",
                    SerialNo = 2,
                    ShowOnMenu = false
                };
                context.Functions.Add(agentSeeAllOrders);
                ModulePermission membershipAgentSeeallOrders = new ModulePermission
                {
                    Module = agents,
                    Permission = agentSeeAllOrders
                };

                context.ModulePermissions.Add(membershipAgentSeeallOrders);
                #endregion

                #endregion

                context.SaveChanges();

                #region Account
                EngagerMark4.ApplicationCore.Account.Entities.GeneralLedger tripCharges = new EngagerMark4.ApplicationCore.Account.Entities.GeneralLedger
                {
                    Created = TimeUtil.GetLocalTime(),
                    Code = "5020",
                    Name = "Trip Charges",
                    IrregularHour = false,
                    LinkToLocation = true,
                    Creditable = true,
                    Taxable = true,
                    DiscountType = false
                };
                context.GeneralLedgers.Add(tripCharges);

                EngagerMark4.ApplicationCore.Account.Entities.GeneralLedger waitingTime = new EngagerMark4.ApplicationCore.Account.Entities.GeneralLedger
                {
                    Created = TimeUtil.GetLocalTime(),
                    Code = "5020",
                    Name = "Waiting time / Disposal",
                    IrregularHour = false,
                    LinkToLocation = true,
                    Creditable = true,
                    Taxable = true,
                    DiscountType = false
                };
                context.GeneralLedgers.Add(waitingTime);

                EngagerMark4.ApplicationCore.Account.Entities.GeneralLedger additiontalStop = new EngagerMark4.ApplicationCore.Account.Entities.GeneralLedger
                {
                    Created = TimeUtil.GetLocalTime(),
                    Code = "5020",
                    Name = "Additional Stop",
                    IrregularHour = false,
                    LinkToLocation = true,
                    Creditable = true,
                    Taxable = true,
                    DiscountType = false
                };
                context.GeneralLedgers.Add(additiontalStop);

                EngagerMark4.ApplicationCore.Account.Entities.GeneralLedger irregularHourCharges = new EngagerMark4.ApplicationCore.Account.Entities.GeneralLedger
                {
                    Created = TimeUtil.GetLocalTime(),
                    Code = "5020",
                    Name = "Irregular Hour Charges",
                    IrregularHour = true,
                    LinkToLocation = true,
                    Creditable = true,
                    Taxable = true,
                    DiscountType = false
                };
                context.GeneralLedgers.Add(irregularHourCharges);

                EngagerMark4.ApplicationCore.Account.Entities.GeneralLedger meetingServices = new EngagerMark4.ApplicationCore.Account.Entities.GeneralLedger
                {
                    Created = TimeUtil.GetLocalTime(),
                    Code = "7210",
                    Name = "Meeting Services",
                    IrregularHour = false,
                    LinkToLocation = true,
                    Creditable = true,
                    Taxable = true,
                    DiscountType = false
                };
                context.GeneralLedgers.Add(meetingServices);

                EngagerMark4.ApplicationCore.Account.Entities.GeneralLedger airportParking = new EngagerMark4.ApplicationCore.Account.Entities.GeneralLedger
                {
                    Created = TimeUtil.GetLocalTime(),
                    Code = "7203",
                    Name = "Airport Parking",
                    IrregularHour = false,
                    LinkToLocation = true,
                    Creditable = true,
                    Taxable = false,
                    DiscountType = false
                };
                context.GeneralLedgers.Add(airportParking);

                EngagerMark4.ApplicationCore.Account.Entities.GeneralLedger harbourfrontParking = new EngagerMark4.ApplicationCore.Account.Entities.GeneralLedger
                {
                    Created = TimeUtil.GetLocalTime(),
                    Code = "7203",
                    Name = "Parking @ Harbourfront",
                    IrregularHour = false,
                    LinkToLocation = true,
                    Creditable = true,
                    Taxable = false,
                    DiscountType = false
                };
                context.GeneralLedgers.Add(harbourfrontParking);

                EngagerMark4.ApplicationCore.Account.Entities.GeneralLedger embassyParking = new EngagerMark4.ApplicationCore.Account.Entities.GeneralLedger
                {
                    Created = TimeUtil.GetLocalTime(),
                    Code = "7203",
                    Name = "Parking @ Embassy",
                    IrregularHour = false,
                    LinkToLocation = true,
                    Creditable = true,
                    Taxable = false,
                    DiscountType = false
                };
                context.GeneralLedgers.Add(embassyParking);

                EngagerMark4.ApplicationCore.Account.Entities.GeneralLedger ERP = new EngagerMark4.ApplicationCore.Account.Entities.GeneralLedger
                {
                    Created = TimeUtil.GetLocalTime(),
                    Code = "7204",
                    Name = "ERP",
                    IrregularHour = false,
                    LinkToLocation = true,
                    Creditable = true,
                    Taxable = false,
                    DiscountType = false
                };
                context.GeneralLedgers.Add(ERP);

                EngagerMark4.ApplicationCore.Account.Entities.GeneralLedger tollFee = new EngagerMark4.ApplicationCore.Account.Entities.GeneralLedger
                {
                    Created = TimeUtil.GetLocalTime(),
                    Code = "7270",
                    Name = "Toll Fee",
                    IrregularHour = false,
                    LinkToLocation = true,
                    Creditable = true,
                    Taxable = false,
                    DiscountType = false
                };
                context.GeneralLedgers.Add(tollFee);

                EngagerMark4.ApplicationCore.Account.Entities.GeneralLedger visaFee = new EngagerMark4.ApplicationCore.Account.Entities.GeneralLedger
                {
                    Created = TimeUtil.GetLocalTime(),
                    Code = "7230",
                    Name = "Visa Fee",
                    IrregularHour = false,
                    LinkToLocation = true,
                    Creditable = true,
                    Taxable = false,
                    DiscountType = false
                };
                context.GeneralLedgers.Add(visaFee);

                EngagerMark4.ApplicationCore.Account.Entities.GeneralLedger PSAPassFee = new EngagerMark4.ApplicationCore.Account.Entities.GeneralLedger
                {
                    Created = TimeUtil.GetLocalTime(),
                    Code = "7230",
                    Name = "PSA Pass Fee",
                    IrregularHour = false,
                    LinkToLocation = true,
                    Creditable = true,
                    Taxable = false,
                    DiscountType = false
                };
                context.GeneralLedgers.Add(PSAPassFee);

                EngagerMark4.ApplicationCore.Account.Entities.GeneralLedger jurongPortFee = new EngagerMark4.ApplicationCore.Account.Entities.GeneralLedger
                {
                    Created = TimeUtil.GetLocalTime(),
                    Code = "7230",
                    Name = "Jurong Port Pass Fee",
                    IrregularHour = false,
                    LinkToLocation = true,
                    Creditable = true,
                    Taxable = false,
                    DiscountType = false
                };
                context.GeneralLedgers.Add(jurongPortFee);

                EngagerMark4.ApplicationCore.Account.Entities.GeneralLedger certificateFee = new EngagerMark4.ApplicationCore.Account.Entities.GeneralLedger
                {
                    Created = TimeUtil.GetLocalTime(),
                    Code = "7230",
                    Name = "Certificate Fee",
                    IrregularHour = false,
                    LinkToLocation = true,
                    Creditable = true,
                    Taxable = false,
                    DiscountType = false
                };
                context.GeneralLedgers.Add(certificateFee);

                EngagerMark4.ApplicationCore.Account.Entities.GeneralLedger hotelAccomodations = new EngagerMark4.ApplicationCore.Account.Entities.GeneralLedger
                {
                    Created = TimeUtil.GetLocalTime(),
                    Code = "7290",
                    Name = "Hotel Accomodation / Meals",
                    IrregularHour = false,
                    LinkToLocation = true,
                    Creditable = true,
                    Taxable = false,
                    DiscountType = false
                };
                context.GeneralLedgers.Add(hotelAccomodations);

                EngagerMark4.ApplicationCore.Account.Entities.GeneralLedger mealAllowance = new EngagerMark4.ApplicationCore.Account.Entities.GeneralLedger
                {
                    Created = TimeUtil.GetLocalTime(),
                    Code = "7290",
                    Name = "Meal Allowance",
                    IrregularHour = false,
                    LinkToLocation = true,
                    Creditable = true,
                    Taxable = false,
                    DiscountType = false
                };
                context.GeneralLedgers.Add(mealAllowance);

                EngagerMark4.ApplicationCore.Account.Entities.GeneralLedger porterageFee = new EngagerMark4.ApplicationCore.Account.Entities.GeneralLedger
                {
                    Created = TimeUtil.GetLocalTime(),
                    Code = "7260",
                    Name = "Porterage Fee",
                    IrregularHour = false,
                    LinkToLocation = true,
                    Creditable = true,
                    Taxable = false,
                    DiscountType = false
                };
                context.GeneralLedgers.Add(porterageFee);

                EngagerMark4.ApplicationCore.Account.Entities.GeneralLedger ferryTicket = new EngagerMark4.ApplicationCore.Account.Entities.GeneralLedger
                {
                    Created = TimeUtil.GetLocalTime(),
                    Code = "7260",
                    Name = "Ferry Ticket",
                    IrregularHour = false,
                    LinkToLocation = true,
                    Creditable = true,
                    Taxable = false,
                    DiscountType = false
                };
                context.GeneralLedgers.Add(ferryTicket);

                EngagerMark4.ApplicationCore.Account.Entities.GeneralLedger handphoneUsage = new EngagerMark4.ApplicationCore.Account.Entities.GeneralLedger
                {
                    Created = TimeUtil.GetLocalTime(),
                    Code = "7750",
                    Name = "Handphone Usage / SIM Card",
                    IrregularHour = false,
                    LinkToLocation = true,
                    Creditable = true,
                    Taxable = false,
                    DiscountType = false
                };
                context.GeneralLedgers.Add(handphoneUsage);

                EngagerMark4.ApplicationCore.Account.Entities.GeneralLedger cnInvoice = new EngagerMark4.ApplicationCore.Account.Entities.GeneralLedger
                {
                    Created = TimeUtil.GetLocalTime(),
                    Code = "5021",
                    Name = "CN Invoice (Discount for Customers)",
                    IrregularHour = false,
                    LinkToLocation = true,
                    Creditable = true,
                    Taxable = true,
                    DiscountType = true
                };
                context.GeneralLedgers.Add(cnInvoice);
                #endregion

                #region Role
                #region General Admin
                Role generalAdmin = new Role
                {
                    Created = TimeUtil.GetLocalTime(),
                    Code = "GeneralAdmin",
                    Name = "General Admin"
                };
                context.EngagerRoles.Add(generalAdmin);

                RolePermission generalAdminCompany = new RolePermission
                {
                    Role = generalAdmin,
                    PermissionId = company.Id
                };
                context.RolePermissions.Add(generalAdminCompany);

                RolePermission generalAdminModule = new RolePermission
                {
                    Role = generalAdmin,
                    PermissionId = module.Id
                };
                context.RolePermissions.Add(generalAdminModule);

                RolePermission generalAdminFunction = new RolePermission
                {
                    Role = generalAdmin,
                    PermissionId = function.Id
                };
                context.RolePermissions.Add(generalAdminFunction);

                RolePermission generalAdminSMTP = new RolePermission
                {
                    Role = generalAdmin,
                    PermissionId = smtp.Id
                };
                context.RolePermissions.Add(generalAdminSMTP);

                RolePermission generalAdminSystemSetting = new RolePermission
                {
                    Role = generalAdmin,
                    PermissionId = systemSetting.Id
                };
                context.RolePermissions.Add(generalAdminSystemSetting);

                RolePermission generalAdminConfigurationGroup = new RolePermission
                {
                    Role = generalAdmin,
                    PermissionId = configurationGroup.Id
                };
                context.RolePermissions.Add(generalAdminConfigurationGroup);

                RolePermission generalAdminCommonConfiguration = new RolePermission
                {
                    Role = generalAdmin,
                    PermissionId = commonConfiguration.Id
                };
                context.RolePermissions.Add(generalAdminCommonConfiguration);

                RolePermission generalAdminLocation = new RolePermission
                {
                    Role = generalAdmin,
                    PermissionId = location.Id
                };
                context.RolePermissions.Add(generalAdminLocation);

                RolePermission generalAdminRole = new RolePermission
                {
                    Role = generalAdmin,
                    PermissionId = role.Id
                };
                context.RolePermissions.Add(generalAdminRole);

                RolePermission generalAdminUser = new RolePermission
                {
                    Role = generalAdmin,
                    PermissionId = userFunction.Id
                };
                context.RolePermissions.Add(generalAdminUser);

                RolePermission generalAdminAccessControl = new RolePermission
                {
                   Role = generalAdmin,
                   PermissionId = accessControl.Id
                };
                context.RolePermissions.Add(generalAdminAccessControl);

                RolePermission generalAdminHome = new RolePermission
                {
                    Role = generalAdmin,
                    PermissionId = home.Id
                };
                context.RolePermissions.Add(generalAdminHome);

                RolePermission generalAdminCustomer = new RolePermission
                {
                    Role = generalAdmin,
                    PermissionId = customer.Id
                };
                context.RolePermissions.Add(generalAdminCustomer);

                RolePermission generalAdminGeneralLedger = new RolePermission
                {
                    Role = generalAdmin,
                    PermissionId = gl.Id
                };
                context.RolePermissions.Add(generalAdminGeneralLedger);

                RolePermission generalAdminGST = new RolePermission
                {
                    Role = generalAdmin,
                    PermissionId = gst.Id
                };
                context.RolePermissions.Add(generalAdminGST);

                RolePermission generalAdminChecklist = new RolePermission
                {
                    Role = generalAdmin,
                    PermissionId = checkList.Id
                };
                context.RolePermissions.Add(generalAdminChecklist);

                RolePermission generalAdminChecklistTemplate = new RolePermission
                {
                    Role = generalAdmin,
                    PermissionId = checkListTemplate.Id
                };
                context.RolePermissions.Add(generalAdminChecklistTemplate);

                RolePermission generalAdminFileUpload = new RolePermission
                {
                    Role = generalAdmin,
                    PermissionId = fileUpload.Id
                };
                context.RolePermissions.Add(generalAdminFileUpload);

                RolePermission generalAdminVehicle = new RolePermission
                {
                    Role = generalAdmin,
                    PermissionId = vehicle.Id
                };
                context.RolePermissions.Add(generalAdminVehicle);

                RolePermission generalAdminCustomerFileUpload = new RolePermission
                {
                    Role = generalAdmin,
                    PermissionId = customerFileUpload.Id,
                };
                context.RolePermissions.Add(generalAdminCustomerFileUpload);

                RolePermission generalAdminUserVehicle = new RolePermission
                {
                    Role = generalAdmin,
                    PermissionId = userVehicle.Id
                };
                context.RolePermissions.Add(generalAdminUserVehicle);

                RolePermission generalAdminUserCustomer = new RolePermission
                {
                    Role = generalAdmin,
                    PermissionId = userCustomer.Id
                };
                context.RolePermissions.Add(generalAdminUserCustomer);

                RolePermission generalAdminCustomerLocation = new RolePermission
                {
                    Role= generalAdmin,
                    PermissionId = customerLocation.Id
                };
                context.RolePermissions.Add(generalAdminCustomerLocation);

                RolePermission generalAdminWorkOrder = new RolePermission
                {
                    Role = generalAdmin,
                    PermissionId = workOrder.Id
                };
                context.RolePermissions.Add(generalAdminWorkOrder);

                RolePermission generalAdminWorkOrderLocation = new RolePermission
                {
                    Role = generalAdmin,
                    PermissionId = workOrderLocation.Id
                };
                context.RolePermissions.Add(generalAdminWorkOrderLocation);

                RolePermission generalAdminWorkOrderMeetingService = new RolePermission
                {
                    Role = generalAdmin,
                    PermissionId = workOrderMeetingService.Id
                };
                context.RolePermissions.Add(generalAdminWorkOrderMeetingService);

                RolePermission generalAdminWorkOrderPassenger = new RolePermission
                {
                    Role = generalAdmin,
                    PermissionId = workOrderPassenger.Id
                };
                context.RolePermissions.Add(generalAdminWorkOrderPassenger);

                RolePermission generalAdminworkOrderFileUpload = new RolePermission
                {
                    Role = generalAdmin,
                    PermissionId = workOrderFileUpload.Id
                };
                context.RolePermissions.Add(generalAdminworkOrderFileUpload);

                RolePermission generalAdminSalesInvoice = new RolePermission
                {
                    Role = generalAdmin,
                    PermissionId = salesInvoice.Id
                };
                context.RolePermissions.Add(generalAdminSalesInvoice);

                RolePermission generalAdminTransferVoucher = new RolePermission
                {
                    Role = generalAdmin,
                    PermissionId = serviceJobPdf.Id
                };
                context.RolePermissions.Add(generalAdminTransferVoucher);

                RolePermission generalAdminInvoicePdf = new RolePermission
                {
                    Role = generalAdmin,
                    PermissionId = invoicePdf.Id
                };
                context.RolePermissions.Add(generalAdminInvoicePdf);

                RolePermission generalAdminCreditNotePDF = new RolePermission
                {
                    Role = generalAdmin,
                    PermissionId = creditNotePDF.Id
                };
                context.RolePermissions.Add(generalAdminCreditNotePDF);


                RolePermission generalAdminCustomerVessel = new RolePermission
                {
                    Role = generalAdmin,
                    PermissionId = customerVessel.Id
                };
                context.RolePermissions.Add(generalAdminCustomerVessel);

                RolePermission generalAdminVessel = new RolePermission
                {
                    Role = generalAdmin,
                    PermissionId = vesselFun.Id
                };
                context.RolePermissions.Add(generalAdminVessel);

                RolePermission generalAdminDriverReport = new RolePermission
                {
                    Role = generalAdmin,
                    PermissionId = driverReport.Id
                };
                context.RolePermissions.Add(generalAdminDriverReport);

                RolePermission generalAdminDailySummaryJobByCompany = new RolePermission
                {
                    Role = generalAdmin,
                    PermissionId = reportDailySummaryJobByCompany.Id
                };
                context.RolePermissions.Add(generalAdminDailySummaryJobByCompany);

                RolePermission generalAdminDailySummaryJobByDriver = new RolePermission
                {
                    Role = generalAdmin,
                    PermissionId = reportDailySummaryJobbyDriver.Id
                };
                context.RolePermissions.Add(generalAdminDailySummaryJobByDriver);

                RolePermission generalAdminIsAgent = new RolePermission
                {
                    Role = generalAdmin,
                    PermissionId = isAgent.Id
                };
                context.RolePermissions.Add(generalAdminIsAgent);

                RolePermission generalAdminIsAgentMgr = new RolePermission
                {
                    Role= generalAdmin,
                    PermissionId = isAgentMgr.Id
                };
                context.RolePermissions.Add(generalAdminIsAgentMgr);

                RolePermission generalAdminIsDriver = new RolePermission
                {
                    Role = generalAdmin,
                    PermissionId = isDriver.Id
                };
                context.RolePermissions.Add(generalAdminIsDriver);

                RolePermission generalAdminExportInvoice = new RolePermission
                {
                    Role = generalAdmin,
                    PermissionId = exportInvoice.Id
                };
                context.RolePermissions.Add(generalAdminExportInvoice);

                RolePermission generalAdminExportCN = new RolePermission
                {
                    Role = generalAdmin,
                    PermissionId = exportCN.Id
                };
                context.RolePermissions.Add(generalAdminExportCN);

                RolePermission generalAdminImportLocation = new RolePermission
                {
                    Role = generalAdmin,
                    PermissionId = importLocationController.Id
                };
                context.RolePermissions.Add(generalAdminImportLocation);

                RolePermission generalAdminCustomerImport = new RolePermission
                {
                    Role = generalAdmin,
                    PermissionId = customerImport.Id
                };
                context.RolePermissions.Add(generalAdminCustomerImport);

                RolePermission generalAdminPriceImport = new RolePermission
                {
                    Role = generalAdmin,
                    PermissionId = priceImport.Id
                };
                context.RolePermissions.Add(generalAdminPriceImport);

                RolePermission generalAdminAgentImport = new RolePermission
                {
                    Role = generalAdmin,
                    PermissionId = agentImport.Id
                };
                context.RolePermissions.Add(generalAdminAgentImport);

                RolePermission generalAdminHotel = new RolePermission
                {
                    Role = generalAdmin,
                    PermissionId = hotelController.Id
                };
                context.RolePermissions.Add(generalAdminHotel);

                RolePermission generalAdminImportHotel = new RolePermission
                {
                    Role = generalAdmin,
                    PermissionId = importHotelController.Id
                };
                context.RolePermissions.Add(generalAdminImportHotel);
                #endregion

                Role admin = new Role
                {
                    Created = TimeUtil.GetLocalTime(),
                    Code = "Admin",
                    Name = "Admin"
                };
                context.EngagerRoles.Add(admin);

                Role accountant = new Role
                {
                    Created = TimeUtil.GetLocalTime(),
                    Code = "Accountant",
                    Name = "Accountant"
                };
                context.EngagerRoles.Add(accountant);

                Role supervisor = new Role
                {
                    Created = TimeUtil.GetLocalTime(),
                    Code = "Supervisor",
                    Name = "Supervisor",
                };
                context.EngagerRoles.Add(supervisor);

                Role opsAdmin = new Role
                {
                    Created = TimeUtil.GetLocalTime(),
                    Code = "OpsAdmin",
                    Name = "Ops Admin"
                };
                context.EngagerRoles.Add(opsAdmin);

                Role invoicingAdmin = new Role
                {
                    Created = TimeUtil.GetLocalTime(),
                    Code = "InvoicingAdmin",
                    Name = "Invoicing Admin"
                };
                context.EngagerRoles.Add(invoicingAdmin);

                Role agent = new Role
                {
                    Code = "Agent",
                    Name = "Agent"
                };
                context.EngagerRoles.Add(agent);

                Role driver = new Role
                {
                    Code = "Driver",
                    Name = "Driver"
                };
                context.EngagerRoles.Add(driver);
                #endregion

                #region System Setting
                SystemSetting googleMapKey = new SystemSetting
                {
                    Code = AppSettingKey.Key.GOOGLE_MAP_KEY.ToString(),
                    Name = AppSettingKey.Key.GOOGLE_MAP_KEY.ToString(),
                    Value = "AIzaSyDjA-Bq8QPw6AYQDeePH43-Qq7qVIvtGLM",
                };
                context.SystemSettings.Add(googleMapKey);
                SystemSetting googleMapUrl = new SystemSetting
                {
                    Code = AppSettingKey.Key.GOOGLE_MAP_URL.ToString(),
                    Name = AppSettingKey.Key.GOOGLE_MAP_URL.ToString(),
                    Value = "https://maps.google.com/maps/api/geocode/xml?address=[postalCode]&sensor=false&key=[key]",
                };
                context.SystemSettings.Add(googleMapUrl);

                SystemSetting priceCodeCount = new SystemSetting
                {
                    Code = AppSettingKey.Key.PRICE_CODE_NUM_COUNT.ToString(),
                    Name = AppSettingKey.Key.PRICE_CODE_NUM_COUNT.ToString(),
                    Value = "4"
                };
                context.SystemSettings.Add(priceCodeCount);

                SystemSetting fcmServerKey = new SystemSetting
                {
                    Code = AppSettingKey.Key.FCM_SERVER_KEY.ToString(),
                    Name = AppSettingKey.Key.FCM_SERVER_KEY.ToString(),
                    Value = "AAAA5KYEjcU:APA91bFVAZxyVyDEPdj6VZjymAqJNrH0GcoiqxPaeavbS7WAE__z3yJUmnObx81z5pLPNh4IHPiAWckyXs7b6EK82QJvcaocd3MJlZsqIrabS2fJoo1tAnPNj0siw1poffV9OK5GIxjK"
                };
                context.SystemSettings.Add(fcmServerKey);

                SystemSetting fcmSenderId = new SystemSetting
                {
                    Code = AppSettingKey.Key.FCM_SERVER_SENDERID.ToString(),
                    Name = AppSettingKey.Key.FCM_SERVER_SENDERID.ToString(),
                    Value = "982037859781"
                };
                context.SystemSettings.Add(fcmSenderId);

                SystemSetting fcmSenderUrl = new SystemSetting
                {
                    Code = AppSettingKey.Key.FCM_URL.ToString(),
                    Name = AppSettingKey.Key.FCM_URL.ToString(),
                    Value = "https://fcm.googleapis.com/fcm/send"
                };
                context.SystemSettings.Add(fcmSenderUrl);

                SystemSetting workOrderPushNotifiedDay = new SystemSetting
                {
                    Code = AppSettingKey.Key.WORKORDER_PUSH_NOTIFIED_DAY.ToString(),
                    Name = AppSettingKey.Key.WORKORDER_PUSH_NOTIFIED_DAY.ToString(),
                    Value = "1"
                };
                context.SystemSettings.Add(workOrderPushNotifiedDay);

                SystemSetting workOrderRemoveCancelledJobsIntervalMin = new SystemSetting
                {
                    Code = AppSettingKey.Key.WORKORDER_CANCELLEDJOB_REMOVAL_INTERVAL_MIN.ToString(),
                    Name = AppSettingKey.Key.WORKORDER_CANCELLEDJOB_REMOVAL_INTERVAL_MIN.ToString(),
                    Value = "2880" //48 Hrs
                };
                context.SystemSettings.Add(workOrderRemoveCancelledJobsIntervalMin);
                #endregion

                #region CheckList setup
                CheckList buyFerryTicket = new CheckList
                {
                    Code = "BuyFerryTicket",
                    Name = "Buy Ferry Ticket",
                    Type = CheckList.ControlType.Checkbox
                };
                context.CheckLists.Add(buyFerryTicket);
                CheckList buyMeals = new CheckList
                {
                    Code = "BuyMeals",
                    Name = "Buy Meals",
                    Type = CheckList.ControlType.Checkbox
                };
                context.CheckLists.Add(buyMeals);
                CheckList carPark = new CheckList
                {
                    Code = "CarPark",
                    Name = "Car Park",
                    Type = CheckList.ControlType.Checkbox
                };
                context.CheckLists.Add(carPark);
                CheckList cashOrder = new CheckList
                {
                    Code = "CashOrder",
                    Name = "Cash Order",
                    Type = CheckList.ControlType.Checkbox
                };
                context.CheckLists.Add(cashOrder);
                CheckList erp = new CheckList
                {
                    Code = "ERP",
                    Name = "ERP",
                    Type = CheckList.ControlType.Checkbox
                };
                context.CheckLists.Add(erp);
                CheckList chkirregularHourCharges = new CheckList
                {
                    Code = "IrregularHourCharges",
                    Name = "Irregular Hour Charges",
                    Type = CheckList.ControlType.Checkbox
                };
                context.CheckLists.Add(chkirregularHourCharges);
                CheckList chkJohorTrip = new CheckList
                {
                    Code = "JohorTrip",
                    Name = "Johor Trip",
                    Type = CheckList.ControlType.Checkbox
                };
                context.CheckLists.Add(chkJohorTrip);
                CheckList chkTollFees = new CheckList
                {
                    Code = "TollFees",
                    Name = "Toll Fees"
                };
                context.CheckLists.Add(chkTollFees);
                CheckList chkVisaFee = new CheckList
                {
                    Code = "VisaFee",
                    Name = "Visa Fees"
                };
                context.CheckLists.Add(chkVisaFee);
                #endregion
            }
        }

        public Company GetCompanyByDomain(string domain)
        {
            return dbSet.SingleOrDefault(x => x.Domain.Contains(domain));
        }
    }
}
