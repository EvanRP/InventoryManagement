using Capstone.Classes;
using System.Windows;

namespace Capstone
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        internal SharedData Shared { get; } = new SharedData();
    }
}
