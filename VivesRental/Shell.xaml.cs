using MahApps.Metro.Controls;
using VivesRental.ViewModels;

namespace VivesRental
{
    /// <summary>
    ///     Interaction logic for Shell.xaml
    /// </summary>
    public partial class Shell : MetroWindow
    {
        public Shell(ShellViewModel viewModel)
        {
            InitializeComponent();
            Loaded += (sender, args) => { DataContext = viewModel; };
        }
    }
}