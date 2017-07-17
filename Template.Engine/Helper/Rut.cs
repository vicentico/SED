namespace Template.Engine.Helper
{
    public class Rut
    {
        public static string FormatearConPuntosGuion(string Rut_)
        {
            var Cont = 0;

            if (string.IsNullOrEmpty(Rut_)) return "";

            Rut_ = Rut_.Replace(".", "");
            Rut_ = Rut_.Replace("-", "");

            var Format = "-" + Rut_.Substring(Rut_.Length - 1);

            for (var I = Rut_.Length - 2; I >= 0; I--)
            {
                Format = Rut_.Substring(I, 1) + Format;
                Cont++;
                if (Cont != 3 || I == 0) continue;
                Format = "." + Format;
                Cont = 0;
            }

            return Format;
        }
    }
}