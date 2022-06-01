using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts.IRepositories;
using Entities.Models;

namespace Repositories.Repositories
{
    public class CommodityRepository : RepositoryBase<Commodity>, ICommodityRepository
    {
        public CommodityRepository(LabelMaker_BP_DBContext repositoryContext)
            : base(repositoryContext)
        {
        }

       
    }
}
