using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace entityframework.entities
{
    public class Address
    {
        public string Country { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string PostalCode { get; set; }
    }
}