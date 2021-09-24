﻿using FriendOrganizerModelLibrary.Models;
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

        public void Add(Friend friend)
        {
            _ctx.Friend.Add(friend);
        }

        public async Task<Friend> GetByIdAsync(int Id)
        {
            try
            {
                return await _ctx.Friend.Include(f => f.PhoneNumbers).SingleAsync(f => f.Id == Id);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public bool HasChanges()
        {
            try
            {
                return _ctx.ChangeTracker.HasChanges();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void Remove(Friend friend)
        {
            _ctx.Remove(friend);
        }

        public async Task SaveAsync()
        {
            try
            {
                //_ctx.Friend.Attach(friend);
                //_ctx.Entry(friend).State = EntityState.Modified;
                await _ctx.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
