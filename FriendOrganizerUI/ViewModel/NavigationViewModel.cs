using FriendOrganizerDataAccessLibrary.Services;
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

        public ObservableCollection<LookupItem> Friends { get; private set; }

        private LookupItem _selectedFriend;
        public LookupItem SelectedFriend
        {
            get { return _selectedFriend; }
            set
            {
                _selectedFriend = value;
                OnPropertyChanged();
                if (_selectedFriend != null)
                {
                    _eventAggregator.GetEvent<OpenFriendDetailViewEvent>()
                        .Publish(_selectedFriend.Id);
                }
            }
        }

        public NavigationViewModel(IItemLookupRepository friendLookupRepository, IEventAggregator eventAggregator)
        {
            _friendLookupRepository = friendLookupRepository;
            _eventAggregator = eventAggregator;            
            Friends = new ObservableCollection<LookupItem>();
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
            var lookup = await _friendLookupRepository.GetFriendLookupAsync();
            Friends.Clear();
            foreach (var item in lookup)
            {
                Friends.Add(item);
            }
        }
    }
}
