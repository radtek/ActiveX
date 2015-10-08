using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
//using System.Data.SqlClient;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Windows.Forms;
using System.Web;

namespace DataServiceClient
{
    class DBUtil
    {
        string siteUrl = System.Configuration.ConfigurationManager.AppSettings["siteUrl"];
        string sitePicturePath = System.Configuration.ConfigurationManager.AppSettings["sitePicturePath"];

        //string dbPictureTable = System.Configuration.ConfigurationManager.AppSettings["dbPictureTable"];
        string dbPictureTable;
        string dbIdCol = System.Configuration.ConfigurationManager.AppSettings["dbIdCol"];
        string dbPictureCol = System.Configuration.ConfigurationManager.AppSettings["dbPictureCol"];
        string dbPictureTableWebService = System.Configuration.ConfigurationManager.AppSettings["dbPictureTableWebService"];
        string dbPictureColWebService = System.Configuration.ConfigurationManager.AppSettings["dbPictureColWebService"];

        internal DBUtil()
        {
            dbPictureTable = getAppSetting("dbPictureTable");
        }

        public byte[] GetPicture(int id)
        {
            SqlConnection conn = null;
            SqlCommand cmd = null;
            SqlDataReader reader = null;

            byte[] buffer = null;

            try
            {
                conn = new SqlConnection(getConnectionString());

                conn.Open();

                cmd = new SqlCommand();
                cmd.Connection = conn;

                cmd.CommandText = "SELECT " + dbPictureCol + " FROM " + dbPictureTable + " WHERE " + dbIdCol + " = @id";

                //cmd.Parameters.Add(new SqlCeParameter("@id", SqlDbType.Int));   // doesn't work
                cmd.Parameters.AddWithValue("@id", id);

                reader = cmd.ExecuteReader();
                //reader.Read();

                //SqlBinary binary;
                //SqlBytes bytes;

                //                if (reader.HasRows)   //Does not work for CE
                if (reader.Read())
                {
                    //if (!reader.IsDBNull(0))
                    //    id = reader.GetInt32(0);
                    if (!reader.IsDBNull(0))
                    {
                        //binary = reader.GetSqlBinary(1);
                        buffer = (byte[])reader[dbPictureCol]; //(byte[])reader["AppImage"];

                        //int maxSize = 200000;
                        //buffer = new byte[maxSize];
                        //reader.GetBytes(1, 0L, buffer, 0, maxSize);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                try
                {
                    if (reader != null)
                        reader.Close();

                    if (conn.State == ConnectionState.Open)
                        conn.Close();

                    if (conn != null)
                        conn = null;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

            return buffer;

        }

        public byte[] GetPictureFromWebService(int id)
        {
            //String url = "http://nomad.host22.com/kuwaitindex/bio_picture.php?id=" + id.ToString();

            String url = siteUrl + sitePicturePath;    // "kuwaitindex/bio_picture.php?id=";
            url += "?id=" + id.ToString();

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";

            byte[] bytes = null;
            using (Stream sm = request.GetResponse().GetResponseStream())
            {
                try
                {
                    //List<JsonResult> result = jsonStr.FromJson<List<JsonResult>>(s);

                    //StreamReader sr = new StreamReader(sm);
                    //String str = sr.ReadToEnd();
                    //sr.Close();
                    DataContractJsonSerializer serialiser = new DataContractJsonSerializer(typeof(List<JsonResult>));
                    List<JsonResult> result = serialiser.ReadObject(sm) as List<JsonResult>;
                    if (result.Count != 0)
                    {
                        if (result[0].result != null && result[0].result != "success")
                            throw new Exception(result[0].result);
                            //MessageBox.Show(result[0].result);
                        else
                        {
                            if (result[0].picture != null)
                            {
                                try
                                {
                                    bytes = System.Convert.FromBase64String(result[0].picture);
                                }
                                catch (Exception ex) { throw new Exception(ex.Message); }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            return bytes;
        }

        internal void SavePicture(int id, ref byte[] buffer)
        {
            SqlConnection conn = null;
            SqlCommand cmd = null;
            //object incrementId = null;
            try
            {
                //System.Windows.Forms.MessageBox.Show("connect str1: " + getConnectionString());
                conn = new SqlConnection(getConnectionString());

                conn.Open();

                cmd = new SqlCommand();
                cmd.Connection = conn;

                cmd.CommandText = String.Format(@"
                            begin tran
                                update {0} with (serializable) SET {1} = @picture where {2} = @id
                                if @@rowcount = 0 
                                begin
                                    insert into {0} ({2}, {1}) values (@id, @picture) 
                                end
                            commit tran ", dbPictureTable, dbPictureCol, dbIdCol);
/*
                cmd.CommandText = @"
                begin tran
                    update visitors with (serializable) SET picture = @picture where id = @id
                    if @@rowcount = 0 
                    begin
                        insert into visitors (id, picture) values (@id, @picture) 
                    end
                commit tran ";
*/
                cmd.Parameters.Add("@picture", SqlDbType.VarBinary);
                cmd.Parameters["@picture"].Value = buffer;

                cmd.Parameters.Add("@id", SqlDbType.Int);
                cmd.Parameters["@id"].Value = id;

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
                throw new Exception(ex.Message);
            }
            finally
            {
                try
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();

                    if (conn != null)
                        conn = null;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        internal void UploadPicture(int id, ref byte[] buffer)
        {
            String url = siteUrl + sitePicturePath;    // "kuwaitindex/bio_picture.php?id=";

            List<string> postData = new List<string>();
            //HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://nomad.host22.com/kuwaitindex/bio_picture.php");
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            postData.Add(HttpUtility.UrlEncode("id") + "=" + HttpUtility.UrlEncode(id.ToString()));

            // Convert the binary input into Base64 UUEncoded output.
            string base64String;
            try
            {
                base64String = System.Convert.ToBase64String(buffer, 0, buffer.Length);
            }
            catch (System.ArgumentNullException ex)
            {
                throw new Exception(ex.ToString());
            }

            postData.Add(HttpUtility.UrlEncode(dbPictureColWebService) + "=" + HttpUtility.UrlEncode(base64String.ToString()));
            string queryString = String.Join("&", postData.ToArray());
            byte[] byteArray = Encoding.UTF8.GetBytes(queryString);
            //write to stream 
            request.ContentLength = byteArray.Length;
            Stream s = request.GetRequestStream();
            s.Write(byteArray, 0, byteArray.Length);
            s.Close();

            //DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(JsonResult));

            using (Stream sm = request.GetResponse().GetResponseStream())
            {
                //StreamReader sr = new StreamReader(sm);
                //String jsonStr = sr.ReadToEnd(); 

                //string json = @"{""Name"" : ""My Product""}";
                //MemoryStream ms = new MemoryStream(Encoding.Unicode.GetBytes(json));
                try
                {
                    //List<JsonResult> result = jsonStr.FromJson<List<JsonResult>>(s);

                    DataContractJsonSerializer serialiser = new DataContractJsonSerializer(typeof(List<JsonResult>));
                    List<JsonResult> result = serialiser.ReadObject(sm) as List<JsonResult>;
                    if (result[0].result != "success")
                        MessageBox.Show(result[0].result);

                    //List<JsonResult> result = JSONHelper.Deserialise<List<JsonResult>>(jsonStr);
                    //JsonResult result = ser.ReadObject(sm) as JsonResult;
                    //MessageBox.Show("Result: " + result.result[0]);
                }
                catch (Exception) { }
            }
            /*
                        s = request.GetResponse().GetResponseStream(); 
                        StreamReader sr = new StreamReader(s); 
                        String str = sr.ReadToEnd(); 
                        sr.Close();
                        s.Close(); 
            */
        }

        internal void DeletePicures()
        {
            SqlConnection conn = null;
            SqlCommand cmd = null;

            try
            {
                conn = new SqlConnection(getConnectionString());
                conn.Open();

                cmd = new SqlCommand();
                cmd.Connection = conn;

                //cmd.CommandText = "DELETE FROM visitors";
                cmd.CommandText = "DELETE FROM " + dbPictureTable;

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                try
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();

                    if (conn != null)
                        conn = null;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

            //return buffer;

        }

        private String getConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        }

        private String getAppSetting(string key)
        {
            var setting = ConfigurationManager.AppSettings[key];
            // If we didn't find setting, try to load it from current dll's config file
            if (string.IsNullOrEmpty(setting))
            {
                var filename = System.Reflection.Assembly.GetExecutingAssembly().Location;
                var configuration = ConfigurationManager.OpenExeConfiguration(filename);
                if (configuration != null)
                    setting = configuration.AppSettings.Settings[key].Value;
            }

            return setting;
        }
    }
}
