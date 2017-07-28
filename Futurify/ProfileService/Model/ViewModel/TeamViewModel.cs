using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProfileService.Model.ViewModel
{
    public class TeamViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int LeaderId { get; set; }
        public IList<int> MemberIds { get; set; }
    }
}
