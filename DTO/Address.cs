using System;
using System.Collections.Generic;
using System.Text;

namespace DTO
{
    public class Address
    {
        public string Country { get; set; } = "US";
        public int Zipcode { get; set; }
        public string Street { get; set; } = String.Empty;
        public string City { get; set; } = String.Empty;
        public string State { get; set; } = String.Empty;
    }
}
