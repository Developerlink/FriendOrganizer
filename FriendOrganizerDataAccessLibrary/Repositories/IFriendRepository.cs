using FriendOrganizerModelLibrary.Models;
using System.Collections.Generic;

namespace FriendOrganizerDataAccessLibrary.Repositories
{
    public interface IFriendRepository : IGenericRepository<Friend>
    {        
        void RemovePhoneNumber(FriendPhoneNumber model);
    }
}