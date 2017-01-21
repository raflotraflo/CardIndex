using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MaterialDesignColors;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CardIndex.Domain.Models;

namespace CardIndex.UI.ViewModel
{
    public class ContractorViewModel : ViewModelBase
    {
        #region Private members

        private Contractor _contractor = new Contractor();
        private AddressViewModel _addressVm = new AddressViewModel();

        #endregion

        #region .ctor

        public ContractorViewModel()
        {
        }

        public ContractorViewModel(Contractor contractor)
        {
            Contractor = contractor;
        }
        #endregion

        #region Properties

        public Contractor Contractor
        {
            get
            {
                _contractor.Address = Address?.Address;
                return _contractor;
            }
            set
            {
                if (_contractor != value)
                {
                    FillProperties(value);
                    RaisePropertyChanged(() => Contractor);
                }
            }
        }

        public int Id
        {
            get
            {
                return _contractor.Id;
            }
            set
            {
                if (_contractor.Id != value)
                {
                    _contractor.Id = value;
                    RaisePropertyChanged(() => Id);
                }
            }
        }

        public string Name
        {
            get
            {
                return _contractor.Name;
            }
            set
            {
                if (_contractor.Name != value)
                {
                    _contractor.Name = value;
                    RaisePropertyChanged(() => Name);
                }
            }
        }

        public string Surname
        {
            get
            {
                return _contractor.Surname;
            }
            set
            {
                if (_contractor.Surname != value)
                {
                    _contractor.Surname = value;
                    RaisePropertyChanged(() => Surname);
                }
            }
        }

        public string Email
        {
            get
            {
                return _contractor.Email;
            }
            set
            {
                if (_contractor.Email != value)
                {
                    _contractor.Email = value;
                    RaisePropertyChanged(() => Email);
                }
            }
        }

        public string PhoneNumber
        {
            get
            {
                return _contractor.PhoneNumber;
            }
            set
            {
                if (_contractor.PhoneNumber != value)
                {
                    _contractor.PhoneNumber = value;
                    RaisePropertyChanged(() => PhoneNumber);
                }
            }
        }

        public DateTime? DateOfBirth
        {
            get
            {
                return _contractor.DateOfBirth;
            }
            set
            {
                if (_contractor.DateOfBirth != value)
                {
                    _contractor.DateOfBirth = value;
                    RaisePropertyChanged(() => DateOfBirth);
                }
            }
        }

        public int? AddressId
        {
            get
            {
                return _contractor.AddressId;
            }
            set
            {
                if (_contractor.AddressId != value)
                {
                    _contractor.AddressId = value;
                    RaisePropertyChanged(() => AddressId);
                }
            }
        }

        public AddressViewModel Address
        {
            get
            {
                return _addressVm;
            }
            set
            {
                if (_addressVm != value)
                {
                    _addressVm = value;
                    RaisePropertyChanged(() => Address);
                }
            }
        }


        #endregion

        #region Methods

        private void FillProperties(Contractor contractor)
        {
            Id = contractor.Id;
            Name = contractor.Name;
            Surname = contractor.Surname;
            Email = contractor.Email;
            PhoneNumber = contractor.PhoneNumber;
            DateOfBirth = contractor.DateOfBirth;
            AddressId = contractor.AddressId;

            Address = new AddressViewModel(contractor.Address);
        }

        #endregion
    }
}
