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

        public void RemovePhoneNumber(FriendPhoneNumber model)
        {
            Context.FriendPhoneNumber.Remove(model);
        }
    }
}
