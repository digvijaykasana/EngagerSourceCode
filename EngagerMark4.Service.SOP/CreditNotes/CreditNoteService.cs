using EngagerMark4.ApplicationCore.Customer.Entities;
using EngagerMark4.ApplicationCore.Customer.IRepository;
using EngagerMark4.ApplicationCore.Entities.Configurations;
using EngagerMark4.ApplicationCore.IRepository.Configurations;
using EngagerMark4.ApplicationCore.IRepository.Users;
using EngagerMark4.ApplicationCore.SOP.Cris;
using EngagerMark4.ApplicationCore.SOP.Entities.CreditNotes;
using EngagerMark4.ApplicationCore.SOP.Entities.WorkOrders;
using EngagerMark4.ApplicationCore.SOP.IRepository.CreditNotes;
using EngagerMark4.ApplicationCore.SOP.IRepository.WorkOrders;
using EngagerMark4.ApplicationCore.SOP.IService.CreditNotes;
using EngagerMark4.ApplicationCore.SOP.IService.WorkOrders;
using EngagerMark4.ApplicationCore.SOP.ViewModels;
using EngagerMark4.Infrastructure.Customer.Repository;
using EngagerMark4.Infrastructure.SOP.Repository;
using EngagerMark4.Infrasturcture.EFContext;
using EngagerMark4.Infrasturcture.Repository;
using EngagerMark4.Infrasturcture.Repository.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.Service.SOP.CreditNotes
{
    public class CreditNoteService : AbstractService<ICreditNoteRepository, CreditNoteCri, CreditNote>, ICreditNoteService
    {
        ApplicationDbContext _context;
        IRolePermissionRepository rolePermissionRepository;
        IWorkOrderRepository workOrderRepository;
        ICustomerRepository customerRepository;
        ICommonConfigurationRepository commonConfigurationRepository;

        public CreditNoteService (ICreditNoteRepository repository, ApplicationDbContext _context, IRolePermissionRepository rolePermissionRepository,
            IWorkOrderRepository workOrderRepository, ICustomerRepository customerRepository, ICommonConfigurationRepository commonConfigurationRepository) : base(repository)
        {
            this._context = _context;
            this.rolePermissionRepository = rolePermissionRepository;
            this.workOrderRepository = workOrderRepository;
            this.customerRepository = customerRepository;
            this.commonConfigurationRepository = commonConfigurationRepository;
        }

        public async Task<CreditNoteReportPDF> GetDetailsReport(CreditNoteCri aCri)
        {
            return await this.repository.GetCreditNoteReportViewModel(aCri);
        }

        public async override Task<long> Save(CreditNote entity)
        {
            bool needToGeneralSerialNo = entity.Id == 0 ? true : false ;

            entity.GSTAmount = Math.Round(entity.GSTAmount, 2, MidpointRounding.AwayFromZero);
            entity.SubTotal = Math.Round(entity.SubTotal, 2, MidpointRounding.AwayFromZero);
            entity.GrandTotal = Math.Round(entity.GrandTotal, 2, MidpointRounding.AwayFromZero);

            this.repository.Save(entity);

            await this.repository.SaveChangesAsync();

            if (needToGeneralSerialNo)
            {
                SerialNoRepository<CreditNoteSerialNo> serialNoRepoitory = new SerialNoRepository<CreditNoteSerialNo>(_context);
                
                WorkOrder woEntity = await workOrderRepository.GetById(entity.OrderId);

                Customer cusEntity = await customerRepository.GetById(woEntity.CustomerId);

                CommonConfiguration comEntity = await commonConfigurationRepository.GetById(entity.DiscountType);

                entity.OrderNo = woEntity.RefereneceNo;

                entity.CreditNoteNo = entity.GetCreditNoteNo(serialNoRepoitory.GetSerialNo(entity.Id), cusEntity);

                entity.DiscountTypeName = comEntity.Name;

               await this.repository.SaveChangesAsync();
            }

            return entity.Id;
        }
    }
}
