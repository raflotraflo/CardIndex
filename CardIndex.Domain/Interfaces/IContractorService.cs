using CardIndex.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardIndex.Domain.Interfaces
{
    public interface IContractorService
    {
        IEnumerable<Contractor> GetAllContractors();
        Result AddContractors(IEnumerable<Contractor> contractors);
        Result DeleteContractors(IEnumerable<Contractor> contractors);
        Result UpdateContractors(IEnumerable<Contractor> contractors);
    }
}
