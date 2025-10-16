using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace entityframework.entities
{
    public class WorkItemTag
    {
        public WorkItem WorkItem { get; set; }
        public int WorkItemId { get; set; }
        public Tag Tag { get; set; }
        public int TagId { get; set; }
        public DateTime PublicationDate { get; set; }
    }
}