using FriendOrganizerUI.ViewModel;
using MahApps.Metro.Controls;
using System.Windows;

namespace FriendOrganizerUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private MainWindowViewModel _vm;

        public MainWindow(MainWindowViewModel vm)
        {
            InitializeComponent();
            _vm = vm;
            DataContext = _vm;
            Loaded += MainWindow_Loaded;
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await _vm.LoadAsync(); 
        }
    }
}
