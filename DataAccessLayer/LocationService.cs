using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessInterfaces;
using System.Web.Http;
using System.Data.SqlClient;
using System.Data;
using Models;
using System.Configuration;
using Newtonsoft.Json;

namespace DataAccessLayer
{
    public class LocationService : ILocationServices
    {
        [HttpPost]
        [ActionName("UpdateLocation")]
        public Status UpdateLocation(double latitude, double longitude, string userName)
        {
            Status status = new Status();
            try
            {
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = ConfigurationManager.ConnectionStrings["DEVConnection"].ConnectionString;
                    conn.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = "UpdateLocation";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = conn;
                    cmd.Parameters.Add(new SqlParameter("@UserName", userName));
                    cmd.Parameters.Add(new SqlParameter("@Latitude", latitude));
                    cmd.Parameters.Add(new SqlParameter("@Longitude", longitude));

                    cmd.ExecuteNonQuery();
                    conn.Close();
                    status.StatusCode = 1;
                    status.StatusMessage = "Sucess";
                    return status;
                }
            }
            catch (Exception ex)
            {
                status.StatusCode = 0;
                status.StatusMessage = "Failure";
                return status;
            }
        }

        [HttpGet]
        [ActionName("GetAllUsersLocation")]
        public List<Location> GetAllUsersLocation()
        {
            List<Location> locations = new List<Location>();
            try
            {
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = ConfigurationManager.ConnectionStrings["DEVConnection"].ConnectionString;
                    conn.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = "GetLocations";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = conn;
                    SqlDataReader reader = cmd.ExecuteReader();
                    // Data is accessible through the DataReader object here.
                    while (reader.Read())
                    {
                        locations.Add(new Location()
                        {
                            UserName = (string)reader["UserName"],
                            Latitude = (double)reader["Latitude"],
                            Longitude = (double)reader["Longitude"]
                        });
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {

            }

            return locations;
        }

        public Location GetLocation(string userName)
        {
            Location userLocation = null;
            try
            {
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = ConfigurationManager.ConnectionStrings["DEVConnection"].ConnectionString;
                    conn.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = "GetLocations";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = conn;
                    SqlDataReader reader = cmd.ExecuteReader();
                    // Data is accessible through the DataReader object here.
                    if (reader.Read())
                    {
                        userLocation = new Location()
                        {
                            UserName = (string)reader["UserName"],
                            Latitude = (double)reader["Latitude"],
                            Longitude = (double)reader["Longitude"]
                        };
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {

            }

            return userLocation;
        }
    }
}
