using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.ComponentModel;
using Capstone.Classes;

namespace Capstone.Windows
{
    /// <summary>
    /// Interaction logic for ProductAdd.xaml
    /// </summary>
    public partial class ProductAdd : Window
    {
        BindingList<Part> partsList;
        SharedData share = (Application.Current as App).Shared;
        Part selectedAddPart;
        Part selectedProductPart;

        public ProductAdd()
        {
            //Get Product Id
            if (share.lastProductId == 0)
            {
                int x = 0;
                foreach (Product p in share.inv.allProducts)
                {
                    if (p.productID > x)
                    {
                        x = p.productID;
                    }
                }
                share.lastProductId = x;
            }
            share.lastProductId += 1;

            InitializeComponent();

            IdTextBox.Text = share.lastProductId.ToString();
            partsList = new BindingList<Part>();

            PartsTable.ItemsSource = share.inv.allParts;
            PartsTable_Copy.ItemsSource = partsList;
           
        }

        private void checkNull()
        {
            //Checks if Boxes are empty
            bool allBoxes =
               !string.IsNullOrEmpty(IdTextBox.Text) &&
               !string.IsNullOrEmpty(NameTextBox.Text) &&
               !string.IsNullOrEmpty(InventoryTextBox.Text) &&
               !string.IsNullOrEmpty(PriceTextBox.Text) &&
               !string.IsNullOrEmpty(MaxTextBox.Text) &&
               !string.IsNullOrEmpty(MinTextBox.Text);
            //Enable or disables save button based in allBoxes
            if (allBoxes)
            {
                saveButton.IsEnabled = true;
            }
            else
            {
                saveButton.IsEnabled = false;
            }
        }

        private void isBoxNull(object sender, TextChangedEventArgs e)
        {
            checkNull();
        }
        private void NumericTextBox(object sender, TextCompositionEventArgs e)
        {
            TextBox s = sender as TextBox;
            if (!int.TryParse(e.Text, out _))
            {
                // Prevent non numeric text
                e.Handled = true;
                MessageBox.Show($"{s.Name} requires numeric input.", "Error", MessageBoxButton.OK);
            }
        }

        private void decTextBox(object sender, TextCompositionEventArgs e)
        {
            TextBox s = sender as TextBox;
            if (e.Text == "." || double.TryParse(e.Text, out _))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
                MessageBox.Show($"{s.Name} requires numeric input or a '.'.", "Error", MessageBoxButton.OK);
            }
        }

        private void searchClicked(object sender, RoutedEventArgs e)
        {
            string s = searchTextBox.Text;
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
        private void selectedAddPartRow(object sender, RoutedEventArgs e)
        {
            selectedAddPart = (Part)PartsTable.SelectedItem;
        }
        private void selectedPartRow(object sender, RoutedEventArgs e)
        {
            selectedProductPart = (Part)PartsTable_Copy.SelectedItem;
        }

        private void addPartClick(object sender, RoutedEventArgs e)
        {
            // if a part is selected add part to products parts list
            if(selectedAddPart != null)
            {
                partsList.Add(selectedAddPart);
                checkNull();
            }
            else
            {
                MessageBox.Show($"Please select a part from the top table.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }

        private void delClicked(object sender, RoutedEventArgs e)
        {
            MessageBoxResult A = MessageBox.Show($"Are you sure you want to delete {selectedProductPart.name} from the associated parts list?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (A == MessageBoxResult.Yes)
            {
                if (selectedProductPart != null)
                {
                    partsList.Remove(selectedProductPart);
                }
                else
                {
                    MessageBox.Show($"Please select a part from the bottom table.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void saveClicked(object sender, RoutedEventArgs e)
        {
            int min = int.Parse(MinTextBox.Text);
            int max = int.Parse(MaxTextBox.Text);
            int stock = int.Parse(InventoryTextBox.Text);
            Product x = share.inv.allProducts.FirstOrDefault(x => x.productID == int.Parse(IdTextBox.Text), null);
            if (min <= max && stock <= max && stock >= min)
            {
                
                    
                //Create new Product and add it to inventory
                Product p = new Product(partsList, int.Parse(IdTextBox.Text),NameTextBox.Text, decimal.Parse(PriceTextBox.Text), int.Parse(InventoryTextBox.Text), int.Parse(MinTextBox.Text), int.Parse(MaxTextBox.Text));
                share.inv.addProduct(p);
                this.Close();
                
            }
            else if (max < min)
            {
                MessageBox.Show($"Min is greater than Max", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else if (stock < min)
            {
                MessageBox.Show($"Stock is less than min", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
                MessageBox.Show($"Stock is greater than max.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void closeClicked(object sender, RoutedEventArgs e)
        {
            share.lastProductId -= 1;
            this.Close();
        }
    }
}
