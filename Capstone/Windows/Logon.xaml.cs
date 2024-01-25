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

namespace Capstone.Windows
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Logon : Window
    {
        public Logon()
        {
            InitializeComponent();
            
        }

        private void LogonClicked(object sender, RoutedEventArgs e)
        {
            Sql db = new();
            string uName = usernameBox.Text;
            string pass = passwordBox.Text;

            User logingIn = new User();
            logingIn = db.getDB().Table<User>().FirstOrDefault(u=>u.getUName() == uName && u.getPassword() == pass);

            if (logingIn.Uid > 0) 
            {
            
            }
            else 
            {
                
            }
        }
    }
}
