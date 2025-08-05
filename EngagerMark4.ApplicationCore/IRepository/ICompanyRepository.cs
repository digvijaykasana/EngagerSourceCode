using EngagerMark4.ApplicationCore.Cris;
using EngagerMark4.ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.IRepository
{
    public interface ICompanyRepository : IBaseRepository<CompanyCri, Company>
    {
        Company GetCompanyByDomain(string domain);
    }
}
