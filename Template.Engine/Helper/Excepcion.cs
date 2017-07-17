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
}
