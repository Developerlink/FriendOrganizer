using FriendOrganizerModelLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendOrganizerUI.Model
{
    public class FriendPhoneNumberModel : ModelWrapper<FriendPhoneNumber>
    {
        public FriendPhoneNumberModel(FriendPhoneNumber model) : base(model)
        {

        }

        public string Number
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }
    }
}
