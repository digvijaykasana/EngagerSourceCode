using EngagerMark4.ApplicationCore.Cris;
using EngagerMark4.ApplicationCore.Entities;
using EngagerMark4.ApplicationCore.IRepository;
using EngagerMark4.ApplicationCore.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.Service.ApplicationCore
{
    /// <summary>
    /// 
    /// </summary>
    public class CompanyService : AbstractService<ICompanyRepository, CompanyCri, Company>, ICompanyService
    {
        public CompanyService(ICompanyRepository repository) : base(repository)
        {
        }
    }
}
