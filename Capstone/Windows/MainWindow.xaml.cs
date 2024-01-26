using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.ComponentModel;
using System.IO;
using System.Windows.Controls;
using Capstone.Classes;

namespace Capstone.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // varaiablies for modify
        Part selectedPart;
        Product selectedProduct;
        SharedData share = (Application.Current as App).Shared;
        Inventory inv;

        // Pages
        

        public void readData(string dataPath)
        {
            BindingList<Part> aParts = new BindingList<Part>();
            BindingList<Product> aProducts = new BindingList<Product>();
            List<int> inHouseIDs = new List<int>();
            List<int> sourcedIDs = new List<int>();
            string[] sections = File.ReadAllLines(dataPath);

            inv = new Inventory(aProducts, aParts);
            share.inv = inv;
            share.inHouseIDs = inHouseIDs;
            share.outSourcedIDs = sourcedIDs;

            foreach (string section in sections)
            {
                string[] sec = section.Split(',');
                //check is line is a part
                if (sec.Length == 8)
                {
                    //check is part is outsourced
                    if (sec[6] == "N/A")
                    {
                        Outsourced o = new Outsourced(int.Parse(sec[0]), sec[1], decimal.Parse(sec[2]), int.Parse(sec[3]), int.Parse(sec[4]), int.Parse(sec[5]), sec[7]);
                        //Part change = o as Part;
                        aParts.Add(o) ;
                        sourcedIDs.Add(int.Parse(sec[0]));
                    }
                    //check is part is inhouse
                    else if (sec[7] == "N/A")
                    {
                        Inhouse i = new Inhouse(int.Parse(sec[0]), sec[1], decimal.Parse(sec[2]), int.Parse(sec[3]), int.Parse(sec[4]), int.Parse(sec[5]), int.Parse(sec[6]));
                        //Part change = i as Part;
                        aParts.Add(i);
                        //inHouseIDs.Add(i.partID);
                        inHouseIDs.Add(i.partID);
                    }
                    
                    else
                    {
                        //Error
                    }
                }
                else if(sec.Length == 7)
                {
                    // line is a Product
                    //Split part ids
                    string[] proParts = sec[0].Split(';');
                    //create part bindingList
                    BindingList<Part> proPartsList = new BindingList<Part>();
                    for (int q = 0; q < proParts.Count(); q++)
                    {
                        string s = proParts[q];
                        bool isInt = int.TryParse(s, out _);
                        if (isInt)
                        {
                            int pid = int.Parse(proParts[q]);
                            Part par = share.inv.lookupPart(pid);
                            proPartsList.Add(par);
                        }
                        else
                        {
                            Console.WriteLine(s);
                        }

                    }
                    Product pro = new Product(proPartsList, int.Parse(sec[1]), sec[2], decimal.Parse(sec[3]), int.Parse(sec[4]), int.Parse(sec[5]), int.Parse(sec[6]));
                    share.inv.addProduct(pro);
                }
                
            }

            
        }

        public void writeData(string dataPath)
        {
            try
            {
                using (StreamWriter saveData = new StreamWriter(dataPath, false, Encoding.UTF8))
                {
                    
                    foreach (Part p in share.inv.allParts)
                    {
                        bool isDone = false;
                        if (!isDone)
                        {
                            for (int a = 0; a < share.inHouseIDs.Count(); a++)
                            {
                                // write Inhouse parts
                                if (p.partID == share.inHouseIDs[a])
                                {
                                    Inhouse i = p as Inhouse;
                                    saveData.WriteLine($"{i.partID},{i.name},{i.price},{i.inStock},{i.min},{i.max},{i.machineID},N/A");
                                    isDone = true;
                                    break;
                                }
                            }
                        }
                        if (!isDone)
                        {
                            for (int a = 0; a < share.outSourcedIDs.Count(); a++)
                            {
                                // Write Outsourced Parts
                                if (p.partID == share.outSourcedIDs[a])
                                {
                                    Outsourced o = p as Outsourced;
                                    saveData.WriteLine($"{o.partID},{o.name},{o.price},{o.inStock},{o.min},{o.max},N/A,{o.companyName}");
                                    isDone = true;
                                    break;
                                }
                            }
                        }
                    }
                    // Write Products
                    foreach(Product p in share.inv.allProducts)
                    {
                        // get list of part ids
                        string l = "";
                        for(int i = 0;i < p.associatedParts.Count(); i++)
                        {
                            if (i == p.associatedParts.Count() - 1)
                            {
                                l += p.associatedParts[i].partID.ToString();
                            }
                            else
                            {
                                l += p.associatedParts[i].partID.ToString() + ";";
                            }
                            
                        }
                        saveData.WriteLine($"{l},{p.productID},{p.name},{p.price},{p.inStock},{p.min},{p.max}");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);

            }
        }
        

        public MainWindow()
        {
            //if (inv == null)
            //{
            //    readData(@"..\\..\\..\\Data.txt");
            //}
 
            InitializeComponent();
            //PartsTable.ItemsSource = share.inv.allParts;
            //ProductTable.ItemsSource = share.inv.allProducts;
        }
       
        //Page Buttons
        private void ExitClicked(object sender, RoutedEventArgs e)
        {
            //writeData(@"..\\..\\..\\Data.txt");
            this.Close();
        }

        private void partSearchClicked(object sender, RoutedEventArgs e)
        {
            string s = partTextBox.Text;
            DataGrid dg = PartsTable;
           
            // check if search box is empty
            if (string.IsNullOrEmpty(s))
            {
                dg.ItemsSource = share.inv.allParts;
                return;
            }
            // if text box alphabetic see if part name contains string
            else if (!int.TryParse(s, out _))
            {
                var searchResults = share.inv.allParts.Where(p => p.name.Contains(s));
                dg.ItemsSource = searchResults;
                return;
            }
            // if text box is numeric check if part id equals string
            else if (int.TryParse(s, out _))
            {
                var searchResults = share.inv.allParts.Where(p => p.partID == int.Parse(s));
                dg.ItemsSource = searchResults;
                return;
            }
        }

        private void productSearchClicked(object sender, RoutedEventArgs e)
        {
            string s = productTextBox.Text;
            DataGrid dg = ProductTable;

            // check if search box is empty
            if (string.IsNullOrEmpty(s))
            {
                dg.ItemsSource = share.inv.allProducts;
                return;
            }
            // if text box is alphabetic check if product name equals string
            else if (!int.TryParse(s, out _))
            {
                var searchResults = share.inv.allProducts.Where(p => p.name.Contains(s));
                dg.ItemsSource = searchResults;
                return;
            }
            // if text box is numeric check if product id equals string
            else if (int.TryParse(s, out _))
            {
                var searchResults = share.inv.allProducts.Where(p => p.productID == int.Parse(s));
                dg.ItemsSource = searchResults;
                return;
            }
        }

        //Parts buttons
        private void addPartClick(object sender, RoutedEventArgs e)
        {
            PartAdd addPart = new PartAdd();
            addPart.ShowDialog();
        }
        private void modPartClick(object sender, RoutedEventArgs e) 
        {
            // if a part is selected open window
            if(selectedPart != null)
            {
                PartModify modding = new PartModify(selectedPart.partID);
                modding.ShowDialog();


            }
            // if no selected part error
            else
            {
                MessageBox.Show($"Please select a part from the Parts table.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
            
        }

        private void delPartClick(object sender, RoutedEventArgs e)
        {
            // if part selected call inventory method delete part
            // if dsuccess true part deleted
            if (selectedPart != null)
            {
                share.inv.deletePart(selectedPart);
               
            }
            else
            {
                MessageBox.Show($"Please select a part from the Parts table.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //Product Buttons

        private void addProductClick(object sender, RoutedEventArgs e)
        {
            ProductAdd proAdd = new ProductAdd();
            proAdd.ShowDialog();
        }

        private void modProductClick(object sender, RoutedEventArgs e)
        {
            // if there is a selected product opens product mod window
            if (selectedProduct !=  null) 
            {
                ProductModify proM = new ProductModify(selectedProduct);
                proM.ShowDialog();
            }
            else
            {
                MessageBox.Show($"Please select a product from the Product table.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }

        private void delProductClick(object sender, RoutedEventArgs e)
        {
            // if there is a product selected 
            // call inventory method removeProduct
            // if dsuccess is true product was deleted
            if (selectedProduct != null)
            {   
                    share.inv.removeProduct(selectedProduct.productID);
            }
        }

        private void selectedPartRow(object sender, RoutedEventArgs e)
        {
            selectedPart = (Part)PartsTable.SelectedItem;
        }

        private void selectedProductRow(object sender, RoutedEventArgs e)
        {
            selectedProduct = (Product)ProductTable.SelectedItem;
        }
    }
}
