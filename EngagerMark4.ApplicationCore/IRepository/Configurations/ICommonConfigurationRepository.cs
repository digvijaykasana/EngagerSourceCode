using EngagerMark4.ApplicationCore.Cris.Configurations;
using EngagerMark4.ApplicationCore.Entities.Configurations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.IRepository.Configurations
{
    public interface ICommonConfigurationRepository : IBaseRepository<CommonConfigurationCri, CommonConfiguration>
    {
        Task Saves(List<CommonConfiguration> configurations);
        bool IsVesselUnderUse(Int64 VesselId);

        CommonConfiguration GetSimilarConfig(string Code, string Name, Int64 ConfigurationGroupId);

        IEnumerable<CommonConfiguration> GetVessels();
    }
}
