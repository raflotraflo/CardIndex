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
    public class AddressViewModel : ViewModelBase
    {
        #region Private members

        private Address _address = new Address();

        #endregion

        #region .ctor

        public AddressViewModel()
        {
        }

        public AddressViewModel(Address address)
        {
            Address = address;
        }
        #endregion

        #region Properties

        public Address Address
        {
            get
            {
                return _address;
            }
            set
            {
                if (_address != value)
                {
                    FillProperties(value);
                    RaisePropertyChanged(() => Address);
                }
            }
        }

        public int Id
        {
            get
            {
                return _address.Id;
            }
            set
            {
                if (_address.Id != value)
                {
                    _address.Id = value;
                    RaisePropertyChanged(() => Id);
                }
            }
        }

        public string Street
        {
            get
            {
                return _address.Street;
            }
            set
            {
                if (_address.Street != value)
                {
                    _address.Street = value;
                    RaisePropertyChanged(() => Street);
                }
            }
        }

        public string ZipCode
        {
            get
            {
                return _address.ZipCode;
            }
            set
            {
                if (_address.Street != value)
                {
                    _address.Street = value;
                    RaisePropertyChanged(() => ZipCode);
                }
            }
        }

        public string City
        {
            get
            {
                return _address.City;
            }
            set
            {
                if (_address.Street != value)
                {
                    _address.Street = value;
                    RaisePropertyChanged(() => City);
                }
            }
        }

        #endregion

        #region Methods

        private void FillProperties(Address address)
        {
            Id = address.Id;
            Street = address.Street;
            ZipCode = address.ZipCode;
            City = address.City;
        }

        #endregion
    }
}
