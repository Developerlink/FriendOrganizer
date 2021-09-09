using FriendOrganizerModelLibrary.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace FriendOrganizerUI.ViewModel
{
    public interface INavigationViewModel
    {
        Task LoadAsync();
    }
}