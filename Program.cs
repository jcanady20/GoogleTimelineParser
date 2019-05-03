using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using CourtCase.Internal;
using CourtCase.Internal.Extensions;
using CourtCase.Internal.Models;

namespace CourtCase
{
    class Program
    {
        static void Main(string[] args)
        {
            var filePath = "./LocationHistory.json";
            if (File.Exists(filePath) == false)
            {
                Console.WriteLine($"Unable to locate the specified file - {filePath}");
                return;
            }
            //  Load the json data into a collection that we can use
            var locations = LoadLocationData(filePath);
            //  Filter Results for April 4th between 2pm and 3pm
            var positions = FilterLocations(locations);
            //  Process the results.
            ProcessPositions(positions);
        }
        static void ProcessPositions(IReadOnlyList<Location> positions)
        {
            var firstPosition = positions.FirstOrDefault();
            if (firstPosition == null)
            {
                Console.WriteLine("Unable to obtain the first position");
                return;
            }
            for (var i = 1; i < positions.Count; i++)
            {
                var secondPosition = positions[i];
                var distance = firstPosition.DistanceTo(secondPosition) / 1609.344;
                var time = secondPosition.Date.Subtract(firstPosition.Date);
                var speed = distance / time.TotalHours;
                WriteOutput(firstPosition.Timestamp, firstPosition.Date, distance, time, speed);
                firstPosition = secondPosition;
            }
        }
        static double CalculateDistance(Location firstLocation, Location secondLocation)
        {
            var fcoord = new GeoCoordinate(firstLocation.Latitude, firstLocation.Longitude);
            var scoord = new GeoCoordinate(secondLocation.Latitude, secondLocation.Longitude);
            var distance = fcoord.GetDistanceTo(scoord);
            return distance;
        }
        static void WriteOutput(long timeStamp, DateTime date, double distance, TimeSpan time, double speed)
        {
            //Console.Write($"{timeStamp} ");
            Console.Write($"{(date.ToString("hh:mm:ss"))} ");
            Console.Write($"Distance [{distance}] ");
            Console.Write($"Time [{(time.ToString("hh\\:mm\\:ss"))}] ");
            Console.WriteLine($"Speed [{speed}] ");
        }
        static IReadOnlyList<Internal.Models.Location> LoadLocationData(string filePath)
        {
            var json = File.ReadAllText(filePath);
            var locationCollection = JsonConvert.DeserializeObject<Internal.Models.LocationCollection>(json);
            var locations = locationCollection.Locations;
            Console.WriteLine($"Loading {locations.Count} locations");
            return locations;
        }
        static IReadOnlyList<Internal.Models.Location> FilterLocations(IReadOnlyList<Location> locations)
        {
            var beginDate = new DateTime(2019, 4, 4, 14, 0, 0);
            var endDate = new DateTime(2019, 4, 4, 15, 0, 0);
            var positions = locations.Where(x => x.Date.IsBetween(beginDate, endDate)).ToList();
            Console.WriteLine($"Found {positions.Count} locations that match the filter");
            return positions.AsReadOnly();
        }
    }
}
