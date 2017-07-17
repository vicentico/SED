using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Template.Engine.XML
{
    public class XmlHelper
    {
        #region Serializer.

        public static string Serializar(object Objeto)
        {
            StreamReader StreamReader = null;
            MemoryStream MemoryStream = null;
            var XmlSerializer = new XmlSerializer(Objeto.GetType());

            try
            {
                MemoryStream = new MemoryStream();
                XmlSerializer.Serialize(MemoryStream, Objeto);
                MemoryStream.Seek(0, SeekOrigin.Begin);
                StreamReader = new StreamReader(MemoryStream);

                var Xml = StreamReader.ReadToEnd();

                #region Reemplazos.

                Xml = Xml.Replace(" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", "");
                Xml = Xml.Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"", "");
                Xml = Xml.Replace(" xsi:type=\"xsd:string\"", "");
                Xml = Xml.Replace(" xsi:type=\"xsd:dateTime\"", "");
                Xml = Xml.Replace(" xsi:type=\"xsd:int\"", "");
                Xml = Xml.Replace(" xsi:type=\"xsd:boolean\"", "");
                Xml = Xml.Replace(" xsi:type=\"xsd:unsignedLong\"", "");
                Xml = Xml.Replace(" xsi:type=\"xsd:decimal\"", "");

                #endregion

                return Xml;
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al Serializar XML", Ex);
            }
            finally
            {
                if (StreamReader != null) StreamReader.Dispose();
                if (MemoryStream != null) MemoryStream.Dispose();
            }
        }

        #endregion

        #region Schema Validation.

        public static bool ValidateSchema(string Xml, string Xsd, string NameSpace)
        {
            try
            {
                if (string.IsNullOrEmpty(Xml)) throw new Exception("Documento XML no puede ser una cadena vacía.");
                if (string.IsNullOrEmpty(Xsd)) throw new Exception("Esquema XSD no puede ser una cadena vacía.");
                if (string.IsNullOrEmpty(NameSpace))
                    throw new Exception("Espacio de Nombre no puede ser una cadena vacía.");

                var StringReader = new StringReader(Xsd);
                var XmlReader = System.Xml.XmlReader.Create(StringReader);
                var XDocument = System.Xml.Linq.XDocument.Parse(Xml);

                var XmlSchemaSet = new XmlSchemaSet();

                XmlSchemaSet.Add(NameSpace, XmlReader);

                try
                {
                    XDocument.Validate(XmlSchemaSet, null);
                }
                catch (XmlSchemaValidationException)
                {
                    return false;
                }

                return true;
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al Validar Esquema del XML.", Ex);
            }
        }

        #endregion.

        #region Signature Validation.

        public static bool ValidateSignature(XmlDocument XmlDocument, string NodoAValidar, int Indice)
        {
            var Flag = false;
            var XmlNodeList = XmlDocument.GetElementsByTagName(NodoAValidar);
            var XmlDocument2 = new XmlDocument();
            var XmlDocument3 = new XmlDocument();
            RSA Rsa = null;

            if (Indice > XmlNodeList.Count) throw new IndexOutOfRangeException();

            var XmlNamespaceManager = new XmlNamespaceManager(XmlDocument.NameTable);
            XmlNamespaceManager.AddNamespace("xmldsg", "http://www.w3.org/2000/09/xmldsig#");
            var XmlNode = XmlNodeList[Indice].ParentNode != null ? XmlNodeList[Indice].ParentNode.SelectSingleNode("xmldsg:Signature", XmlNamespaceManager) : null;

            XmlDocument2.PreserveWhitespace = true;
            XmlDocument3.PreserveWhitespace = true;
            XmlDocument2.LoadXml(XmlDocument.OuterXml);

            if ((XmlNode != null ? XmlNode.SelectSingleNode("xmldsg:KeyInfo/xmldsg:KeyValue", XmlNamespaceManager) : null) == null)
            {
                var XmlNode2 = XmlNode != null ? XmlNode.SelectSingleNode("xmldsg:KeyInfo/xmldsg:X509Data/xmldsg:X509Certificate",
                    XmlNamespaceManager) : null;

                if (XmlNode2 == null) throw new CryptographicException("The public key could not be found.");

                var X509Certificate2 = new X509Certificate2(Convert.FromBase64String(XmlNode2.InnerText));
                Rsa = (RSA) X509Certificate2.PublicKey.Key;
            }

            try
            {
                var XmlSigned = new XmlSigned(XmlDocument2);

                XmlSigned.LoadXml((XmlElement) XmlNode);

                Flag = Rsa == null ? XmlSigned.CheckSignature() : XmlSigned.CheckSignature(Rsa);

                if (!Flag)
                {
                    XmlDocument3.LoadXml(XmlNodeList[Indice].ParentNode.OuterXml);
                    XmlSigned = new XmlSigned(XmlDocument3);
                    XmlSigned.LoadXml((XmlElement) XmlNode);

                    Flag = Rsa == null ? XmlSigned.CheckSignature() : XmlSigned.CheckSignature(Rsa);

                    if (!Flag)
                    {
                        var Str = RemoveNameSpaces(XmlNodeList[Indice]);

                        XmlDocument3.LoadXml(Str);
                        XmlSigned = new XmlSigned(XmlDocument3);
                        XmlSigned.LoadXml((XmlElement) XmlNode);

                        Flag = Rsa == null ? XmlSigned.CheckSignature() : XmlSigned.CheckSignature(Rsa);
                    }
                }
            }
            catch
            {
                // ignored
            }

            bool Result;
            if (Flag) Result = true;
            else
            {
                var XWseSignedXml = new XWseSignedXml(XmlDocument2);
                XWseSignedXml.LoadXml((XmlElement) XmlNode);

                Flag = Rsa == null ? XWseSignedXml.CheckSignature() : XWseSignedXml.CheckSignature(Rsa);

                if (Flag) Result = true;
                else
                {
                    XmlDocument3.LoadXml(XmlNodeList[Indice].ParentNode.OuterXml);
                    XWseSignedXml = new XWseSignedXml(XmlDocument3);
                    XWseSignedXml.LoadXml((XmlElement) XmlNode);

                    Flag = Rsa == null ? XWseSignedXml.CheckSignature() : XWseSignedXml.CheckSignature(Rsa);

                    if (Flag) Result = true;
                    else
                    {
                        var Str2 = RemoveNameSpaces(XmlNodeList[Indice]);

                        XmlDocument3.LoadXml(Str2);
                        XWseSignedXml = new XWseSignedXml(XmlDocument3);
                        XWseSignedXml.LoadXml((XmlElement) XmlNode);

                        Result = Rsa == null ? XWseSignedXml.CheckSignature() : XWseSignedXml.CheckSignature(Rsa);
                    }
                }
            }

            return Result;
        }

        public static bool ValidateSignature(XmlDocument XmlDocument, string NodoAValidar)
        {
            var XmlNodeList = XmlDocument.GetElementsByTagName(NodoAValidar);

            for (var I = 0; I < XmlNodeList.Count; I++)
            {
                var Flag = ValidateSignature(XmlDocument, NodoAValidar, I);

                if (Flag) continue;

                return false;
            }

            return true;
        }

        #endregion.

        #region Remove Name Space.

        public static string RemoveNameSpaces(string Xml)
        {
            return Regex.Replace(Xml,
                "( xmlns.*=[\",'](http|https):\\/\\/[\\w\\-_]+(\\.[\\w\\-_]+)+([\\w\\-\\.,@?^=%&:/~\\+#]*[\\w\\-\\@?^=%&/~\\+#])[\",'])",
                ReplaceNameSpaces);
        }

        public static string RemoveNameSpaces(XmlNode XmlNode)
        {
            return RemoveNameSpaces(XmlNode.OuterXml);
        }

        private static string ReplaceNameSpaces(Match Match)
        {
            var Result = Match.Groups[0].Value.IndexOf("www.w3.org", StringComparison.Ordinal) > -1
                ? Match.Groups[0].Value
                : "";

            return Result;
        }

        #endregion.
    }
}