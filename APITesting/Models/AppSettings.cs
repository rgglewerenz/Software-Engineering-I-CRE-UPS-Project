using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APITesting.Models
{
    public class AppSettings
    {
        public BingAppSettings BingSettings { get; set; }
        public TestData TestData { get; set; }
        public GPSSettings GPSSettings { get; set; }
        public AppConfig AppConfig { get; set; }
    }
}
