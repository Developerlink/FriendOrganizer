using FriendOrganizerModelLibrary.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace FriendOrganizerDataAccessLibrary.Repositories
{
    public class MeetingRepository : GenericRepository<Meeting, FriendOrganizerDbContext>, IMeetingRepository
    {
        public MeetingRepository(FriendOrganizerDbContext context)
            : base(context)
        {
        }

        public async override Task<Meeting> GetByIdAsync(int id)
        {
            return await Context.Meeting.Include(m => m.Friends).SingleAsync(m => m.Id == id);
        }
    }
}
