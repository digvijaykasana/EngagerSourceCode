using EngagerMark4.ApplicationCore.Cris.Configurations;
using EngagerMark4.ApplicationCore.Entities.Configurations;
using System;
using System.Collections.Generic;

namespace EngagerMark4.ApplicationCore.IService.Configurations
{
    public interface ICommonConfigurationService : IBaseService<CommonConfigurationCri, CommonConfiguration>
    {
        bool IsVesselUnderUse(Int64 VesselId);

        CommonConfiguration GetSimilarConfig(string Code, string Name, Int64 ConfigurationGroupId);

        IEnumerable<CommonConfiguration> GetVessels();
    }
}
