using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
//using System.Data.SqlClient;
using System.Data;
using System.Data.SqlTypes;
using System.Data.Common;
using System.Data.SqlClient;
using System.Net;
using System.Collections;
using System.IO;
using System.Web;
using System.Runtime.Serialization.Json;
using System.Windows.Forms;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
//using PublicWCFServices;

namespace DataServiceClient
{
    class WebDataService : IDataService
    {
/*
        string siteUrl = System.Configuration.ConfigurationManager.AppSettings["siteUrl"];
        string sitePictureTablePath = System.Configuration.ConfigurationManager.AppSettings["sitePictureTablePath"];
        string siteFingerTablePath = System.Configuration.ConfigurationManager.AppSettings["siteFingerTablePath"];

        string dbPictureTableWebService = System.Configuration.ConfigurationManager.AppSettings["dbPictureTableWebService"];
        string dbFingerTableWebService = System.Configuration.ConfigurationManager.AppSettings["dbPictureTableWebService"];

        string dbIdColumnWebService = System.Configuration.ConfigurationManager.AppSettings["dbIdColumnWebService"];
        string dbPictureColumnWebService = System.Configuration.ConfigurationManager.AppSettings["dbPictureColumnWebService"];
        string dbFingerColumnWebService = System.Configuration.ConfigurationManager.AppSettings["dbFingerColumnWebService"];
*/
        string siteUrl = GrabImageClient.Helper.getAppSetting("siteUrl");
        string sitePictureTablePath = GrabImageClient.Helper.getAppSetting("sitePictureTablePath");
        string siteFingerTablePath = GrabImageClient.Helper.getAppSetting("siteFingerTablePath");

        string dbPictureTableWebService = GrabImageClient.Helper.getAppSetting("dbPictureTableWebService");
        string dbFingerTableWebService = GrabImageClient.Helper.getAppSetting("dbPictureTableWebService");

        string dbIdColumnWebService = GrabImageClient.Helper.getAppSetting("dbIdColumnWebService");
        string dbPictureColumnWebService = GrabImageClient.Helper.getAppSetting("dbPictureColumnWebService");
        string dbFingerColumnWebService = GrabImageClient.Helper.getAppSetting("dbFingerColumnWebService");
/*
        internal WebDataService()
        {
            siteUrl = GrabImageClient.Helper.getAppSetting("siteUrl");
            sitePictureTablePath = getAppSetting("sitePictureTablePath");
            siteFingerTablePath = getAppSetting("siteFingerTablePath");

            dbPictureTableWebService = getAppSetting("dbPictureTableWebService");
            dbFingerTableWebService = getAppSetting("dbPictureTableWebService");

            dbIdColumnWebService = getAppSetting("dbIdColumnWebService");
            dbPictureColumnWebService = getAppSetting("dbPictureColumnWebService");
            dbFingerColumnWebService = getAppSetting("dbFingerColumnWebService");

        }
*/
        public byte[] GetImage(IMAGE_TYPE imageType, int id)
        {
            //String url = "http://nomad.host22.com/kuwaitindex/bio_picture.php?id=" + id.ToString();
            String url;

            //throw new Exception(siteUrl);

            if (imageType == IMAGE_TYPE.picture)
                url = siteUrl + sitePictureTablePath;    // "kuwaitindex/bio_picture.php?id=";
            else
                url = siteUrl + siteFingerTablePath;    // "kuwaitindex/bio_wsq.php?id=";

            //throw new Exception(url);

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
                            try
                            {
                                if (imageType == IMAGE_TYPE.picture)
                                {
                                    if (result[0].picture != null)
                                        bytes = System.Convert.FromBase64String(result[0].picture);
                                }
                                else
                                {
                                    if (result[0].wsq != null)
                                        bytes = System.Convert.FromBase64String(result[0].wsq);
                                }
                            }
                            catch (Exception ex) { throw new Exception(ex.Message); }
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

        public void SendImage(IMAGE_TYPE imageType, int id, ref byte[] buffer)
        {
            //String url = siteUrl + sitePicturePath;    // "kuwaitindex/bio_picture.php?id=";
            String url;

            if (imageType == IMAGE_TYPE.picture)
                url = siteUrl + sitePictureTablePath;    // "kuwaitindex/bio_picture.php?id=";
            else
                url = siteUrl + siteFingerTablePath;    // "kuwaitindex/bio_wsq.php?id=";

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

            if (imageType == IMAGE_TYPE.picture)
                postData.Add(HttpUtility.UrlEncode(dbPictureColumnWebService) + "=" + HttpUtility.UrlEncode(base64String.ToString()));
            else
                postData.Add(HttpUtility.UrlEncode(dbFingerColumnWebService) + "=" + HttpUtility.UrlEncode(base64String.ToString()));

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
                        throw new Exception(result[0].result);

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
/*
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
*/
    }
}
