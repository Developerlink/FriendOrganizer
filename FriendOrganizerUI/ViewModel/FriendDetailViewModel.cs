using FriendOrganizerDataAccessLibrary.Repositories;
using FriendOrganizerModelLibrary.Models;
using FriendOrganizerUI.Event;
using FriendOrganizerUI.Model;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FriendOrganizerUI.ViewModel
{
    public class FriendDetailViewModel : BaseViewModel, IFriendDetailViewModel
    {
        private readonly IFriendRepository _friendRepository;
        private readonly IEventAggregator _eventAggregator;
        private FriendModel _friendModel;
        public FriendModel FriendModel
        {
            get { return _friendModel; }
            set
            {
                _friendModel = value;
                OnPropertyChanged();
            }
        }
        public ICommand SaveCommand { get; }

        public FriendDetailViewModel(IFriendRepository friendRepository, IEventAggregator eventAggregator)
        {
            _friendRepository = friendRepository;
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<OpenFriendDetailViewEvent>()
                .Subscribe(OnOpenFriendDetailView);
            SaveCommand = new DelegateCommand(OnSaveExecute, OnSaveCanExecute);
        } 

        private bool OnSaveCanExecute()
        {
            // TODO: Check if friend has changes
            return FriendModel != null && !FriendModel.HasErrors;
        }

        private void OnSaveExecute()
        {
            _friendRepository.SaveAsync(FriendModel.Model);
            _eventAggregator.GetEvent<AfterFriendSavedEvent>()
                .Publish(new AfterFriendSavedEventArgs
                {
                    Id = FriendModel.Id,
                    DisplayMember = $"{FriendModel.FirstName} {FriendModel.LastName}"
                }); 
        }
        
        public async Task LoadAsync(int friendId)
        {
            var friend = await _friendRepository.GetByIdAsync(friendId);
            FriendModel = new FriendModel(friend);
            FriendModel.PropertyChanged += FriendModel_PropertyChanged;
            // Executes the command which will execute the CanExecute which will check if button can execute
            // In this case it checks if nothing is selected
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }

        private void FriendModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            // Executes the command which will execute the CanExecute which will check if button can execute
            // In this case it checks if there are validation errors
            if (e.PropertyName == nameof(FriendModel.HasErrors))
            {
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            }
        }

        private async void OnOpenFriendDetailView(int friendId)
        {
            await LoadAsync(friendId);
        }


    }
}
