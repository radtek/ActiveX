using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Configuration;
using System.ServiceModel;
using TiTGActiveXVideoControl.ConfigurationService;
using System;
//using System;

namespace GrabImageClient
{
    static class Helper
    {
        
        static ConfigurationServiceClient configurationServiceClient;

        static Helper() {
            //System.Diagnostics.Debug.Listeners.Add(new System.Diagnostics.TextWriterTraceListener(@"c:\temp\debug.log"));
            //System.Diagnostics.Debug.WriteLine("kuku", "Helper1::");

            //configurationServiceClient = new ConfigurationServiceClient();

            //BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.None);
            //binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
            //binding.Security.Message.ClientCredentialType = BasicHttpMessageCredentialType.UserName;

            BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.TransportCredentialOnly);
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Windows;

            binding.Name = "BasicHttpBinding_IConfigurationService";
            binding.MessageEncoding = WSMessageEncoding.Mtom;

            //EndpointAddress endpointAddress = new EndpointAddress("http://pscservice/CommonService/ConfigurationService.svc");
            var assemly = System.Reflection.Assembly.GetExecutingAssembly();
            var configuration = ConfigurationManager.OpenExeConfiguration(assemly.Location);
            string url = configuration.AppSettings.Settings["siteConfigurationServiceUrl"].Value;
            EndpointAddress endpointAddress = new EndpointAddress(url);
            configurationServiceClient = new ConfigurationServiceClient(binding, endpointAddress);
        }

        internal static void saveJpegToStream(out MemoryStream stream, Bitmap img, long quality)
        {
            stream = new MemoryStream();
            // Encoder parameter for image quality
            EncoderParameter qualityParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);

            // Jpeg image codec
            ImageCodecInfo jpegCodec = getEncoderInfo("image/jpeg");

            if (jpegCodec == null)
                return;

            EncoderParameters encoderParams = new EncoderParameters(1);
            encoderParams.Param[0] = qualityParam;

            img.Save(stream, jpegCodec, encoderParams);
        }

        internal static void saveJpegToFile(string path, Bitmap img, long quality)
        {
            // Encoder parameter for image quality
            EncoderParameter qualityParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);

            // Jpeg image codec
            ImageCodecInfo jpegCodec = getEncoderInfo("image/jpeg");

            if (jpegCodec == null)
                return;

            EncoderParameters encoderParams = new EncoderParameters(1);
            encoderParams.Param[0] = qualityParam;

            img.Save(path, jpegCodec, encoderParams);
        }

        private static ImageCodecInfo getEncoderInfo(string mimeType)
        {
            // Get image codecs for all image formats
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();

            // Find the correct image codec
            for (int i = 0; i < codecs.Length; i++)
                if (codecs[i].MimeType == mimeType)
                    return codecs[i];
            return null;
        }

        internal static string getAppSetting(string key)
        {
            //System.Diagnostics.Debug.WriteLine(key, "DEBUG2::");
            //var configurationServiceClient = new TiTGActiveXVideoControl.ConfigurationService.ConfigurationServiceClient();
            return configurationServiceClient.getAppSetting(key);

            //var setting = ConfigurationManager.AppSettings[key];
            //// If we didn't find setting, try to load it from current dll's config file
            //if (string.IsNullOrEmpty(setting))
            //{
            //    var assemly = System.Reflection.Assembly.GetExecutingAssembly();
            //    var configuration = ConfigurationManager.OpenExeConfiguration(assemly.Location);
            //    var val = configuration.AppSettings.Settings[key];
            //    if (val != null)
            //    {
            //        setting = val.Value;
            //    }
            //}
            //return setting;
        }

        internal static string getConnectionString(string name = "ConnectionString")
        {
            string conn = configurationServiceClient.getConnectionString(name);
            try
            {
                string server = "Server=";
                int index1 = conn.IndexOf(server);
                if (index1 == -1)
                {
                    server = "Data Source=";
                    index1 = conn.IndexOf(server);
                }

                if (index1 == -1)
                    throw new Exception("The database connection string is not valid");

                int index2 = conn.IndexOf(";", index1);

                int indx = index1 + server.Length;
                if (configurationServiceClient.Endpoint.Address.Uri.Host != "localhost" && (conn.Substring(indx, index2 - indx) == "(local)" || conn.Substring(indx, index2 - indx) == "localhost"))
                {
                    var str = new System.Text.StringBuilder();
                    str.Append(conn.Substring(0, indx));
                    str.Append(configurationServiceClient.Endpoint.Address.Uri.Host);
                    if (index2 != -1)
                        str.Append(conn.Substring(index2));
                    conn = str.ToString();
                }
                //System.Diagnostics.Debug.WriteLine("4: {0}: {1} us", conn2, stw.ElapsedTicks * 1000000 / System.Diagnostics.Stopwatch.Frequency);
                //System.Diagnostics.Debug.WriteLine("4: {0}: {1} ms", conn2, stw.Elapsed.TotalMilliseconds);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
                ///throw new Exception("Connection to a database is not available");
            }

            return conn;
            //return configurationServiceClient.getConnectionString(name);

            //return ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();


            //string connectionString = null;
            //var connectionStringSettings = ConfigurationManager.ConnectionStrings[name];
            //if (connectionStringSettings != null)
            //    connectionString = connectionStringSettings.ConnectionString;

            //// If we didn't find setting, try to load it from current dll's config file
            //if (string.IsNullOrEmpty(connectionString))
            //{
            //    var assemly = System.Reflection.Assembly.GetExecutingAssembly();
            //    var configuration = ConfigurationManager.OpenExeConfiguration(assemly.Location);
            //    var value = configuration.ConnectionStrings.ConnectionStrings[name];
            //    if (value != null)
            //    {
            //        connectionString = value.ConnectionString;
            //    }
            //}
            //return connectionString;
        }

    }
}
