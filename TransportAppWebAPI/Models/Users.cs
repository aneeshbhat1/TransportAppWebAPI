using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TransportAppWebAPI.Models
{
    public class Users
    {
        public String Name { get; set; }
        public int UserId { get; set; }
        public String UserName { get; set; }
        public string Password { get; set; }
        public string LicenseNumber { get; set; }
        public string VehicleType { get; set; }
        public string ReferredBy { get; set; }
        public int CashbackEarned { get; set; }
        public string MobileNumber { get; set; }
        public string ReferralCode { get; set; }
    }
}