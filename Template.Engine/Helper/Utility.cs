using System;
using Template.Engine.Helper.Alert;
using Template.Engine.Security;

namespace Template.Engine.Helper
{
    public class Utility
    {
        public static string TimeAgo(DateTime? DateTime_)
        {
            if (DateTime_ == null) return "Nunca";
            var TimeSpan_ = DateTime.Now - (DateTime)DateTime_;

            if (TimeSpan_.Days > 365)
            {
                var Years_ = (TimeSpan_.Days / 365);
                
                if (TimeSpan_.Days % 365 != 0) Years_ += 1;
                
                return string.Format("hace {0} {1} atrás", Years_, Years_ == 1 ? "año" : "años");
            }
            
            if (TimeSpan_.Days > 30)
            {
                var Months_ = (TimeSpan_.Days / 30);
                
                if (TimeSpan_.Days % 31 != 0) Months_ += 1;
                
                return string.Format("hace {0} {1} atrás", Months_, Months_ == 1 ? "mes" : "meses");
            }

            if (TimeSpan_.Days > 0) return string.Format("hace {0} {1} atrás", TimeSpan_.Days, TimeSpan_.Days == 1 ? "día" : "días");
            if (TimeSpan_.Hours > 0) return string.Format("hace {0} {1} atrás", TimeSpan_.Hours, TimeSpan_.Hours == 1 ? "hora" : "horas");
            if (TimeSpan_.Minutes > 0) return string.Format("hace {0} {1} atrás", TimeSpan_.Minutes, TimeSpan_.Minutes == 1 ? "minuto" : "minutos");
            if (TimeSpan_.Seconds > 5) return string.Format("hace {0} segundos atrás", TimeSpan_.Seconds);
            
            return TimeSpan_.Seconds <= 5 ? "hace un momento" : "Nunca";
        }

        public static string GetRandomColorStyle()
        {
            var IdxMenu = new Random().Next(1, 6);
            var EstiloMenu = "primary";

            switch (IdxMenu)
            {
                case (int)MessageType.Primary:
                    EstiloMenu = "primary";
                    break;
                case (int)MessageType.Secondary:
                    EstiloMenu = "secondary";
                    break;
                case (int)MessageType.Information:
                    EstiloMenu = "info";
                    break;
                case (int)MessageType.Success:
                    EstiloMenu = "success";
                    break;
                case (int)MessageType.Warning:
                    EstiloMenu = "warning";
                    break;
                case (int)MessageType.Error:
                    EstiloMenu = "danger";
                    break;
            }

            return EstiloMenu;
        }

        public static string GetRandomId()
        {
            var Id = new Random().Next(0, 9);
            var Id_ = Coder.Encode(Id.ToString()).Replace("=", "");

            return Id_;
        }
    }
}
