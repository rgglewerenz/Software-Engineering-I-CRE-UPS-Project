using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location_API_Interface.Interface
{
    public interface IGPSService
    {
        Action<Coordinate>? OnCoordinateChage { get; set; }

        Task<Coordinate> GetCurrentCoordinates();
    }
}
