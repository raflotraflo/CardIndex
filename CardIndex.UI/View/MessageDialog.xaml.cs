using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CardIndex.UI.View
{
    /// <summary>
    /// Interaction logic for MessageDialog.xaml
    /// </summary>
    public partial class MessageDialog : UserControl
    {
        private string _headerText;

        public MessageDialog()
        {
            InitializeComponent();
            txtHeader.Visibility = System.Windows.Visibility.Hidden;
        }

        public string HeaderText
        {
            get { return _headerText; }
            set
            {
                _headerText = value;
                txtHeader.Text = value;

                if (string.IsNullOrEmpty(value))
                {
                    txtHeader.Visibility = System.Windows.Visibility.Hidden;
                }
                else
                {
                    txtHeader.Visibility = System.Windows.Visibility.Visible;
                }
            }
        }
    }
}
