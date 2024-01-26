using SQLite;
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
        //public BindingList<Part> associatedParts { get; private set; }
        public string associatedParts { get; set; }
        [PrimaryKey]
        public int productID { get; set; }
        public string name { get; set; }
        public decimal price { get; set; } 
        public int inStock { get; set; }
        public int min {  get; set; }
        public int max { get; set; }

        // Constructor
        public Product(string partIdList, int proId, string n, decimal p, int stock, int inMin, int inMax)
        {
            this.productID = proId;
            this.associatedParts = partIdList;
            this.name = n;
            this.price = p;
            this.inStock = stock;
            this.min = inMin;
            this.max = inMax;

        }
        public Product()
        {
            this.productID = 0;
            this.associatedParts = "";
            this.name = "";
            this.price= 0.0M;
            this.inStock = 0;
            this.min = 0;
            this.max = 0;
        }
        // Methods

        public void addAssociatedPart(Part x)
        {
            //this.associatedParts.Add(x);
            if(this.associatedParts.Length > 0)
            {
                string id = ",";
                id = id + x.partID.ToString();
                this.associatedParts += id;
            }
            else
            {
                this.associatedParts = x.partID.ToString();
            }
        }

        public bool removeAssociatedPart(int x)
        {
            string toReplace = x.ToString();
            string[] list = this.associatedParts.Split(',');
           
            foreach (string id in list)
            {
                if(id == toReplace)
                {
                    if (this.associatedParts.Length > 1)
                    {
                        toReplace = "," + toReplace;
                        this.associatedParts = this.associatedParts.Replace(toReplace, "");
                        return true;
                    }
                    else
                    {
                        this.associatedParts = "";
                        return true;
                    }
                }
            }
            return false;
            
        }

        public Part lookupAssoicatedPart(int x)
        {
            Sql db = new();

            return db.GetPart(x);
                  
        }
        public BindingList<Part> GetAssociatedParts()
        {
            string[] pids = this.associatedParts.Split(",");
            BindingList<Part> blParts = new BindingList<Part>();

            foreach (string id in pids)
            {
                blParts.Add(lookupAssoicatedPart(int.Parse(id)));
            }
            return blParts;
        }

        public static string PartsListToString(BindingList<Part> blParts)
        {
            string pidList = "";
            foreach (Part p in blParts)
            {
                if (pidList.Length > 0)
                {
                    string toAdd = "," + p.partID.ToString();
                    pidList += toAdd;
                }
                else
                {
                    pidList = p.partID.ToString();
                }
            }
            return pidList;
        }
    }
}
