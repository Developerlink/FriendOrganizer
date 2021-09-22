using FriendOrganizerDataAccessLibrary.Repositories;
using FriendOrganizerModelLibrary.Models;
using FriendOrganizerUI.Event;
using FriendOrganizerUI.Model;
using FriendOrganizerUI.View.Services;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace FriendOrganizerUI.ViewModel
{
    public class FriendDetailViewModel : BaseViewModel, IFriendDetailViewModel
    {
        private readonly IFriendRepository _friendRepository;
        private readonly IEventAggregator _eventAggregator;
        private readonly IMessageDialogService _messageDialogService;
        private FriendModel _friendModel;
        private bool _hasChanges;

        public FriendModel FriendModel
        {
            get { return _friendModel; }
            set
            {
                _friendModel = value;
                OnPropertyChanged();
            }
        }

        public bool HasChanges
        {
            get { return _hasChanges; }
            set
            {
                if (_hasChanges != value)
                {
                    _hasChanges = value;
                    OnPropertyChanged();
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            }
        }

        public ICommand SaveCommand { get; }
        public ICommand DeleteCommand { get; }

        public FriendDetailViewModel(IFriendRepository friendRepository, IEventAggregator eventAggregator, IMessageDialogService messageDialogService)
        {
            _friendRepository = friendRepository;
            _eventAggregator = eventAggregator;
            _messageDialogService = messageDialogService;

            SaveCommand = new DelegateCommand(OnSaveExecute, OnSaveCanExecute);
            DeleteCommand = new DelegateCommand(OnDeleteExecute);
        }

        private async void OnDeleteExecute()
        {
            var result = _messageDialogService.ShowOkCancelDialog("Do you really want to delete?", "Question");
            if (result == MessageDialogResult.Ok)
            {
                _friendRepository.Remove(FriendModel.Model);
                await _friendRepository.SaveAsync();
                _eventAggregator.GetEvent<AfterFriendDeletedEvent>()
                    .Publish(FriendModel.Id);
            }
        }

        private bool OnSaveCanExecute()
        {
            return FriendModel != null && !FriendModel.HasErrors && HasChanges;
        }

        private void OnSaveExecute()
        {
            _friendRepository.SaveAsync();
            //HasChanges = _friendRepository.HasChanges();
            HasChanges = false;
            _eventAggregator.GetEvent<AfterFriendSavedEvent>()
                .Publish(new AfterFriendSavedEventArgs
                {
                    Id = FriendModel.Model.Id,
                    DisplayMember = $"{FriendModel.FirstName} {FriendModel.LastName}"
                });
        }

        public async Task LoadAsync(int? friendId)
        {
            var friend = friendId.HasValue ? await _friendRepository.GetByIdAsync(friendId.Value) : CreateNewFriend();
            FriendModel = new FriendModel(friend);
            FriendModel.PropertyChanged += FriendModel_PropertyChanged;
            // Executes the command which will execute the CanExecute which will check if button can execute
            // In this case it checks if nothing is selected
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            if (FriendModel.Id == 0)
            {
                // Little trick to trigger validation when creating a new Friend
                FriendModel.FirstName = "";
            }
        }

        private Friend CreateNewFriend()
        {
            var friend = new Friend();
            _friendRepository.Add(friend);
            return friend;
        }

        private void FriendModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (!HasChanges)
            {
                HasChanges = _friendRepository.HasChanges();
            }
            // Executes the command which will execute the CanExecute which will check if button can execute
            // In this case it checks if there are validation errors
            if (e.PropertyName == nameof(FriendModel.HasErrors))
            {
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            }
        }



    }
}
