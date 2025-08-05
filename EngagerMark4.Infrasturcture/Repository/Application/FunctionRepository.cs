using EngagerMark4.ApplicationCore.Cris.Application;
using EngagerMark4.ApplicationCore.Entities.Application;
using EngagerMark4.ApplicationCore.IRepository.Application;
using EngagerMark4.Common;
using EngagerMark4.Infrasturcture.EFContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.Infrasturcture.Repository.Application
{
    public class FunctionRepository : GenericRepository<ApplicationDbContext, PermissionCri, Function> , IFunctionRepository
    {
        public FunctionRepository(ApplicationDbContext aContext) : base(aContext)
        {
        }

        protected override void ApplyCri(PermissionCri cri)
        {
            if (cri != null && !string.IsNullOrEmpty(cri.SearchValue))
                base.queryableData = queryableData.Where(x => x.Name.ToLower().Contains(cri.SearchValue.ToLower()));
        }

        public Function GetByController(string controller)
        {
            return context.Functions.AsNoTracking().FirstOrDefault(x => x.Controller.Equals(controller) && x.ParentCompanyId == GlobalVariable.COMPANY_ID);
        }
    }
}
