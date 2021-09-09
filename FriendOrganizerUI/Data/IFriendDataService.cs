using FriendOrganizerModelLibrary.Models;
using System.Collections.Generic;

namespace FriendOrganizerUI.Data
{
    public interface IFriendDataService
    {
        IEnumerable<Friend> GetAll();
    }
}