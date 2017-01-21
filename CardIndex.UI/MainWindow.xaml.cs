using MahApps.Metro.Controls;
using System.Reflection;
namespace CardIndex.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            string appVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            this.Title = string.Format("{0} ({1})", this.Title, appVersion);
        }
    }
}
