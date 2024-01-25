using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Capstone.Classes;

namespace Capstone.Windows
{
    /// <summary>
    /// Interaction logic for PartModify.xaml
    /// </summary>
    public partial class PartModify : Window
    {
        SharedData share = (Application.Current as App).Shared;
        Part toMod;
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
        string oType;

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

        private void NumericTextBox(object sender, TextCompositionEventArgs e)
        {
            TextBox s = sender as TextBox;
            if (!int.TryParse(e.Text, out _))
            {
                e.Handled = true;
                MessageBox.Show($"{s.Name} requires numeric input.", "Error", MessageBoxButton.OK);
            }
        }

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

        

        public PartModify(int toModID)
        {

            mainInv = share.inv;
            toMod = mainInv.lookupPart(toModID);

            iD = toMod.partID;
            name = toMod.name;
            price = toMod.price;
            stock = toMod.inStock;
            min = toMod.min;
            max = toMod.max;

            InitializeComponent();

            partTextBox.Text = iD.ToString();
            nameTextBox.Text = name;
            inventoryTextBox.Text = stock.ToString();
            priceTextBox.Text = price.ToString();
            maxTextBox.Text = max.ToString();
            minTextBox.Text = min.ToString();

            // Figure out if Part is Inhouse ot OutSourced
            for (int index = 0; index < share.inHouseIDs.Count; index++)
            {

                if (iD == share.inHouseIDs[index])
                {
                    Inhouse i = toMod as Inhouse;
                    machineTextBox.Text = i.machineID.ToString();
                    inHouseRadio.IsChecked = true;
                    oType = "in";
                    break;
                }
                if (index == share.inHouseIDs.Count - 1)
                {
                    Outsourced o = toMod as Outsourced;
                    companyTextBox.Text = o.companyName.ToString();
                    outSourcedRadio.IsChecked = true;
                    oType = "out";
                }
            }


        }

        private void inHouseChecked(object sender, EventArgs e)
        {
            isIn = true;

            machineInputLabel.Visibility = Visibility.Visible;
            machineTextBox.Visibility = Visibility.Visible;
            companyInputLabel.Visibility = Visibility.Hidden;
            companyTextBox.Visibility = Visibility.Hidden;
            checkNull();
        }

        private void outSourcedChecked(object sender, EventArgs e)
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
            bool notFree = false;

            // if inHouse is checked create new part and update part
            // also is part typw was changed update partid lists
            if (min <= max && stock >= min && stock <= max)
            {
                if (isIn)
                {
                    machineID = int.Parse(machineTextBox.Text);
                    Inhouse n = new Inhouse(iD, name, price, stock, min, max, machineID);
                    mainInv.updatePart(iD, n);
                    if (oType != "in")
                    {
                        share.inHouseIDs.Add(iD);
                        share.outSourcedIDs.Remove(iD);
                    }
                    this.Close();
                }
                // if OutSourced is checked create new part and update part
                // also is part typw was changed update partid lists
                else
                {
                    cName = companyTextBox.Text;
                    Outsourced n = new Outsourced(iD, name, price, stock, min, max, cName);
                    mainInv.updatePart(iD, n);
                    if (oType != "out")
                    {
                        share.inHouseIDs.Remove(iD);
                        share.outSourcedIDs.Add(iD);
                    }
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
            this.Close();
        }

    }
}
