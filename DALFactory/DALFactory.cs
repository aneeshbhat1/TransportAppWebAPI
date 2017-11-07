using BusinessInterfaces;
using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DALResolver
{
    public class DALFactory
    {
        private IUserServices userService;
        private ILocationServices locationService;
        private ITokenService tokenService;

        public IUserServices UserService
        {
            get
            {
                if(userService == null)
                {
                    userService = new UserService();
                }

                return userService;
            }
        }

        public ILocationServices LocationService
        {
            get
            {
                if(locationService == null)
                {
                    locationService = new LocationService();
                }
                return locationService;
            }
        }

        public ITokenService TokenService
        {
            get
            {
                if(tokenService == null)
                {
                    tokenService = new TokenService();
                }
                return tokenService;
            }
        }
    }
}
