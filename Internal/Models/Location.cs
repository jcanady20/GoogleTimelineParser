using System;
using Newtonsoft.Json;

namespace CourtCase.Internal.Models
{
    public class Location
    {
        private readonly static double _divider = 10000000.00;

        public Location()
        { }

        [JsonProperty("timestampMs")]
        public long Timestamp { get; set; }
        [JsonProperty("latitudeE7")]
        public long LatitudeE7 { get; set; }
        [JsonProperty("longitudeE7")]
        public long LongitudeE7 { get; set; }
        public int Accuracy { get; set; }
        public int Velocity { get; set; }
        public int Heading { get; set; }
        public int Altitude { get; set; }
        public int VerticalAccuracy { get; set; }
        public DateTime Date => DateTimeOffset.FromUnixTimeMilliseconds(Timestamp).DateTime.AddHours(-5);
        public double Longitude => ((LongitudeE7 > 1800000000) ? (LongitudeE7- 4294967296) : LongitudeE7) / _divider;
        public double Latitude => ((LatitudeE7 > 900000000) ? (LatitudeE7 - 4294967296) : LatitudeE7) / _divider;
        public GeoCoordinate GeoCoordinate => new GeoCoordinate(Latitude, Longitude);
        public double DistanceTo(Location location) => this.GeoCoordinate.GetDistanceTo(location.GeoCoordinate);

        public override string ToString()
        {
            return $"{Latitude}, {Longitude}";
        }
    }
}