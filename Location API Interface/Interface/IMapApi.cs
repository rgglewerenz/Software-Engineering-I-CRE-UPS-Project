using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location_API_Interface.Interface
{
    public interface IMapApi
    {
        Task<Address> GetAddressFromCoordinate(Coordinate Coordinate);
        Task<Coordinate> GetAddressInPointFormAsync(Address address);
        Task<double> GetDistanceAsync(Address origin, Address destination, string units = "mi");
        Task<double> GetDistanceAsync(Coordinate origin, Coordinate destination, string units = "mi");
        Task<double> GetDistanceAsync(Address origin, Coordinate coordinate2, string units = "mi");
    }
}
