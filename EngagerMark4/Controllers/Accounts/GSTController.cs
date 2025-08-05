using EngagerMark4.ApplicationCore.Account.Cris;
using EngagerMark4.ApplicationCore.Account.Entities;
using EngagerMark4.ApplicationCore.Account.IService;
using EngagerMark4.ApplicationCore.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EngagerMark4.Controllers.Accounts
{
    /// <summary>
    /// Controller Class for GST
    /// Created     : Ye Kaung Aung
    /// Modified    : 
    /// </summary>
    public class GSTController : BaseController<GSTCri, GST, IGSTService>
    {
        public GSTController(IGSTService service) : base(service)
        {
        }
    }
}