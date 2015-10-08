using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataServiceClient
{
    public enum IMAGE_TYPE
    {
        picture = 0,
        wsq = 1
    }

    public interface IDataService
    {
        byte[] GetImage(IMAGE_TYPE imageType, int id);
        void SendImage(IMAGE_TYPE imageType, int id, ref byte[] buffer);
    }

    public class JsonResult
    {
#pragma warning disable 0649    //warning CS0649: Field 'DataSourceServices.JsonResult.result' is never assigned to, and will always have its default value null
        public string result;
        public string picture;
        public string wsq;
#pragma warning restore 0649
    }

}
