using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using DALResolver;
using TransportAppWebAPI.Filters;
using Models;
using Newtonsoft.Json;
using System.Net;
using TransportAppWebAPI.ActionFilters;

namespace TransportAppWebAPI.Controllers
{
    
    public class TransportAppController : ApiController
    {
        public String Get()
        {
            return "Hi, I am up and running!";
        }

        [HttpGet]
        [ActionName("GetUserDetails")]
        [AuthorizationRequired]
        public IHttpActionResult GetUserDetails(string mobileNumber)
        {
            try
            {
                UserModel user = DALFactory.Instance.UserService.GetUserDetails(mobileNumber);
                if (user != null)
                {
                    return this.JsonString(JsonConvert.SerializeObject(user), HttpStatusCode.OK);
                }
                else
                {
                    return this.JsonString("", HttpStatusCode.NotFound);
                }
            }
            catch (Exception ex)
            {
                return this.JsonString(ex.Message, HttpStatusCode.BadRequest); ;
            }
        }

        // POST: api/TransportApp
        [HttpPost]
        [ActionName("RegisterUserDetails")]
        public IHttpActionResult RegisterUserDetails([FromBody]dynamic jsonString)
        {
            UserModel user = JsonConvert.DeserializeObject<UserModel>(jsonString.ToString());
            try
            {
                Status status = DALFactory.Instance.UserService.RegisterUserDetails(user) ;
                if(status.StatusCode == 1)
                {
                    return this.JsonString(JsonConvert.SerializeObject(status), HttpStatusCode.OK);
                }
                else if(status.StatusCode == -1)
                {
                    return this.JsonString(JsonConvert.SerializeObject(status), HttpStatusCode.BadRequest);
                }
                else if (status.StatusCode == 2)
                {
                    return this.JsonString(JsonConvert.SerializeObject(status), HttpStatusCode.Conflict);
                }
                else
                {
                    return this.JsonString(JsonConvert.SerializeObject(status), HttpStatusCode.ExpectationFailed);
                }
            }
            catch (Exception ex)
            {
                return this.JsonString(ex.Message, HttpStatusCode.BadRequest);
            }
        }
    }
}