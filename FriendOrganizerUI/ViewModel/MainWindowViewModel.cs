using FriendOrganizerModelLibrary;
using FriendOrganizerUI.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendOrganizerUI.ViewModel
{
    public class MainWindowViewModel : BaseViewModel
    {
        private IFriendDataService _friendDataService;
        private Friend _selectedFriend;

        public MainWindowViewModel(IFriendDataService frienDataService)
        {
            Friends = new ObservableCollection<Friend>();
            _friendDataService = frienDataService; 
        }

        public void Load()
        {
            var friends = _friendDataService.GetAll();
            Friends.Clear();
            foreach(var friend in friends)
            {
                Friends.Add(friend);
            }
        }
        public ObservableCollection<Friend> Friends { get; set; }

        public Friend SelectedFriend
        {
            get { return _selectedFriend; }
            set 
            { 
                _selectedFriend = value;
                OnPropertyChanged();
            }
        }

    }
}
