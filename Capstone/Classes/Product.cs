using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace Capstone.Classes
{
    class Product
    {
        public BindingList<Part> associatedParts { get; private set; }
        public int productID { get; set; }
        public string name { get; set; }
        public decimal price { get; set; } 
        public int inStock { get; set; }
        public int min {  get; set; }
        public int max { get; set; }

        // Constructor
        public Product(BindingList<Part> blPart, int proId, string n, decimal p, int stock, int inMin, int inMax)
        {
            this.productID = proId;
            this.associatedParts = blPart;
            this.name = n;
            this.price = p;
            this.inStock = stock;
            this.min = inMin;
            this.max = inMax;

        }

        // Methods

        public void addAssociatedPart(Part x)
        {
            this.associatedParts.Add(x);
        }

        public bool removeAssociatedPart(int x)
        {
            
            Part partToRemove = this.associatedParts.FirstOrDefault(p => p.partID == x);
            return this.associatedParts.Remove(partToRemove);
        }

        public Part lookupAssoicatedPart(int x)
        {

            return this.associatedParts.FirstOrDefault(p => p.partID == x);      
        }
    }
}
