using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FriendOrganizerModelLibrary
{
    public class Friend
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        [StringLength(50)]
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
