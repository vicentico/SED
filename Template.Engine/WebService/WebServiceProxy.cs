using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Xml.Linq;

namespace IConstruye.BLL.WebService
{
    public class WebServiceProxy
    {
        public string ServiceUrl { get; set; }
        public string EndPointAction { get; set; }
        public Dictionary<string, string> Parameters = new Dictionary<string, string>();
        public XDocument ResultXml;
        public string ResultString;

        public WebServiceProxy(string serviceUrl, string endPointAction)
        {
            ServiceUrl = serviceUrl;
            EndPointAction = endPointAction;
        }

        public void Invocar(string soapRequest, ICredentials credenciales = null)
        {
            var tiempoEsperaServiciosWeb = 5; //Minutos
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(ServiceUrl);
            httpWebRequest.Timeout = tiempoEsperaServiciosWeb * 1000 * 60;

            if (credenciales != null)
            {
                httpWebRequest.CookieContainer = new CookieContainer();
                httpWebRequest.UseDefaultCredentials = true;
                httpWebRequest.Credentials = credenciales;
            }

            httpWebRequest.KeepAlive = true;
            httpWebRequest.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate");
            httpWebRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            httpWebRequest.Headers.Add("SOAPAction", $"\"{EndPointAction}\"");
            httpWebRequest.ContentType = "text/xml; charset=\"utf-8\"";
            httpWebRequest.Accept = "text/xml";
            httpWebRequest.Method = "POST";

            using (var stream = httpWebRequest.GetRequestStream())
            {
                using (var stmw = new StreamWriter(stream))
                {
                    stmw.Write(soapRequest);
                }
            }

            var responseStream = httpWebRequest.GetResponse().GetResponseStream();

            if (responseStream == null) return;
            using (var responseReader = new StreamReader(responseStream))
            {
                var result = responseReader.ReadToEnd();

                ResultXml = XDocument.Parse(result);
                ResultString = result;
            }
        }
    }
}
