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
using System.Windows.Shapes;
using Configuration = BG.Infrastructure.UICommon.Configurations.Configuration;

namespace BG.Application.DocumentsGenerator
{
    public class LicenseValue
    {
        public string Key { set; get; }
    }

    /// <summary>
    /// Interaction logic for LicenseKey.xaml
    /// </summary>
    public partial class LicenseKey : Window
    {
        public LicenseKey()
        {
            InitializeComponent();

            this.DataContext = new LicenseValue();
        }

        LicenseValue Value => this.DataContext as LicenseValue;

        private void SetKey_Click(object sender, RoutedEventArgs e)
        {
            Guid guid = Guid.Empty;

            if (string.IsNullOrWhiteSpace(Value?.Key) || Guid.TryParse(Value?.Key, out guid))
            {
                MessageBox.Show("Не определен ключ лицензии");
                DialogResult = false;
                return;
            }

            Configuration.SetLicenseGuid(guid);
            DialogResult = true;
        }
    }
}
