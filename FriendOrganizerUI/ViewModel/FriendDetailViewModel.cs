using FriendOrganizerDataAccessLibrary.Repositories;
using FriendOrganizerModelLibrary.Models;
using FriendOrganizerUI.Event;
using FriendOrganizerUI.Model;
using FriendOrganizerUI.View.Services;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace FriendOrganizerUI.ViewModel
{
    public class FriendDetailViewModel : DetailViewModelBase, IFriendDetailViewModel
    {
        private readonly IFriendRepository _friendRepository;
        private readonly IProgrammingLanguageLookupRepository _programmingLanguageLookupRepository;
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
        private FriendPhoneNumberModel _selectedPhoneNumber;

        public FriendPhoneNumberModel SelectedPhoneNumber        
        {
            get { return _selectedPhoneNumber; }
            set { _selectedPhoneNumber = value;
                OnPropertyChanged();
                ((DelegateCommand)RemovePhoneNumberCommand).RaiseCanExecuteChanged();
            }
        }
       
        public ICommand AddPhoneNumberCommand { get; }
        public ICommand RemovePhoneNumberCommand { get; }
        public ObservableCollection<LookupItem> ProgrammingLanguages { get; }
        public ObservableCollection<FriendPhoneNumberModel> PhoneNumbers { get; }
        public FriendDetailViewModel(IFriendRepository friendRepository,
            IEventAggregator eventAggregator,
            IMessageDialogService messageDialogService,
            IProgrammingLanguageLookupRepository programmingLanguageLookupRepository)
            :base(eventAggregator, messageDialogService)
        {
            _friendRepository = friendRepository;
            _programmingLanguageLookupRepository = programmingLanguageLookupRepository;

            eventAggregator.GetEvent<AfterCollectionSavedEvent>()
                .Subscribe(AfterCollectionSaved);

            AddPhoneNumberCommand = new DelegateCommand(OnAddPhoneNumberExecute);
            RemovePhoneNumberCommand = new DelegateCommand(OnRemovePhoneNumbeExecute, OnRemovePhoneNumberCanExecute);

            ProgrammingLanguages = new ObservableCollection<LookupItem>();
            PhoneNumbers = new ObservableCollection<FriendPhoneNumberModel>();
        }

        private async void AfterCollectionSaved(AfterCollectionSavedEventArgs args)
        {
            if (args.ViewModelName == nameof(ProgrammingLanguageDetailViewModel))
            {
                await LoadProgrammingLanguagesLookupAsync();
            }
        }

        private bool OnRemovePhoneNumberCanExecute()
        { 
            if (SelectedPhoneNumber != null)
            {
                return true;
            }
            return false;
        }

        private void OnRemovePhoneNumbeExecute()
        {
            SelectedPhoneNumber.PropertyChanged -= FriendPhoneNumberModel_PropertyChanged;
            _friendRepository.RemovePhoneNumber(SelectedPhoneNumber.Model);
            //FriendModel.Model.PhoneNumbers.Remove(SelectedPhoneNumber.Model);
            PhoneNumbers.Remove(SelectedPhoneNumber);
            SelectedPhoneNumber = null;
            HasChanges = _friendRepository.HasChanges();
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }

        private void OnAddPhoneNumberExecute()
        {
            var newNumber = new FriendPhoneNumberModel(new FriendPhoneNumber());
            newNumber.PropertyChanged += FriendPhoneNumberModel_PropertyChanged;
            PhoneNumbers.Add(newNumber);
            FriendModel.Model.PhoneNumbers.Add(newNumber.Model);
            newNumber.Number = "";
        }

        protected override async void OnDeleteExecute()
        {
            if (await _friendRepository.HasMeetingsAsync(FriendModel.Id))
            {
                MessageDialogService.ShowInfoDialog($"{FriendModel.FirstName} {FriendModel.LastName} can't be deleted because that person is part of at least one meeting.");
                return;
            }

            var result = MessageDialogService.ShowOkCancelDialog("Do you really want to delete?", "Question");
            if (result == MessageDialogResult.Ok)
            {
                _friendRepository.Remove(FriendModel.Model);
                await _friendRepository.SaveAsync();
                RaiseDetailDeletedEvent(FriendModel.Id);
            }
        }

        protected override bool OnSaveCanExecute()
        {
            return FriendModel != null 
                && !FriendModel.HasErrors 
                && PhoneNumbers.All(p => !p.HasErrors)
                && HasChanges;
        }

        protected override void OnSaveExecute()
        {
            _friendRepository.SaveAsync();
            //HasChanges = _friendRepository.HasChanges();
            SetTitle($"{FriendModel.FirstName} {FriendModel.LastName}");
            Id = FriendModel.Id;
            HasChanges = false;
            RaiseDetailSavedEvent(FriendModel.Id, $"{FriendModel.FirstName} {FriendModel.LastName}");
        }

        public override async Task LoadAsync(int friendId)
        {
            var friend = friendId > 0 ? await _friendRepository.GetByIdAsync(friendId) : CreateNewFriend();

            Id = friendId;
            SetTitle($"{friend.FirstName} {friend.LastName}");

            InitializeFriend(friend);
            InitializeFriendPhoneNumbers(friend.PhoneNumbers);

            await LoadProgrammingLanguagesLookupAsync();
        }

        private void InitializeFriendPhoneNumbers(ICollection<FriendPhoneNumber> phoneNumbers)
        {
            foreach (var model in PhoneNumbers)
            {
                model.PropertyChanged -= FriendPhoneNumberModel_PropertyChanged;
            }
            PhoneNumbers.Clear();
            foreach (var friendPhoneNumber in phoneNumbers)
            {
                var model = new FriendPhoneNumberModel(friendPhoneNumber);
                PhoneNumbers.Add(model);
                model.PropertyChanged += FriendPhoneNumberModel_PropertyChanged;
            }
        } 

        private void FriendPhoneNumberModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!HasChanges)
            {
                HasChanges = _friendRepository.HasChanges();
            }
            if (e.PropertyName == nameof(FriendPhoneNumberModel.HasErrors))
            {
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            }
        }

        private void InitializeFriend(Friend friend)
        {
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

        private async Task LoadProgrammingLanguagesLookupAsync()
        {
            ProgrammingLanguages.Clear();
            ProgrammingLanguages.Add(new NullLookupItem() { DisplayMember = " - " });
            var lookup = await _programmingLanguageLookupRepository.GetProgrammingLanguageLookupAsync();
            foreach (var lookupItem in lookup)
            {
                ProgrammingLanguages.Add(lookupItem);
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
            if (e.PropertyName == nameof(FriendModel.FirstName) || e.PropertyName == nameof(FriendModel.LastName))
            {
                SetTitle($"{FriendModel.FirstName} {FriendModel.LastName}");
            }
        }

        
    }
}
