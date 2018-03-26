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
    /// Interakční logika pro Kategorie_Page.xaml
    /// </summary>
    public partial class Kategorie_Page : Page
    {
        private int vyber;

        DatabaseConf database_kategorie = new DatabaseConf("Data.db3");

        List<Kategorie_Items> categoryItem = new List<Kategorie_Items>();
        ObservableCollection<string> categoryShow = new ObservableCollection<string>();

        List<int> categoryList = new List<int>();

        public Kategorie_Page()
        {
            InitializeComponent();

            categoryItem = database_kategorie.GetCategoryRowAsync_KategorieItems().Result;

            foreach (Kategorie_Items data in categoryItem)
            {
                categoryShow.Add(data.Kategorie);
                categoryList.Add(data.ID);
            }

            Zobrazeni_Kategorii.ItemsSource = categoryShow;
        }

        private async void Vytvor_Kategorie_Click(object sender, RoutedEventArgs e)
        {
            string categoryNameEntry = Nova_Kategorie.Text;

            Kategorie_Items entry = new Kategorie_Items() { Kategorie = categoryNameEntry };

            await database_kategorie.SaveItemAsync_KategorieItems(entry);
        }

        private async void Smaz_Katerogie_Click(object sender, RoutedEventArgs e)
        {
            int test = categoryList[vyber];

            await database_kategorie.DeleteCategoryAsync_CategoryItems(test);
        }        

        private void Ulozit_Click(object sender, RoutedEventArgs e)
        {
            FromCategory.Content = new MainPage();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            FromCategory.Content = new MainPage();
        }

        private void Zobrazeni_Kategorii_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            vyber = Zobrazeni_Kategorii.SelectedIndex;
        }
    }
}
