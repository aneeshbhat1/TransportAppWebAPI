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
    [AuthorizationRequired]
    public class TransportAppController : ApiController
    {
       [HttpGet]
       [ActionName("GetUserDetails")]
        public IHttpActionResult GetUserDetails(string userName,string password)
        {
            try
            {
                UserModel user = DALFactory.Instance.UserService.GetUserDetails(userName, password);
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
        public string RegisterUserDetails([FromBody]dynamic jsonString)
        {
            UserModel user = JsonConvert.DeserializeObject<UserModel>(jsonString.ToString());
            try
            {
                return DALFactory.Instance.UserService.RegisterUserDetails(user) ;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}