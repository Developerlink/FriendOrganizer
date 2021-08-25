using System;

namespace FriendOrganizerModelLibrary
{
    public class Friend
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string FullName 
        {
            get
            {
                string fullName = "";
                if (!string.IsNullOrEmpty(FirstName))
                {
                    fullName += FirstName;
                }
                if (!string.IsNullOrEmpty(LastName))
                {
                    fullName += $" {LastName}";
                }
                return fullName;
            }
        }
    }
}
