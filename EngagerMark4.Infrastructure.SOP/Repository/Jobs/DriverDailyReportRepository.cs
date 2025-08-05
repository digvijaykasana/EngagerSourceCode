using EngagerMark4.ApplicationCore.SOP.IRepository.SerivceJobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EngagerMark4.ApplicationCore.Cris.Users;
using EngagerMark4.ApplicationCore.ReportViewModels;
using EngagerMark4.Infrasturcture.EFContext;
using EngagerMark4.Common;

namespace EngagerMark4.Infrastructure.SOP.Repository.Jobs
{
    public class DriverDailyReportRepository : IDriverDailyReportRepository
    {
        ApplicationDbContext _context;
        public DriverDailyReportRepository(ApplicationDbContext context)
        {
            this._context = context;
        }

        public IEnumerable<DriverDailyReportViewModel> GetDriverDailyReport(DriverDailyReportCri cri)
        {
            var query = from serviceJob in _context.ServiceJobs.AsNoTracking()
                        join workOrder in _context.WorkOrders.AsNoTracking() on serviceJob.WorkOrderId equals workOrder.Id
                        join user in _context.EngagerUsers on serviceJob.UserId equals user.Id
                        join agent in _context.EngagerUsers on workOrder.AgentId equals agent.Id into temp
                        from oAgent in temp.DefaultIfEmpty()
                        join vehicle in _context.Vehicles on serviceJob.VehicleId equals vehicle.Id
                        join vessel in _context.CommonConfigurations on workOrder.VesselId equals vessel.Id into temp2
                        from oVessel in temp2.DefaultIfEmpty()
                        where serviceJob.ParentCompanyId == GlobalVariable.COMPANY_ID
                        select new
                        DriverDailyReportViewModel
                        {                           
                            WorkOrderNo = workOrder.RefereneceNo,
                            DriverId = user.Id,
                            DriverName = user.LastName+" "+user.FirstName,
                            VehicleNo = vehicle.VehicleNo,
                            PickUpDate = workOrder.PickUpdateDate,
                            StartExecutionTime = serviceJob.StartExecutionTime,
                            EndExecutionTime = serviceJob.EndExecutionTime,
                            AgendId = oAgent.Id,
                            AgentName = oAgent.LastName + " " + oAgent.FirstName,
                            VesselId = oVessel.Id,
                            VesselName = oVessel.Name,
                            PickUpPoint = workOrder.PickUpPoint,
                            DropOffPoint = workOrder.DropPoint,
                            NoOfPax = workOrder.NoOfPax,
                            PickupPax = serviceJob.Id1, //Pickup Pax
                            Remark = serviceJob.DriverRemark,
                            MS = serviceJob.MSFees,
                            Trip = serviceJob.TripFees,
                            CheckListIds = serviceJob.CheckListIds
                        };

            if(cri.Date !=null)
            {
                query = query.Where(x => x.PickUpDate >= cri.Date);
            }

            if(cri.ToDate != null)
            {
                var tmr = cri.ToDate.Value.AddDays(1);
                query = query.Where(x => x.PickUpDate < tmr);
            }

            if(cri.DriverId!=null)
            {
                query = query.Where(x => x.DriverId == cri.DriverId);
            }

            return query.OrderBy(x => x.PickUpDate);
        }

        public IEnumerable<DriverDailyReportViewModelMobile> GetDriverDailyReportForMobile(DriverDailyReportCri cri)
        {
            try
            {
                var query = from serviceJob in _context.ServiceJobs.AsNoTracking()
                            join workOrder in _context.WorkOrders.AsNoTracking() on serviceJob.WorkOrderId equals workOrder.Id
                            join customer in _context.Customers.AsNoTracking() on workOrder.CustomerId equals customer.Id
                            join user in _context.EngagerUsers.AsNoTracking() on serviceJob.UserId equals user.Id
                            join agent in _context.EngagerUsers.AsNoTracking() on workOrder.AgentId equals agent.Id into temp
                            from oAgent in temp.DefaultIfEmpty()
                            join vehicle in _context.Vehicles.AsNoTracking() on serviceJob.VehicleId equals vehicle.Id
                            where serviceJob.ParentCompanyId == GlobalVariable.COMPANY_ID 
                            && workOrder.Status >= ApplicationCore.SOP.Entities.WorkOrders.WorkOrder.OrderStatus.Assigned
                            && workOrder.Status < ApplicationCore.SOP.Entities.WorkOrders.WorkOrder.OrderStatus.Verified
                            select new
                            DriverDailyReportViewModelMobile
                            {
                                WorkOrderId = workOrder.Id,
                                ServiceJobId = serviceJob.Id,
                                WorkOrderNo = workOrder.RefereneceNo,
                                Description = workOrder.Description,
                                DriverId = user.Id,
                                DriverName = user.LastName + " " + user.FirstName,
                                VehicleNo = vehicle.VehicleNo,
                                PickUpDate = workOrder.PickUpdateDate,
                                StandByDate = workOrder.StandByDate,
                                StartExecutionTime = serviceJob.StartExecutionTime,
                                EndExecutionTime = serviceJob.EndExecutionTime,
                                AgendId = oAgent.Id,
                                AgentName = oAgent.LastName + " " + oAgent.FirstName,
                                CompanyName = customer.Name,
                                VesselId = workOrder.VesselId,
                                VesselName = workOrder.ShortText1, //Vessel Name
                                PickUpPoint = workOrder.PickUpPoint,
                                DropOffPoint = workOrder.DropPoint,
                                Remark = workOrder.AdminRemarksToDriver,
                                Status = workOrder.Status,
                                ServiceJobStatus = serviceJob.Status,
                                PickupPax = serviceJob.Id1,
                                BoardTypeId = workOrder.BoardTypeId,
                                TFRequireAllPassSignatures = customer.TFRequireAllPassSignatures
                            };


                //PCR2021 - Remove 48 hours validation check for drivers to submit report
                if (cri.Date != null)
                {
                    var today = cri.Date.Value.AddDays(-30);
                    query = query.Where(x => x.PickUpDate >= today);
                }

                if (cri.ToDate != null)
                {
                    var tmr = cri.ToDate.Value.AddDays(2);
                    query = query.Where(x => x.PickUpDate < tmr);
                }

                if (cri.DriverId != null)
                {
                    query = query.Where(x => x.DriverId == cri.DriverId);
                }

                if(cri.ServiceJobId != null && cri.ServiceJobId!= 0)
                {
                    query = query.Where(x => x.ServiceJobId == cri.ServiceJobId);
                }

                if (cri.WorkOrderId != null && cri.WorkOrderId != 0)
                {
                    query = query.Where(x => x.WorkOrderId == cri.WorkOrderId);
                }

                return query.OrderBy(x => x.PickUpDate);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
