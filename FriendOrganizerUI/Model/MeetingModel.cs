using FriendOrganizerModelLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendOrganizerUI.Model
{
    public class MeetingModel : ModelWrapper<Meeting>
    {
        public MeetingModel(Meeting model) : base(model)
        {
        }

        public int Id { get { return Model.Id; } }

        public string Title
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public DateTime DateFrom
        {
            get { return GetValue<DateTime>(); }
            set
            {
                SetValue(value);
                if (DateTo < DateFrom)
                {
                    DateTo = DateFrom;
                }
            }
        }

        public DateTime DateTo
        {
            get { return GetValue<DateTime>(); }
            set
            {
                SetValue(value);
                if (DateTo < DateFrom)
                {
                    DateFrom = DateTo;
                }
            }
        }
    }
}
