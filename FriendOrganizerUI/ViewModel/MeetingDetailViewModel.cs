using FriendOrganizerDataAccessLibrary.Repositories;
using FriendOrganizerModelLibrary.Models;
using FriendOrganizerUI.Model;
using FriendOrganizerUI.View.Services;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendOrganizerUI.ViewModel
{
    public class MeetingDetailViewModel : DetailViewModelBase, IMeetingDetailViewModel
    {
        private IMeetingRepository _meetingRepository;
        private IMessageDialogService _messageDialogService;
        private List<Friend> _allFriends;

        private MeetingModel _meetingModel;
        public MeetingModel MeetingModel
        {
            get { return _meetingModel; }
            private set
            {
                _meetingModel = value;
                OnPropertyChanged();
            }
        }
        public DelegateCommand AddFriendCommand { get; }
        public DelegateCommand RemoveFriendCommand { get; }
        public ObservableCollection<Friend> AddedFriends { get; private set; }
        public ObservableCollection<Friend> AvailableFriends { get; private set; }

        private Friend _selectedAddedFriend;
        public Friend SelectedAddedFriend
        {
            get { return _selectedAddedFriend; }
            set
            {
                _selectedAddedFriend = value;
                OnPropertyChanged();
                ((DelegateCommand)AddFriendCommand).RaiseCanExecuteChanged();
            }
        }

        private Friend _selectedAvailableFriend;
        public Friend SelectedAvailableFriend
        {
            get { return _selectedAvailableFriend; }
            set
            {
                _selectedAvailableFriend = value;
                OnPropertyChanged();
                ((DelegateCommand)RemoveFriendCommand).RaiseCanExecuteChanged();
            }
        }

        public MeetingDetailViewModel(IEventAggregator eventAggregator,
            IMessageDialogService messageDialogService,
            IMeetingRepository meetingRepository)
            : base(eventAggregator) // Constructor takes the eventAggregator argument and passes it into the base constructor
        {
            _meetingRepository = meetingRepository;
            _messageDialogService = messageDialogService;

            AddedFriends = new ObservableCollection<Friend>();
            AvailableFriends = new ObservableCollection<Friend>();
            AddFriendCommand = new DelegateCommand(OnAddFriendExecute, OnAddFriendCanExecute);
            RemoveFriendCommand = new DelegateCommand(OnRemoveFriendExecute, OnRemoveFriendCanExecute);
        }


        private bool OnRemoveFriendCanExecute()
        {
            return SelectedAvailableFriend != null;
        }

        private void OnRemoveFriendExecute()
        {
            throw new NotImplementedException();
        }

        private bool OnAddFriendCanExecute()
        {
            return SelectedAddedFriend != null;
        }

        private void OnAddFriendExecute()
        {
            throw new NotImplementedException();
        }

        public override async Task LoadAsync(int? meetingId)
        {
            var meeting = meetingId.HasValue
                ? await _meetingRepository.GetByIdAsync(meetingId.Value)
                : CreateNewMeeting();

            InitializeMeeting(meeting);

            _allFriends = await _meetingRepository.GetAllFriendsAsync();

            SetupPickList();
        }

        private void SetupPickList()
        {
            var meetingFriendIds = MeetingModel.Model.Friends.Select(f => f.Id).ToList();
            var addedFriends = _allFriends.Where(f => meetingFriendIds.Contains(f.Id)).OrderBy(f => f.FirstName);
            var availableFriends = _allFriends.Except(addedFriends).OrderBy(f => f.FirstName);

            AddedFriends.Clear();
            AvailableFriends.Clear();
            foreach (var friend in addedFriends)
            {
                AddedFriends.Add(friend);
            }
            foreach (var friend in availableFriends)
            {
                AvailableFriends.Add(friend);
            }
        }

        protected override void OnDeleteExecute()
        {
            var result = _messageDialogService.ShowOkCancelDialog("Are you sure you want to delete?", "Question");
            if (result == MessageDialogResult.Ok)
            {
                _meetingRepository.Remove(MeetingModel.Model);
                _meetingRepository.SaveAsync();
                RaiseDetailDeletedEvent(MeetingModel.Id);
            }
        }

        protected override bool OnSaveCanExecute()
        {
            return MeetingModel != null && !MeetingModel.HasErrors && HasChanges;
        }

        protected override async void OnSaveExecute()
        {
            await _meetingRepository.SaveAsync();
            HasChanges = _meetingRepository.HasChanges();
            RaiseDetailSavedEvent(MeetingModel.Id, MeetingModel.Title);
        }

        private Meeting CreateNewMeeting()
        {
            var meeting = new Meeting
            {
                DateFrom = DateTime.Now.Date,
                DateTo = DateTime.Now.Date
            };
            _meetingRepository.Add(meeting);
            return meeting;
        }

        private void InitializeMeeting(FriendOrganizerModelLibrary.Models.Meeting meeting)
        {
            MeetingModel = new MeetingModel(meeting);
            MeetingModel.PropertyChanged += MeetingModel_PropertyChanged;
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();

            if (MeetingModel.Id == 0)
            {
                // Little trick to trigger the validation
                MeetingModel.Title = "";
            }
        }

        private void MeetingModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (!HasChanges)
            {
                HasChanges = _meetingRepository.HasChanges();
            }

            if (e.PropertyName == nameof(MeetingModel.HasErrors))
            {
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            }
        }
    }
}
