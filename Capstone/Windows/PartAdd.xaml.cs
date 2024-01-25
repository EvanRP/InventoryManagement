using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Capstone.Classes;


namespace Capstone.Windows
{
    /// <summary>
    /// Interaction logic for PartAdd.xaml
    /// </summary>
    public partial class PartAdd : Window
    {
        SharedData share = (Application.Current as App).Shared;
        Inventory mainInv;
        int iD;
        string name;
        decimal price;
        int stock;
        int min;
        int max;
        bool isIn;
        int machineID;
        string cName;

        private void checkNull()
        {
            bool allBoxes =
               !string.IsNullOrEmpty(partTextBox.Text) &&
               !string.IsNullOrEmpty(nameTextBox.Text) &&
               !string.IsNullOrEmpty(priceTextBox.Text) &&
               !string.IsNullOrEmpty(inventoryTextBox.Text) &&
               !string.IsNullOrEmpty(minTextBox.Text) &&
               !string.IsNullOrEmpty(maxTextBox.Text);
            if (isIn)
            {
                bool inBoxes = allBoxes && !string.IsNullOrEmpty(machineTextBox.Text);
                if (inBoxes)
                {
                    save.IsEnabled = true;
                }
                else
                {
                    save.IsEnabled = false;
                }
            }
            else
            {
                bool outBoxes = allBoxes && !string.IsNullOrEmpty(companyTextBox.Text);
                if (outBoxes)
                {
                    save.IsEnabled = true;
                }
                else
                {
                    save.IsEnabled = false;
                }
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
            //Same as NumericTextBox but allows decimal points
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
        public PartAdd()
        {
            mainInv = share.inv;
            if (share.lastPartId == 0)
            {
                int x = 0;
                foreach (Part p in share.inv.allParts)
                {
                    if (p.partID > x)
                    {
                        x = p.partID;
                    }
                }
                share.lastPartId = x;
            }
            share.lastPartId += 1;
            
            InitializeComponent();
            partTextBox.Text = share.lastPartId.ToString();
        }

        private void inHouseChecked(object sender, RoutedEventArgs e)
        {
            isIn = true;

            machineInputLabel.Visibility = Visibility.Visible;
            machineTextBox.Visibility = Visibility.Visible;
            companyInputLabel.Visibility = Visibility.Hidden;
            companyTextBox.Visibility = Visibility.Hidden;
            checkNull();
        }

        private void outSourcedChecked(object sender, RoutedEventArgs e)
        {
            isIn = false;
            machineInputLabel.Visibility = Visibility.Hidden;
            machineTextBox.Visibility = Visibility.Hidden;
            companyInputLabel.Visibility = Visibility.Visible;
            companyTextBox.Visibility = Visibility.Visible;
            checkNull();
        }

        private void saveClicked(object sender, RoutedEventArgs e)
        {
            iD = int.Parse(partTextBox.Text);
            name = nameTextBox.Text;
            stock = int.Parse(inventoryTextBox.Text);
            max = int.Parse(maxTextBox.Text);
            min = int.Parse(minTextBox.Text);
            price = decimal.Parse(priceTextBox.Text);

            // if inHouse is checked create new part and update part
            // adds ID to parts Id list
            if (min <= max && stock >= min && stock <= max)
            {
                if (isIn)
                {
                    machineID = int.Parse(machineTextBox.Text);
                    Inhouse n = new Inhouse(iD, name, price, stock, min, max, machineID);
                    mainInv.updatePart(iD, n);
                    share.inHouseIDs.Add(iD);
                    this.Close();
                }
                // if OutSourced is checked create new part and update part
                // adds ID to parts Id list
                else
                {
                    cName = companyTextBox.Text;
                    Outsourced n = new Outsourced(iD, name, price, stock, min, max, cName);
                    mainInv.updatePart(iD, n);
                    share.outSourcedIDs.Add(iD);
                    this.Close();
                }
            }
            else if (max < min)
            {
                MessageBox.Show($"Min is greater than Max.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else if (stock < min)
            {
                MessageBox.Show($"Inventory is less than min.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
                MessageBox.Show($"Inventory is greater than max.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }


        }

        private void closeClicked(object sender, RoutedEventArgs e) 
        {
            share.lastPartId -= 1;
            this.Close();
        }
    }

    
}
