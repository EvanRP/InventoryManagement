using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Capstone.Classes;
using System.ComponentModel;

namespace Capstone.Windows
{
    /// <summary>
    /// Interaction logic for Report.xaml
    /// </summary>
    public partial class Report : Window
    {
        public Report()
        {
            int orderMoreBy = 5;
            Sql db = new();
            InitializeComponent();
            DateTime dt = DateTime.Now;

            // get inventory
            BindingList<Part> partInv = db.GetParts();
            BindingList<Product> proInv = db.GetProducts();

            // main stack panel
            StackPanel mainStack = new();
            
            // report section titles
            Label partsLabel = new Label()
            {
                Content = "Parts With Low Inventory" + "    " + dt.ToString("dd-MMM-yyyy HH:mm")
            };
            Label productLabel = new Label()
            {
                Content = "Products With Low Inventory" + "    " + dt.ToString("dd-MMM-yyyy HH:mm")
            };

            // add part label to main stack and change the alignment of the section titles
            mainStack.Children.Add(partsLabel);
            partsLabel.HorizontalAlignment = HorizontalAlignment.Center;
            productLabel.HorizontalAlignment = HorizontalAlignment.Center;

            // create part columns
            StackPanel partNameCol = new()
            {
                Orientation = Orientation.Vertical,
                Margin = new Thickness(10, 5, 10, 5)
            };
            StackPanel partInStockCol = new()
            {
                Orientation = Orientation.Vertical,
                Margin = new Thickness(10, 5, 10, 5)
            };
            StackPanel partMinStockCol = new()
            {
                Orientation = Orientation.Vertical,
                Margin = new Thickness(10, 5, 10, 5)
            };

            // create product columns
            StackPanel proNameCol = new()
            {
                Orientation = Orientation.Vertical,
                Margin = new Thickness(10, 5, 10, 5)
            };
            StackPanel proInStockCol = new()
            {
                Orientation = Orientation.Vertical,
                Margin = new Thickness(10, 5, 10, 5)
            };
            StackPanel proMinStockCol = new()
            {
                Orientation = Orientation.Vertical,
                Margin = new Thickness(10, 5, 10, 5)
            };

            // create stack panel that will hold part columns
            StackPanel partReport = new()
            {
                Orientation = Orientation.Horizontal,
                Margin = new Thickness(10, 10, 10, 10)
            };
            // create stack panel that will hold product columns
            StackPanel productReport = new()
            {
                Orientation = Orientation.Horizontal,
                Margin = new Thickness(10, 10, 10, 10)
            };

            // create borders for reports
            Border productBorder = new Border()
            {
                BorderBrush = Brushes.Black,
                BorderThickness = new Thickness(1)
            };

            Border partBorder = new Border()
            {
                BorderBrush = Brushes.Black,
                BorderThickness = new Thickness(1)
            };
            

            // create column titles

            TextBlock nameTitle = new TextBlock
            {
                Text = "Part Name",
                TextDecorations = TextDecorations.Underline
            };
            TextBlock inStockTitle = new TextBlock
            {
                Text = "Current Stock",
                TextDecorations = TextDecorations.Underline
            };
            TextBlock minStockTitle = new TextBlock
            {
                Text = "Minimum Required",
                TextDecorations = TextDecorations.Underline
            };
            partNameCol.Children.Add(nameTitle);
            partInStockCol.Children.Add(inStockTitle);
            partMinStockCol.Children.Add(minStockTitle);

            // fill columns
            foreach(Part p in partInv)
            {
                if(p.inStock - orderMoreBy <= p.min)
                {
                    Label partName = new Label()
                    {
                        Content = p.name
                    };
                    Label inStockAmount = new Label()
                    {
                        Content = p.inStock
                    };
                    Label minStockAmount = new Label()
                    {
                        Content = p.min
                    };

                    partNameCol.Children.Add(partName);
                    partInStockCol.Children.Add(inStockAmount);
                    partMinStockCol.Children.Add(minStockAmount);
                }
            }
            // add columns to report stack
            partReport.Children.Add(partNameCol);
            partReport.Children.Add(partInStockCol);
            partReport.Children.Add(partMinStockCol);
            // put border around report
            partBorder.Child = partReport;
            // put part report on main stack
            mainStack.Children.Add(partBorder);

            // create product report column titles
            TextBlock productNameTitle = new TextBlock
            {
                Text = "Product Name",
                TextDecorations = TextDecorations.Underline
            };
            TextBlock productInStockTitle = new TextBlock
            {
                Text = "Current Stock",
                TextDecorations = TextDecorations.Underline
            };
            TextBlock productMinStockTitle = new TextBlock
            {
                Text = "Minimum Required",
                TextDecorations = TextDecorations.Underline
            };
            // add titles to columns
            proNameCol.Children.Add(productNameTitle);
            proInStockCol.Children.Add(productInStockTitle);
            proMinStockCol.Children.Add(productMinStockTitle);
            // put product report title on main stack
            mainStack.Children.Add(productLabel);
            // fill product report columns
            foreach(Product p in proInv)
            {
                if (p.inStock - orderMoreBy <= p.min)
                {
                    Label partName = new Label()
                    {
                        Content = p.name
                    };
                    Label inStockAmount = new Label()
                    {
                        Content = p.inStock
                    };
                    Label minStockAmount = new Label()
                    {
                        Content = p.min
                    };

                    proNameCol.Children.Add(partName);
                    proInStockCol.Children.Add(inStockAmount);
                    proMinStockCol.Children.Add(minStockAmount);
                }
            }
            //put product columns in report stack
            productReport.Children.Add(proNameCol);
            productReport.Children.Add(proInStockCol);
            productReport.Children.Add(proMinStockCol);
            // add border to product report stack
            productBorder.Child = productReport;
            // put product report on the main stack
            mainStack.Children.Add(productBorder);
            // add main stack to scroll view
            sView.Content = mainStack;
        }

    }
}
