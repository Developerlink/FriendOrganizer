using Autofac.Features.Indexed;
using FriendOrganizerUI.Event;
using FriendOrganizerUI.View.Services;
using Prism.Commands;
using Prism.Events;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace FriendOrganizerUI.ViewModel
{
    public class MainWindowViewModel : BaseViewModel
    {
        private IEventAggregator _eventAggregator;
        private IDetailViewModel _detailViewModel;
        private IMessageDialogService _messageDialogService;
        private IIndex<string, IDetailViewModel> _detailViewModelCreator;

        public MainWindowViewModel(
            INavigationViewModel navigationViewModel, 
            IIndex<string, IDetailViewModel> detailViewModelCreator,
            IEventAggregator eventAggregator, 
            IMessageDialogService messageDialogService)
        {
            _messageDialogService = messageDialogService;
            _detailViewModelCreator = detailViewModelCreator;
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<OpenDetailViewEvent>()
                .Subscribe(OnOpenDetailView);
            _eventAggregator.GetEvent<AfterDetailDeletedEvent>()
                .Subscribe(AfterDetailDeleted);

            CreateNewDetailCommand = new DelegateCommand<Type>(OnCreateNewDetailExecute);

            NavigationViewModel = navigationViewModel;
        }

        public async Task LoadAsync()
        {
            await NavigationViewModel.LoadAsync();
        }

        public ICommand CreateNewDetailCommand { get; }
        public INavigationViewModel NavigationViewModel { get; }
        public IDetailViewModel DetailViewModel
        {
            get { return _detailViewModel; }
            private set
            {
                _detailViewModel = value;
                OnPropertyChanged();
            }
        }

        private async void OnOpenDetailView(OpenDetailViewEventArgs args)
        {
            if (DetailViewModel != null && DetailViewModel.HasChanges)
            {
                var result = _messageDialogService.ShowOkCancelDialog("You have made changes. Do you want to navigate away anyway?", "Question");
                if (result == MessageDialogResult.Cancel)
                {
                    return;
                }
            }

            //if (args.ViewModelName == nameof(FriendDetailViewModel))
            //{
            //    DetailViewModel = _friendDetailViewModelCreator();
            //}
            //else if (args.ViewModelName == nameof(MeetingDetailViewModel))
            //{
            //    DetailViewModel = _meetingDetailViewModelCreator();
            //}
            //else
            //{
            //    throw new Exception($"ViewModel {args.ViewModelName} not mapped");
            //}

            DetailViewModel = _detailViewModelCreator[args.ViewModelName];
            await DetailViewModel.LoadAsync(args.Id);
        }

        private void OnCreateNewDetailExecute(Type viewModelType)
        {
            OnOpenDetailView(new OpenDetailViewEventArgs { ViewModelName = viewModelType.Name });
        }

        private void AfterDetailDeleted(AfterDetailDeletedEventArgs args)
        {
            DetailViewModel = null;
        }
    }
}
