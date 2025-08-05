using EngagerMark4.ApplicationCore.Account.Entities;
using EngagerMark4.ApplicationCore.Account.IService;
using EngagerMark4.ApplicationCore.Customer.Entities;
using EngagerMark4.ApplicationCore.Customer.IService;
using EngagerMark4.ApplicationCore.Entities.Configurations;
using EngagerMark4.ApplicationCore.Inventory.DataImportViewModel;
using EngagerMark4.ApplicationCore.Inventory.Entities;
using EngagerMark4.ApplicationCore.Inventory.IRepository.Price;
using EngagerMark4.ApplicationCore.IService.Configurations;
using EngagerMark4.Common.Configs;
using EngagerMark4.DocumentProcessor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace EngagerMark4.Controllers.Inventory.Prices
{
    public class PriceImportController : Controller
    {
        IPriceRepository _priceRepository;
        ICustomerService _customerService;
        ILocationService _locationService;
        IGeneralLedgerService _generalLedgerService;

        public PriceImportController(IPriceRepository priceRepository,
            ICustomerService customerService,
            ILocationService locationService,
            IGeneralLedgerService generalLedgerService)
        {
            this._priceRepository = priceRepository;
            this._customerService = customerService;
            this._locationService = locationService;
            this._generalLedgerService = generalLedgerService;
        }

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Aung Ye Kaung - 20190423
        /// </summary>
        /// <param name="post"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(string post = "")
        {
            int originalCount = 0;
            int addedCount = 0;
            int updatedCount = 0;
            int viceVersaCount = 0;
            int failedCount = 0;
            string failedRows = "";
            int processedCount = 0;
            StringBuilder rowsSb = new StringBuilder();

            //Get All Customers From Db
            List<Customer> customers = (await _customerService.GetByCri(null)).ToList();

            //Get All Locations From Db
            List<Location> locations = (await _locationService.GetByCri(null)).ToList();

            //Get All GL Code Items from Db
            List<GeneralLedger> generalLedgers = (await _generalLedgerService.GetByCri(null)).ToList();

            List<PriceDataImportViewModel> prices = new List<PriceDataImportViewModel>();

            //Transform Excel Data into List
            foreach (string uploadFile in Request.Files)
            {
                HttpPostedFileBase file = Request.Files[uploadFile] as HttpPostedFileBase;

                if (file != null && file.ContentLength > 0)
                {
                    ExcelProcessor<PriceDataImportViewModel> excelProcessor = new ExcelProcessor<PriceDataImportViewModel>();
                    prices = excelProcessor.ImportFromExcel(file.InputStream);
                }
            }

            List<Price> priceList = new List<Price>();

            //Find Customer for Current Import List
            var customer = customers.FirstOrDefault(x => x.Name.ToLower().Trim().Equals(prices.FirstOrDefault().Customer.ToLower().Trim()));

            if (customer == null)
            {
                TempData["color"] = ApplicationConfig.MESSAGE_DELETE_COLOR;
                TempData["message"] = "Customer cannot be found.";
                return RedirectToAction(nameof(Index));
            }

            //Get all original price items under the customer
            var originalPriceList = _priceRepository.GetByCustomerId(customer.Id).ToList();

            if(originalPriceList != null)
            {
                originalCount = originalPriceList.Count();
            }

            //Loop through each row from excel
            foreach (var priceViewModel in prices)
            {
                processedCount++;

                //Get corresponding GL Code
                var gL = generalLedgers.FirstOrDefault(x => x.Name.ToLower().Trim().Equals(priceViewModel.GLCode.ToLower().Trim()));

                //Get corresponding Pickup Point Location
                var pickUpPoint = locations.FirstOrDefault(x => x.Name.ToLower().Trim().Equals(priceViewModel.PickupPoint.ToLower().Trim()));

                //Get corresponding Drop Off Location
                var dropOffPoint = locations.FirstOrDefault(x => x.Name.ToLower().Trim().Equals(priceViewModel.DropOffPoint.ToLower().Trim()));
                
                if ( gL == null )
                {
                    rowsSb.Append(processedCount.ToString() + " - Corresponding GL Code is empty. ;");
                    failedCount++;
                    continue;
                }

                if ((!String.IsNullOrEmpty(priceViewModel.PickupPoint)) && pickUpPoint == null)
                {
                    rowsSb.Append(processedCount.ToString() + " - Pickup Point '" + priceViewModel.PickupPoint  + "' cannot be found. ;");
                    failedCount++;
                    continue;
                }

                if ((!String.IsNullOrEmpty(priceViewModel.DropOffPoint)) && dropOffPoint == null)
                {
                    rowsSb.Append(processedCount.ToString() + " - Drop Point '" + priceViewModel.DropOffPoint + "' cannot be found. ;");
                    failedCount++;
                    continue;
                }

                Price price = new Price();

                try
                {
                    //Check whether the item exists originally
                    var existing = originalPriceList.Where(x => x.PickUpPointId == pickUpPoint.Id &&
                                                                x.DropPointId == dropOffPoint.Id &&
                                                                x.GLCodeId == gL.Id);

                    if (existing != null && existing.Count() > 0)
                    {
                        //If the item exists, update all properties except for Customer, Locations and GL Code Type
                        price = existing.FirstOrDefault();

                        price.Name = priceViewModel.ProductName;
                        price.AssignedPrice = priceViewModel.AssignedPrice;
                        price.MaxPax = (Int64)priceViewModel.MaxPax;
                        price.ExceedAmt = priceViewModel.ExceededPrice;
                        price.DiscountPercent = priceViewModel.DiscountPercent;
                        price.DiscountAmt = priceViewModel.DiscountAmount;
                        price.OrderBy = priceViewModel.Order;
                        price.ViceVersa = priceViewModel.ViceVersa == "y" ? true : false;
                        price.IsTaxable = priceViewModel.Gst == "y" ? true : false;

                        updatedCount++;

                    }
                    else
                    {
                        //If the item doesn't exist, create a new price item
                        price = new Price
                        {
                            Name = priceViewModel.ProductName,
                            AssignedPrice = priceViewModel.AssignedPrice,
                            MaxPax = (Int64)priceViewModel.MaxPax,
                            ExceedAmt = priceViewModel.ExceededPrice,
                            DiscountPercent = priceViewModel.DiscountPercent,
                            DiscountAmt = priceViewModel.DiscountAmount,
                            OrderBy = priceViewModel.Order,
                            ViceVersa = priceViewModel.ViceVersa == "y" ? true : false,
                            IsTaxable = priceViewModel.Gst == "y" ? true : false
                        };

                        price.CustomerId = customer.Id;
                        price.GLCodeId = gL.Id;

                        if(pickUpPoint != null)
                        {
                            price.PickUpPointId = pickUpPoint.Id;
                            price.PickUpPoint = pickUpPoint.Display;

                            PriceLocation pickupLocation = new PriceLocation
                            {
                                LocationId = pickUpPoint.Id,
                                Type = PriceLocation.PriceLocationType.PickUp
                            };
                            price.PriceLocationList.Add(pickupLocation);
                        }


                        if(dropOffPoint != null)
                        {
                            price.DropPointId = dropOffPoint.Id;
                            price.DropPoint = dropOffPoint.Display;

                            PriceLocation dropOffLocation = new PriceLocation
                            {
                                LocationId = dropOffPoint.Id,
                                Type = PriceLocation.PriceLocationType.DropOff
                            };

                            price.PriceLocationList.Add(dropOffLocation);
                        }

                        addedCount++;
                    }
                }
                catch(Exception ex)
                {
                    //If there is an exception, redirect back to the page. 

                    TempData["color"] = ApplicationConfig.MESSAGE_DELETE_COLOR;
                    TempData["message"] = "Something went wrong. Message : " + ex.Message;
                    return RedirectToAction(nameof(Index));
                }

                
                if (!string.IsNullOrEmpty(priceViewModel.Gst))
                {
                    price.IsTaxable = priceViewModel.Gst.ToLower().Equals("y");
                }

                priceList.Add(price);                

                if(price.ViceVersa && price.PickUpPointId != price.DropPointId)
                {
                    //If the price needs a Vice Versa counterpart
                    Price viceVersaPrice = new Price();


                    //Check if the vice versa item exists
                    var viceVersaExisting = originalPriceList.Where(x => x.PickUpPointId == dropOffPoint.Id && x.DropPointId == pickUpPoint.Id &&
                                            x.GLCodeId == gL.Id);

                    if (viceVersaExisting != null && viceVersaExisting.Count() > 0)
                    {
                        //if exists, update relevant properties
                        viceVersaPrice = viceVersaExisting.FirstOrDefault();
                        viceVersaPrice.AssignedPrice = price.AssignedPrice;
                        viceVersaPrice.MaxPax = (Int64)price.MaxPax;
                        viceVersaPrice.ExceedAmt = price.ExceedAmt;
                        viceVersaPrice.DiscountPercent = price.DiscountPercent;
                        viceVersaPrice.DiscountAmt = price.DiscountAmt;
                        viceVersaPrice.OrderBy = price.OrderBy;
                        viceVersaPrice.ViceVersa = price.ViceVersa;
                        viceVersaPrice.IsTaxable = price.IsTaxable;

                        updatedCount++;
                    }
                    else
                    {
                        //if it doesn't exist, create a new price item
                        viceVersaPrice = new Price()
                        {
                            Name = price.Name,
                            AssignedPrice = price.AssignedPrice,
                            CustomerId = price.CustomerId,
                            GLCodeId = price.GLCodeId,
                            PickUpPointId = dropOffPoint.Id,
                            PickUpPoint = dropOffPoint.Display,
                            DropPointId = pickUpPoint.Id,
                            DropPoint = pickUpPoint.Display,
                            MaxPax = (Int64)price.MaxPax,
                            ExceedAmt = price.ExceedAmt,
                            DiscountPercent = price.DiscountPercent,
                            DiscountAmt = price.DiscountAmt,
                            OrderBy = price.OrderBy,
                            ViceVersa = price.ViceVersa,
                            IsTaxable = price.IsTaxable
                        };

                        viceVersaPrice.PriceLocationList.Clear();

                        if(dropOffPoint != null)
                        {
                            PriceLocation viceVersaPickupLocation = new PriceLocation
                            {
                                LocationId = dropOffPoint.Id,
                                Type = PriceLocation.PriceLocationType.PickUp
                            };

                            viceVersaPrice.PriceLocationList.Add(viceVersaPickupLocation);
                        }

                        if(pickUpPoint != null)
                        {
                            PriceLocation viceVersaDropOffLocation = new PriceLocation
                            {
                                LocationId = pickUpPoint.Id,
                                Type = PriceLocation.PriceLocationType.DropOff
                            };

                            viceVersaPrice.PriceLocationList.Add(viceVersaDropOffLocation);
                        }

                        addedCount++;
                    }

                    priceList.Add(viceVersaPrice);

                    viceVersaCount++;
                }
            }

            await _priceRepository.Saves(priceList);

            if (rowsSb.ToString().Length > 0)
            {
                failedRows = rowsSb.ToString();
                //failedRows = failedRows.Substring(0, failedRows.Length - 1);
            }

            TempData["color"] = ApplicationConfig.MESSAGE_SAVE_COLOR;
            TempData["message"] = ApplicationConfig.MESSAGE_IMPORT_MESSAGE + " Original Count: " + originalCount.ToString() +
                                                                             "; Processed Count: " + processedCount.ToString() +
                                                                             "; Added Count: " + addedCount.ToString() +
                                                                             "; Updated Count: " + updatedCount.ToString() +
                                                                             "; Vice Versa Count: " + viceVersaCount.ToString() +
                                                                             "; Failed Count: " + failedCount.ToString() + 
                                                                             "; Failed Rows: '" + failedRows + "'";
            return RedirectToAction(nameof(Index));
            
        }


        /// <summary>
        /// Author - Kyaw Min Htut - Obsolete
        /// </summary>
        /// <param name="post"></param>
        /// <returns></returns>
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Index(string post="")
        //{
        //    List<Customer> customers = (await _customerService.GetByCri(null)).ToList();
        //    List<Location> locations = (await _locationService.GetByCri(null)).ToList();
        //    List<GeneralLedger> generalLedgers = (await _generalLedgerService.GetByCri(null)).ToList();

        //    List<PriceDataImportViewModel> prices = new List<PriceDataImportViewModel>();

        //    foreach(string uploadFile in Request.Files)
        //    {
        //        HttpPostedFileBase file = Request.Files[uploadFile] as HttpPostedFileBase;

        //        if(file!=null && file.ContentLength > 0)
        //        {
        //            ExcelProcessor<PriceDataImportViewModel> excelProcessor = new ExcelProcessor<PriceDataImportViewModel>();
        //            prices = excelProcessor.ImportFromExcel(file.InputStream);
        //        }
        //    }

        //    List<Price> priceList = new List<Price>();

        //    foreach(var priceViewModel in prices)
        //    {
        //        Price price = new Price
        //        {
        //            Name = priceViewModel.ProductName,
        //            AssignedPrice = priceViewModel.AssignedPrice,
        //            MaxPax = (Int64)priceViewModel.MaxPax,
        //            ExceedAmt = priceViewModel.ExceededPrice,
        //            DiscountPercent =priceViewModel.DiscountPercent,
        //            DiscountAmt = priceViewModel.DiscountAmount,
        //            OrderBy = priceViewModel.Order,
        //        };
        //        if(!string.IsNullOrEmpty(priceViewModel.Gst))
        //        {
        //            price.IsTaxable = priceViewModel.Gst.ToLower().Equals("y");
        //        }
        //        priceList.Add(price);
        //        var customer = customers.FirstOrDefault(x => x.Name.ToLower().Trim().Equals(priceViewModel.Customer.ToLower().Trim()));
        //        if (customer != null) price.CustomerId = customer.Id;
        //        var gL = generalLedgers.FirstOrDefault(x => x.Name.ToLower().Trim().Equals(priceViewModel.GLCode.ToLower().Trim()));
        //        if (gL != null) price.GLCodeId = gL.Id;
        //        var pickUpPoint = locations.FirstOrDefault(x => x.Name.ToLower().Trim().Equals(priceViewModel.PickupPoint.ToLower().Trim()));
        //        if(pickUpPoint !=null)
        //        {
        //            price.PickUpPointId = pickUpPoint.Id;
        //            price.PickUpPoint = pickUpPoint.Display;
        //            PriceLocation pickupLocation = new PriceLocation
        //            {
        //                LocationId = pickUpPoint.Id,
        //                Type = PriceLocation.PriceLocationType.PickUp
        //            };
        //            price.PriceLocationList.Add(pickupLocation);
        //        }
        //        var dropOffPoint = locations.FirstOrDefault(x => x.Name.ToLower().Trim().Equals(priceViewModel.DropOffPoint.ToLower().Trim()));
        //        if(dropOffPoint!=null)
        //        {
        //            price.DropPointId = dropOffPoint.Id;
        //            price.DropPoint = dropOffPoint.Display;
        //            PriceLocation dropOffLocation = new PriceLocation
        //            {
        //                LocationId = dropOffPoint.Id,
        //                Type = PriceLocation.PriceLocationType.DropOff
        //            };
        //            price.PriceLocationList.Add(dropOffLocation);
        //        }
        //    }

        //    await _priceRepository.Saves(priceList);

        //    TempData["color"] = ApplicationConfig.MESSAGE_SAVE_COLOR;
        //    TempData["message"] = ApplicationConfig.MESSAGE_IMPORT_MESSAGE;
        //    return RedirectToAction(nameof(Index));
        //}
    }
}