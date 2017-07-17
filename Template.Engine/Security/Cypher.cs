using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Template.Engine.Security
{
    public class Cypher
    {
        public static string Encriptar(string StrEncriptar, string StrIv)
        {
            var AsciiEncoding = new ASCIIEncoding();

            using (var RijndaelManaged = new RijndaelManaged())
            {
                try
                {
                    var Iv = AsciiEncoding.GetBytes((StrIv + "GZd/SxJlLn|b2l79").Substring(0, 16));
                    var Key = AsciiEncoding.GetBytes("qa54ivdmLCpuo5zgof1obtHy3cZqCyiO");

                    using (var CryptoTransform = RijndaelManaged.CreateEncryptor(Key, Iv))
                    {
                        using (var MemoryStream = new MemoryStream())
                        {
                            using (
                                var CryptoStream = new CryptoStream(MemoryStream, CryptoTransform,
                                    CryptoStreamMode.Write))
                            {
                                var ToEncrypt = AsciiEncoding.GetBytes(StrEncriptar);

                                CryptoStream.Write(ToEncrypt, 0, ToEncrypt.Length);
                                CryptoStream.FlushFinalBlock();
                            }

                            return Convert.ToBase64String(MemoryStream.ToArray()).Replace("+", "|");
                        }
                    }
                }
                catch
                {
                    return "";
                }
            }
        }

        public static string Desencriptar(string StrDesEncriptar, string StrIv)
        {
            var AsciiEncoding = new ASCIIEncoding();

            using (var RijndaelManaged = new RijndaelManaged())
            {
                try
                {
                    var Iv = AsciiEncoding.GetBytes((StrIv + "GZd/SxJlLn|b2l79").Substring(0, 16));
                    var Key = AsciiEncoding.GetBytes("qa54ivdmLCpuo5zgof1obtHy3cZqCyiO");

                    var Encrypted = Convert.FromBase64String(StrDesEncriptar.Replace("|", "+"));

                    using (var CryptoTransform = RijndaelManaged.CreateDecryptor(Key, Iv))
                    {
                        using (var MsDecrypt = new MemoryStream())
                        {
                            using (var CryptoStream = new CryptoStream(MsDecrypt, CryptoTransform, CryptoStreamMode.Write))
                            {
                                CryptoStream.Write(Encrypted, 0, Encrypted.Length);
                                CryptoStream.FlushFinalBlock();
                            }

                            return AsciiEncoding.GetString(MsDecrypt.ToArray());
                        }
                    }
                }
                catch
                {
                    return "";
                }
            }
        }
    }
}