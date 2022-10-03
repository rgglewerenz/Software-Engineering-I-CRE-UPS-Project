using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DTO;
using Location_API_Interface.Interface;

namespace Location_API_Interface.Service
{
        public class MapApi : IMapApi
        {
            string api_key;
            HttpClient client;
            public MapApi(string apiKey)
            {
                api_key = apiKey;
                client = new HttpClient();
            }
            public async Task<double> GetDistanceAsync(Address origin, Address destination, string units = "mi")
            {
                System.Threading.Thread.Sleep(1000);
                double distance = 0;
                int solutions = 1;
                //string from = origin.Text;
                //string to = destination.Text;
                var task1 = GetAddressInPointFormAsync(origin);
                var task2 = GetAddressInPointFormAsync(destination);


                await Task.WhenAll(new Task<Coordinate>[] { task1, task2 });

                var coordinate1 = task1.Result;
                var coordinate2 = task2.Result;
                if (coordinate1 == null || coordinate2 == null)
                {
                    return 0;
                }

                string bingUrl = $"http://dev.virtualearth.net/REST/v1/Routes?wayPoint.1={coordinate1.Latitude},{coordinate1.Longitude}&Waypoint.2={coordinate2.Latitude},{coordinate2.Longitude}&maxSolutions={solutions}&distanceUnit={units}&key={api_key}";
                string content = await GetAsync(bingUrl);
                JObject o = JObject.Parse(content);
                try
                {
                    distance = (double)o.SelectToken("resourceSets[0].resources[0].travelDistance");
                    return distance;
                }
                catch
                {
                    return distance;
                }
                //ResultingDistance.Text = distance;
            }
            public async Task<double> GetDistanceAsync(Coordinate origin, Coordinate destination, string units = "mi")
            {
                System.Threading.Thread.Sleep(1000);
                double distance = 0;
                int solutions = 1;
                //string from = origin.Text;
                //string to = destination.Text;

                string bingUrl = $"http://dev.virtualearth.net/REST/v1/Routes?wayPoint.1={origin.Latitude},{origin.Longitude}&Waypoint.2={origin.Latitude},{origin.Longitude}&maxSolutions={solutions}&distanceUnit={units}&key={api_key}";
                string content = await GetAsync(bingUrl);
                JObject o = JObject.Parse(content);
                try
                {
                    distance = (double)o.SelectToken("resourceSets[0].resources[0].travelDistance");
                    return distance;
                }
                catch
                {
                    return distance;
                }
                //ResultingDistance.Text = distance;
            }
            public async Task<double> GetDistanceAsync(Address origin, Coordinate coordinate2, string units = "mi")
            {
                System.Threading.Thread.Sleep(1000);
                double distance = 0;
                int solutions = 1;
                //string from = origin.Text;
                //string to = destination.Text;
                var coordinate1 = await GetAddressInPointFormAsync(origin);
                if (coordinate1 == null || coordinate2 == null)
                {
                    return 0;
                }

                string bingUrl = $"http://dev.virtualearth.net/REST/v1/Routes?wayPoint.1={coordinate1.Latitude},{coordinate1.Longitude}&Waypoint.2={coordinate2.Latitude},{coordinate2.Longitude}&maxSolutions={solutions}&distanceUnit={units}&key={api_key}";
                string content = await GetAsync(bingUrl);
                JObject o = JObject.Parse(content);
                try
                {
                    distance = (double)o.SelectToken("resourceSets[0].resources[0].travelDistance");
                    return distance;
                }
                catch
                {
                    return distance;
                }
                //ResultingDistance.Text = distance;
            }
            public async Task<Coordinate> GetAddressInPointFormAsync(Address address)
            {
                string url = $"http://dev.virtualearth.net/REST/v1/Locations?countryRegion={address.Country}&locality={address.City.Replace(" ", "%20")}&postalCode={address.Zipcode}&addressLine={address.Street.Replace(" ", "%20")}&includeNeighborhood=false&maxResults=1&key={api_key}";
                var response = await GetAsync(url);
                if (response == "")
                {
                    return null;
                }
                JObject o = JObject.Parse(response);
                return new Coordinate()
                {
                    Latitude = (double)o.SelectToken("resourceSets[0].resources[0].point.coordinates[0]"),
                    Longitude = (double)o.SelectToken("resourceSets[0].resources[0].point.coordinates[1]")
                };
            }

        public async Task<Address> GetAddressFromCoordinate(Coordinate Coordinate)
        {
            string test = $"http://dev.virtualearth.net/REST/v1/Locations/{Coordinate.Latitude},{Coordinate.Longitude}?key={api_key}";  
            string url = $"http://dev.virtualearth.net/REST/v1/Locations?point={Coordinate.Latitude},{Coordinate.Longitude}&key={api_key}";
            var response = await GetAsync(test);
            if (response == "")
            {
                return null;
            }
            JObject o = JObject.Parse(response);
            return new Address()
            {
                Street = (string)o.SelectToken("resourceSets[0].resources[0].address.addressLine"),
                City = (string)o.SelectToken("resourceSets[0].resources[0].address.locality"),
                State = (string)o.SelectToken("resourceSets[0].resources[0].address.adminDistrict"),
                Zipcode = (int)o.SelectToken("resourceSets[0].resources[0].address.postalCode") 
            };
        }
        protected async Task<string> GetAsync(string url)
            {
                try
                {
                    string sContents = string.Empty;
                    string me = string.Empty;
                    var response = await client.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        return await response.Content.ReadAsStringAsync();
                    }
                    return string.Empty;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Unable to perform Http request error: \n{ex.Message}");
                    return String.Empty;
                }
            }
        }
}