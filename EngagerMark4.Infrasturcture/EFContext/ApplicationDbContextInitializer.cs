using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.Infrasturcture.EFContext
{
    public class ApplicationDbContextInitializer : CreateDatabaseIfNotExists<ApplicationDbContext>
    {
        /// <summary>
        /// Initialize the database with necessary data
        /// Created by: Kyaw Min Htut
        /// Created date: 06-04-2015
        /// </summary>
        /// <param name="context"></param>
        protected override void Seed(ApplicationDbContext context)
        {
            base.Seed(context);


        }
    }
}
