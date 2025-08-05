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
    public class ModuleService : AbstractService<IModuleRepository, ModuleCri, Module>, IModuleService
    {
        IModulePermissionRepository modulePermissionRepository;

        public ModuleService(IModuleRepository repository,
            IModulePermissionRepository modulePermissionRepository) : base(repository)
        {
            this.modulePermissionRepository = modulePermissionRepository;
        }

        public async override Task<Module> GetById(object id)
        {
            var module = await base.GetById(id);
            var cri = new ModulePermissionCri();
            cri.Includes = new List<string>();
            cri.Includes.Add("Permission");
            cri.NumberCris = new Dictionary<string, EngagerMark4.ApplicationCore.Cris.IntValue>();
            cri.NumberCris["ModuleId"] = new EngagerMark4.ApplicationCore.Cris.IntValue { ComparisonOperator = EngagerMark4.ApplicationCore.Cris.BaseCri.NumberComparisonOperator.Equal, Value = module.Id };
            var functionList = await this.modulePermissionRepository.GetByCri(cri);
            module.FunctionList = functionList.ToList();
            return module;
        }

        public async override Task<long> Save(Module entity)
        {
            this.repository.Save(entity);

            if(entity.Id !=0)
            {
                // Delete the existing Module Permission
                var cri = new ModulePermissionCri();
                cri.NumberCris = new Dictionary<string, EngagerMark4.ApplicationCore.Cris.IntValue>();
                cri.NumberCris["ModuleId"] = new EngagerMark4.ApplicationCore.Cris.IntValue { ComparisonOperator = EngagerMark4.ApplicationCore.Cris.BaseCri.NumberComparisonOperator.Equal, Value = entity.Id };
                var functionList = await this.modulePermissionRepository.GetByCri(cri);

                if (functionList != null)
                {
                    foreach (var function in functionList)
                    {
                        this.modulePermissionRepository.Delete(function);
                    }
                }
            }

            foreach(var function in entity.GetFunctionList())
            {
                this.modulePermissionRepository.Save(function);
            }

            await this.repository.SaveChangesAsync();

            return entity.Id;
        }
    }
}
