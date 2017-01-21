using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CardIndex.Domain.Models;
using CardIndex.Repository;
using CardIndex.Domain.Interfaces;
namespace CardIndex.Core.Services
{
    public class ContractorService : IContractorService
    {

        IContractorRepository _userRepository;
        IAddressRepository _addressRepository;

        public ContractorService()
        {
            RepositoryContext db = new RepositoryContext("RepositoryContext");

             _userRepository = new ContractorRepository(db);
             _addressRepository = new AddressRepository(db);
        }


        public IEnumerable<Contractor> GetAllContractors()
        {
            var allUsers = _userRepository.All();

            return allUsers;
        }

        public Result AddContractors(IEnumerable<Contractor> contractors)
        {
            try
            {

                foreach (var c in contractors)
                {
                    _userRepository.Add(c);
                }

                _userRepository.SaveChanges();
                return new Result() { Error = false };
            }
            catch(Exception ex)
            {
                return new Result() { Error = true, Message = ex.Message };
            }
        }


        public Result DeleteContractors(IEnumerable<Contractor> contractors)
        {
            try
            {
                foreach (var c in contractors)
                {
                    var toDelete = _userRepository.GetById(c.Id);
                    _userRepository.Delete(toDelete);
                }

                _userRepository.SaveChanges();
                return new Result() { Error = false };
            }
            catch (Exception ex)
            {
                return new Result() { Error = true, Message = ex.Message };
            }
        }

        public Result UpdateContractors(IEnumerable<Contractor> contractors)
        {
            try
            {

                foreach (var c in contractors)
                {
                    var toUpdate = _userRepository.GetById(c.Id);

                    toUpdate.Name = c.Name;
                    toUpdate.DateOfBirth = c.DateOfBirth;
                    toUpdate.Email = c.Email;
                    toUpdate.PhoneNumber = c.PhoneNumber;
                    toUpdate.Surname = c.Surname;
                    
                    if(c.Address != null)
                    {
                        toUpdate.Address = UpdateAddress(c.Address);
                        toUpdate.AddressId = toUpdate.Address.Id;
                    }

                    _userRepository.Update(toUpdate);
                }

                _userRepository.SaveChanges();
                return new Result() { Error = false };
            }
            catch (Exception ex)
            {
                return new Result() { Error = true, Message = ex.Message };
            }
        }

        private Address UpdateAddress(Address addres)
        {
            if(addres.Id != 0)
            {
               return _addressRepository.Add(addres);
            }
            else
            {
                var toUpdate = _addressRepository.GetById(addres.Id);

                toUpdate.Street = addres.Street;
                toUpdate.ZipCode = addres.ZipCode;
                toUpdate.City = addres.City;

                return _addressRepository.Update(addres);
            }
        }
    }
}
