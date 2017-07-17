using System.Security.Cryptography.Xml;
using System.Xml;

namespace Template.Engine.XML
{
    internal class XmlSigned : SignedXml
    {
        public XmlSigned(XmlDocument Document) : base(Document)
        {
        }

        public override XmlElement GetIdElement(XmlDocument XmlDocument, string IdValor)
        {
            XmlElement XmlElement;

            if (XmlDocument == null) XmlElement = null;
            else
            {
                var XmlElement2 = XmlDocument.GetElementById(IdValor);

                if (XmlElement2 != null) XmlElement = XmlElement2;
                else
                {
                    XmlElement2 = (XmlDocument.SelectSingleNode("//*[@Id=\"" + IdValor + "\"]") as XmlElement);

                    if (XmlElement2 != null) XmlElement = XmlElement2;
                    else
                    {
                        XmlElement2 = (XmlDocument.SelectSingleNode("//*[@id=\"" + IdValor + "\"]") as XmlElement);

                        if (XmlElement2 != null) XmlElement = XmlElement2;
                        else XmlElement = (XmlDocument.SelectSingleNode("//*[@ID=\"" + IdValor + "\"]") as XmlElement);
                    }
                }
            }

            return XmlElement;
        }
    }
}