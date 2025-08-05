using EngagerMark4.ApplicationCore.Cris.Configurations;
using EngagerMark4.ApplicationCore.Customer.IService;
using EngagerMark4.ApplicationCore.Entities.Configurations;
using EngagerMark4.ApplicationCore.IRepository.Configurations;
using EngagerMark4.ApplicationCore.IService.Configurations;
using System;
using System.Collections.Generic;

namespace EngagerMark4.Service.ApplicationCore.Configurations
{
    public class CommonConfigurationService : AbstractService<ICommonConfigurationRepository, CommonConfigurationCri, CommonConfiguration>, ICommonConfigurationService
    {
        IConfigurationGroupService configGrpService;
        ICustomerService customerService;

        public CommonConfigurationService(ICommonConfigurationRepository repository, IConfigurationGroupService _configGrpService, ICustomerService _customerService) : base(repository)
        {
            this.configGrpService = _configGrpService;
            this.customerService = _customerService;
        }

        public bool IsVesselUnderUse(long VesselId)
        {
            return this.repository.IsVesselUnderUse(VesselId);
        }

        public CommonConfiguration GetSimilarConfig(string Code, string Name, Int64 ConfigurationGroupId)
        {
            return this.repository.GetSimilarConfig(Code, Name, ConfigurationGroupId);
        }

        public IEnumerable<CommonConfiguration> GetVessels()
        {
            return this.repository.GetVessels();
        }
    }
}
