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
    /// Interakční logika pro EditMain_Page.xaml
    /// </summary>
    public partial class EditMain_Page : Page
    {
        private int vyber;

        DatabaseConf database = new DatabaseConf("Data.db3");

        List<Finance_Items> financeItemm = new List<Finance_Items>();
        List<int> financeList = new List<int>();

        public EditMain_Page()
        {
            InitializeComponent();

            ListViewShow();
        }

        public async void ListViewShow()
        {
            financeItemm = await database.GetAll_FinanceItems();

            foreach (Finance_Items data in financeItemm)
            {
                financeList.Add(data.ID);
            }

            foreach (Finance_Items data in financeItemm)
            {
                if (data.Vydej == "0")
                {
                    listView.Items.Add(new ItemToList { Nazev = data.Nazev, Datum = data.Datum.ToString(), Kategorie = data.Kategorie, Cena = "+" + data.Prijem });
                }
                if (data.Prijem == "0")
                {
                    listView.Items.Add(new ItemToList { Nazev = data.Nazev, Datum = data.Datum.ToString(), Kategorie = data.Kategorie, Cena = "-" + data.Vydej });
                }

            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            FromEditMain.Content = new MainPage();
        }

        private void Ulozit_Click(object sender, RoutedEventArgs e)
        {
            FromEditMain.Content = new MainPage();
        }

        private async void Delete_Click(object sender, RoutedEventArgs e)
        {
            int test = financeList[vyber];

            await database.DeleteEntryAsync_FinanceItems(test);
        }

        private void listView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            vyber = listView.SelectedIndex;
        }
    }
}
