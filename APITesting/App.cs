using APITesting.Models;
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
            var coords = (await GPSService.GetCurrentCoordinates());
            var destination = await MapApi.GetAddressInPointFormAsync(StartingAddress);

            var o = ReadFromFile(FileHandlingInterface.AskUserForFileLocWithPrompt("Json Files (*.json)|*.json|Text Files (*.txt)|*.txt"))
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


            while (!IsWithin(coords, destination))
            {
                coords = (await GPSService.GetCurrentCoordinates());
            };

            var new_file_loc = FileHandlingInterface.AskUserForNewFileLocWithPrompt("TestLocation.txt", "Json Files (*.json)|*.json|Text Files (*.txt)|*.txt");

            WriteToFile(new_file_loc, o);


            Console.WriteLine("Arrived");
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

        #region Compair Functions
        //Checks to see if the current position, and the expected positon is within the margin
        //that is defined within the appsettings.json
        bool IsWithin(Coordinate ExpectedPosition, Coordinate CurrentPosition)
        {
            return (Math.Abs(ExpectedPosition.Latitude - CurrentPosition.Latitude) 
                        <= appSettings.GPSSettings.ErrorBounds.Latitude)
                            && (Math.Abs(ExpectedPosition.Longitude - CurrentPosition.Longitude) 
                                <= appSettings.GPSSettings.ErrorBounds.Longitude);
        }
        #endregion Compair Functions

        #region Read/Write to File
        void WriteToFile(string fileLocation, JsonSerializedDataObject o)
        {
            using (TextWriter file = new StreamWriter(fileLocation))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, o);
            }
        }
        JsonSerializedDataObject ReadFromFile(string fileLocation)
        {
            try
            {
                using (StreamReader file = File.OpenText(fileLocation))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    var obj = (JsonSerializedDataObject)serializer.Deserialize(file, typeof(JsonSerializedDataObject));
                    return obj.Packages == null ? null : obj;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion Read/Write to File


    }
}
