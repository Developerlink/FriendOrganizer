using FriendOrganizerModelLibrary.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendOrganizerDataAccessLibrary.Repositories
{
    public class LookupRepository : 
        IFriendLookupRepository, 
        IProgrammingLanguageLookupRepository,
        IMeetingLookupRepository
    {
        private readonly Func<FriendOrganizerDbContext> _ctxCreator;

        public LookupRepository(Func<FriendOrganizerDbContext> ctxCreator)
        {
            _ctxCreator = ctxCreator;
        }

        public async Task<IEnumerable<LookupItem>> GetFriendLookupAsync()
        {
            try
            {
                using var ctx = _ctxCreator();
                var friends = await ctx.Friend.AsNoTracking().Select(f => new LookupItem
                {
                    Id = f.Id,
                    DisplayMember = f.FirstName + " " + f.LastName
                })
                .ToListAsync();
                return friends;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<LookupItem>> GetMeetingLookupAsync()
        {
            try
            {
                using var ctx = _ctxCreator();
                var meetings = await ctx.Meeting.AsNoTracking().Select(f => new LookupItem
                {
                    Id = f.Id,
                    DisplayMember = f.Title
                })
                .ToListAsync();
                return meetings;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<LookupItem>> GetProgrammingLanguageLookupAsync()
        {
            try
            {
                using var ctx = _ctxCreator();
                var friends = await ctx.ProgrammingLanguage.AsNoTracking().Select(f => new LookupItem
                {
                    Id = f.Id,
                    DisplayMember = f.Name
                })
                .ToListAsync();
                return friends;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
