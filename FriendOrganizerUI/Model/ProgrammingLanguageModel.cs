using FriendOrganizerModelLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendOrganizerUI.Model
{
    public class ProgrammingLanguageModel : ModelWrapper<ProgrammingLanguage>
    {
        public ProgrammingLanguageModel(ProgrammingLanguage model) : base(model)
        {
        }

        public int Id { get { return Model.Id; } }
        public string Name
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }
    }
}
