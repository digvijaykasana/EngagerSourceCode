using EngagerMark4.ApplicationCore.Cris.Configurations;
using EngagerMark4.ApplicationCore.Customer.IService;
using EngagerMark4.ApplicationCore.Entities.Configurations;
using EngagerMark4.ApplicationCore.IRepository.Configurations;
using EngagerMark4.ApplicationCore.IService.Configurations;
using EngagerMark4.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EngagerMark4.ApplicationCore.Common.GeneralConfig;

namespace EngagerMark4.Service.ApplicationCore.Configurations
{
    public class VesselService : AbstractService<IVesselRepository, CommonConfigurationCri, CommonConfiguration>, IVesselService
    {
        IConfigurationGroupService _configGrpService;
        ICustomerService _customerService;
        ICommonConfigurationService _commonConfigService;

        public VesselService(IVesselRepository repository, IConfigurationGroupService configGrpService, ICustomerService customerService, ICommonConfigurationService commonConfigService) : base(repository)
        {
            this._configGrpService = configGrpService;
            this._customerService = customerService;
            this._commonConfigService = commonConfigService;
        }


        public async override Task<bool> Delete(CommonConfiguration entity)
        {
            var configGrps = await _configGrpService.GetByCri(null);

            Int64 vesselGrpId = configGrps.Where(x => x.Code.Equals(ConfigurationGrpCodes.VesselId.ToString())).FirstOrDefault().Id;

            if (entity.ConfigurationGroupId == vesselGrpId)
            {
                var vessels = _customerService.GetCustomerVesselList();

                var existingVessels = vessels.Where(x => x.VesselId == entity.Id).ToList();

                if (existingVessels.Count > 0)
                {
                    throw new CannotDeleteException("Vessel is in use!");
                }
            }

            this.repository.Delete(entity);
            await this.repository.SaveChangesAsync();
            return true;
        }

        public async override Task<Int64> Save(CommonConfiguration entity)
        {
                if (entity.Id == 0)
                {
                    var vessels = await this._commonConfigService.GetByCri(null);

                    var vessel = vessels.Where(x => x.Code.ToLower().Equals(entity.Code.ToLower()) || x.Name.ToLower().Equals(entity.Name.ToLower())).ToList();

                    if (vessel != null && vessel.Count > 0)
                    {
                        throw new DbUpdateException("Record Already Exists");
                    }
                }


                this.repository.Save(entity);
                await this.repository.SaveChangesAsync();
                return entity.Id;
        }
    }
}
