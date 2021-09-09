using FriendOrganizerDataAccessLibrary;
using FriendOrganizerModelLibrary.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendOrganizerUI.Data
{
    public class FriendDataService : IFriendDataService
    {

        public IEnumerable<Friend> GetAll()
        {
            yield return new Friend { FirstName = "Diana", LastName = "Prince" };
            yield return new Friend { FirstName = "Selina", LastName = "Kyle" };
            yield return new Friend { FirstName = "Bruce", LastName = "Wayne" };
            yield return new Friend { FirstName = "Clark", LastName = "Kent" };
        }



    }
}
