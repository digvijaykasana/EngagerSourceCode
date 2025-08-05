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
    public class SystemSettingService : AbstractService<ISystemSettingRepository, SystemSettingCri, SystemSetting>, ISystemSettingService
    {
        public SystemSettingService(ISystemSettingRepository repository) : base(repository)
        {
        }
    }
}
