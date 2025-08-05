using EngagerMark4.ApplicationCore.Account.Cris;
using EngagerMark4.ApplicationCore.Account.Entities;
using EngagerMark4.ApplicationCore.Account.IRepository;
using EngagerMark4.ApplicationCore.Account.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.Service.Account.ApplicationCore
{
    public class GeneralLedgerService : AbstractService<IGeneralLedgerRepository, GeneralLedgerCri, GeneralLedger>, IGeneralLedgerService
    {
        public GeneralLedgerService(IGeneralLedgerRepository repository) : base(repository)
        {
        }
    }
}
