using System.Collections.Generic;

namespace Template.MVCApp.Model
{
    public class MenuVM
    {
        public int Id { get; set; }
        public int? Padre_Id { get; set; }
        public string Url { get; set; }
        public string Texto { get; set; }
        public string Icono { get; set; }
        public string Ayuda { get; set; }
        public List<MenuVM> SubMenu { get; set; }
    }
}
