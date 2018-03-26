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
    /// Interakční logika pro PlanovanyVydaj_Page.xaml
    /// </summary>
    public partial class PlanovanyVydaj_Page : Page
    {
        private string _sazba = "0";
        private string _pujceno;
        private string _dluhOd;

        private string _puvodni_zustatek;

        private DateTime _datumPoc;
        private DateTime _datumKon;

        DatabaseConf database = new DatabaseConf("Data.db3");

        List<Dluhy_Items> dluhyItems = new List<Dluhy_Items>();
        List<string> dluhyShow = new List<string>();

        List<Finance_Items> financeItem = new List<Finance_Items>();
        List<string> financeShow = new List<string>();

        public PlanovanyVydaj_Page()
        {
            InitializeComponent();

            financeItem = database.GetCategoryRowAsync_FinanceItems().Result;

            foreach (Finance_Items data in financeItem)
            {
                financeShow.Add(data.Zustatek);
                _puvodni_zustatek = financeShow[0];
            }

            ListDluhViewShow();
        }

        private async void ListDluhViewShow()
        {
            dluhyItems = await database.GetAll_DluhyItems();

            foreach (Dluhy_Items data in dluhyItems)
            {
                listViewDluh.Items.Add(new DluhToList { Nazev = data.Nazev, PocatekPujcky = data.PocatekPujcky.ToString(), KonecPujcky = data.KonecPujcky.ToString(), Sazba = data.Sazba + "%", VysePujcky = data.VysePujcky});
            }
        }

        private async void Uloz_Prijem_Click(object sender, RoutedEventArgs e)
        {
            _dluhOd = Pujceno_Nazev.Text;
            _pujceno = Pujceno_Castka.Text;
            _sazba = Sazba.Text;

            if (PocatekKalendar.SelectedDate.HasValue)
            {
                _datumPoc = PocatekKalendar.SelectedDate.Value;
            }
            else
            {
                _datumPoc = DateTime.Now;
            }

            if (KonecKalendar.SelectedDate.HasValue)
            {
                _datumKon = KonecKalendar.SelectedDate.Value;
            }
            else
            {
                _datumKon = DateTime.Now;
            }

            int.TryParse(_puvodni_zustatek, out int _puvodni_zustatekF);
            int.TryParse(_pujceno, out int _pujcenoF);

            _puvodni_zustatekF = _puvodni_zustatekF - _pujcenoF;
            _puvodni_zustatek = _puvodni_zustatekF.ToString();



            Dluhy_Items entry1 = new Dluhy_Items() { Nazev = _dluhOd, PocatekPujcky = _datumPoc, KonecPujcky = _datumKon, Sazba = _sazba, VysePujcky = _pujceno};
            await database.SaveItemAsync_DluhyItems(entry1);

            Finance_Items entry2 = new Finance_Items() { Zustatek = _puvodni_zustatek, Prijem = "0", Vydej = _pujceno, Nazev = _dluhOd + "- Půjčka", Kategorie = "Půjčka", Datum = _datumPoc };
            await database.SaveItemAsync_FinanceItems(entry2);

            FromDluh.Content = new MainPage();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            FromDluh.Content = new MainPage();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            FromDluh.Content = new EditDluhy_Page();
        }
    }
}
