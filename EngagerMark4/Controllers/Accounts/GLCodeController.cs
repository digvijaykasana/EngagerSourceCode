using EngagerMark4.ApplicationCore.Account.Cris;
using EngagerMark4.ApplicationCore.Account.Entities;
using EngagerMark4.ApplicationCore.Account.IService;
using EngagerMark4.ApplicationCore.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EngagerMark4.Controllers.Configurations
{
    /// <summary>
    /// Controller Class for General Ledger Code
    /// Created     : Ye Kaung Aung
    /// Modified    : 
    /// </summary>
    public class GLCodeController : BaseController<GeneralLedgerCri, GeneralLedger, IGeneralLedgerService>
    {
        public GLCodeController(IGeneralLedgerService service) : base(service)
        {
        }
    }
}