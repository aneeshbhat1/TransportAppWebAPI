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

namespace TransportAppWebAPI.Controllers
{
    [ApiAuthenticationFilter]
    public class TransportAppController : ApiController
    {
        private DALFactory factory = new DALFactory();
        // GET: api/TransportApp
        [HttpGet]
        [ActionName("GetUserDetails")]
        public IHttpActionResult GetUserDetails(string userName)
        {
            try
            {
                UserModel user = factory.UserService.GetUserDetails(userName);
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
                return factory.UserService.RegisterUserDetails(user) ;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}