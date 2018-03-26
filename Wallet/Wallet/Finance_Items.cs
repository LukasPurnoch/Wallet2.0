using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace Wallet
{
    class Finance_Items
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Nazev { get; set; }
        public string Zustatek { get; set; }
        public string Prijem { get; set; }
        public string Vydej { get; set; }
        public string Kategorie { get; set; }
        public DateTime Datum { get; set; }
    }
}
