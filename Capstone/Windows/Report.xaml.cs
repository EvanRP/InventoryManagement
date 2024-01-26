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

            // get inventory
            BindingList<Part> partInv = db.GetParts();
            BindingList<Product> proInv = db.GetProducts();

            // main stack panel
            StackPanel mainStack = new();
            
            // report section titles
            Label partsLabel = new Label()
            {
                Content = "Parts With Low Inventory"
            };
            Label productLabel = new Label()
            {
                Content = "Products With Low Inventory"
            };

            mainStack.Children.Add(partsLabel);
            partsLabel.HorizontalAlignment = HorizontalAlignment.Center;
            productLabel.HorizontalAlignment = HorizontalAlignment.Center;

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

            StackPanel partReport = new()
            {
                Orientation = Orientation.Horizontal,
                Margin = new Thickness(10, 10, 10, 10)
            };
            StackPanel productReport = new()
            {
                Orientation = Orientation.Horizontal,
                Margin = new Thickness(10, 10, 10, 10)
            };

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
            

            // create colomn titles

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
            partReport.Children.Add(partNameCol);
            partReport.Children.Add(partInStockCol);
            partReport.Children.Add(partMinStockCol);

            partBorder.Child = partReport;

            mainStack.Children.Add(partBorder);

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

            proNameCol.Children.Add(productNameTitle);
            proInStockCol.Children.Add(productInStockTitle);
            proMinStockCol.Children.Add(productMinStockTitle);

            mainStack.Children.Add(productLabel);
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
            productReport.Children.Add(proNameCol);
            productReport.Children.Add(proInStockCol);
            productReport.Children.Add(proMinStockCol);

            productBorder.Child = productReport;
            mainStack.Children.Add(productBorder);

            sView.Content = mainStack;
        }

    }
}
