using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class UserModel
    {
        public int CashbackEarned { get; set; }
        public string LicenseNumber { get;  set; }
        public string MobileNumber { get;  set; }
        public string Name { get;  set; }
        public string ReferralCode { get;  set; }
        public string ReferredBy { get;  set; }
        public int UserId { get;  set; }
        public string UserName { get;  set; }
        public string VehicleType { get;  set; }
    }
}
