using EngagerMark4.ApplicationCore.Account.Cris;
using EngagerMark4.ApplicationCore.Account.Entities;
using EngagerMark4.ApplicationCore.Account.IRepository;
using EngagerMark4.Infrasturcture.EFContext;
using EngagerMark4.Infrasturcture.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.Infrastructure.Account.Repository
{
    public class GSTRepository : GenericRepository<ApplicationDbContext, GSTCri, GST>, IGSTRepository
    {
        public GSTRepository (ApplicationDbContext aContext) : base(aContext)
        {
            
        }
    }
}
