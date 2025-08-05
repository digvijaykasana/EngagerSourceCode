using EngagerMark4.ApplicationCore.Entities;
using EngagerMark4.ApplicationCore.Inventory.Entities;
using EngagerMark4.ApplicationCore.SOP.Entities.Jobs;
using EngagerMark4.ApplicationCore.SOP.Entities.SalesInvoices;
using EngagerMark4.ApplicationCore.SOP.Entities.CreditNotes;
using EngagerMark4.ApplicationCore.SOP.Entities.WorkOrders;
using EngagerMark4.Common;
using EngagerMark4.Common.Utilities;
using EngagerMark4.Infrasturcture.EFContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.Infrasturcture.Repository
{
    public class SerialNoRepository<T>
        where T : SerialNo
    {
        ApplicationDbContext _context;

        public SerialNoRepository(ApplicationDbContext db)
        {
            this._context = db;
            type = (T)Activator.CreateInstance<T>();
        }

        T type;

        public string Table
        {
            get
            {
                if (type is WorkOrderSerialNo)
                {
                    return "SOP.Tb_WorkOrderSerialNo";
                }
                else if (type is PriceSerialNo)
                {
                    return "Inventory.Tb_PriceSerialNo";
                }
                else if (type is ServiceJobSerialNo)
                {
                    return "SOP.Tb_ServiceJobSerialNo";
                }
                else if(type is SalesInvoiceSerialNo)
                {
                    return "SOP.Tb_SalesInvoiceSerialNo";
                }
                else if (type is CreditNoteSerialNo)
                {
                    return "SOP.Tb_CreditNoteSerialNo";
                }
                else if(type is SalesInvoiceReportSerialNo)
                {
                    return "SOP.Tb_SalesInvoiceReportSerialNo";
                }
                else if(type is  CreditNoteReportSerialNo)
                {
                    return "SOP.Tb_CreditNoteReportSerialNo";
                }
                else
                {
                    return "";
                }
            }
        }

        public void DeleteByRefId(Int64 refId)
        {
            var serialNo = _context.Set<T>().FirstOrDefault(x => x.ReferenceId == refId);
            if (serialNo != null)
                _context.Set<T>().Remove(serialNo);
            _context.SaveChanges();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="refId"></param>
        /// <returns></returns>
        /// 
        //        INSERT INTO QuotationNoSerialNoes(CompanyId, Year, Month, Day, RunningNo, Created, Modified, ParentCompanyId, State, CreatedBy, ModifiedBy, ReferenceId)
        //VALUES(1,2017,04,07,(SELECT MAX(RunningNo) FROM QuotationNoSerialNoes WHERE CompanyId = 1 AND Year = 2017)+1,'2017-04-04','2017-04-04',1,3,0,0,0)
        //INSERT INTO QuotationNoSerialNoes(CompanyId, Year, Month, Day, RunningNo, Created, Modified, ParentCompanyId, State, CreatedBy, ModifiedBy, ReferenceId)
        //VALUES(1,2017,04,07,1,'2017-04-04','2017-04-04',1,3,0,0,0)
        //SELECT* FROM QuotationNoSerialNoes;
        public Int64 GetSerialNoWithNoTimeConstraint(Int64 refId)
        {
            DateTime now = TimeUtil.GetLocalTime();
            string createDate = Util.ConvertDateToString(now);
            string modifiedDate = Util.ConvertDateToString(now);
            try
            {
                string query = "INSERT INTO " + Table + " (CompanyId,Year,Month,Day,RunningNo,Created,Modified,ParentCompanyId,State,CreatedBy,ModifiedBy,ReferenceId) " +
                                "VALUES(" + GlobalVariable.COMPANY_ID + ", " + now.Year + ", " + now.Month + ", " + now.Day + ", (SELECT MAX(RunningNo) FROM " + Table + " WHERE CompanyId = " + GlobalVariable.COMPANY_ID + ") + 1,'" + createDate + "','" + modifiedDate + "'," + GlobalVariable.COMPANY_ID + ",3,0,0," + refId + ");";
                _context.Database.ExecuteSqlCommand(query);
            }
            catch (Exception ex)
            {
                string query = "INSERT INTO " + Table + " (CompanyId, Year, Month, Day, RunningNo, Created, Modified, ParentCompanyId, State, CreatedBy, ModifiedBy, ReferenceId)" +
                "VALUES(" + GlobalVariable.COMPANY_ID + ", " + now.Year + ", " + now.Month + ", " + now.Day + ", 1, '" + createDate + "', '" + modifiedDate + "', " + GlobalVariable.COMPANY_ID + ", 3, 0, 0, " + refId + ")";
                _context.Database.ExecuteSqlCommand(query);
            }

            foreach (var serialNo in _context.Set<T>().Where(x => x.ReferenceId == refId))
            {
                return serialNo.RunningNo;
            }
            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="refId"></param>
        /// <returns></returns>
        /// 
        //        INSERT INTO QuotationNoSerialNoes(CompanyId, Year, Month, Day, RunningNo, Created, Modified, ParentCompanyId, State, CreatedBy, ModifiedBy, ReferenceId)
        //VALUES(1,2017,04,07,(SELECT MAX(RunningNo) FROM QuotationNoSerialNoes WHERE CompanyId = 1 AND Year = 2017)+1,'2017-04-04','2017-04-04',1,3,0,0,0)
        //INSERT INTO QuotationNoSerialNoes(CompanyId, Year, Month, Day, RunningNo, Created, Modified, ParentCompanyId, State, CreatedBy, ModifiedBy, ReferenceId)
        //VALUES(1,2017,04,07,1,'2017-04-04','2017-04-04',1,3,0,0,0)
        //SELECT* FROM QuotationNoSerialNoes;
        public Int64 GetSerialNo(Int64 refId)
        {
            DateTime now = TimeUtil.GetLocalTime();
            string createDate = Util.ConvertDateToString(now);
            string modifiedDate = Util.ConvertDateToString(now);
            try
            {
                string query = "INSERT INTO " + Table + " (CompanyId,Year,Month,Day,RunningNo,Created,Modified,ParentCompanyId,State,CreatedBy,ModifiedBy,ReferenceId) " +
                                "VALUES(" + GlobalVariable.COMPANY_ID + ", " + now.Year + ", " + now.Month + ", " + now.Day + ", (SELECT MAX(RunningNo) FROM " + Table + " WHERE CompanyId = " + GlobalVariable.COMPANY_ID + " AND Year = " + now.Year + ") + 1,'" + createDate + "','" + modifiedDate + "'," + GlobalVariable.COMPANY_ID + ",3,0,0," + refId + ");";
                _context.Database.ExecuteSqlCommand(query);
            }
            catch (Exception ex)
            {
                string query = "INSERT INTO " + Table + " (CompanyId, Year, Month, Day, RunningNo, Created, Modified, ParentCompanyId, State, CreatedBy, ModifiedBy, ReferenceId)" +
                "VALUES(" + GlobalVariable.COMPANY_ID + ", " + now.Year + ", " + now.Month + ", " + now.Day + ", 1, '" + createDate + "', '" + modifiedDate + "', " + GlobalVariable.COMPANY_ID + ", 3, 0, 0, " + refId + ")";
                _context.Database.ExecuteSqlCommand(query);
            }

            foreach (var serialNo in _context.Set<T>().Where(x => x.ReferenceId == refId))
            {
                return serialNo.RunningNo;
            }
            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="refId"></param>
        /// <returns></returns>
        /// 
        //        INSERT INTO QuotationNoSerialNoes(CompanyId, Year, Month, Day, RunningNo, Created, Modified, ParentCompanyId, State, CreatedBy, ModifiedBy, ReferenceId)
        //VALUES(1,2017,04,07,(SELECT MAX(RunningNo) FROM QuotationNoSerialNoes WHERE CompanyId = 1 AND Year = 2017)+1,'2017-04-04','2017-04-04',1,3,0,0,0)
        //INSERT INTO QuotationNoSerialNoes(CompanyId, Year, Month, Day, RunningNo, Created, Modified, ParentCompanyId, State, CreatedBy, ModifiedBy, ReferenceId)
        //VALUES(1,2017,04,07,1,'2017-04-04','2017-04-04',1,3,0,0,0)
        //SELECT* FROM QuotationNoSerialNoes;
        public Int64 GetSerialNoByMonth(Int64 refId, DateTime transDate)
        {
            DateTime now = TimeUtil.GetLocalTime();
            string createDate = Util.ConvertDateToString(now);
            string modifiedDate = Util.ConvertDateToString(now);
            try
            {
                string query = "INSERT INTO " + Table + " (CompanyId,Year,Month,Day,RunningNo,Created,Modified,ParentCompanyId,State,CreatedBy,ModifiedBy,ReferenceId) " +
                                "VALUES(" + GlobalVariable.COMPANY_ID + ", " + transDate.Year + ", " + transDate.Month + ", " + transDate.Day + ", (SELECT MAX(RunningNo) FROM " + Table + " WHERE CompanyId = " + GlobalVariable.COMPANY_ID + " AND Year = " + transDate.Year + " AND Month = " + transDate.Month + ") + 1,'" + createDate + "','" + modifiedDate + "'," + GlobalVariable.COMPANY_ID + ",3,0,0," + refId + ");";
                _context.Database.ExecuteSqlCommand(query);
            }
            catch (Exception ex)
            {
                string query = "INSERT INTO " + Table + " (CompanyId, Year, Month, Day, RunningNo, Created, Modified, ParentCompanyId, State, CreatedBy, ModifiedBy, ReferenceId)" +
                "VALUES(" + GlobalVariable.COMPANY_ID + ", " + transDate.Year + ", " + transDate.Month + ", " + transDate.Day + ", 1, '" + createDate + "', '" + modifiedDate + "', " + GlobalVariable.COMPANY_ID + ", 3, 0, 0, " + refId + ")";
                _context.Database.ExecuteSqlCommand(query);
            }

            foreach (var serialNo in _context.Set<T>().Where(x => x.ReferenceId == refId))
            {
                return serialNo.RunningNo;
            }
            return 0;
        }

    }
}
