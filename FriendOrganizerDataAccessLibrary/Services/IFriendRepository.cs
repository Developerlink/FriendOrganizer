using FriendOrganizerModelLibrary;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FriendOrganizerDataAccessLibrary.Services
{
    public interface IFriendRepository
    {
        Task<List<Friend>> GetAllAsync();
    }
}