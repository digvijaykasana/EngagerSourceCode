using EngagerMark4.ApplicationCore.Cris.Application;
using EngagerMark4.ApplicationCore.Entities.Application;
using EngagerMark4.ApplicationCore.IRepository.Application;
using EngagerMark4.Infrasturcture.EFContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using EngagerMark4.Common;

namespace EngagerMark4.Infrasturcture.Repository.Application
{
    public class ModulePermissionRepository : GenericRepository<ApplicationDbContext, ModulePermissionCri, ModulePermission>, IModulePermissionRepository
    {
        public ModulePermissionRepository(ApplicationDbContext aContext) : base(aContext)
        {
        }

        public List<Module> GetModules()
        {
            List<Module> modules = new List<Module>();
            List<ModulePermission> modulePermissions = context.ModulePermissions.Include(x => x.Module).Include(x => x.Permission).Where(x => x.ParentCompanyId == GlobalVariable.COMPANY_ID).ToList();
            foreach (var modulePermission in modulePermissions)
            {
                var module = modules.FirstOrDefault(x => x.Id == modulePermission.ModuleId);

                if (module == null)
                {
                    modules.Add(modulePermission.Module);
                    module = modulePermission.Module;
                }
                if(module.Id == modulePermission.ModuleId)
                {
                    module.FunctionList.Add(modulePermission);
                }
            }
            return modules;
        }
    }
}
