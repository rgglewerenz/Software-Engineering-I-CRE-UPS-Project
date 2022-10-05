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
        public static bool IsWithin(Coordinate ExpectedPosition, Coordinate CurrentPosition, DTO.AppSettings.ErrorBounds errorBounds)
        {
            return (Math.Abs(ExpectedPosition.Latitude - CurrentPosition.Latitude)
                        <= errorBounds.Latitude)
                            && (Math.Abs(ExpectedPosition.Longitude - CurrentPosition.Longitude)
                                <= errorBounds.Longitude);
        }
        public static double GetDistanceFromCoords(Coordinate ExpectedPosition, Coordinate CurrentPosition)
        {
            return Math.Sqrt(
                Math.Pow(ExpectedPosition.Latitude - CurrentPosition.Latitude, 2) +
                Math.Pow(ExpectedPosition.Longitude - CurrentPosition.Longitude, 2));
        }
    }
}
