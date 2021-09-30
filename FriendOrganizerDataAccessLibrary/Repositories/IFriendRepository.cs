using FriendOrganizerModelLibrary.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FriendOrganizerDataAccessLibrary.Repositories
{
    public interface IFriendRepository : IGenericRepository<Friend>
    {        
        void RemovePhoneNumber(FriendPhoneNumber model);
        Task<bool> HasMeetingsAsync(int friendId);
    }
}