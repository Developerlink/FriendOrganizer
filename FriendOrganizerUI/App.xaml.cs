using Autofac;
using FriendOrganizerUI.Data;
using FriendOrganizerUI.Startup;
using FriendOrganizerUI.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace FriendOrganizerUI
{ 
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var bootstrapper = new Bootstrapper();
            var container = bootstrapper.Bootstrap();

            // Resolve will go to the MainWindow constructor and see that it needs a MainWindowViewModel
            // It then goes to the MainWindowViewModel and see that it needs an IFriendDataService interface
            // And it has been set up to provide an instance of FriendDataService whenever an IFriendDataService interface is needed
            var mainWindow = container.Resolve<MainWindow>();
            mainWindow.Show();
        }
    }
}
