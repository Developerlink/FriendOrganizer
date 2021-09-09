using FriendOrganizerModelLibrary.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FriendOrganizerDataAccessLibrary.Services
{
    public interface IItemLookupRepository
    {
        Task<IEnumerable<LookupItem>> GetFriendLookupAsync();
    }
}