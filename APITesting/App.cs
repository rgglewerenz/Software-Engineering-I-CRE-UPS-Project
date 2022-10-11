using DTO;
using Location_API_Interface;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Device.Location;
using Location_API_Interface.Interface;
using Location_API_Interface.Service;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Alerts_Api;
using DTO.AppSettings;
using Tools;

namespace APITesting
{
    public class App
    {
        AppSettings appSettings;
        IMapApi MapApi;
        List<Address> TestData;
        Address StartingAddress;
        IGPSService GPSService;

        public App(IConfigurationRoot config)
        {
            AddConfigurations(config);
        }
        public async Task Run()
        {
            JsonSerializedDataObject o = new JsonSerializedDataObject()
            {
                Packages = new List<Package>()
                    {
                        new Package()
                    {
                        Address = new Address()
                        {
                            City = "Farmington Hills",
                            Country = "US",
                            State = "MI",
                            Street = "30382 Nantucet Dr",
                            Zipcode = 48336
                        },
                        TrackingID = 0
                    },
                        new Package()
                    {
                        Address = new Address()
                        {
                            City = "Farmington Hills",
                            Country = "US",
                            State = "MI",
                            Street = "30332 Tuck Rd",
                            Zipcode = 48336
                        },
                        TrackingID = 1
                    },
                        new Package()
                    {
                        Address = new Address()
                        {
                            City = "SouthField",
                            Country = "US",
                            State = "MI",
                            Street = "21000 W 10 Mile Rd",
                            Zipcode = 48075
                        },
                        TrackingID = 2
                    }
                    }
            };
            if (appSettings.AppConfig.AskUserInput)
            {
                o = FileHandler.ReadFromFile(FileHandlingInterface.AskUserForFileLocWithPrompt("Json Files (*.json)|*.json|Text Files (*.txt)|*.txt"))
                ?? // Genereates a default one in case none is found in the location
                new JsonSerializedDataObject()
                {
                    Packages = new List<Package>()
                    {
                        new Package()
                    {
                        Address = new Address()
                        {
                            City = "Farmington Hills",
                            Country = "US",
                            State = "MI",
                            Street = "30382 Nantucet Dr",
                            Zipcode = 48336
                        },
                        TrackingID = 0
                    },
                        new Package()
                    {
                        Address = new Address()
                        {
                            City = "Farmington Hills",
                            Country = "US",
                            State = "MI",
                            Street = "30332 Tuck Rd",
                            Zipcode = 48336
                        },
                        TrackingID = 1
                    },
                        new Package()
                    {
                        Address = new Address()
                        {
                            City = "SouthField",
                            Country = "US",
                            State = "MI",
                            Street = "21000 W 10 Mile Rd",
                            Zipcode = 48075
                        },
                        TrackingID = 2
                    }
                    }
                };
            }

            /*
             * Test Writing to file with prompt
             * var new_file_loc = FileHandlingInterface.AskUserForNewFileLocWithPrompt("TestLocation.txt", "Json Files (*.json)|*.json|Text Files (*.txt)|*.txt
             * WriteToFile(new_file_loc, o);
            */


            var curr = await GPSService.GetCurrentCoordinates();
            o.Packages = await Sorter.SortAsync(o.Packages, async (package) =>
            {
                package.DistanceFromPoint = await MapApi.GetDistanceAsync(package.Address, curr);
                return package;
            });

            PrintLocations(o.Packages);

            var point = await MapApi.GetAddressInPointFormAsync(o.Packages[0].Address);
            while (!Comp.IsWithin(point, 
                (await GPSService.GetCurrentCoordinates()), appSettings.GPSSettings.ErrorBounds))
            {
                var distance = await MapApi.GetDistanceAsync(o.Packages[0].Address, await GPSService.GetCurrentCoordinates());
                Console.WriteLine("Not there yet....");
                Console.WriteLine($"distance from destination {distance} mi");
                Console.WriteLine($"distance from destination {Comp.GetDistanceFromCoords(await GPSService.GetCurrentCoordinates(), point)}");
                await Task.Delay(1000);
            }


            var d = await MapApi.GetAddressFromCoordinate((await GPSService.GetCurrentCoordinates()));
            var url = MapApi.GenerateGoogleMapsUrl(d, o.Packages[2].Address);
            Console.WriteLine("You have arrived at your destintion");
        }

        void AddConfigurations(IConfigurationRoot root)
        {
            appSettings = root.Get<AppSettings>();
            //Initlizes the api with the bing key
            MapApi = new MapApi(appSettings.BingSettings.API_KEY);
            TestData = appSettings.TestData.TestLocations;
            StartingAddress = appSettings.TestData.StartingAddress;
            GPSService = new GPSService();
        }

        #region Printing to user
        void PrintLocations(List<Package> packages)
        {
            int i = 0;
            foreach (var package in packages)
            {
                Console.WriteLine($"{i}:\tid: {package.TrackingID}, address: {package.Address.Street} {package.Address.City},\n\t{package.Address.State} {package.Address.Zipcode}\n\tdistance: {package.DistanceFromPoint}");
                Console.WriteLine("\n\n");
                i++;
            }
        }
        #endregion Printing to user
    }
}
