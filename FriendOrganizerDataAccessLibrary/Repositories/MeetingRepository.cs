using FriendOrganizerModelLibrary.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FriendOrganizerDataAccessLibrary.Repositories
{
    public class MeetingRepository : GenericRepository<Meeting, FriendOrganizerDbContext>, IMeetingRepository
    {
        public MeetingRepository(FriendOrganizerDbContext context)
            : base(context)
        {
        }

        public async Task<List<Friend>> GetAllFriendsAsync()
        {
            return await Context.Friend.ToListAsync();
        }

        public async override Task<Meeting> GetByIdAsync(int id)
        {
            return await Context.Meeting.Include(m => m.Friends).SingleAsync(m => m.Id == id);
        }

        public async Task ReloadFriendAsync(int id)
        {
            var dbEntityEntry = Context.ChangeTracker.Entries<Friend>().SingleOrDefault(db => db.Entity.Id == id);
            if (dbEntityEntry != null)
            {
                await dbEntityEntry.ReloadAsync(); 
            }
        }
    }
}
