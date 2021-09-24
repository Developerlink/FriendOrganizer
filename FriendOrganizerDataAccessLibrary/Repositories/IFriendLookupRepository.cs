using FriendOrganizerModelLibrary.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FriendOrganizerDataAccessLibrary.Repositories
{
    public interface IFriendLookupRepository
    {
        Task<IEnumerable<LookupItem>> GetFriendsLookupAsync();
    }
}