using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MaterialDesignThemes.Wpf;
using System;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using CardIndex.UI.View;
using MahApps.Metro.Controls.Dialogs;

namespace CardIndex.UI.ViewModel
{
    public class RootViewModel : ViewModelBase
    {
        #region Public members

        public RelayCommand ShowSettingsCommand { get; private set; }
        public RelayCommand ShowMainCommand { get; private set; }
        public RelayCommand ShowInfoDialogCommand { get; private set; }
        public RelayCommand<EventArgs> ClosingWindowEventToCommand { get; private set; }

        #endregion Public members

        #region Private members

        private UserControl _selectedUserControl;
        private MainViewModel _main;
        private SettingsView _settingView;
        private MainView _mainView;
        // private ApplicationSettingsViewModel _applicationSettings;
        private ThemeViewModel _themeSettings;
        private const string ROOT_DIALOG = "RootDialog";
        private bool _canOpenClosingDialog;

        #endregion Private members

        public RootViewModel(MainViewModel main, ThemeViewModel themeSettings)
        {
            if (!ViewModelBase.IsInDesignModeStatic)
            {
                ShowSettingsCommand = new RelayCommand(() => ShowSettings());
                ShowMainCommand = new RelayCommand(async () => await ShowMainAsync());
                ShowInfoDialogCommand = new RelayCommand(async () => await ShowInfoDialogAsync());
                ClosingWindowEventToCommand = new RelayCommand<EventArgs>(async (x) => await ClosingWindowEvent(x));

                _main = main;
                _themeSettings = themeSettings;
                _canOpenClosingDialog = true;

                _mainView = new MainView();
                SelectedUserControl = _mainView;
                _main.IsActive = true;
            }
        }

        #region Properties

        public UserControl SelectedUserControl
        {
            get
            {
                return _selectedUserControl;
            }
            private set
            {
                if (_selectedUserControl != value)
                {
                    _selectedUserControl = value;
                    RaisePropertyChanged(() => SelectedUserControl);
                }
            }
        }

        public bool IsShowMain { get; private set; }

        #endregion Properties

        #region Methods

        private void ShowSettings()
        {
            if (_settingView == null)
                _settingView = new SettingsView();

            SelectedUserControl = _settingView;
            _main.IsActive = false;
        }

        private async Task ShowMainAsync()
        {
            if (_themeSettings.IsNewChangesToSave)
            {
                MessageYesNoDialog messageDialog = new MessageYesNoDialog() { DataContext = Properties.Resources.Msg_Question_ExitWithoutSavingChanges };
                bool result = (bool)await DialogHost.Show(messageDialog, ROOT_DIALOG).ConfigureAwait(true);

                if (result)
                {
                    //_applicationSettings.CancelConfiguration();
                    _themeSettings.CancelThemeConfiguration();
                }
                else
                {
                    return;
                }
            }

            if (_mainView == null)
                _mainView = new MainView();

            SelectedUserControl = _mainView;
            _main.IsActive = true;
        }

        private async Task ShowInfoDialogAsync()
        {
            int currentYear = DateTime.Now.Year;
            string year = string.Format(" {0}", currentYear);

            if (currentYear > 2016)
            {
                year = string.Format(" {0} - {1}", 2016, currentYear);
            }

            string appVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            string appPath = System.AppDomain.CurrentDomain.BaseDirectory;

            StringBuilder sb = new StringBuilder();
            //sb.AppendLine(Properties.Resources.Msg_Program_Info);
            sb.AppendLine(string.Format("Version: {0}", appVersion));
            sb.AppendLine(string.Format("Path: {0}", appPath));
            sb.AppendLine(" ");
            sb.Append(Properties.Resources.Msg_Program_Info_Version).AppendLine(year);

            View.MessageDialog messageDialog = new View.MessageDialog() { HeaderText = Properties.Resources.Msg_Program_Info, DataContext = sb.ToString() };
            await DialogHost.Show(messageDialog, ROOT_DIALOG).ConfigureAwait(false);

            return;
        }

        private async Task ClosingWindowEvent(EventArgs e)
        {
            System.ComponentModel.CancelEventArgs args = (System.ComponentModel.CancelEventArgs)e;
            args.Cancel = true;

            if (_canOpenClosingDialog)
            {
                _canOpenClosingDialog = false;

                MessageYesNoDialog messageDialog = new MessageYesNoDialog() { DataContext = Properties.Resources.Msg_Question_CloseApp };
                bool result = (bool)await DialogHost.Show(messageDialog, ROOT_DIALOG).ConfigureAwait(true);

                if (result)
                {
                    //_main.ServiceStateText = "Closing app...";

                    //if (ConfigurationHelper.Instance.ApplicationSettings.StopServiceWithCloseUI)
                    //{
                    //    //zatrzymanie serwisu podczas zamykania aplikacji UI
                    //    await _main.StopProcessServiceAppIfExistAsync();
                    //}

                    App.Current.Shutdown();
                }

                _canOpenClosingDialog = true;
            }
        }

        #endregion Methods
    }
}
