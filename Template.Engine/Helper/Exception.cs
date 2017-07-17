using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Template.Engine.Helper
{
    public class Excepcion
    {
        public int Indice { get; set; }
        public string Tipo { get; set; }
        public string Mensaje { get; set; }
        public string Componente { get; set; }
        public string Clase { get; set; }
        public string Metodo { get; set; }
#if DEBUG
        public string LineaError { get; set; }
#endif
    }

    public class ExcepcionWeb
    {
        public string Url { get; set; }
        public string Mensaje { get; set; }
    }

    public static class ExceptionExtensions
    {
        public static string ObtenerExcepcionCompleta(this Exception Exception)
        {
            var I = 1;
            var Excepciones = new List<Excepcion>();

            while (Exception != null)
            {
                var Excepcion = new Excepcion
                {
                    Indice = I,
                    Tipo = Exception.GetType().FullName,
                    Mensaje = Exception.Message,
                    Componente = Exception.TargetSite.Module.Name,
                    Clase = Exception.TargetSite.ReflectedType != null ? Exception.TargetSite.ReflectedType.FullName : null,
                    Metodo = Exception.TargetSite.Name
                };

#if DEBUG
                var StrTemp = Exception.StackTrace.Substring(Exception.StackTrace.Length - 50).Split(':');

                if (StrTemp.Length > 1) Excepcion.LineaError = StrTemp[1].Replace("línea ", "").Replace("line ", "");
#endif

                Excepciones.Add(Excepcion);
                Exception = Exception.InnerException;
                I++;
            }

            var Salida = JsonConvert.SerializeObject(Excepciones.OrderByDescending(X => X.Indice));

            return Salida;
        }

        public static Exception GetBaseExcepcion(this Exception Exception_) {
            while (Exception_.InnerException != null) Exception_ = Exception_.InnerException;

            return Exception_;
        }

        public static Excepcion ObtenerExcepcion(this Exception Exception)
        {
            var Excepcion_ = new Excepcion {
                Tipo = Exception.GetType().FullName,
                Mensaje = Exception.Message,
                Componente = Exception.TargetSite.Module.Name,
                Clase = Exception.TargetSite.ReflectedType != null ? Exception.TargetSite.ReflectedType.FullName : null,
                Metodo = Exception.TargetSite.Name
            };

            return Excepcion_;
        }
    }
}
