using Autofac;
using FriendOrganizerDataAccessLibrary;
using FriendOrganizerDataAccessLibrary.Repositories;
using FriendOrganizerUI.Data;
using FriendOrganizerUI.View.Services;
using FriendOrganizerUI.ViewModel;
using Prism.Events;

namespace FriendOrganizerUI.Startup
{
    public class Bootstrapper
    {
        public IContainer Bootstrap()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<EventAggregator>().As<IEventAggregator>().SingleInstance();

            builder.RegisterType<FriendOrganizerDbContext>().AsSelf();
            builder.RegisterType<MainWindow>().AsSelf();
            builder.RegisterType<MainWindowViewModel>().AsSelf();
            builder.RegisterType<FriendRepository>().As<IFriendRepository>();
            builder.RegisterType<LookupRepository>().AsImplementedInterfaces();
            builder.RegisterType<NavigationViewModel>().As<INavigationViewModel>();
            builder.RegisterType<FriendDetailViewModel>().As<IFriendDetailViewModel>();
            builder.RegisterType<FriendDataService>().As<IFriendDataService>();
            builder.RegisterType<MessageDialogService>().As<IMessageDialogService>();

            return builder.Build();
        }
    }
}
