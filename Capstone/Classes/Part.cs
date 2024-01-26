using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace Capstone.Classes
{
    abstract class Part
    {
        [PrimaryKey]
        public int partID { get; set; }
        public string name { get; set; }
        public decimal price { get; set; }
        public int inStock { get; set; }
        public int min { get; set; }
        public int max { get; set; }

    }
}
