﻿using FriendOrganizerDataAccessLibrary.Repositories;
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
        }

        private void AfterFriendSaved(AfterFriendSavedEventArgs obj)
        {
            var lookupItem = Friends.Single(f => f.Id == obj.Id);
            lookupItem.DisplayMember = obj.DisplayMember;
            //await LoadAsync();
        }

        public async Task LoadAsync()
        {
            var lookup = await _friendLookupRepository.GetFriendsLookupAsync();
            Friends.Clear();
            foreach (var item in lookup)
            {
                Friends.Add(new NavigationItemViewModel(item.Id, item.DisplayMember,_eventAggregator));
            }
        }
    }
}
