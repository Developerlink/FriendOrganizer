using FriendOrganizerDataAccessLibrary.Repositories;
using FriendOrganizerModelLibrary.Models;
using FriendOrganizerUI.Model;
using FriendOrganizerUI.View.Services;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendOrganizerUI.ViewModel
{
    public class MeetingDetailViewModel : DetailViewModelBase, IMeetingDetailViewModel
    {
        private IMeetingRepository _meetingRepository;
        private IMessageDialogService _messageDialogService;

        public MeetingDetailViewModel(IEventAggregator eventAggregator,
            IMessageDialogService messageDialogService,
            IMeetingRepository meetingRepository)
            : base(eventAggregator) // Constructor takes the eventAggregator argument and passes it into the base constructor
        {
            _meetingRepository = meetingRepository;
            _messageDialogService = messageDialogService;
        }

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

        public override async Task LoadAsync(int? meetingId)
        {
            var meeting = meetingId.HasValue
                ? await _meetingRepository.GetByIdAsync(meetingId.Value)
                : CreateNewMeeting();

            InitializeMeeting(meeting);
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

            if(MeetingModel.Id == 0)
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

            if(e.PropertyName == nameof(MeetingModel.HasErrors))
            {
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            }
        }
    }
}
