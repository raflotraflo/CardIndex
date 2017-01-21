using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight;
using MaterialDesignThemes.Wpf;
using CardIndex.UI.View;
using CardIndex.Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CardIndex.Domain.Interfaces;
using System.Windows.Threading;


namespace CardIndex.UI.ViewModel
{
    public class GenerateDataViewModel : ViewModelBase
    {
        #region Public members

        public RelayCommand GeneraDataCommand { get; private set; }

        #endregion Public members

        #region Private members

        private ContractorFactoryViewModel _contractorFactoryViewModel;
        private IContractorService _contractorService;

        #endregion Private members

        public GenerateDataViewModel(ContractorFactoryViewModel contractorFactoryViewModel, IContractorService contractorService)
        {
            _contractorFactoryViewModel = contractorFactoryViewModel;
            _contractorService = contractorService;
            GeneraDataCommand = new RelayCommand(async () => await GeneraDataAsync().ConfigureAwait(true));
        }

        #region Methods

        private async Task GeneraDataAsync()
        {
            var toAdd = new List<Contractor>();

            for (int i = 1; i <= 5; i++)
            {
                var newContractor = new Contractor()
                {
                    Name = "Rafał " + i,
                    Surname = "Chodzidło",
                    Email = "raflotraflo@gmail.com",
                    PhoneNumber = "530529985",
                    DateOfBirth = new DateTime(1992, 3, 17)
                };

                if (i % 2 == 1)
                {
                    var newAdress = new Address()
                    {
                        Street = "Wieczorka " + i,
                        City = "Studzionka",
                        ZipCode = "43-245"
                    };

                    newContractor.Address = newAdress;
                }

                toAdd.Add(newContractor);

            }

            _contractorService.AddContractors(toAdd);

            Task task = Task.Run(() => _contractorFactoryViewModel.RefreshContractorCommand.Execute(null));

            await task.ConfigureAwait(true);
        }



        #endregion Methods
    }
}
