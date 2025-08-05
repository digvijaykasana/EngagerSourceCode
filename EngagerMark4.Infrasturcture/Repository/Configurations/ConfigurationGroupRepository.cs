using EngagerMark4.ApplicationCore.Cris.Configurations;
using EngagerMark4.ApplicationCore.Entities.Configurations;
using EngagerMark4.ApplicationCore.IRepository.Configurations;
using EngagerMark4.Infrasturcture.EFContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace EngagerMark4.Infrasturcture.Repository.Configurations
{
    public class ConfigurationGroupRepository : GenericRepository<ApplicationDbContext, ConfigurationGroupCri, ConfigurationGroup>, IConfigurationGroupRepository
    {
        public ConfigurationGroupRepository(ApplicationDbContext aContext) : base(aContext)
        {
        }
    }
}
