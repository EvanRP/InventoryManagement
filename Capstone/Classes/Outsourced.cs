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
        public Outsourced()
        {
            this.partID=0;
            this.name = "";
            this.price = 0.0M;
            this.inStock = 0;
            this.min = 0;
            this.max = 0;
            this.companyName = "";
        }
    }
}
