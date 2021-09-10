﻿
namespace FriendOrganizerModelLibrary.Models
{
    public class LookupItem : NotifyPropertyChangedBase
    {
        public int Id { get; set; }

        private string _displayMember;
        public string DisplayMember
        {
            get { return _displayMember; }
            set { _displayMember = value;
                OnPropertyChanged();
            }
        }
    }
}
