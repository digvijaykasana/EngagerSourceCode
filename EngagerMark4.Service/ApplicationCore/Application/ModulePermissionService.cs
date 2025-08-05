using EngagerMark4.ApplicationCore.Cris.Application;
using EngagerMark4.ApplicationCore.Entities.Application;
using EngagerMark4.ApplicationCore.IRepository.Application;
using EngagerMark4.ApplicationCore.IService.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.Service.ApplicationCore.Application
{
    public class ModulePermissionService : AbstractService<IModulePermissionRepository, ModulePermissionCri, ModulePermission>, IModulePermissionService
    {
        public ModulePermissionService(IModulePermissionRepository repository) : base(repository)
        {
        }

        public List<Module> GetModules()
        {
            return this.repository.GetModules();
        }
    }
}
