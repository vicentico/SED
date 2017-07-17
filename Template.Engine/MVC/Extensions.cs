using System.Web.Mvc;

namespace Template.Engine.MVC
{
    public static class Extensions
    {
        public static string MakeActiveClass(this UrlHelper UrlHelper_, string Url_)
        {
            var Result = "active";
            var Url2 = UrlHelper_.Action().ToLower().Replace("index", "");
            Url_ = Url_.ToLower().Replace("index", "");

            if (Url2[Url2.Length - 1] == '/') Url2 = Url2.Remove(Url2.Length - 1);
            if (Url_[Url_.Length - 1] == '/') Url_ = Url_.Remove(Url_.Length - 1);

            if (Url_ != Url2) Result = null;
            
            return Result;
        }
    }
}
