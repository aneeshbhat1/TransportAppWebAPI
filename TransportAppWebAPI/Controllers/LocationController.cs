using DALResolver;
using Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace TransportAppWebAPI.Controllers
{
    public class LocationController : ApiController
    {
        [System.Web.Http.ActionName("GetLocation")]
        public IHttpActionResult GetLocation(string userName)
        {
            Location userLocation = DALFactory.Instance.LocationService.GetLocation(userName);
            return this.JsonString(JsonConvert.SerializeObject(userLocation), HttpStatusCode.OK);
        }
        [System.Web.Http.ActionName("GetAllUsersLocations")]
        public IHttpActionResult GetAllUsersLocations()
        {
            List<Location> locations = DALFactory.Instance.LocationService.GetAllUsersLocation();
            return this.JsonString(JsonConvert.SerializeObject(locations), HttpStatusCode.OK);
        }

        public IHttpActionResult UpdateLocation(double latitude, double longitude, string userName)
        {
            Status status = DALFactory.Instance.LocationService.UpdateLocation(latitude, longitude, userName);
            if(status.StatusCode == 1)
            {
                return this.JsonString(JsonConvert.SerializeObject(status), HttpStatusCode.OK);
            }
            else
            {
                return this.JsonString(JsonConvert.SerializeObject(status), HttpStatusCode.ExpectationFailed);
            }
        }
    }
}