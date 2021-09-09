using FriendOrganizerDataAccessLibrary;
using FriendOrganizerModelLibrary.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendOrganizerDataAccessLibrary.Services
{
    public class FriendRepository : IFriendRepository
    {
        private readonly Func<FriendOrganizerDbContext> _ctxCreater;

        // Define a func that returns a FriendOrganizerDbContext

        public FriendRepository(Func<FriendOrganizerDbContext> ctxCreator)
        {
            _ctxCreater = ctxCreator;
        }

        public async Task<Friend> GetByIdAsync(int Id)
        {
            try
            {
                using var ctx = _ctxCreater();
                var friend = await ctx.Friend.FindAsync(Id);
                return friend;
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
                using var ctx = _ctxCreater();
                ctx.Add(friend);
                ctx.Entry(friend).State = EntityState.Modified;
                await ctx.SaveChangesAsync(); 
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
