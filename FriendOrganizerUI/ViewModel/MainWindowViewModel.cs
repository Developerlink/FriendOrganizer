using FriendOrganizerDataAccessLibrary.Services;
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
        private IFriendRepository _friendRepository;
        private Friend _selectedFriend;

        public MainWindowViewModel(IFriendRepository frienRepository)
        {
            Friends = new ObservableCollection<Friend>();
            _friendRepository = frienRepository; 
        }

        public async Task LoadAsync()
        {
            var friends = await _friendRepository.GetAllAsync();
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
