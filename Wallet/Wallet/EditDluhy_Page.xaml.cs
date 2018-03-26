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

namespace Wallet
{
    /// <summary>
    /// Interakční logika pro EditDluhy_Page.xaml
    /// </summary>
    public partial class EditDluhy_Page : Page
    {
        private int vyber;

        DatabaseConf database = new DatabaseConf("Data.db3");

        List<Dluhy_Items> dluhyItems = new List<Dluhy_Items>();
        List<int> dluhyList = new List<int>();

        public EditDluhy_Page()
        {
            InitializeComponent();

            ListDluhViewShow();
        }

        private async void ListDluhViewShow()
        {
            dluhyItems = await database.GetAll_DluhyItems();

            foreach (Dluhy_Items data in dluhyItems)
            {
                dluhyList.Add(data.ID);
            }

            foreach (Dluhy_Items data in dluhyItems)
            {
                listViewDluh.Items.Add(new DluhToList { Nazev = data.Nazev, PocatekPujcky = data.PocatekPujcky.ToString(), KonecPujcky = data.KonecPujcky.ToString(), Sazba = data.Sazba + "%", VysePujcky = data.VysePujcky });
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            FromEditLoan.Content = new PlanovanyVydaj_Page();
        }

        private void Ulozit_Click(object sender, RoutedEventArgs e)
        {
            FromEditLoan.Content = new PlanovanyVydaj_Page();
        }

        private async void Delete_Click(object sender, RoutedEventArgs e)
        {
            int test = dluhyList[vyber];

            await database.DeleteLoanAsync_DluhyItems(test);
        }

        private void listViewDluh_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            vyber = listViewDluh.SelectedIndex;
        }
    }
}
