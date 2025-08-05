using EngagerMark4.ApplicationCore.Cris.Application;
using EngagerMark4.ApplicationCore.Entities.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.IRepository.Application
{
    public interface IModulePermissionRepository : IBaseRepository<ModulePermissionCri, ModulePermission>
    {
        List<Module> GetModules();
    }
}
