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
    /// Interakční logika pro MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        private string _zustatek = "0";

        int click = 0;

        public int perMonth;
        public int perYear;
        public int costVydaj;
        public int costPrijem;

        private int _dluhy;
        private int _dluhySoucet;

        DatabaseConf database = new DatabaseConf("Data.db3");
        
        List<Finance_Items> financeItem = new List<Finance_Items>();
        List<Finance_Items> financeItemm = new List<Finance_Items>();
        List<Dluhy_Items> dluhItem = new List<Dluhy_Items>();
        List<Dluhy_Items> loanCheck = new List<Dluhy_Items>();
        List<string> financeShow = new List<string>();
        List<string> financeShoww = new List<string>();
        List<string> dluhShow = new List<string>();
        List<string> loanCheckk = new List<string>();
        

        public MainPage()
        {
            InitializeComponent();

            ListViewShow();
            OutPerMonth();
            OutPerYear();
            LoanCheck();

            LoadRest();
        }

        public void LoadRest()
        {
            financeItem = database.GetCategoryRowAsync_FinanceItems().Result;

            foreach (Finance_Items data in financeItem)
            {
                financeShow.Add(data.Zustatek);

                _zustatek = financeShow[0];
            }

            Zustatek.Content = _zustatek + ",-Kč";
        }

        public void LoadLoans()
        {
            dluhItem = database.GetCategoryRowAsync_DluhyItems().Result;

            foreach (Dluhy_Items data in dluhItem)
            {
                int.TryParse(data.VysePujcky, out _dluhy);

                _dluhySoucet += _dluhy;
            }

            Dluhy.Content = _dluhySoucet + ",-Kč";
        }

        public async void LoanCheck()
        {
            loanCheck = await database.GetDatesAsync_DluhyItems();

            foreach (Dluhy_Items data in loanCheck)
            {

                DateTime dateNow = DateTime.Now;
                DateTime dataDate = data.KonecPujcky;

                if (dataDate < dateNow)
                {
                    int sazba = Convert.ToInt32(data.Sazba);
                    int vysePujcky = Convert.ToInt32(data.VysePujcky);

                    int navyseni = (sazba * vysePujcky) / 100;

                    MessageBox.Show("Dluh " + data.Nazev + ", přesáhl svůj limit splatnosti. Navýšení dluhu o " + navyseni);

                    data.VysePujcky = (int.Parse(data.VysePujcky) + navyseni).ToString();
                    data.KonecPujcky = data.KonecPujcky.AddMonths(1);

                    await database.SaveItemAsync_DluhyItems(data);

                    LoadLoans();
                    LoadRest();

                    //var prodlouzeni = data.KonecPujcky;

                    //prodlouzeni.AddMonths(1);

                    //navyseni += Convert.ToInt32(data.VysePujcky);

                    //await database.UpdateProdlouzeniAsync_DluhyItems(prodlouzeni, data.ID);
                    //await database.UpdateNavyseniAsync_DluhyItems(navyseni.ToString(), data.ID);
                    
                    //Prodloužení o 1 další měsíc kvůli Messageboxu
                }
                else
                {
                    LoadLoans();
                    LoadRest();
                }
            }            
        }

        public async void ListViewShow()
        {
            financeItemm = await database.GetAll_FinanceItems();

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

        public async void OutPerMonth()
        {
            financeItemm = await database.GetAll_FinanceItems();
                        
            var today = DateTime.Today;
            var todayMonth = today.Month;
                        
            foreach (Finance_Items data in financeItemm)
            {
                var todayData = data.Datum;
                var todayMonthData = todayData.Month;

                costVydaj = Int32.Parse(data.Vydej);

                if (todayMonth == todayMonthData)
                {
                    if (data.Prijem == "0")
                    {
                        perMonth += costVydaj;
                    }

                    Za_Mesic.Content = "-" + perMonth + ",-Kč";
                }
            }
        }

        public async void OutPerYear()
        {
            financeItemm = await database.GetAll_FinanceItems();

            var today = DateTime.Today;
            var todayYear = today.Year;

            foreach (Finance_Items data in financeItemm)
            {
                var todayData = data.Datum;
                var todayYearData = todayData.Year;

                costVydaj = Int32.Parse(data.Vydej);

                if (todayYear == todayYearData)
                {
                    if (data.Prijem == "0")
                    {
                        perYear += costVydaj;
                    }

                    Za_Rok.Content = "-" + perYear + ",-Kč";
                }
            }
        }

        private void Vydaj_Click(object sender, RoutedEventArgs e)
        {
            BaseFrame.Content = new Vydaj_Page();
        }

        private void Prijem_Click(object sender, RoutedEventArgs e)
        {
            BaseFrame.Content = new Prijem_Page();
        }

        private void Planovany_Vydaj_Click(object sender, RoutedEventArgs e)
        {
            BaseFrame.Content = new PlanovanyVydaj_Page();
        }

        private void Kategorie_Click(object sender, RoutedEventArgs e)
        {
            BaseFrame.Content = new Kategorie_Page();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (click == 0)
            {
                Vydaj.Visibility = Visibility.Visible;
                Prijem.Visibility = Visibility.Visible;
                Planovany_Vydaj.Visibility = Visibility.Visible;
                Kategorie.Visibility = Visibility.Visible;

                click += 1;
            }
            else
            {
                Vydaj.Visibility = Visibility.Hidden;
                Prijem.Visibility = Visibility.Hidden;
                Planovany_Vydaj.Visibility = Visibility.Hidden;
                Kategorie.Visibility = Visibility.Hidden;

                click = 0;
            }

        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            BaseFrame.Content = new EditMain_Page();
        }
    }
}
