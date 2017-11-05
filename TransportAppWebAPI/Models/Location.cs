using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TransportAppWebAPI.Models
{
    public class Location
    {
        public int User_Id { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}