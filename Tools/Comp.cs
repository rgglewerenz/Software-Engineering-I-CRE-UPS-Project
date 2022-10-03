using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools
{
    public class Comp
    {
        public static bool IsWithin(Coordinate ExpectedPosition, Coordinate CurrentPosition, DTO.AppSettings.AppSettings appSettings)
        {
            return (Math.Abs(ExpectedPosition.Latitude - CurrentPosition.Latitude)
                        <= appSettings.GPSSettings.ErrorBounds.Latitude)
                            && (Math.Abs(ExpectedPosition.Longitude - CurrentPosition.Longitude)
                                <= appSettings.GPSSettings.ErrorBounds.Longitude);
        }
        public static double GetDistanceFromCoords(Coordinate ExpectedPosition, Coordinate CurrentPosition)
        {
            return Math.Sqrt(
                Math.Pow(ExpectedPosition.Latitude - CurrentPosition.Latitude, 2) +
                Math.Pow(ExpectedPosition.Longitude - CurrentPosition.Longitude, 2));
        }
    }
}
