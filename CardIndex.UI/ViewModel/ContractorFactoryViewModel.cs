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

namespace CardIndex.UI.ViewModel
{
    public class ContractorFactoryViewModel : ViewModelBase
    {
        #region Public members

        public RelayCommand SaveCommand { get; private set; }
        public RelayCommand<ContractorViewModel> DeleteContractorCommand { get; private set; }
        public RelayCommand<ContractorViewModel> EditContractorCommand { get; private set; }
        public RelayCommand AddContractorCommand { get; private set; }
        public RelayCommand RefreshContractorCommand { get; private set; }

        public RelayCommand<object> ShowContractorDetailCommand { get; private set; }

        #endregion Public members

        #region Private members

        private const string CONTRACTOR_DIALOG = "CONTRACTOR_DIALOG";

        private ObservableCollection<ContractorViewModel> _contractorListVM;
        private List<ContractorViewModel> _addedContractors;
        private List<ContractorViewModel> _deletedContractors;
        private List<ContractorViewModel> _changedContractors;
        private ContractorViewModel _selectedContractorVM;
        private bool _isNewChangesToSave;

        #endregion Private members

        #region .Ctor

        public ContractorFactoryViewModel()
        {
            if (!ViewModelBase.IsInDesignModeStatic)
            {
                InitializeCommands();

                _addedContractors = new List<ContractorViewModel>();
                _deletedContractors = new List<ContractorViewModel>();
                _changedContractors = new List<ContractorViewModel>();

                ContractorListVM = new ObservableCollection<ContractorViewModel>();

                ContractorListVM.Add(new ContractorViewModel() { Id = 10, Name = "Rafał" });

                Task.Factory.StartNew(RefreshContractorExecuteAsync);
            }
        }

        #endregion .Ctor

        #region Properties

        public ObservableCollection<ContractorViewModel> ContractorListVM
        {
            get
            {
                return _contractorListVM;
            }
            set
            {
                if (_contractorListVM != value)
                {
                    _contractorListVM = value;
                    RaisePropertyChanged(() => ContractorListVM);
                }
            }
        }

        public ContractorViewModel SelectedContractorVM
        {
            get
            {
                return _selectedContractorVM;
            }
            set
            {
                if (_selectedContractorVM != value)
                {
                    _selectedContractorVM = value;
                    RaisePropertyChanged(() => SelectedContractorVM);
                }
            }
        }

        public bool IsNewChangesToSave
        {
            get
            {
                return _isNewChangesToSave;
            }
            set
            {
                if (_isNewChangesToSave != value)
                {
                    _isNewChangesToSave = value;
                    RaisePropertyChanged(() => IsNewChangesToSave);
                }
            }
        }

        #endregion Properties

        #region Methods


        private void InitializeCommands()
        {
            SaveCommand = new RelayCommand(async () => await SaveExecuteAsync().ConfigureAwait(true));
            DeleteContractorCommand = new RelayCommand<ContractorViewModel>(async (x) => await DeleteContractorExecuteAsync(x).ConfigureAwait(true));
            EditContractorCommand = new RelayCommand<ContractorViewModel>(async (x) => await EditContractorExecuteAsync(x).ConfigureAwait(true));
            AddContractorCommand = new RelayCommand(async () => await AddContractorExecuteAsync().ConfigureAwait(true));
            RefreshContractorCommand = new RelayCommand(async () => await RefreshContractorExecuteAsync().ConfigureAwait(true));
        }


        private async Task RefreshContractorExecuteAsync()
        {
            ContractorViewModel newContractor = new ContractorViewModel();
            AddContractorView addNewContractorDialog = new AddContractorView { DataContext = newContractor };

            while (true)
            {
                object result = await DialogHost.Show(addNewContractorDialog, CONTRACTOR_DIALOG).ConfigureAwait(true);

                if ((bool)result == false)
                {
                    break;
                }

                ContractorListVM.Add(newContractor);
                _addedContractors.Add(newContractor);

                CheckIfNewChangesToSave();
                break;
            }
        }



        private async Task SaveExecuteAsync()
        {
            MessageYesNoDialog messageDialog = new MessageYesNoDialog() { DataContext = Properties.Resources.Msg_Question_SaveChanges };
            bool result = (bool)await DialogHost.Show(messageDialog, CONTRACTOR_DIALOG).ConfigureAwait(true);

            if (result)
            {
                ////saving configuration
                //List<IPLCDriver> plcDrivers = new List<IPLCDriver>();

                //foreach (PlcDriverViewModel plc in ContractorListVM)
                //{
                //    DispatchIfNecessary((Action)delegate
                //    {
                //        plcDrivers.Add(plc.PlcDriver);
                //    });
                //}

                //ConfigurationHelper.Instance.SaveSettings(plcDrivers);

                //if (_serviceStateManager.ServiceState)
                //{
                //    //sendPipeMessage
                //    foreach (IContractor add in _addedContractors)
                //    {
                //        SendPipeMessage<IContractor>(_pipeClientHelper.SendAddContractor, add);
                //    }

                //    foreach (IContractor delete in _deletedContractors)
                //    {
                //        SendPipeMessage<IContractor>(_pipeClientHelper.SendDeleteContractor, delete);
                //    }
                //}

                _addedContractors.Clear();
                _deletedContractors.Clear();
                CheckIfNewChangesToSave();
            }
        }




        private async Task DeleteContractorExecuteAsync(ContractorViewModel contractor)
        {
            MessageYesNoDialog yesNoMessageDialog = new MessageYesNoDialog() { DataContext = string.Format(Properties.Resources.Msg_Question_DeleteContractor, contractor.Name) };

            bool result = (bool)await DialogHost.Show(yesNoMessageDialog, CONTRACTOR_DIALOG).ConfigureAwait(true);

            if (result)
            {
                bool isDeleted = ContractorListVM.Remove(contractor);

                if (isDeleted)
                {
                    bool temporaryContractor = _addedContractors.Any(a => a.Email == contractor.Email);

                    if (temporaryContractor)
                    {
                        _addedContractors.RemoveAll(a => a.Email == contractor.Email);
                    }
                    else
                    {
                        _deletedContractors.Add(contractor);
                    }

                    CheckIfNewChangesToSave();
                }
            }
        }

        private async Task AddContractorExecuteAsync()
        {
            ContractorViewModel newContractor = new ContractorViewModel();
            AddContractorView addNewContractorDialog = new AddContractorView { DataContext = newContractor };

            while (true)
            {
                object result = await DialogHost.Show(addNewContractorDialog, CONTRACTOR_DIALOG).ConfigureAwait(true);

                if ((bool)result == false)
                {
                    break;
                }

                ContractorListVM.Add(newContractor);
                _addedContractors.Add(newContractor);

                CheckIfNewChangesToSave();
                break;
            }
        }



        private async Task EditContractorExecuteAsync(ContractorViewModel contractor)
        {

            AddContractorView addNewContractorDialog = new AddContractorView { DataContext = contractor };

            while (true)
            {
                object result = await DialogHost.Show(addNewContractorDialog, CONTRACTOR_DIALOG).ConfigureAwait(true);

                if ((bool)result == false || _changedContractors.Contains(contractor))
                {
                    break;
                }

                _changedContractors.Add(contractor);
                

                CheckIfNewChangesToSave();
                break;
            }
        }


        private void CheckIfNewChangesToSave()
        {
            if (_addedContractors.Any() || _deletedContractors.Any() || _changedContractors.Any())
            {
                IsNewChangesToSave = true;
                return;
            }

            IsNewChangesToSave = false;
        }


        #endregion Methods
    }
}
