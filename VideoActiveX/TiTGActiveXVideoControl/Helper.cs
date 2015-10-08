using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Configuration;
using System.ServiceModel;
using TiTGActiveXVideoControl.ConfigurationService;
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

            //EndpointAddress endpointAddress = new EndpointAddress("http://biooffice/WcfSiteConfigurationService/ConfigurationService.svc");
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
            //return ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            return configurationServiceClient.getConnectionString(name);

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
