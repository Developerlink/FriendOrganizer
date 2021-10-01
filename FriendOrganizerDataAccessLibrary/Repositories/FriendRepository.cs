using FriendOrganizerModelLibrary.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendOrganizerDataAccessLibrary.Repositories
{
    public class FriendRepository : GenericRepository<Friend, FriendOrganizerDbContext>,
                                    IFriendRepository
    {
        public FriendRepository(FriendOrganizerDbContext context)
            : base(context)
        {
        }

        public override async Task<Friend> GetByIdAsync(int Id)
        {
            return await Context.Friend.Include(f => f.PhoneNumbers).SingleAsync(f => f.Id == Id);
        }

        public async Task<bool> HasMeetingsAsync(int friendId)
        {
            return await Context.Meeting.AsNoTracking().Include(m => m.Friends)
                // Does any meeting satisfy the condition that a friend with that id is participating?
                .AnyAsync(m => m.Friends.Any(f => f.Id == friendId)); 
        }

        public void RemovePhoneNumber(FriendPhoneNumber model)
        {
            Context.FriendPhoneNumber.Remove(model);
        }

        public async Task SaveFriendAsync(Friend friend)
        {
            await Context.AddAsync(friend);
            await SaveAsync();
        }
    }
}
