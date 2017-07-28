using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProfileService.Model.BindingModel
{
    public class TeamBindingModel
    {
        public int Id { get; set; }
        public string TeamName { get; set; }
        public int LeaderId { get; set; }
        public IList<int> MemberIds { get; set; }
        public string Description { get; set; }
    }
}
