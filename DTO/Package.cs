using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class Package
    {
        public int TrackingID { get; set; }
        public Address? Address { get; set; }
    }
}
