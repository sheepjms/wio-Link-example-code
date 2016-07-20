using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;
using System.IO;
using Newtonsoft.Json.Linq;

namespace Wolink
{
    class Wio
    {
        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true; //Always accept
        }
        private string Request(String url,String method,String body)
        {
            HttpWebRequest request =null;
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                request = WebRequest.Create(url) as HttpWebRequest;
                request.ProtocolVersion = HttpVersion.Version11;
            }
            else
            {
                request = WebRequest.Create(url) as HttpWebRequest;
            }
            request.Method = method;
            if (method == "POST" && body != null)
            {
                byte[] data = Encoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(body));
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }
            HttpWebResponse response = null;
            try
            {
                response = request.GetResponse() as HttpWebResponse;
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                return null;
            }
            Stream receiveStream = response.GetResponseStream();
            Encoding encode = System.Text.Encoding.GetEncoding("utf-8");
            StreamReader readStream = new StreamReader(receiveStream, encode);
            var responseBody = readStream.ReadToEnd();
            return responseBody;
        }
        public bool POST1()
        {
            //POST the parameter from your OUTPUT sensor  
            var url = "";//input the url from the API of Wio Link APP
            if (Request(url, "POST", null) != null)
                return true;
            return true;
        }
		
        public String Get1()
        {
            //Get data from the INPUT sensor
            var url = "";//input the url from the API of Wio Link APP
            var body=Request(url, "GET", null);
            if (body != null)
            {
                try {
                    var jobject = JObject.Parse(body);
                    var t=(float)jobject[""];//input the key word in the url 
                    return t.ToString();
                }
                catch (Exception e)
                {
                    Console.Write(e.Message);
                }
            }
            return null;
        }
       
        static void Main(string[] args)
        {
            Wio sensor1 = new Wio();
            Wio sensor2 = new Wio();
            var data1 = sensor1.Get1();
            var data2 = sensor2.Get1();
            sensor1.POST1();
            while (true)
            {
                if (data1 != null)
                {
                    var result1 = sensor1.POST(data1);
                    Console.WriteLine(result1);
                }
                if (data2 != null)
                {
                    var result2 = p2.SetScreenLine2(data2);
                    Console.WriteLine(result2);
                }
                System.Threading.Thread.Sleep(2000);
            }
           //Console.ReadKey();
        }
    }
}
