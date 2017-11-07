using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessInterfaces
{
    public interface ILocationServices
    {
        string GetAllUsersLocation();
        void UpdateLocation(double latitude, double longitude, int user_id);
    }
}
