using CardIndex.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CardIndex.Domain.Interfaces;

namespace CardIndex.QuicklyTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //
            try
            {
                RepositoryContext db = new RepositoryContext("RepositoryContext");

                IContractorRepository userRepository = new ContractorRepository(db);

                var allUsers = userRepository.All().ToList();
            }
            catch (Exception ex)
            {
                ;
            }


            Console.WriteLine("Hellow world");
            Console.ReadKey();
        }
    }
}
