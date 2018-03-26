using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace Wallet
{
    class Kategorie_Items
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Kategorie { get; set; }
    }
}
