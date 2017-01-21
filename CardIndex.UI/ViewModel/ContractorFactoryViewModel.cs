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
        private IContractorService _contractorService;

        #endregion Private members

        #region .Ctor

        public ContractorFactoryViewModel(IContractorService contractorService)
        {
            if (!ViewModelBase.IsInDesignModeStatic)
            {
                _contractorService = contractorService;

                InitializeCommands();

                _addedContractors = new List<ContractorViewModel>();
                _deletedContractors = new List<ContractorViewModel>();
                _changedContractors = new List<ContractorViewModel>();

                ContractorListVM = new ObservableCollection<ContractorViewModel>();

                ContractorListVM.Add(new ContractorViewModel() { Id = 10, Name = "Rafał" });

                Task.Factory.StartNew(RefreshContractorAsync);
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
            RefreshContractorCommand = new RelayCommand(async () => await RefreshContractorAsync().ConfigureAwait(true));
        }


        private async Task RefreshContractorAsync()
        {
            bool isError = false;
            try
            {
                _addedContractors.Clear();
                _deletedContractors.Clear();
                _changedContractors.Clear();

                Task task = Task.Run(() => RefreshAll());

                await task.ConfigureAwait(true);

                CheckIfNewChangesToSave();
            }
            catch(Exception ex)
            {
                isError = true;
            }

            if(isError)
            {
                MessageDialog messageDialog = new MessageDialog() { DataContext = Properties.Resources.Msg_Error_DataBase };
                await DialogHost.Show(messageDialog, CONTRACTOR_DIALOG);
            }
        }

        private void RefreshAll()
        {
            var all = _contractorService.GetAllContractors().ToList();

            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(
                () => {
                    ContractorListVM.Clear();
                    all.ForEach(x =>
                      {
                          ContractorListVM.Add(new ContractorViewModel(x));
                      });
                  }));

        }

        private async Task SaveExecuteAsync()
        {
            MessageYesNoDialog messageDialog = new MessageYesNoDialog() { DataContext = Properties.Resources.Msg_Question_SaveChanges };
            bool result = (bool)await DialogHost.Show(messageDialog, CONTRACTOR_DIALOG).ConfigureAwait(true);

            if (result)
            {
                var toDelete = new List<Contractor>();
                var toUpdate = new List<Contractor>();
                var toAdd = new List<Contractor>();

                _addedContractors.ForEach(x =>
                {
                    toAdd.Add(x.Contractor);
                });

                _deletedContractors.ForEach(x =>
                {
                    toDelete.Add(x.Contractor);
                });

                _changedContractors.ForEach(x =>
                {
                    toUpdate.Add(x.Contractor);
                });

                _contractorService.AddContractors(toAdd);
                _contractorService.DeleteContractors(toDelete);
                _contractorService.UpdateContractors(toUpdate);

                await RefreshContractorAsync();
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
