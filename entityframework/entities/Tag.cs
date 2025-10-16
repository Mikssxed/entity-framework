using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace entityframework.entities
{
    public class Tag
    {
        public int Id { get; set; }
        public string Value { get; set; }

        // public List<WorkItemTag> WorkItemTags { get; set; } = new List<WorkItemTag>();
        public List<WorkItem> WorkItems { get; set; }
    }
}