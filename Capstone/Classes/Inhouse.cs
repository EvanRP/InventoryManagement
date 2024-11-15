﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.Classes
{
    class Inhouse : Part
    {
        public int machineID {  get; set; }

        public Inhouse(int pID, string Name, decimal newPrice, int stock, int inMin, int inMax, int mID)
        {
            this.partID = pID;
            this.name = Name;
            this.price = newPrice;
            this.inStock = stock;
            this.min = inMin;
            this.max = inMax;
            this.machineID = mID;
        }
    }
}
