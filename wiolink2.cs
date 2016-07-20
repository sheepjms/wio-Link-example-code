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
    class Program
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
        public bool ClearScreen()
        {
            //Clear the whole screen  
            var url = "";
            if (Request(url, "POST", null) != null)
                return true;
            return true;
        }
 /*       public String GetHumidity()
        {
            //Get humidity data
            var url = "https://cn.iot.seeed.cc/v1/node/GroveTempHumD0/humidity?access_token=6ae2f43ab4ce1a25557ba1c865723a97";
            var body=Request(url, "GET", null);
            if (body != null)
            {
                try {
                    var jobject = JObject.Parse(body);
                    var t=(float)jobject["humidity"];
                    return t.ToString();
                }
                catch (Exception e)
                {
                    Console.Write(e.Message);
                }
            }
            return null;
        }*/
        public String GetDuty()
        {
            //Get duty precent
            var url = "";
            var body = Request(url, "GET", null);
            if (body != null)
            {
                try
                {
                    var jobject = JObject.Parse(body);
                    var t = (float)jobject["duty_percent"];
                    return t.ToString();
                }
                catch (Exception e)
                {
                    Console.Write(e.Message);
                }
            }
            return null;
        }

        public String SendLedStrip()
        {
            //Get Freq
            var url = "";
            var body = Request(url, "GET", null);
            if (body != null)
            {
                try
                {
                    var jobject = JObject.Parse(body);
                    var t = (float)jobject["freq"];
                    return t.ToString();
                }
                catch (Exception e)
                {
                    Console.Write(e.Message);
                }
            }
            return null;
        }
        /*
        public String GetTemperature()
        {
            //Get celsius_degree data
            var url = "https://cn.iot.seeed.cc/v1/node/GroveTempHumD0/temperature?access_token=6ae2f43ab4ce1a25557ba1c865723a97";
            var body = Request(url, "GET", null);
            if (body != null)
            {
                try
                {
                    var jobject = JObject.Parse(body);
                    var t = (float)jobject["celsius_degree"];
                    return t.ToString();
                }
                catch (Exception e)
                {
                    Console.Write(e.Message);
                }
            }
            return null;
        }
         * */
        public bool ledstrip1()
        {
            //Display data in the first line of screen
            var url = String.Format("https://iot.seeed.cc/v1/node/GroveLedWs2812D1/clear/30/000011?access_token=ecefcecd3c462ec5bc3d74e29da67c8a");
            if (Request(url, "POST", null) != null)
                return true;
            return false;
        }
        public bool SetScreenLine2(String h)
        {
            //Display data in the second line of screen
            var url = String.Format("", h);
            if (Request(url, "POST", null) != null)
                return true;
            return false;
        }
        static void Main(string[] args)
        {
       
            /*
            var humidity = p1.GetHumidity();
            var temperature = p2.GetTemperature();
             * */
            Program p3 = new Program();
           
            while (true)
            {

                p3.ledstrip1();
                System.Threading.Thread.Sleep(2000);
            }
           //Console.ReadKey();
        }
    }
}
