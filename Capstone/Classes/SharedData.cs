using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.Classes
{
    internal class SharedData 
    {
        public Inventory inv { get; set; }
        public List<int> inHouseIDs { get; set; }
        public List<int> outSourcedIDs { get; set; }

        public int lastPartId { get; set; }

        public int lastProductId { get; set; }


    }
}
