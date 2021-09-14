using FriendOrganizerModelLibrary.Models;
using System.Threading.Tasks;

namespace FriendOrganizerUI.ViewModel
{
    public interface IFriendDetailViewModel
    {
        Task LoadAsync(int friendId);
        bool HasChanges { get; }
    }
}