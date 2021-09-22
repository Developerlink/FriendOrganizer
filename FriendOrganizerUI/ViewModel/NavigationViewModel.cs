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
        public IItemLookupRepository _friendLookupRepository;
        private readonly IEventAggregator _eventAggregator;

        public ObservableCollection<NavigationItemViewModel> Friends { get; private set; }

        public NavigationViewModel(IItemLookupRepository friendLookupRepository, IEventAggregator eventAggregator)
        {
            _friendLookupRepository = friendLookupRepository;
            _eventAggregator = eventAggregator;
            Friends = new ObservableCollection<NavigationItemViewModel>();
            _eventAggregator.GetEvent<AfterFriendSavedEvent>().Subscribe(AfterFriendSaved);
            _eventAggregator.GetEvent<AfterFriendDeletedEvent>().Subscribe(AfterFriendDeleted);
        }

        private void AfterFriendDeleted(int friendId)
        {
            var friend = Friends.SingleOrDefault(f => f.Id == friendId);
            if (friend != null)
            {
                Friends.Remove(friend);
            }
        }

        private async void AfterFriendSaved(AfterFriendSavedEventArgs obj)
        {
            //var lookupItem = Friends.SingleOrDefault(f => f.Id == obj.Id);
            //if (lookupItem == null)
            //{
            //    Friends.Add(new NavigationItemViewModel(obj.Id, obj.DisplayMember, _eventAggregator));
            //}
            //else
            //{
            //    lookupItem.DisplayMember = obj.DisplayMember;
            //}
            await LoadAsync();
            await LoadAsync();
        }

        public async Task LoadAsync()
        {
            var lookups = await _friendLookupRepository.GetFriendsLookupAsync();
            Friends.Clear();
            foreach (var item in lookups)
            {
                Friends.Add(new NavigationItemViewModel(item.Id, item.DisplayMember, _eventAggregator));
            }
        }
    }
}
