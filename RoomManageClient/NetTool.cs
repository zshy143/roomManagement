using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace hotleManagement
{
    class NetTool
    {
          public static string baseUrl = "";
          
          public static string PostUrl(string url,string postData)
        {
            string result = "";
            byte[] data = Encoding.UTF8.GetBytes(postData);
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "POST";
            req.Timeout = 800;
            req.ContentType = "application/json";
            req.ContentLength = data.Length;
            using (Stream reqStream = req.GetRequestStream())
            {
                reqStream.Write(data, 0, data.Length);
                reqStream.Close();
            }
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            Stream stream = resp.GetResponseStream();
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                result = reader.ReadToEnd();
            }
            stream.Close();

            return result;
        }
           public static String GetUrl(String url,Encoding encode)
        {

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "text/html,application/xhtml+xml,*/*";
            HttpWebResponse resp = (HttpWebResponse)request.GetResponse();
            Stream rs = resp.GetResponseStream();
            StreamReader sr = new StreamReader(rs, encode);
            var result = sr.ReadToEnd();
            sr.Close(); 
            rs.Close();
            return result;
        }
    }
}
