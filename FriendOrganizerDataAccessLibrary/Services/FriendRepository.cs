using FriendOrganizerDataAccessLibrary;
using FriendOrganizerModelLibrary;
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
        private Func<FriendOrganizerDbContext> _ctxCreater;

        // Define a func that returns a FriendOrganizerDbContext

        public FriendRepository(Func<FriendOrganizerDbContext> ctxCreator)
        {
            _ctxCreater = ctxCreator;
        }

        public Task<List<Friend>> GetAllAsync()
        {
            try
            {
                using (var ctx = _ctxCreater())
                {
                    var friends = ctx.Friend.AsNoTracking().ToListAsync();
                    return friends;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }



    }
}
