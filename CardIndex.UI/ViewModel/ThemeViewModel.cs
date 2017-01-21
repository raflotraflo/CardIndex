using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MaterialDesignColors;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using CardIndex.UI.View;


namespace CardIndex.UI.ViewModel
{
    public class ThemeViewModel : ViewModelBase
    {
        #region Public members

        public RelayCommand<Swatch> ApplyAccentCommand { get; private set; }
        public RelayCommand<Swatch> ApplyPrimaryCommand { get; private set; }
        public RelayCommand<bool> ToggleBaseCommand { get; private set; }
        public RelayCommand SaveChangesCommand { get; private set; }
        public RelayCommand CancelChangesCommand { get; private set; }

        #endregion Public members

        #region Private members

        private bool _isDark;
        private bool _isNewChangesToSave;

        private bool _newBase;
        private string _newPrimary;
        private string _newAccent;

        private const string SETTINGSMAIN_DIALOG = "RootDialog";

        #endregion Private members

        #region .ctor

        public ThemeViewModel()
        {
            if (!ViewModelBase.IsInDesignModeStatic)
            {
                InitializeCommands();

                Swatches = new SwatchesProvider().Swatches;

                IsDark = _newBase = Properties.Settings.Default.ThemeBase;
                _newAccent = Properties.Settings.Default.ThemeAccent;
                _newPrimary = Properties.Settings.Default.ThemePrimary;
            }
        }

        #endregion .Ctor

        #region Properties

        public IEnumerable<Swatch> Swatches { get; set; }

        public bool IsDark
        {
            get { return _isDark; }
            set
            {
                _isDark = value;
                RaisePropertyChanged(() => IsDark);
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

        public void CancelThemeConfiguration()
        {
            CancelChanges();
        }

        private void InitializeCommands()
        {
            ApplyAccentCommand = new RelayCommand<Swatch>((x) => ApplyAccent(x));
            ApplyPrimaryCommand = new RelayCommand<Swatch>((x) => ApplyPrimary(x));
            ToggleBaseCommand = new RelayCommand<bool>((x) => ApplyBase(x));
            SaveChangesCommand = new RelayCommand(async () => await SaveChangesAsync());
            CancelChangesCommand = new RelayCommand(async () => await CancelChangesWithAskingAsync());
        }

        private void ApplyBase(bool isDark)
        {
            IsDark = isDark;

            if (_newBase != isDark)
            {
                new PaletteHelper().SetLightDark(isDark);
                _newBase = isDark;
                CheckIsNewChanges();
            }
        }

        private void ApplyPrimary(Swatch swatch)
        {
            if (_newPrimary != swatch.Name)
            {
                new PaletteHelper().ReplacePrimaryColor(swatch);
                _newPrimary = swatch.Name;
                CheckIsNewChanges();
            }
        }

        private void ApplyAccent(Swatch swatch)
        {
            if (_newAccent != swatch.Name)
            {
                new PaletteHelper().ReplaceAccentColor(swatch);
                _newAccent = swatch.Name;
                CheckIsNewChanges();
            }
        }

        private void CheckIsNewChanges()
        {
            IsNewChangesToSave = !(Properties.Settings.Default.ThemeBase == _newBase)
                || !(Properties.Settings.Default.ThemeAccent == _newAccent)
                || !(Properties.Settings.Default.ThemePrimary == _newPrimary);
        }

        private async Task SaveChangesAsync()
        {
            MessageYesNoDialog messageDialog = new MessageYesNoDialog() { DataContext = Properties.Resources.Msg_Question_SaveChanges };
            bool result = (bool)await DialogHost.Show(messageDialog, SETTINGSMAIN_DIALOG).ConfigureAwait(true);

            if (result)
            {
                Properties.Settings.Default.ThemeAccent = _newAccent;
                Properties.Settings.Default.ThemeBase = _newBase;
                Properties.Settings.Default.ThemePrimary = _newPrimary;

                Properties.Settings.Default.Save();
                IsNewChangesToSave = false;
            }
        }

        private async Task CancelChangesWithAskingAsync()
        {
            MessageYesNoDialog messageDialog = new MessageYesNoDialog() { DataContext = Properties.Resources.Msg_Question_CancelChanges };
            bool result = (bool)await DialogHost.Show(messageDialog, SETTINGSMAIN_DIALOG).ConfigureAwait(true);

            if (result)
            {
                CancelChanges();
            }
        }

        private void CancelChanges()
        {
            try
            {
                new PaletteHelper().SetLightDark(Properties.Settings.Default.ThemeBase);

                if (Properties.Settings.Default.ThemeAccent != string.Empty)
                {
                    try
                    {
                        Swatch swatch = new SwatchesProvider().Swatches.Where(s => s.Name == Properties.Settings.Default.ThemeAccent).First();
                        new PaletteHelper().ReplaceAccentColor(swatch);
                    }
                    catch (Exception) { }
                }

                if (Properties.Settings.Default.ThemePrimary != string.Empty)
                {
                    try
                    {
                        Swatch swatch = new SwatchesProvider().Swatches.Where(s => s.Name == Properties.Settings.Default.ThemePrimary).First();
                        new PaletteHelper().ReplacePrimaryColor(swatch);
                    }
                    catch (Exception) { }
                }
            }
            catch (Exception) { }

            IsDark = _newBase = Properties.Settings.Default.ThemeBase;
            _newAccent = Properties.Settings.Default.ThemeAccent;
            _newPrimary = Properties.Settings.Default.ThemePrimary;

            IsNewChangesToSave = false;
        }

        #endregion Methods
    }
}
