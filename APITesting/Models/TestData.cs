using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APITesting.Models
{
    public class TestData
    {
        public Address StartingAddress { get; set; }
        public List<Address> TestLocations { get; set; }
    }
}
