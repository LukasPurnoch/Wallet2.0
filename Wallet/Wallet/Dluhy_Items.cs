using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace Wallet
{
    class Dluhy_Items
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Nazev { get; set; }
        public string VysePujcky { get; set; }
        public string Sazba { get; set; }
        public DateTime PocatekPujcky { get; set; }
        public DateTime KonecPujcky { get; set; }
    }
}
