using BusinessInterfaces;
using Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class TokenService:ITokenService
    {
        private List<TokenModel> tokens;

        public TokenService()
        {
            tokens = this.GetAllTokens();
        }

        #region Public member methods.

        /// <summary>
        ///  Function to generate unique token with expiry against the provided userId.
        ///  Also add a record in database for generated token.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public TokenModel GenerateToken(int userId)
        {
            string token = Guid.NewGuid().ToString();
            DateTime issuedOn = DateTime.Now;
            DateTime expiredOn = DateTime.Now.AddSeconds(
            Convert.ToDouble(ConfigurationManager.AppSettings["AuthTokenExpiry"]));
            var tokenModel = new TokenModel
            {
                User_Id = userId,
                Token = token,
                IssuedOn = issuedOn,
                ExpiresOn = expiredOn
            };
            tokens.Add(tokenModel);
            try
            {
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = ConfigurationManager.ConnectionStrings["DEVConnection"].ConnectionString;
                    conn.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = "SaveTokenData";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = conn;
                    cmd.Parameters.Add(new SqlParameter("@User_Id", userId));
                    cmd.Parameters.Add(new SqlParameter("@IssuedOn", issuedOn));
                    cmd.Parameters.Add(new SqlParameter("@ExpiresOn", expiredOn));
                    cmd.Parameters.Add(new SqlParameter("@AuthToken", token));

                    cmd.ExecuteNonQuery();
                    conn.Close();

                    return tokenModel;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Method to validate token against expiry and existence in database.
        /// </summary>
        /// <param name="tokenId"></param>
        /// <returns></returns>
        public bool ValidateToken(string tokenId)
        {   
            try
            {
                if(tokens.Any(token=>token.Token == tokenId))
                {
                    TokenModel tokenModel = tokens.First(token => token.Token == tokenId);
                    if (tokenModel != null && !(DateTime.Now > tokenModel.ExpiresOn))
                    {
                        tokenModel.ExpiresOn = tokenModel.ExpiresOn.AddSeconds(
                        Convert.ToDouble(ConfigurationManager.AppSettings["AuthTokenExpiry"]));
                        using (SqlConnection conn = new SqlConnection())
                        {
                            conn.ConnectionString = ConfigurationManager.ConnectionStrings["DEVConnection"].ConnectionString;
                            conn.Open();
                            SqlCommand cmd = new SqlCommand();
                            cmd.CommandText = "UpdateTokenData";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Connection = conn;
                            cmd.Parameters.Add(new SqlParameter("@AuthToken", tokenId));
                            cmd.Parameters.Add(new SqlParameter("@ExpiresOn", tokenModel.ExpiresOn));
                            cmd.ExecuteNonQuery();
                            conn.Close();
                        }
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return false;
        }

        /// <summary>
        /// Method to kill the provided token id.
        /// </summary>
        /// <param name="tokenId">true for successful delete</param>
        public bool DeleteByTokenId(string tokenId)
        {
            try
            {
                if (tokens.Any(token => token.Token == tokenId))
                {
                    TokenModel tokenModel = tokens.First(token => token.Token == tokenId);
                    tokens.Remove(tokenModel);
                    if (tokenModel != null)
                    {
                        using (SqlConnection conn = new SqlConnection())
                        {
                            conn.ConnectionString = ConfigurationManager.ConnectionStrings["DEVConnection"].ConnectionString;
                            conn.Open();
                            SqlCommand cmd = new SqlCommand();
                            cmd.CommandText = "RemoveTokenDataByTokenId";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Connection = conn;
                            cmd.Parameters.Add(new SqlParameter("@AuthToken", tokenId));
                            cmd.ExecuteNonQuery();
                            conn.Close();
                        }
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return false;
        }

        /// <summary>
        /// Delete tokens for the specific deleted user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>true for successful delete</returns>
        public bool DeleteByUserId(int userId)
        {
            try
            {
                if (tokens.Any(token => token.User_Id == userId))
                {
                    TokenModel tokenModel = tokens.First(token => token.User_Id == userId);
                    tokens.Remove(tokenModel);
                    if (tokenModel != null)
                    {
                        using (SqlConnection conn = new SqlConnection())
                        {
                            conn.ConnectionString = ConfigurationManager.ConnectionStrings["DEVConnection"].ConnectionString;
                            conn.Open();
                            SqlCommand cmd = new SqlCommand();
                            cmd.CommandText = "RemoveTokenDataByUserId";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Connection = conn;
                            cmd.Parameters.Add(new SqlParameter("@User_Id", userId));
                            cmd.ExecuteNonQuery();
                            conn.Close();
                        }
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return false;
        }

        #endregion

        private List<TokenModel> GetAllTokens()
        {
            List<TokenModel> tokens = new List<TokenModel>();
            try
            {
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = ConfigurationManager.ConnectionStrings["DEVConnection"].ConnectionString;
                    conn.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = "GetTokenData";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = conn;
                    SqlDataReader reader = cmd.ExecuteReader();
                    // Data is accessible through the DataReader object here.
                    while (reader.Read())
                    {
                        tokens.Add(new TokenModel()
                        {
                            User_Id = (int)reader["User_Id"],
                            ExpiresOn = (DateTime)reader["ExpiresOn"],
                            IssuedOn = (DateTime)reader["IssuedOn"],
                            Token = (string)reader["AuthToken"],
                        });
                    }

                    conn.Close();
                    return tokens;
                }
            }
            catch (Exception ex)
            {
                return null;
            }

        }
    }
}
