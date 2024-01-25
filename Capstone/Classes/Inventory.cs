using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Capstone.Classes
{
    internal class Inventory
    {
        public BindingList<Product> allProducts {  get; set; }

        public BindingList<Part> allParts { get; set; }

        // Constructor
        public Inventory(BindingList<Product> blProduct, BindingList<Part> blPart)
        {
            this.allProducts = blProduct;
            this.allParts = blPart;
        }

        //Methods 

        //Products
        public void addProduct(Product p)
        {
            this.allProducts.Add(p);
        }

        public bool removeProduct(int x)
        {
            Product toRemove = this.allProducts.FirstOrDefault(p => p.productID == x);
            //return this.allProducts.Remove(toRemove);
            if (toRemove.associatedParts.Count() == 0)
            {
                MessageBoxResult A = MessageBox.Show($"Are you sure you want to delete product {toRemove.name}?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (A == MessageBoxResult.Yes)
                {
                    return this.allProducts.Remove(toRemove);
                }
                else
                {
                    return false;
                }
            }
            else
            {
                MessageBox.Show($"Product {toRemove.name} can not be deleted because a part is assigned to it.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }


        }

        public Product lookupProduct(int x)
        {
            return this.allProducts.FirstOrDefault(p => p.productID == x,null);
        }

        public void updateProduct(int x, Product p)
        {
            Product toUpdate = this.allProducts.FirstOrDefault(thing => thing.productID == x);
            allProducts.Remove(toUpdate);
            allProducts.Add(p);
        }

       
        //Parts
        public void addPart(Part part)
        {
            allParts.Add(part);
        }

        public bool deletePart(Part p) 
        {
            SharedData share = (Application.Current as App).Shared;
            Product isPartOfProduct = this.allProducts.FirstOrDefault(t => t.associatedParts.Contains(p),null);
            MessageBoxResult A;
            if (isPartOfProduct != null)
            {
                //Get conformation that part is to be removed from product
                if(isPartOfProduct.associatedParts.Count() > 1)
                {
                     A = MessageBox.Show($"Are you sure you want to delete \n\nPart Number: {p.partID} \nPart Name: {p.name}.\n It is apart of Product {isPartOfProduct.name}.", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                }
                //Inform if part is last part of product
                else if(isPartOfProduct.associatedParts.Count() == 1)
                {
                     A = MessageBox.Show($"Are you sure you want to delete \n\nPart Number: {p.partID} \nPart Name: {p.name}\n It will leave Product {isPartOfProduct.name} without any parts.", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                }
                else
                {
                    //error
                    A = MessageBoxResult.No;
                }
                if(A ==  MessageBoxResult.Yes)
                {
                    //remove from product
                    //and remove partid from inhouse and outsoured id lists
                    isPartOfProduct.associatedParts.Remove(p);
                    if (share.inHouseIDs.Contains(p.partID))
                    {
                        share.inHouseIDs.Remove(p.partID);
                    }
                    else if (share.outSourcedIDs.Contains(p.partID))
                    {
                        share.outSourcedIDs.Remove(p.partID);
                    }
                    //return bool of of deleted part
                    return this.allParts.Remove(p);
                }
                else
                {
                    return false;
                }
            }
            else
            {
                A = MessageBox.Show($"Are you sure you want to delete \n\nPart Number: {p.partID} \nPart Name: {p.name}.", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if(A == MessageBoxResult.Yes)
                {
                    //remove from product
                    //and remove partid from inhouse and outsoured id lists
                    if (share.inHouseIDs.Contains(p.partID))
                    {
                        share.inHouseIDs.Remove(p.partID);
                    }
                    else if (share.outSourcedIDs.Contains(p.partID))
                    {
                        share.outSourcedIDs.Remove(p.partID);
                    }
                    //return bool of of deleted part
                    return this.allParts.Remove(p);
                }
                else
                {
                    return false;
                }
               
            }

        }

        public Part lookupPart(int x)
        {
            return this.allParts.FirstOrDefault(p => p.partID == x,null);
        }

        public void updatePart(int x, Part p)
        {
            Part toUpdate = this.allParts.FirstOrDefault(thing => thing.partID == x,null);
            Product isPartOfProduct = this.allProducts.FirstOrDefault(t => t.associatedParts.Contains(toUpdate), null);

            if (isPartOfProduct != null)
            {
                isPartOfProduct.associatedParts.Remove(toUpdate);
                isPartOfProduct.associatedParts.Add(p);
            }

            this.allParts.Remove(toUpdate);
            this.allParts.Add(p);
            
        }
    }
}
