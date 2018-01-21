using BusinessInterfaces;
using Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace DataAccessLayer
{
    public class UserService : IUserServices
    {
        public int AuthenticateUser(string userName, string password)
        {
            int user_id = -1;
            try
            {
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = ConfigurationManager.ConnectionStrings["HostedConnection"].ConnectionString;
                    conn.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = "AuthenticateUser";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = conn;
                    cmd.Parameters.Add(new SqlParameter("@UserName", userName));
                    cmd.Parameters.Add(new SqlParameter("@Password", password));

                    SqlDataReader reader = cmd.ExecuteReader();
                    List<UserModel> user = new List<UserModel>();
                    // Data is accessible through the DataReader object here.
                    if (reader.Read())
                    {
                        user_id = (int)reader["User_Id"];
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
            }

            return user_id;
        }

        public UserModel GetUserDetails(string mobileNumber)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = ConfigurationManager.ConnectionStrings["HostedConnection"].ConnectionString;
                    conn.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = "GetUserDetails";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = conn;
                    cmd.Parameters.Add(new SqlParameter("@MobileNumber", mobileNumber));
                    //cmd.Parameters.Add(new SqlParameter("@Password", pwd));

                    SqlDataReader reader = cmd.ExecuteReader();
                    List<UserModel> user = new List<UserModel>();
                    // Data is accessible through the DataReader object here.
                    while (reader.Read())
                    {
                        user.Add(new UserModel()
                        {
                            UserId = (int)reader["User_Id"],
                            UserName = (string)reader["UserName"],
                            Name = (string)reader["Name"],
                            LicenseNumber = (string)reader["LicenseNumber"],
                            VehicleType = (string)reader["VehicleType"],
                            ReferralCode = (string)reader["ReferralCode"],
                            MobileNumber = (string)reader["MobileNumber"],
                            ReferredBy = (string)reader["ReferredBy"],
                            CashbackEarned = (int)reader["CashbackEarned"]
                        });
                    }
                    conn.Close();
                    if (user.Count > 0)
                    {
                        return user[0];

                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public Status RegisterUserDetails(UserModel user)
        {
            Status status = new Status();
            if (user == null)
            {
                status.StatusCode = -1;
                status.StatusMessage = "Bad input";
                return status;
            }

            try
            {
                if (this.GetUserDetails(user.MobileNumber) != null)
                {
                    status.StatusCode = 2;
                    status.StatusMessage = "Already Registered";
                    return status;
                }
                else
                {
                    using (SqlConnection conn = new SqlConnection())
                    {
                        conn.ConnectionString = ConfigurationManager.ConnectionStrings["HostedConnection"].ConnectionString;
                        conn.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd.CommandText = "RegisterUserDetails";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = conn;
                        cmd.Parameters.Add(new SqlParameter("@Name", user.Name));
                        cmd.Parameters.Add(new SqlParameter("@UserName", user.UserName));
                        cmd.Parameters.Add(new SqlParameter("@Password", string.IsNullOrEmpty(user.Password) ? "" : user.Password));
                        cmd.Parameters.Add(new SqlParameter("@MobileNumber", user.MobileNumber));
                        cmd.Parameters.Add(new SqlParameter("@Gender", user.Gender));
                        cmd.Parameters.Add(new SqlParameter("@PaytmRegisteredNumber", user.PaytmRegisteredNumber));
                        cmd.Parameters.Add(new SqlParameter("@RegistrationType", user.RegistrationType));
                        cmd.Parameters.Add(new SqlParameter("@VehicleCategory", user.VehicleCategory));
                        cmd.Parameters.Add(new SqlParameter("@RegistrationNo", user.RegistrationNo));
                        cmd.Parameters.Add(new SqlParameter("@VehicleType", user.VehicleType));
                        cmd.Parameters.Add(new SqlParameter("@LicenseNumber", string.IsNullOrEmpty(user.LicenseNumber) ? "" : user.LicenseNumber));
                        cmd.Parameters.Add(new SqlParameter("@ReferredBy", user.ReferredBy));
                        cmd.Parameters.Add(new SqlParameter("@ReferralCode", GenerateCode(6, new Random())));
                        cmd.Parameters.Add(new SqlParameter("@CashbackEarned", user.CashbackEarned));

                        cmd.ExecuteNonQuery();
                        conn.Close();
                        status.StatusCode = 1;
                        status.StatusMessage = "Successs";
                        return status;
                    }
                }
            }
            catch (Exception ex)
            {
                status.StatusCode = 0;
                status.StatusMessage = "Failure";
                return status;
            }
        }
        /// <summary>
        /// Generates a unique code for every user registered user
        /// </summary>
        /// <param name="length"></param>
        /// <param name="random"></param>
        /// <returns></returns>
        private static string GenerateCode(int length, Random random)
        {
            string characters = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            StringBuilder result = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                result.Append(characters[random.Next(characters.Length)]);
            }
            return result.ToString();
        }

    }
}
