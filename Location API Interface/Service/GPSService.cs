using DTO;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Location_API_Interface.Interface;

namespace Location_API_Interface.Service
{
    public class GPSService : IGPSService
    {
        private Coordinate? _CurrentCoordinates { get; set; }
        public Action<Coordinate>? OnCoordinateChage { get; set; }
        GeoCoordinateWatcher _watcher { get; set; }

        public GPSService()
        {
            _watcher = new GeoCoordinateWatcher();
            _watcher.StatusChanged +=
                            new EventHandler<GeoPositionStatusChangedEventArgs>(_gcw_StatusChanged);
            _watcher.PositionChanged +=
                    new EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>>(_gcw_PositionChanged);
            _watcher.MovementThreshold = 50;

            // Do not suppress prompt, and wait 1000 milliseconds to start.
            _watcher.TryStart(false, TimeSpan.FromMilliseconds(1000));
        }
        public async Task<Coordinate> GetCurrentCoordinates()
        {
            //Lets the program run in the background so it can get the coordinates if need be
            await Task.Run(() =>
            {
                while (_CurrentCoordinates == null);
            });
            return _CurrentCoordinates??new Coordinate();
        }
        private void _gcw_StatusChanged(object sender, GeoPositionStatusChangedEventArgs e)
        {
            if (e.Status == GeoPositionStatus.Ready)
            {
                var coords = _watcher.Position.Location;
                _CurrentCoordinates = new Coordinate()
                {
                    Latitude = coords.Latitude,
                    Longitude = coords.Longitude
                };
            }

        }
        private void _gcw_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            var coord = e.Position.Location;
            _CurrentCoordinates = new Coordinate()
            {
                Latitude = coord.Latitude,
                Longitude = coord.Longitude
            };
            OnCoordinateChage?.Invoke(_CurrentCoordinates);
        }
    }
}
