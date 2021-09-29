using FriendOrganizerDataAccessLibrary.Repositories;
using FriendOrganizerModelLibrary.Models;
using FriendOrganizerUI.Event;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FriendOrganizerUI.ViewModel
{
    public class NavigationViewModel : BaseViewModel, INavigationViewModel
    {
        public IFriendLookupRepository _friendLookupRepository;
        private readonly IMeetingLookupRepository _meetingLookupRepository;
        private readonly IEventAggregator _eventAggregator;

        public ObservableCollection<NavigationItemViewModel> Friends { get; private set; }
        public ObservableCollection<NavigationItemViewModel> Meetings { get; private set; }


        public NavigationViewModel(
            IFriendLookupRepository friendLookupRepository,
            IMeetingLookupRepository meetingLookupRepository,
            IEventAggregator eventAggregator)
        {
            _friendLookupRepository = friendLookupRepository;
            _meetingLookupRepository = meetingLookupRepository;
            _eventAggregator = eventAggregator;
            Friends = new ObservableCollection<NavigationItemViewModel>();
            Meetings = new ObservableCollection<NavigationItemViewModel>();
            _eventAggregator.GetEvent<AfterDetailSavedEvent>().Subscribe(AfterDetailSavedHandler);
            _eventAggregator.GetEvent<AfterDetailDeletedEvent>().Subscribe(AfterDetailDeletedHandler);
        }

        private void AfterDetailDeletedHandler(AfterDetailDeletedEventArgs args)
        {
            if (args.ViewModelName == nameof(FriendDetailViewModel))
            {
                AfterDetailDeleted(Friends, args);
            }
            else if (args.ViewModelName == nameof(MeetingDetailViewModel))
            {
                AfterDetailDeleted(Meetings, args);
            }
        }

        private async void AfterDetailDeleted(ObservableCollection<NavigationItemViewModel> items, AfterDetailDeletedEventArgs args)
        {
            var item = items.SingleOrDefault(f => f.Id == args.Id);
            if (item != null)
            {
                items.Remove(item);
            }
            await LoadAsync();
        }

        private async void AfterDetailSavedHandler(AfterDetailSavedEventArgs args)
        {
            if (args.ViewModelName == nameof(FriendDetailViewModel))
            {
                AfterDetailSaved(Friends, args);
            }
            else if (args.ViewModelName == nameof(MeetingDetailViewModel))
            {
                AfterDetailSaved(Meetings, args);
            }
            await LoadAsync();
            //await LoadAsync();
        }

        private void AfterDetailSaved(ObservableCollection<NavigationItemViewModel> items, AfterDetailSavedEventArgs args)
        {
            var lookupItem = items.SingleOrDefault(f => f.Id == args.Id);
            if (lookupItem == null)
            {
                items.Add(new NavigationItemViewModel(args.Id, args.DisplayMember, _eventAggregator, args.ViewModelName));
            }
            else
            {
                lookupItem.DisplayMember = args.DisplayMember;
            }
        }

        public async Task LoadAsync()
        {
            var lookups = await _friendLookupRepository.GetFriendLookupAsync();
            Friends.Clear();
            foreach (var item in lookups)
            {
                Friends.Add(new NavigationItemViewModel(item.Id, item.DisplayMember,
                    _eventAggregator, nameof(FriendDetailViewModel)));
            }

            lookups = await _meetingLookupRepository.GetMeetingLookupAsync();
            Meetings.Clear();
            foreach (var item in lookups)
            {
                Meetings.Add(new NavigationItemViewModel(item.Id, item.DisplayMember,
                    _eventAggregator, nameof(MeetingDetailViewModel)));
            }
        }
    }
}
