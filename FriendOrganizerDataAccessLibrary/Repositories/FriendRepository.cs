using FriendOrganizerModelLibrary.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendOrganizerDataAccessLibrary.Repositories
{
    public class FriendRepository : IFriendRepository
    {
        private readonly FriendOrganizerDbContext _ctx;

        // Define a func that returns a FriendOrganizerDbContext

        public FriendRepository(FriendOrganizerDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<Friend> GetByIdAsync(int Id)
        {
            try
            {
                return await _ctx.Friend.FindAsync(Id);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task SaveAsync(Friend friend)
        {
            try
            {
                _ctx.Friend.Attach(friend);
                _ctx.Entry(friend).State = EntityState.Modified;
                await _ctx.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
