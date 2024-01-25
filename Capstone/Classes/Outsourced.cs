using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Capstone.Classes
{
    class Outsourced : Part
    {
        public string companyName {  get; private set; }

        public void setCompanyName(string s)
        {
            this.companyName = s;
        }

        public Outsourced(int pID, string Name, decimal newPrice, int stock, int inMin, int inMax, string cName)
        {

            this.partID = pID;
            this.name = Name;
            this.price = newPrice;
            this.inStock = stock;
            this.min = inMin;
            this.max = inMax;
            this.companyName = cName;
        }
    }
}
