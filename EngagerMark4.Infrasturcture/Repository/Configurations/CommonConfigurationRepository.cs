using EngagerMark4.ApplicationCore.Cris.Configurations;
using EngagerMark4.ApplicationCore.Entities.Configurations;
using EngagerMark4.ApplicationCore.IRepository.Configurations;
using EngagerMark4.Common.Utilities;
using EngagerMark4.Infrasturcture.EFContext;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using static EngagerMark4.ApplicationCore.Common.GeneralConfig;

namespace EngagerMark4.Infrasturcture.Repository.Configurations
{
    public class CommonConfigurationRepository : GenericRepository<ApplicationDbContext, CommonConfigurationCri, CommonConfiguration>, ICommonConfigurationRepository
    {
        public CommonConfigurationRepository(ApplicationDbContext aContext) : base(aContext)
        {
        }

        public override void Save(CommonConfiguration model)
        {
            base.Save(model);

            string userName = HttpContext.Current.User.Identity.GetUserName();

            string domain = Util.GetDomain(userName);

            ////List<CommonConfiguration> configurations = Cache.CommonConfigurations[domain];

            //var configuration = configurations.FirstOrDefault(x => x.Code.Equals(model.Code));

            //if (configuration == null)
            //    configurations.Add(model);
        }

        public async Task Saves(List<CommonConfiguration> configurations)
        {
            foreach (var configuration in configurations)
            {
                var dbConfiguration = context.CommonConfigurations.FirstOrDefault(x => x.Code.Equals(configuration.Code));
                if (dbConfiguration == null)
                {
                    context.CommonConfigurations.Add(configuration);
                }
                else
                {
                    configuration.Id = dbConfiguration.Id;
                }
            }

            await this.SaveChangesAsync();
        }

        protected override void ApplyCri(CommonConfigurationCri cri)
        {
            base.ApplyCri(cri);

            if (cri != null && !string.IsNullOrEmpty(cri.SearchValue))
                base.queryableData = queryableData.Where(x => x.Code.ToLower().Contains(cri.SearchValue.ToLower()) || x.Name.ToLower().Contains(cri.SearchValue.ToLower()));
        }

        public bool IsVesselUnderUse(long VesselId)
        {
            try
            {
                if (context.WorkOrders.AsNoTracking().Where(x => x.VesselId.HasValue && x.VesselId.Value == VesselId).Any()) return true;

                if (context.SalesInvoiceSummaries.AsNoTracking().Where(x => x.VesselId == VesselId).Any()) return true;

                if (context.SalesInvoices.AsNoTracking().Where(x => x.VesselId.HasValue && x.VesselId.Value == VesselId).Any()) return true;

                if (context.CreditNotes.AsNoTracking().Where(x => x.VesselId.HasValue && x.VesselId.Value == VesselId).Any()) return true;

                if (context.Customers.AsNoTracking().Where(x => x.VesselId.HasValue && x.VesselId.Value == VesselId).Any()) return true;

                if (context.CustomerVessels.AsNoTracking().Where(x => x.VesselId == VesselId).Any()) return true;

                if (context.WorkOrderMeetingServices.AsNoTracking().Where(x => x.VesselId.HasValue && x.VesselId.Value == VesselId).Any()) return true;

                return false;

            }
            catch (Exception ex)
            {
                return true;
            }
        }

        public CommonConfiguration GetSimilarConfig(string Code, string Name, long ConfigurationGroupId)
        {
            try
            {
                return context.CommonConfigurations.AsNoTracking()
                    .Where(x => x.Code.Trim().ToLower().Equals(Code.Trim().ToLower())
                    && x.Name.Trim().ToLower().Equals(Name.Trim().ToLower())
                    && x.ConfigurationGroupId == ConfigurationGroupId).FirstOrDefault();

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public IEnumerable<CommonConfiguration> GetVessels()
        {
            try
            {
                var veselConfigurationGroup = context.ConfigurationGroups.AsNoTracking().Where(x => x.Code.Equals(ConfigurationGrpCodes.VesselId.ToString())).FirstOrDefault();

                if (veselConfigurationGroup == null) return new List<CommonConfiguration>();

                var vessels = context.CommonConfigurations.AsNoTracking().Where(x => x.ConfigurationGroupId == veselConfigurationGroup.Id).OrderBy(x => x.Name).AsEnumerable();

                return vessels;
            }
            catch (Exception ex)
            {
                return new List<CommonConfiguration>();
            }
        }
    }
}
