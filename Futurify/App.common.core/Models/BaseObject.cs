using System;
using System.Collections.Generic;
using System.Text;

namespace App.common.core.Models
{
    public class BaseObject
    { 
        public DateTime? CreatedOn { get; set; } 
        public DateTime? ModifiedOn { get; set; } 
        public int? CreatedBy { get; set; } 
        public int? ModifiedBy { get; set; }
    }
}
