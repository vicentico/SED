namespace Template.Engine.Data.Models
{
    public class UserData
    {
        public UserData()
        {
            FullName = "Desconocido";
            LastUrlVisited = "/";
            UserId = "";
            CompanyId = 0;
            CompanyName = "";
            CompanyAlias = "";
        }

        public string FullName { get; set; }
        public string LastUrlVisited { get; set; }
        public string UserId { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAlias { get; set; }
    }
}