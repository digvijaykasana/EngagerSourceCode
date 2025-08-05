using EngagerMark4.ApplicationCore.Customer.DataImportViewModel;
using EngagerMark4.ApplicationCore.Customer.Entities;
using EngagerMark4.ApplicationCore.Customer.IRepository;
using EngagerMark4.ApplicationCore.Entities.Configurations;
using EngagerMark4.ApplicationCore.IRepository.Configurations;
using EngagerMark4.Common.Configs;
using EngagerMark4.DocumentProcessor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static EngagerMark4.ApplicationCore.Common.GeneralConfig;

namespace EngagerMark4.Controllers.Customers
{
    public class CustomerImportController : Controller
    {
        ICustomerRepository _customerRepsoitory;
        IConfigurationGroupRepository _configurationGroupRepository;
        ICommonConfigurationRepository _commonConfigurationRepository;

        public CustomerImportController(ICustomerRepository customerRepository,
            IConfigurationGroupRepository configurationGroupRepository,
            ICommonConfigurationRepository commonConfigurationRepository)
        {
            this._customerRepsoitory = customerRepository;
            this._configurationGroupRepository = configurationGroupRepository;
            this._commonConfigurationRepository = commonConfigurationRepository;
        }

        // GET: CustomerImport
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(string post="")
        {
            List<CustomerDataImportViewModel> customers = new List<CustomerDataImportViewModel>();
            foreach(string uploadFile in Request.Files)
            {
                HttpPostedFileBase file = Request.Files[uploadFile] as HttpPostedFileBase;
                if(file!=null && file.ContentLength > 0)
                {
                    ExcelProcessor<CustomerDataImportViewModel> excelProcessor = new ExcelProcessor<CustomerDataImportViewModel>();
                    customers = excelProcessor.ImportFromExcel(file.InputStream);
                }
            }
            List<CommonConfiguration> vessels = await GetVessels(customers);
            try
            {
                await this._commonConfigurationRepository.Saves(vessels);
            }
            catch(Exception ex)
            {
                string message = ex.Message;
            }
            List<Customer> customerEntities = GetCustomers(customers, vessels);
            await this._customerRepsoitory.Saves(customerEntities);
            TempData["color"] = ApplicationConfig.MESSAGE_SAVE_COLOR;
            TempData["message"] = ApplicationConfig.MESSAGE_IMPORT_MESSAGE;
            return RedirectToAction(nameof(Index));
        }

        private async Task<List<CommonConfiguration>> GetVessels(List<CustomerDataImportViewModel> customers)
        {
            List<ConfigurationGroup> configurationGroups = (await _configurationGroupRepository.GetByCri(null)).ToList();
            var vesselGroup = configurationGroups.FirstOrDefault(x => x.Code.Equals(ConfigurationGrpCodes.VesselId.ToString()));
            List<CommonConfiguration> vessels = new List<CommonConfiguration>();
            foreach(var customer in customers)
            {
                if (customer.Vessel.Contains(','))
                {
                    foreach(var vesselStr in customer.Vessel.Split(','))
                    {
                        if (!string.IsNullOrEmpty(vesselStr))
                        {
                            var inVessel = vessels.FirstOrDefault(x => x.Code.Equals(vesselStr.Trim()));
                            if (inVessel == null)
                            {
                                CommonConfiguration vessel = new CommonConfiguration
                                {
                                    Code = vesselStr.Trim(),
                                    Name = vesselStr.Trim(),
                                    ConfigurationGroupId = vesselGroup.Id
                                };
                                vessels.Add(vessel);
                            }
                        }
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(customer.Vessel))
                    {
                        var inVessel = vessels.FirstOrDefault(x => x.Code.Equals(customer.Vessel.Trim()));
                        if (inVessel == null)
                        {
                            CommonConfiguration vessel = new CommonConfiguration
                            {
                                Code = customer.Vessel.Trim(),
                                Name = customer.Vessel.Trim(),
                                ConfigurationGroupId = vesselGroup.Id
                            };
                            vessels.Add(vessel);
                        }
                    }
                }
            }

            return vessels.Distinct().ToList();
        }

        private List<Customer> GetCustomers(List<CustomerDataImportViewModel> viewModels, List<CommonConfiguration> vessels)
        {
            List<Customer> customers = new List<Customer>();
            foreach (var viewModel in viewModels)
            {
                Customer customer = new Customer
                {
                    Name = viewModel.Name,
                    Email = viewModel.Email,
                    OfficeNo = viewModel.OfficeNo,
                    AccNo = viewModel.AccNo,
                    Fax = viewModel.Fax,
                    Address = viewModel.Address,
                    Acronym = viewModel.Acronym,
                    DiscountPercent = viewModel.DiscountPercentage,
                    DiscountAmt = viewModel.DiscountAmt,
                };

                if (!string.IsNullOrEmpty(viewModel.Vessel))
                {
                    if (viewModel.Vessel.Contains(','))
                    {
                        foreach (var vesselStr in viewModel.Vessel.Split(','))
                        {

                            var vessel = vessels.FirstOrDefault(x => x.Code.Equals(vesselStr.Trim()));
                            if (vessel == null) continue;
                            var inVessel = customer.VesselList.FirstOrDefault(x => x.Vessel.Equals(vesselStr.Trim()));
                            if (inVessel == null)
                            {
                                CustomerVessel customerVessel = new CustomerVessel
                                {
                                    VesselId = vessel.Id,
                                    Vessel = vessel.Name
                                };
                                customer.VesselList.Add(customerVessel);
                            }

                        }

                    }
                    else
                    {
                        var vessel = vessels.FirstOrDefault(x => x.Code.Equals(viewModel.Vessel.Trim()));
                        var inVessel = customer.VesselList.FirstOrDefault(x => x.Vessel.Equals(vessel.Name));
                        if (inVessel == null)
                        {
                            CustomerVessel customerVessel = new CustomerVessel
                            {
                                VesselId = vessel.Id,
                                Vessel = vessel.Name
                            };
                            customer.VesselList.Add(customerVessel);
                        }
                    }
                }
                customers.Add(customer);
            }
            return customers;

        }
    }
}