using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using TransportAppWebAPI.Models;

namespace TransportAppWebAPI.Controllers
{
    public class TransportAppController : ApiController
    {
        // GET: api/TransportApp
        [HttpGet]
        [ActionName("GetUserDetails")]
        public string GetUserDetails(string userName,string pwd)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = @"Server=ANEESH\SQLEXPRESS;Database=AneeshDatabase;User ID=ANEESH\Archana;Trusted_Connection=true";
                    conn.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = "GetUserDetails";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = conn;
                    cmd.Parameters.Add(new SqlParameter("@UserName", userName));
                    cmd.Parameters.Add(new SqlParameter("@Password", pwd));

                    SqlDataReader reader = cmd.ExecuteReader();
                    List<Users> user = new List<Users>() ;
                    // Data is accessible through the DataReader object here.
                    while (reader.Read())
                    {
                        user.Add( new Users()
                        {
                            UserId = (int)reader["User_Id"],
                            UserName = (string)reader["UserName"],
                            Name = (string)reader["Name"],
                            LicenseNumber = (string)reader["LicenseNumber"],
                            VehicleType = (string)reader["VehicleType"],
                            ReferralCode = (string)reader["ReferralCode"],
                            ReferredBy = (string)reader["ReferredBy"],
                            CashbackEarned = (int)reader["CashbackEarned"]
                        });
                    }
                    conn.Close();
                    if (user.Count > 0)
                    {
                        return JsonConvert.SerializeObject(user[0]);
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                return "Failure";
            }
        }

        // POST: api/TransportApp
        [HttpPost]
        [ActionName("RegisterUserDetails")]
        public string RegisterUserDetails([FromBody]dynamic jsonString)
        {
            Users user = JsonConvert.DeserializeObject<Users>(jsonString.ToString());
            try
            {
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = @"Server=ANEESH\SQLEXPRESS;Database=AneeshDatabase;User ID=ANEESH\Archana;Trusted_Connection=true";
                    conn.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = "RegisterUserDetails";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = conn;
                    cmd.Parameters.Add(new SqlParameter("@Name", user.Name));
                    cmd.Parameters.Add(new SqlParameter("@UserName", user.UserName));
                    cmd.Parameters.Add(new SqlParameter("@Password", string.IsNullOrEmpty(user.Password)?"": user.Password));
                    cmd.Parameters.Add(new SqlParameter("@MobileNumber", user.MobileNumber));
                    cmd.Parameters.Add(new SqlParameter("@VehicleType", user.VehicleType));
                    cmd.Parameters.Add(new SqlParameter("@LicenseNumber", string.IsNullOrEmpty(user.LicenseNumber) ? "" : user.LicenseNumber));
                    cmd.Parameters.Add(new SqlParameter("@ReferredBy", user.ReferredBy));
                    cmd.Parameters.Add(new SqlParameter("@ReferralCode", GenerateCode(6,new Random())));
                    cmd.Parameters.Add(new SqlParameter("@CashbackEarned", user.CashbackEarned));

                    cmd.ExecuteNonQuery();
                    conn.Close();
                    return "Success";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// Generates a unique code for every user registered user
        /// </summary>
        /// <param name="length"></param>
        /// <param name="random"></param>
        /// <returns></returns>
        public static string GenerateCode(int length, Random random)
        {
            string characters = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            StringBuilder result = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                result.Append(characters[random.Next(characters.Length)]);
            }
            return result.ToString();
        }

        //[HttpPost]
        //[ActionName("UpdateLocation")]
        //public void UpdateLocation(double latitude,double longitude,int user_id)
        //{
        //    try
        //    {
        //        using (SqlConnection conn = new SqlConnection())
        //        {
        //            conn.ConnectionString = @"Server=ANEESH\SQLEXPRESS;Database=AneeshDatabase;User ID=ANEESH\Archana;Trusted_Connection=true";
        //            conn.Open();
        //            SqlCommand cmd = new SqlCommand();
        //            cmd.CommandText = "UpdateLocation";
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.Connection = conn;
        //            cmd.Parameters.Add(new SqlParameter("@User_Id", user_id));
        //            cmd.Parameters.Add(new SqlParameter("@Latitude", latitude));
        //            cmd.Parameters.Add(new SqlParameter("@Longitude", longitude));

        //            cmd.ExecuteNonQuery();
        //            conn.Close();
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}

        //[HttpGet]
        //[ActionName("GetAllUsersLocation")]
        //public string GetAllUsersLocation()
        //{
        //    List<Location> locations = new List<Location>();
        //    try
        //    {
        //        using (SqlConnection conn = new SqlConnection())
        //        {
        //            conn.ConnectionString = @"Server=ANEESH\SQLEXPRESS;Database=AneeshDatabase;User ID=ANEESH\Archana;Trusted_Connection=true";
        //            conn.Open();
        //            SqlCommand cmd = new SqlCommand();
        //            cmd.CommandText = "UpdateLocation";
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.Connection = conn;
        //            SqlDataReader reader = cmd.ExecuteReader();
        //            // Data is accessible through the DataReader object here.
        //            while (reader.Read())
        //            {
        //                locations.Add(new Location()
        //                {
        //                    User_Id = (int)reader["User_Id"],
        //                    Latitude = (double)reader["Latitude"],
        //                    Longitude = (double)reader["Longitude"]
        //                });
        //            }
        //            conn.Close();
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }

        //    return JsonConvert.SerializeObject(locations);
        //}
    }
}
