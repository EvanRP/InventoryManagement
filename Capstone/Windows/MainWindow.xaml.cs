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
        
        public MainWindow()
        {
            Sql db = new();

            if (inv == null)
            {
                inv = new Inventory(db.GetProducts(), db.GetParts());
                share.inv = inv;
            }
            
            InitializeComponent();
            PartsTable.ItemsSource = share.inv.allParts;
            ProductTable.ItemsSource = share.inv.allProducts;
        }
       
        //Page Buttons
        private void ExitClicked(object sender, RoutedEventArgs e)
        {
           
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
                bool del = share.inv.removeProduct(selectedProduct.productID);

                if( !del )
                {
                    MessageBox.Show($"Product not deleted.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
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
