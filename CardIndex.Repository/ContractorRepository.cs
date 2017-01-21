using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CardIndex.Domain.Models;
using CardIndex.Domain.Interfaces;

namespace CardIndex.Repository
{
    public class ContractorRepository : RepositoryBase<Contractor>, IContractorRepository
    {
        public ContractorRepository(IRepositoryContext context) : base (context)
        {

        }
    }
}
