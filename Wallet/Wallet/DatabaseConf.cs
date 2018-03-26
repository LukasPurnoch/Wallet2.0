using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace Wallet
{
    class DatabaseConf
    {
        private SQLiteAsyncConnection database;

        public DatabaseConf(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<Kategorie_Items>().Wait();
            database.CreateTableAsync<Finance_Items>().Wait();
            database.CreateTableAsync<Dluhy_Items>().Wait();
        }
        public Task<int> SaveItemAsync_KategorieItems(Kategorie_Items item)
        {
            if (item.ID != 0)
            {
                return database.UpdateAsync(item);
            }
            else
            {
                return database.InsertAsync(item);
            }
        }
        public Task<int> SaveItemAsync_FinanceItems(Finance_Items item)
        {
            if (item.ID != 0)
            {
                return database.UpdateAsync(item);
            }
            else
            {
                return database.InsertAsync(item);
            }
        }
        public Task<int> SaveItemAsync_DluhyItems(Dluhy_Items item)
        {
            var result = GetAll_DluhyItems().Result;

            foreach (Dluhy_Items dluh in result)
            {
                if (item.ID == dluh.ID)
                {
                    return database.UpdateAsync(item);
                }
            }

            return database.InsertAsync(item);
        }

        public Task<List<Kategorie_Items>> GetCategoryRowAsync_KategorieItems()
        {
            return database.QueryAsync<Kategorie_Items>("SELECT * FROM [Kategorie_Items] ORDER BY ID DESC");
        }
        public Task<List<Finance_Items>> GetCategoryRowAsync_FinanceItems()
        {
            return database.QueryAsync<Finance_Items>("SELECT * FROM [Finance_Items] ORDER BY ID DESC");
        }

        public Task<List<Finance_Items>> GetAll_FinanceItems()
        {
            return database.QueryAsync<Finance_Items>("SELECT * FROM [Finance_Items] ORDER BY ID DESC");
        }

        public Task<List<Dluhy_Items>> GetAll_DluhyItems()
        {
            return database.QueryAsync<Dluhy_Items>("SELECT * FROM [Dluhy_Items] ORDER BY ID DESC");
        }

        public Task<List<Dluhy_Items>> GetCategoryRowAsync_DluhyItems()
        {
            return database.QueryAsync<Dluhy_Items>("SELECT * FROM [Dluhy_Items] ORDER BY ID DESC");
        }

        public Task<List<Dluhy_Items>> GetDatesAsync_DluhyItems()
        {
            return database.QueryAsync<Dluhy_Items>("SELECT * FROM [Dluhy_Items] ORDER BY ID DESC");
        }

        /*public Task<List<Dluhy_Items>> UpdateProdlouzeniAsync_DluhyItems(DateTime prodlouzeni, int id)
        {
            return database.QueryAsync<Dluhy_Items>("UPDATE [Dluhy_Items] SET KonecPujcky = '" + prodlouzeni + "' WHERE [ID] = '" + id + "' ");
        }

        public Task<List<Dluhy_Items>> UpdateNavyseniAsync_DluhyItems(string navyseni, int id)
        {
            return database.QueryAsync<Dluhy_Items>("UPDATE [Dluhy_Items] SET VysePujcky = '" + navyseni + "' WHERE [ID] = '" + id + "' ");
        }*/

        public Task<List<Kategorie_Items>> DeleteCategoryAsync_CategoryItems(int entry)
        {
            return database.QueryAsync<Kategorie_Items>("DELETE FROM [Kategorie_Items] WHERE[ID] = '" + entry + "' ");
        }

        public Task<List<Finance_Items>> DeleteEntryAsync_FinanceItems(int entry)
        {
            return database.QueryAsync<Finance_Items>("DELETE FROM [Finance_Items] WHERE[ID] = '" + entry + "' ");
        }

        public Task<List<Dluhy_Items>> DeleteLoanAsync_DluhyItems(int entry)
        {
            return database.QueryAsync<Dluhy_Items>("DELETE FROM [Dluhy_Items] WHERE[ID] = '" + entry + "' ");
        }
    }
}
