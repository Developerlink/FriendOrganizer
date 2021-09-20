using FriendOrganizerModelLibrary.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FriendOrganizerDataAccessLibrary.Repositories
{
    public interface IFriendRepository
    {
        Task<Friend> GetByIdAsync(int Id);
        //Task<List<Friend>> GetAllAsync();
        Task SaveAsync(Friend friend);
        bool HasChanges();
        void Add(Friend friend);
    }
}