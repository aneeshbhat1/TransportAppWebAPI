using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessInterfaces
{
    public interface ILocationServices
    {
        Location GetLocation(string userName);
        List<Location> GetAllUsersLocation();
        Status UpdateLocation(double latitude, double longitude, string userName);
    }
}
