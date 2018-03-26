using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interakční logika pro Vydaj_Page.xaml
    /// </summary>
    public partial class Vydaj_Page : Page
    {
        private string _cena;
        private DateTime _datum;
        private string _utracenoZa;
        private string _selectedCategory = "None";

        private string _zustatek = "0";

        DatabaseConf database = new DatabaseConf("Data.db3");

        List<Kategorie_Items> categoryItem = new List<Kategorie_Items>();
        ObservableCollection<string> categoryShow = new ObservableCollection<string>();

        List<Finance_Items> financeItem = new List<Finance_Items>();
        List<string> financeShow = new List<string>();

        public Vydaj_Page()
        {
            InitializeComponent();

            categoryItem = database.GetCategoryRowAsync_KategorieItems().Result;

            foreach (Kategorie_Items data in categoryItem)
            {
                categoryShow.Add(data.Kategorie);
            }

            financeItem = database.GetCategoryRowAsync_FinanceItems().Result;

            foreach (Finance_Items data in financeItem)
            {
                financeShow.Add(data.Zustatek);
                _zustatek = financeShow[0];
            }

            Kategorie.ItemsSource = categoryShow;
        }

        private async void Uloz_Vydaj_Click(object sender, RoutedEventArgs e)
        {
            _cena = Vydana_Castka.Text;
            _utracenoZa = UtracenoZa.Text;
            
            if (Kalendar.SelectedDate.HasValue)
            {
                _datum = Kalendar.SelectedDate.Value;
            }
            else
            {
                _datum = DateTime.Now.Date;
            }

            int.TryParse(_zustatek, out int _zustatekF);
            int.TryParse(_cena, out int _cenaF);

            _zustatekF -= _cenaF;

            _zustatek = _zustatekF.ToString();

            Finance_Items entry1 = new Finance_Items() { Zustatek = _zustatek, Prijem = "0", Vydej = _cena, Nazev = _utracenoZa, Kategorie = _selectedCategory, Datum = _datum };

            await database.SaveItemAsync_FinanceItems(entry1);

            FromVydaj.Content = new MainPage();
        }

        private void Kategorie_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedCategory = Kategorie.SelectedItem.ToString();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            FromVydaj.Content = new MainPage();
        }
    }
}
