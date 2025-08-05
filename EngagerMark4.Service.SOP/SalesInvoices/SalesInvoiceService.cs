using EngagerMark4.ApplicationCore.Account.Cris;
using EngagerMark4.ApplicationCore.Account.Entities;
using EngagerMark4.ApplicationCore.Account.IService;
using EngagerMark4.ApplicationCore.Cris;
using EngagerMark4.ApplicationCore.Inventory.Entities;
using EngagerMark4.ApplicationCore.Inventory.IService.Price;
using EngagerMark4.ApplicationCore.IService.Configurations;
using EngagerMark4.ApplicationCore.Job.IService;
using EngagerMark4.ApplicationCore.SOP.Cris;
using EngagerMark4.ApplicationCore.SOP.Entities.CreditNotes;
using EngagerMark4.ApplicationCore.SOP.Entities.SalesInvoices;
using EngagerMark4.ApplicationCore.SOP.Entities.WorkOrders;
using EngagerMark4.ApplicationCore.SOP.IRepository.CreditNotes;
using EngagerMark4.ApplicationCore.SOP.IRepository.SalesInvoices;
using EngagerMark4.ApplicationCore.SOP.IService.SalesInvoices;
using EngagerMark4.Common.Configs;
using EngagerMark4.Infrasturcture.EFContext;
using EngagerMark4.Infrasturcture.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EngagerMark4.Service.SOP.SalesInvoices
{
    /// <summary>
    /// 
    /// </summary>
    public class SalesInvoiceService : AbstractService<ISalesInvoiceRepository, SalesInvoiceCri, SalesInvoice>, ISalesInvoiceService
    {
        ICreditNoteRepository _creditNoteRepository;
        ApplicationDbContext _context;
        IPriceService _priceService;
        IGeneralLedgerService _glService;
        ICheckListService _checkListService;
        IGSTService _gstService;
        ILocationService _locationService;
        IHotelService _hotelService;

        public SalesInvoiceService(ISalesInvoiceRepository repository,
            ICreditNoteRepository creditNoteRepository,
            IPriceService priceService,
            IGeneralLedgerService glService,
            ICheckListService checkListService,
            IGSTService gstService,
            ILocationService locationService,
            IHotelService hotelService,
            ApplicationDbContext context) : base(repository)
        {
            this._creditNoteRepository = creditNoteRepository;
            this._context = context;
            this._priceService = priceService;
            this._glService = glService;
            this._checkListService = checkListService;
            this._gstService = gstService;
            this._locationService = locationService;
            this._hotelService = hotelService;
        }

        //Save Sales Invoice  < Assigning GST, Save Sales Invoice, Generate Credit Note, Link Credit Note to Work Order >
        //Modified By: Kaung - 20180528
        public async override Task<long> Save(SalesInvoice entity)
        {
            try
            {
                long id = 0;

                //Query GST and Assign to Sales Invoice

                var gst = await this._context.GSTs.AsNoTracking().FirstOrDefaultAsync(x => x.Id == entity.GSTId);
                if (gst == null) gst = new EngagerMark4.ApplicationCore.Account.Entities.GST();
                entity.GSTPercent = gst.GSTPercent;


                //Query Work Order to check Versioning
                var workOrder = this._context.WorkOrders.FirstOrDefault(x => x.Id == entity.WorkOrderId);

                foreach (var detail in entity.Details)
                {
                    if (detail.PriceId != 0)
                    {
                        var price = await _priceService.GetById(detail.PriceId);

                        //if(price.Name == "Trip Charges")
                        //{
                        //    string locationStr = workOrder.GetLocationStr();

                        //    detail.Code = price.Code + " - " + locationStr;

                        //}
                    }
                }

                if (workOrder != null)
                {

                    if (workOrder.Status == WorkOrder.OrderStatus.Billed && workOrder.InvoiceId != null)
                    {
                        //Save Sales Invoice with Version
                        entity.requiresVersioning = true;

                        id = await SaveSIVersion(entity);
                    }
                    else
                    {
                        //Save Sales Invoice
                        id = await base.Save(entity);
                    }

                    //Generate Credit Note

                    CreditNote creditNote = await _creditNoteRepository.GetByWorkOrderId(entity.WorkOrderId == null ? 0 : entity.WorkOrderId.Value);
                    if (creditNote == null) creditNote = new CreditNote();
                    creditNote.GenerateCreditNote(entity, gst);
                    this._creditNoteRepository.Save(creditNote);

                    var needToGenerateCode = false;
                    if (creditNote.Id == 0)
                        needToGenerateCode = true;
                    await this._context.SaveChangesAsync();

                    if (needToGenerateCode)
                    {
                        SerialNoRepository<CreditNoteSerialNo> serialNoRepository = new SerialNoRepository<CreditNoteSerialNo>(_context);
                        var customer = await _context.Customers.AsNoTracking().FirstOrDefaultAsync(x => x.Id == creditNote.CustomerId);
                        if (customer == null) customer = new EngagerMark4.ApplicationCore.Customer.Entities.Customer();
                        creditNote.CreditNoteNo = creditNote.GetCreditNoteNo(serialNoRepository.GetSerialNoByMonth(creditNote.Id, creditNote.Created), customer);
                        await this._context.SaveChangesAsync();
                    }

                    //Link Credit Note to Work Order
                    workOrder.CreditNoteId = creditNote.Id;
                    workOrder.CreditNoteNo = creditNote.CreditNoteNo;
                    await this._context.SaveChangesAsync();

                }

                return id;


            }
            catch (Exception ex)
            {
                return 0;
            }
            #region Obsolete 04/04/2019
            //long id = 0;

            ////Query GST and Assign to Sales Invoice

            //var gst = await this._context.GSTs.AsNoTracking().FirstOrDefaultAsync(x => x.Id == entity.GSTId);
            //if (gst == null) gst = new EngagerMark4.ApplicationCore.Account.Entities.GST();
            //entity.GSTPercent = gst.GSTPercent;


            ////Query Work Order to check Versioning
            //var workOrder = this._context.WorkOrders.FirstOrDefault(x => x.Id == entity.WorkOrderId);

            //if (workOrder != null)
            //{

            //    if (workOrder.Status == WorkOrder.OrderStatus.Billed && workOrder.InvoiceId != null)
            //    {
            //        //Save Sales Invoice with Version
            //        entity.requiresVersioning = true;

            //        id = await SaveSIVersion(entity);
            //    }
            //    else
            //    {
            //        //Save Sales Invoice
            //        id = await base.Save(entity);
            //    }

            //    //Generate Credit Note

            //    CreditNote creditNote = await _creditNoteRepository.GetByWorkOrderId(entity.WorkOrderId == null ? 0 : entity.WorkOrderId.Value);
            //    if (creditNote == null) creditNote = new CreditNote();
            //    creditNote.GenerateCreditNote(entity, gst);
            //    this._creditNoteRepository.Save(creditNote);

            //    var needToGenerateCode = false;
            //    if (creditNote.Id == 0)
            //        needToGenerateCode = true;
            //    await this._context.SaveChangesAsync();

            //    if (needToGenerateCode)
            //    {
            //        SerialNoRepository<CreditNoteSerialNo> serialNoRepository = new SerialNoRepository<CreditNoteSerialNo>(_context);
            //        var customer = await _context.Customers.AsNoTracking().FirstOrDefaultAsync(x => x.Id == creditNote.CustomerId);
            //        if (customer == null) customer = new EngagerMark4.ApplicationCore.Customer.Entities.Customer();
            //        creditNote.CreditNoteNo = creditNote.GetCreditNoteNo(serialNoRepository.GetSerialNoByMonth(creditNote.Id, creditNote.Created), customer);
            //        await this._context.SaveChangesAsync();
            //    }

            //    //Link Credit Note to Work Order
            //    workOrder.CreditNoteId = creditNote.Id;
            //    workOrder.CreditNoteNo = creditNote.CreditNoteNo;
            //    await this._context.SaveChangesAsync();

            //}

            //return id;
            #endregion
        }

        public async Task<Int64> SaveSIVersion(SalesInvoice entity)
        {
            long? versionNumber = this.repository.SaveVersionedInvoice(entity);
            this.repository.UpdateVersions(entity.WorkOrderId, versionNumber);
            await this.repository.SaveChangesAsync();
            return entity.Id;
        }

        /* Original Save By Min
        public async override Task<long> Save(SalesInvoice entity)
        {
            //GST Value to Sales Invoice Entity
            var gst = await this._context.GSTs.AsNoTracking().FirstOrDefaultAsync(x => x.Id == entity.GSTId);
            if (gst == null) gst = new EngagerMark4.ApplicationCore.Account.Entities.GST();
            entity.GSTPercent = gst.GSTPercent;

            //Save Sales Invoice
            var id = await base.Save(entity);

            //Generate Credit Note
            CreditNote creditNote = await _creditNoteRepository.GetByWorkOrderId(entity.WorkOrderId == null ? 0 : entity.WorkOrderId.Value);
            if (creditNote == null) creditNote = new CreditNote();
            creditNote.GenerateCreditNote(entity, gst);
            this._creditNoteRepository.Save(creditNote);


            var needToGenerateCode = false;
            if (creditNote.Id == 0)
                needToGenerateCode = true;
            await this._context.SaveChangesAsync();

            if(needToGenerateCode)
            {
                SerialNoRepository<CreditNoteSerialNo> serialNoRepository = new SerialNoRepository<CreditNoteSerialNo>(_context);
                var customer = await _context.Customers.AsNoTracking().FirstOrDefaultAsync(x => x.Id == creditNote.CustomerId);
                if (customer == null) customer = new EngagerMark4.ApplicationCore.Customer.Entities.Customer();
                creditNote.CreditNoteNo = creditNote.GetCreditNoteNo(serialNoRepository.GetSerialNoByMonth(creditNote.Id, creditNote.Created), customer);
                await this._context.SaveChangesAsync();
            }
            var workOrder = this._context.WorkOrders.FirstOrDefault(x => x.Id == entity.WorkOrderId);

            if(workOrder!=null)
            {
                workOrder.CreditNoteId = creditNote.Id;
                workOrder.CreditNoteNo = workOrder.CreditNoteNo;
                await this._context.SaveChangesAsync();
            }

            return id;
        }*/

        public Task<IEnumerable<SalesInvoice>> GetByInvoiceNo(string invoiceNo)
        {
            return this.repository.GetByInvoiceNo(invoiceNo);
        }

        public async Task PreparePriceListFromTransferVoucher(WorkOrder workOrder, List<Price> prices)
        {
            try
            {
                var price = await this._priceService.FindByPickUpAndDropOff(workOrder.GetPickupPointId(), workOrder.GetDropOffPointId(), workOrder.CustomerId == null ? 0 : workOrder.CustomerId.Value);
                if (price != null)
                {
                    Regex regex = new Regex(@"(^[\w,\d,\s,A-Z,\@]*\s[0-9][0-9][0-9][0-9][0-9][0-9]$)+"); //Location in 'Airport 123456' format
                    Regex regexWithDash = new Regex(@"(^[\w,\d,\s,A-Z,\-,\@]*\s[0-9][0-9][0-9][0-9][0-9][0-9]$)+"); //Location in 'Airport - 123456' format
                    Regex regexWithoutPostalCode = new Regex(@"(^[\w,\d,\s,A-Z,\-,\@]*\s[\-]$)+"); //Location in 'Airport - ' format

                    foreach (var serviceJob in workOrder.GetServiceJobs())
                    {
                        string pickupPointString = workOrder.PickUpPoint;

                        if (regex.Match(pickupPointString).Success)
                        {
                            pickupPointString = workOrder.PickUpPoint.Substring(0, workOrder.PickUpPoint.Length - 6).Trim();
                        }
                        else if (regexWithDash.Match(pickupPointString).Success)
                        {
                            pickupPointString = workOrder.PickUpPoint.Substring(0, workOrder.PickUpPoint.Length - 8).Trim();
                        }
                        else if (regexWithoutPostalCode.Match(pickupPointString.Trim()).Success)
                        {
                            pickupPointString = workOrder.PickUpPoint.Trim().Substring(0, workOrder.PickUpPoint.Length - 2).Trim();
                        }

                        if (pickupPointString.Contains("Hotel - "))
                        {
                            pickupPointString = pickupPointString.Replace("Hotel - ", "");
                        }

                        string dropPointString = workOrder.DropPoint;

                        if (regex.Match(dropPointString).Success)
                        {
                            dropPointString = workOrder.DropPoint.Substring(0, workOrder.DropPoint.Length - 6).Trim();
                        }
                        else if (regexWithDash.Match(dropPointString).Success)
                        {
                            dropPointString = workOrder.DropPoint.Substring(0, workOrder.DropPoint.Length - 8).Trim();
                        }
                        else if (regexWithoutPostalCode.Match(dropPointString.Trim()).Success)
                        {
                            dropPointString = workOrder.DropPoint.Trim().Substring(0, workOrder.DropPoint.Length - 2).Trim();
                        }

                        if (dropPointString.Contains("Hotel - "))
                        {
                            dropPointString = dropPointString.Replace("Hotel - ", "");
                        }

                        price.Name = pickupPointString + " to " + dropPointString;

                        //If there is additional stop
                        var additionalStops = workOrder.WorkOrderLocationList.Where(x => x.Type == WorkOrderLocation.LocationType.AdditionalStop);

                        if (additionalStops != null && additionalStops.Count() > 0)
                        {
                            price.Name = pickupPointString + "/ ";

                            foreach (var stop in additionalStops)
                            {
                                if (stop.HotelId == null || stop.HotelId == 0)
                                {
                                    if (regex.Match(stop.Location.Display).Success)
                                    {
                                        price.Name += stop.Location.Display.Substring(0, stop.Location.Display.Length - 6).Trim() + "/ ";
                                    }
                                    else if (regexWithDash.Match(stop.Location.Display).Success)
                                    {
                                        price.Name += stop.Location.Display.Substring(0, stop.Location.Display.Length - 8).Trim() + "/ ";
                                    }
                                    else if (regexWithoutPostalCode.Match(stop.Location.Display.Trim()).Success)
                                    {
                                        price.Name += stop.Location.Display.Trim().Substring(0, stop.Location.Display.Length - 2).Trim() + "/ ";
                                    }
                                    else
                                    {
                                        price.Name += stop.Location.Display + "/ ";
                                    }
                                }
                                else
                                {
                                    var hotel = await _hotelService.GetById(stop.HotelId);

                                    if (hotel != null)
                                    {
                                        price.Name += hotel.Name + "/ ";
                                    }
                                }
                            }

                            price.Name += dropPointString;
                        }

                        price.IncludeCustomerDiscountAmount = true;
                        prices.Add(price);

                        if (additionalStops != null && additionalStops.Count() > 0)
                        {
                            var cri = new GeneralLedgerCri
                            {
                                StringCris = new Dictionary<string, StringValue>()
                            };

                            cri.StringCris.Add("Name", new StringValue { ComparisonOperator = BaseCri.StringComparisonOperator.Contains, Value = GLCodesConfig.additionalStop });
                            var glList = await _glService.GetByCri(cri);
                            var glAdditionalStop = glList.FirstOrDefault();
                            if (glAdditionalStop != null)
                            {
                                Price additionalStopPrice = await _priceService.GetByGLCodeId(glAdditionalStop.Id, workOrder.CustomerId.Value);

                                if (additionalStopPrice != null)
                                {
                                    additionalStopPrice.Name = additionalStops.Count().ToString();

                                    if (additionalStops.Count() == 1) additionalStopPrice.Name += " Stop";

                                    if (additionalStops.Count() > 1)
                                    {
                                        additionalStopPrice.Name += " Stops";
                                        additionalStopPrice.AssignedPrice = additionalStopPrice.AssignedPrice * additionalStops.Count();
                                    }

                                    prices.Add(additionalStopPrice);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task PreparePriceListForMeetingService(WorkOrder workOrder, List<Price> prices)
        {
            try
            {
                if (workOrder.MeetingServiceList.Count == 0)
                    return;

                var cri = new GeneralLedgerCri
                {
                    StringCris = new Dictionary<string, StringValue>()
                };

                cri.StringCris.Add("Name", new StringValue { ComparisonOperator = BaseCri.StringComparisonOperator.Contains, Value = "Meeting Services" });
                var glList = await _glService.GetByCri(cri);
                var glMeetingService = glList.FirstOrDefault();
                if (glMeetingService == null)
                    return;

                Price meetingServicePrice = await _priceService.GetByGLCodeId(glMeetingService.Id, workOrder.CustomerId.Value);

                if (meetingServicePrice == null)
                    return;

                meetingServicePrice.AssignedPrice = 0;

                foreach (var meetingService in workOrder.MeetingServiceList)
                {
                    meetingServicePrice.AssignedPrice += meetingService.Charges;
                    meetingServicePrice.Name = meetingServicePrice.Name + " " + meetingService.FlightNo + " ";
                }

                prices.Add(meetingServicePrice);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task PreparePriceList(WorkOrder workOrder, List<Price> prices)
        {
            try
            {
                if (prices == null) prices = new List<Price>();
                if (workOrder.ServiceJobList == null) workOrder.ServiceJobList = new List<EngagerMark4.ApplicationCore.SOP.Entities.Jobs.ServiceJob>();
                foreach (var serviceJob in workOrder.GetServiceJobs())
                {
                    if (!string.IsNullOrEmpty(serviceJob.CheckListIds))
                    {
                        var sjChecklistItems = serviceJob.GetChecklistItemList();

                        if (sjChecklistItems.Any() && sjChecklistItems.Count > 0)
                        {
                            foreach (var checklistItem in sjChecklistItems)
                            {
                                var chkId = checklistItem.ChecklistId;

                                var checklist = await _checkListService.GetById(chkId);
                                if (checklist != null)
                                {
                                    if (workOrder.CustomerId == null)
                                        continue;
                                    var price = await _priceService.GetByGLCodeId(checklist.GLCodeId, workOrder.CustomerId == null ? 0 : workOrder.CustomerId.Value);
                                    if (price == null) continue;

                                    if (checklist.Name.ToLower().Trim().Contains("waiting time"))
                                    {
                                        price.Name = price.Name + " " + serviceJob.WaitingTime;
                                        price.IncludeCustomerDiscountAmount = true;
                                    }

                                    if (checklist.Name.ToLower().Trim().Contains("additional stop"))
                                    {
                                        price.Name = price.Name + " " + serviceJob.AdditionalStops;
                                    }

                                    if (price != null)
                                    {
                                        prices.Add(price);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Int64> UpdatePriceList(SalesInvoice invoice, List<Price> prices)
        {
            try
            {

                if (prices == null) prices = new List<Price>();

                var gst = await _gstService.GetById(invoice.GSTId);

                if (gst == null) gst = new GST();

                foreach (var price in prices.OrderBy(x => x.OrderBy))
                {
                    var inPrice = invoice.Details.FirstOrDefault(x => x.PriceId == price.Id);


                    if (inPrice != null)
                    {
                        if (inPrice.SalesInvoiceId == invoice.Id)
                        {
                            inPrice.SalesInvoice = invoice;
                        }

                        //inPrice.Qty++;
                        inPrice.CalculateTotal();
                        continue;
                    }
                    SalesInvoiceDetails details = new SalesInvoiceDetails
                    {
                        PriceId = price.Id,
                        Type = SalesInvoiceDetails.DetailsType.NonInventory,
                        Code = price.Display,
                        Qty = 1,
                        Price = price.AssignedPrice,
                        DiscountPercent = price.DiscountPercent,
                        DiscountAmount = price.DiscountAmt
                    };
                    details.SalesInvoice = invoice;
                    //details.Taxable = price.GeneralLedger == null ? true : price.GeneralLedger.Taxable;
                    details.Taxable = price.IsTaxable;
                    details.CalculateTotal();
                    invoice.Details.Add(details);
                }
                invoice.CalculateTotal(gst);

                this.repository.Save(invoice);

                await this.repository.SaveChangesAsync();

                return invoice.Id;

            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }
}
