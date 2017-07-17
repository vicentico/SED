using System;
using System.Text;

namespace Template.Engine.Security
{
    public class Coder
    {
        public static int Xor = 46;

        public static string Encode(string String_)
        {
            try
            {
                var Entry = Encoding.UTF8.GetBytes(String_);

                for (var I = 0; I < Entry.Length; I++) Entry[I] = Convert.ToByte(Xor ^ Convert.ToInt32(Entry[I]));

                var Output = Convert.ToBase64String(Entry).Replace("+", "|");

                return Output;
            }
            catch (Exception)
            {
                return "";
            }
        }

        public static string Decode(string String_)
        {
            try
            {
                var Entry = Convert.FromBase64String(String_.Replace("|", "+"));

                for (var I = 0; I < Entry.Length; I++) Entry[I] = Convert.ToByte(Xor ^ Convert.ToInt32(Entry[I]));

                var Output = Encoding.UTF8.GetString(Entry);

                return Output;
            }
            catch (Exception)
            {
                return "";
            }
        }

        public static string Base64Encode(string Cadena)
        {
            var Byte_ = Encoding.UTF8.GetBytes(Cadena);

            return Convert.ToBase64String(Byte_);
        }

        public static string Base64Decode(string Cadena)
        {
            var Byte_ = Convert.FromBase64String(Cadena);

            return Encoding.UTF8.GetString(Byte_);
        }
    }
}