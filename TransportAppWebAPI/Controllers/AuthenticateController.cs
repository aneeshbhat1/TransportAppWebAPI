using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TransportAppWebAPI.Filters;
using BusinessInterfaces;
using DALResolver;

namespace WebApi.Controllers
{
    [ApiAuthenticationFilter]
    public class AuthenticateController : ApiController
    {
        /// &lt;summary>
        /// Authenticates user and returns token with expiry.
        /// &lt;/summary>
        /// &lt;returns>&lt;/returns>
        //[POST("login")]
        //[POST("authenticate")]
        //[POST("get/token")]
        public HttpResponseMessage Authenticate()
        {
            if (System.Threading.Thread.CurrentPrincipal != null && System.Threading.Thread.CurrentPrincipal.Identity.IsAuthenticated)
            {
                var basicAuthenticationIdentity = System.Threading.Thread.CurrentPrincipal.Identity as BasicAuthenticationIdentity;
                if (basicAuthenticationIdentity != null)
                {
                    var userId = basicAuthenticationIdentity.UserId;
                    return GetAuthToken(userId);
                }
            }
            return null;
        }

        /// &lt;summary>
        /// Returns auth token for the validated user.
        /// &lt;/summary>
        /// &lt;param name="userId">&lt;/param>
        /// &lt;returns>&lt;/returns>
        private HttpResponseMessage GetAuthToken(int userId)
        {
            var token = DALFactory.Instance.TokenService.GenerateToken(userId);
            var response = Request.CreateResponse(HttpStatusCode.OK, "Authorized");
            response.Headers.Add("Token", token.Token);
            response.Headers.Add("TokenExpiry", ConfigurationManager.AppSettings["AuthTokenExpiry"]);
            response.Headers.Add("Access-Control-Expose-Headers", "Token,TokenExpiry");
            return response;
        }
    }
}